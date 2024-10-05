using UnityEngine;

public class LevelsUnlockDialog : MonoBehaviour
{
	public bool isButton;

	public static GameObject Create()
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("UI/PageLevelsLocked")) as GameObject;
		gameObject.name = "RegistrationDialog";
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("HUDCamera");
		gameObject.transform.position = gameObject2.transform.position + Vector3.forward * 5f;
		if (Singleton<GameManager>.Instance.LevelCount - Singleton<GameManager>.Instance.LevelCount / 5 < GameProgress.GetMinimumLockedLevel(Singleton<GameManager>.Instance.CurrentEpisodeIndex) + 10)
		{
			gameObject.transform.FindChildRecursively("BuyButton10").gameObject.SetActive(value: false);
			gameObject.transform.FindChildRecursively("BuyButtonWholeEpisode").gameObject.SetActive(value: false);
			gameObject.transform.FindChildRecursively("DescriptionText10").gameObject.SetActive(value: false);
			gameObject.transform.FindChildRecursively("DescriptionTextWhole").gameObject.SetActive(value: false);
			gameObject.transform.FindChildRecursively("SmokeParticles10").gameObject.SetActive(value: false);
			gameObject.transform.FindChildRecursively("SmokeParticlesWhole").gameObject.SetActive(value: false);
			gameObject.transform.FindChildRecursively("BuyButton9").gameObject.SetActive(value: true);
			gameObject.transform.FindChildRecursively("DescriptionTextWhole").gameObject.SetActive(value: false);
			gameObject.transform.FindChildRecursively("DescriptionText9").gameObject.SetActive(value: true);
			gameObject.transform.FindChildRecursively("SmokeParticles9").gameObject.SetActive(value: true);
		}
		return gameObject;
	}

	private void OnEnable()
	{
		Singleton<GuiManager>.Instance.GrabPointer(this);
		Singleton<KeyListener>.Instance.GrabFocus(this);
		KeyListener.keyReleased += HandleKeyReleased;
		if (!(base.gameObject.transform.FindChildRecursively("Dialog") == null))
		{
			GameObject gameObject = base.gameObject.transform.FindChildRecursively("Dialog").gameObject;
			if (Singleton<BuildCustomizationLoader>.Instance.CustomerID == "chinatelecom" && gameObject.transform.Find("AboutChinatelecom") != null)
			{
				gameObject.transform.Find("AboutChinatelecom").gameObject.SetActive(value: true);
				gameObject.transform.Find("AboutChinatelecom").transform.localPosition = new Vector3(0f, -7.5f, -1f);
				gameObject.transform.Find("Popup").transform.localScale = new Vector3(1f, 1.2f, 1f);
				gameObject.transform.Find("Popup").transform.localPosition = new Vector3(0f, -1.7f, 1f);
			}
		}
	}

	private void OnDisable()
	{
		Singleton<GuiManager>.Instance.ReleasePointer(this);
		Singleton<KeyListener>.Instance.ReleaseFocus(this);
		KeyListener.keyReleased -= HandleKeyReleased;
	}

	private void HandleKeyReleased(KeyCode obj)
	{
		if (obj == KeyCode.Escape)
		{
			DismissDialog();
		}
	}

	public void DismissDialog()
	{
		if (!isButton)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void HandleKeyPressed(KeyCode key)
	{
		if (key == KeyCode.Escape)
		{
			DismissDialog();
		}
	}

	public void TextUpdated(GameObject textObject)
	{
		TextMesh component = textObject.GetComponent<TextMesh>();
		if (component.text.Contains("%"))
		{
			if (isButton)
			{
				component.text = component.text.Replace("%3", base.gameObject.transform.FindChildRecursively("PriceText").GetComponent<TextMesh>().text);
			}
			string text = GameProgress.GetMinimumLockedLevel(Singleton<GameManager>.Instance.CurrentEpisodeIndex).ToString();
			component.text = component.text.Replace("%1", text);
			string text2 = Mathf.Min(Singleton<GameManager>.Instance.LevelCount - Singleton<GameManager>.Instance.LevelCount / 5, int.Parse(text) + 9).ToString();
			component.text = component.text.Replace("%2", text2);
			if (text == text2)
			{
				component.text = component.text.Replace("s " + text2 + "-" + text2, " " + text2);
				component.text = component.text.Replace(text2 + "-" + text2, text2);
			}
		}
	}
}
