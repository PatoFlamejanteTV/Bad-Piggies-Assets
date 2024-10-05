using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TextMeshHelper : MonoBehaviour
{
	[SerializeField]
	private int maxCharacters = 24;

	[SerializeField]
	private int maxKanjiCharacters = 20;

	[SerializeField]
	private bool splitHyphen;

	private void TextUpdated(GameObject go)
	{
		if (go != null && go == base.gameObject)
		{
			Wrap(go.GetComponent<TextMesh>(), (!UsesKanjiCharacters()) ? maxCharacters : maxKanjiCharacters, splitHyphen);
		}
	}

	public static bool UsesKanjiCharacters()
	{
		if (!Singleton<Localizer>.Instance.CurrentLocale.Equals("zh-CN"))
		{
			return Singleton<Localizer>.Instance.CurrentLocale.Equals("ja-JP");
		}
		return true;
	}

	public static void Wrap(TextMesh[] textMesh, int maxCharactersOnRow)
	{
		if (textMesh == null || textMesh.Length == 0)
		{
			return;
		}
		for (int i = 0; i < textMesh.Length; i++)
		{
			if (textMesh[i] != null)
			{
				Wrap(textMesh[i], maxCharactersOnRow);
			}
		}
	}

	public static void Wrap(TextMesh textMesh, int maxCharactersOnRow, bool splitHyphen = false)
	{
		if (textMesh == null || maxCharactersOnRow <= 0)
		{
			return;
		}
		string text = textMesh.text;
		text = text.Replace("\n", " ");
		text = text.Replace("\r", " ");
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		bool flag = false;
		if (UsesKanjiCharacters())
		{
			for (int i = 0; i < text.Length; i++)
			{
				if (!flag && text[i].Equals('<'))
				{
					flag = true;
				}
				if (!flag)
				{
					num++;
					if (num > maxCharactersOnRow)
					{
						num = 0;
						stringBuilder.Append('\n');
					}
				}
				else if (text[i].Equals('>'))
				{
					flag = false;
				}
				stringBuilder.Append(text[i]);
			}
		}
		else
		{
			char[] array;
			if (splitHyphen)
			{
				char[] obj = new char[2] { ' ', '\0' };
				array = obj;
				obj[1] = '-';
			}
			else
			{
				(array = new char[1])[0] = ' ';
			}
			char[] separator = array;
			string[] array2 = text.Split(separator);
			int num2 = 0;
			string[] array3 = array2;
			foreach (string text2 in array3)
			{
				bool flag2 = false;
				if (!flag && text2.Contains("<quad"))
				{
					flag = true;
				}
				if (!flag)
				{
					num2++;
					num += text2.Length;
					if (num2 > 1 && num >= maxCharactersOnRow)
					{
						num = text2.Length;
						stringBuilder.Append('\n');
					}
				}
				else if (text2.Contains("/>"))
				{
					num++;
					if (num2 > 1 && num >= maxCharactersOnRow)
					{
						num = 1;
						flag2 = true;
					}
					if (!text2.Contains("/><quad"))
					{
						flag = false;
					}
				}
				if (text2.Contains("/><quad") && flag2)
				{
					stringBuilder.Append(text2.Replace("><", ">\n<"));
					flag2 = false;
				}
				else
				{
					stringBuilder.Append(text2);
				}
				if (flag2)
				{
					stringBuilder.Append('\n');
				}
				if (num2 < array2.Length)
				{
					stringBuilder.Append(' ');
				}
			}
		}
		textMesh.text = stringBuilder.ToString();
	}

	public static void UpdateTextMeshes(TextMesh[] textMeshes, string text, bool refreshTranslations = false)
	{
		if (textMeshes == null || textMeshes.Length == 0)
		{
			return;
		}
		for (int i = 0; i < textMeshes.Length; i++)
		{
			if (textMeshes[i] != null)
			{
				textMeshes[i].text = ((!string.IsNullOrEmpty(text)) ? text : string.Empty);
			}
			if (refreshTranslations)
			{
				TextMeshLocale component = textMeshes[i].GetComponent<TextMeshLocale>();
				if (component != null)
				{
					component.RefreshTranslation();
				}
			}
		}
	}

	public static void ForceWrapText(TextMesh[] textMeshes, string text, int linebreak)
	{
		List<char> list = new List<char>();
		for (int i = 0; i < text.Length; i++)
		{
			if (i != 0 && i % linebreak == 0)
			{
				list.Add('\n');
			}
			list.Add(text[i]);
		}
		string text2 = new string(list.ToArray());
		for (int j = 0; j < textMeshes.Length; j++)
		{
			textMeshes[j].text = text2;
		}
	}
}
