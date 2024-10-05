using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class WPFPrefs : UnityEngine.Object
{
	private static CryptoUtility m_crypto = new CryptoUtility("3b91A049Ca7HvSjhxT35");

	public static void WriteGhostPlayerData(string filename, GhostPlayer gp)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(GhostPlayer));
		FileStream fileStream = new FileStream(Application.persistentDataPath + "/" + filename, FileMode.Create);
		xmlSerializer.Serialize(fileStream, gp);
		fileStream.Close();
	}

	public static GhostPlayer ReadGhostPlayerData(string filename)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(GhostPlayer));
		GhostPlayer result = new GhostPlayer();
		try
		{
			FileStream fileStream = new FileStream(Application.persistentDataPath + "/" + filename, FileMode.Open);
			result = xmlSerializer.Deserialize(fileStream) as GhostPlayer;
			fileStream.Close();
		}
		catch
		{
		}
		return result;
	}

	public static string ContraptionFileName(string levelName)
	{
		return BitConverter.ToString(CryptoUtility.ComputeHash(Encoding.UTF8.GetBytes(levelName))).Substring(0, 30).Replace("-", string.Empty) + ".contraption";
	}

	public static void SaveContraptionDataset(string levelName, ContraptionDataset cds)
	{
		if (!INSettings.GetBool(INFeature.NewContraptionData))
		{
			SaveOriginalContraptionDataset(INUnity.DataPath + "/contraptions", levelName, cds);
		}
		else
		{
			INContraptionDataManager.Instance.SaveContraptionData(levelName, cds);
		}
	}

	public static void SaveOriginalContraptionDataset(string directory, string levelName, ContraptionDataset cds)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(ContraptionDataset));
		MemoryStream memoryStream = new MemoryStream();
		StreamWriter textWriter = new StreamWriter(memoryStream, Encoding.UTF8);
		xmlSerializer.Serialize(textWriter, cds);
		byte[] clearTextBytes = memoryStream.ToArray();
		memoryStream.Close();
		byte[] array = m_crypto.Encrypt(clearTextBytes);
		string text = ContraptionFileName(levelName);
		Directory.CreateDirectory(directory);
		FileStream fileStream = new FileStream(directory + "/" + text, FileMode.Create);
		fileStream.Write(array, 0, array.Length);
		fileStream.Close();
	}

	public static void SaveLevelBluePrint(string levelName, ContraptionDataset cds, bool isSuperBlueprint = false, int superBluePrintIndex = 0)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(ContraptionDataset));
		MemoryStream memoryStream = new MemoryStream();
		StreamWriter textWriter = new StreamWriter(memoryStream, Encoding.UTF8);
		xmlSerializer.Serialize(textWriter, cds);
		byte[] array = memoryStream.ToArray();
		memoryStream.Close();
		string path = ((!isSuperBlueprint) ? "Data/Contraptions" : "Data/SuperContraptions");
		string path2 = levelName + "_contraption" + ((!isSuperBlueprint) ? string.Empty : $"_{superBluePrintIndex + 1:00}") + ".xml";
		FileStream fileStream = new FileStream(Path.Combine(Path.Combine(Application.dataPath, path), path2), FileMode.Create);
		fileStream.Write(array, 0, array.Length);
		fileStream.Close();
	}

	public static ContraptionDataset LoadContraptionDataset(string levelName)
	{
		if (!INSettings.GetBool(INFeature.NewContraptionData))
		{
			return LoadOriginalContraptionDataset(INUnity.DataPath + "/contraptions", levelName);
		}
		return INContraptionDataManager.Instance.LoadContraptionData(levelName);
	}

	public static ContraptionDataset LoadOriginalContraptionDataset(string directory, string levelName)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(ContraptionDataset));
		ContraptionDataset result = new ContraptionDataset();
		string text = ContraptionFileName(levelName);
		try
		{
			FileStream fileStream = new FileStream(directory + "/" + text, FileMode.Open);
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, array.Length);
			MemoryStream stream = new MemoryStream(m_crypto.Decrypt(array, 0));
			result = xmlSerializer.Deserialize(stream) as ContraptionDataset;
			fileStream.Close();
		}
		catch
		{
		}
		return result;
	}

	public static ContraptionDataset LoadContraptionDataset(TextAsset textAsset)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(ContraptionDataset));
		ContraptionDataset result = new ContraptionDataset();
		try
		{
			MemoryStream memoryStream = new MemoryStream(textAsset.bytes);
			result = xmlSerializer.Deserialize(memoryStream) as ContraptionDataset;
			memoryStream.Close();
		}
		catch
		{
		}
		return result;
	}
}
