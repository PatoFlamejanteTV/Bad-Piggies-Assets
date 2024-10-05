using System;
using System.Collections.Generic;
using System.Globalization;
using CakeRace;
using UnityEngine;

public class GameData : ScriptableObject
{
	public class PartReaction
	{
		public float fun;

		public float fear;

		public PartReaction(float fun, float fear)
		{
			this.fun = fun;
			this.fear = fear;
		}
	}

	public bool m_ghostFeatureEnabled;

	public float m_turboChargePowerFactor = 3f;

	public GUIStyle m_buttonStyle;

	public GameObject m_particles;

	public GameObject m_ballonParticles;

	public GameObject m_dustParticles;

	public GameObject m_constructionParticles;

	public Transform m_constructionUIPrefab;

	public Transform m_contraptionPrefab;

	public Transform m_blueprintPrefab;

	public Transform m_hudPrefab;

	public GameObject m_krakSprite;

	public GameObject m_snapSprite;

	public GameObject m_screenDimmer;

	public GameObject m_bangSprite;

	public GameObject m_glueSprite;

	public GameObject m_alienGlueSprite;

	public GameObject m_superMagnetEffect;

	public GameObject m_turboChargeEffect;

	public AudioClip m_audioAmbience;

	public float m_jointConnectionStrengthWeak;

	public float m_jointConnectionStrengthNormal;

	public float m_jointConnectionStrengthHigh;

	public float m_jointConnectionStrengthExtreme;

	public float m_jointConnectionStrengthHighlyExtreme;

	public GameObject singletonSpawner;

	public GameObject effectManager;

	public CommonAudio commonAudioCollection;

	public TextAsset m_partOrderList;

	public TextAsset m_partReactionList;

	public AnimationClip m_partAppearAnimation;

	public GameObject m_partAppearBackground;

	public GoalChallenge m_basicGoal;

	public GoalChallenge m_eggTransportGoal;

	public GoalChallenge m_pumpkinTransportGoal;

	public List<GameObject> m_parts;

	public List<CustomPartInfo> m_customParts;

	public List<Episode> m_episodeLevels;

	public SandboxLevels m_sandboxLevels;

	public RaceLevels m_raceLevels;

	public List<GameObject> m_desserts;

	public int m_LevelDessertsCount = 3;

	public GameObject m_BonusDessert;

	public GameObject m_unlockLevelAdButtonPrefab;

	public GameObject m_tooltipPrefab;

	public GameObject m_levelUnlockDialog;

	public GameObject m_sandboxUnlockDialog;

	public GameObject m_specialSandboxUnlockDialog;

	public GameObject m_unlockPartTierDialog;

	public GameObject m_genericButtonPrefab;

	public GameObject m_levelRowUnlockPanel;

	public GameObject m_lockedLevelRowPanel;

	public GameObject m_purchaseProductConfirmDialog;

	public GameObject m_purchaseLootcrateConfirmDialog;

	public GameObject m_lpaUnlockDialog;

	public GameObject m_snoutCoinButtonPrefab;

	public GameObject m_scrapButtonPrefab;

	public GameObject m_snoutCoinAdRewardDialog;

	public GameObject m_starCountInfoPrefab;

	public GameObject m_roadHogsUnlockDialog;

	public GameObject m_starterPackDialog;

	public GameObject m_videoNotFoundDialog;

	public GameObject m_dailyChallengeDialog;

	public GameObject m_confirmationFailedDialog;

	public GameObject m_customizePartUIPrefab;

	public GameObject m_getMoreScrapDialog;

	public GameObject m_notificationQueryDialog;

	public DailyChallengeData m_dailyChallengeData;

	public List<GameObject> m_lootCrates;

	public List<GameObject> m_lootCrateLargeIcons;

	public GameObject m_lootcrateOpenDialog;

	public GameObject m_partListing;

	public GameObject m_workshopIntroduction;

	public CakeRaceData m_cakeRaceData;

	public GameObject m_resourceBar;

	public GameObject m_cakeRaceLockedPopup;

	public GameObject m_cakeRaceUnlockedPopup;

	public GameObject m_saveToCloudPopup;

	public GameObject m_disconnectCloudSavePopup;

	public GameObject m_noFreeCrateSlotsPopup;

	public GameObject m_superGlueIcon;

	public GameObject m_superMagnetIcon;

	public GameObject m_superMechanicIcon;

	public GameObject m_nightVisionIcon;

	public GameObject m_turboChargeIcon;

	public GameObject m_tutorialPointer;

	public GameObject m_tutorialPointerClick;

	public GameObject m_xpParticlesSmall;

	public GameObject m_xpParticlesMedium;

	public GameObject m_xpParticlesLarge;

	public GameObject m_lootCrateGraphicsSpawner;

	public GameObject m_cakeRaceBombParticles;

	public GameObject m_alienPartParticles;

	public GameObject m_heartParticles;

	public GameObject m_kingsFavoriteDialog;

	private bool initialized;

	private Dictionary<BasePart.PartType, PartReaction> m_partReactions;

	public GameObject GetPart(BasePart.PartType type)
	{
		foreach (GameObject part in m_parts)
		{
			if (part.GetComponent<BasePart>().m_partType == type)
			{
				return part;
			}
		}
		return null;
	}

	public CustomPartInfo GetCustomPart(BasePart.PartType type)
	{
		foreach (CustomPartInfo customPart in m_customParts)
		{
			if ((type != BasePart.PartType.Pumpkin || GameProgress.HasKey("SecretPumpkin") || INSettings.GetBool(INFeature.UnlockCustomParts)) && customPart.PartType == type)
			{
				return customPart;
			}
		}
		return null;
	}

	public int GetCustomPartIndex(BasePart.PartType type, string partName)
	{
		if (INSettings.GetBool(INFeature.RuntimeGameData))
		{
			return Singleton<INRuntimeGameData>.Instance.GetCustomPartIndex(type, partName);
		}
		GameObject part = GetPart(type);
		if (part != null && part.name.Equals(partName))
		{
			return 0;
		}
		CustomPartInfo customPart = GetCustomPart(type);
		for (int i = 0; i < customPart.PartList.Count; i++)
		{
			if (customPart.PartList[i].name.Equals(partName))
			{
				return i + 1;
			}
		}
		return -1;
	}

	public BasePart GetCustomPart(BasePart.PartType type, int customIndex)
	{
		if (INSettings.GetBool(INFeature.RuntimeGameData))
		{
			return Singleton<INRuntimeGameData>.Instance.GetCustomPart(type, customIndex);
		}
		if (customIndex <= 0)
		{
			GameObject part = GetPart(type);
			if (part == null)
			{
				return null;
			}
			return part.GetComponent<BasePart>();
		}
		CustomPartInfo customPart = GetCustomPart(type);
		if (customIndex > 0 && customIndex - 1 < customPart.PartList.Count)
		{
			return customPart.PartList[customIndex - 1];
		}
		return null;
	}

	public PartReaction GetPartReaction(BasePart.PartType partType)
	{
		PartReaction value = null;
		m_partReactions.TryGetValue(partType, out value);
		return value;
	}

	public RaceLevels.LevelData FindRaceLevel(string identifier)
	{
		return m_raceLevels.GetLevelData(identifier);
	}

	private void OnEnable()
	{
		ReadPartReactions();
	}

	private void ReadPartReactions()
	{
		m_partReactions = new Dictionary<BasePart.PartType, PartReaction>();
		string text = m_partReactionList.text;
		char[] separator = new char[1] { '\n' };
		string[] array = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
		int num = 0;
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			char[] separator2 = new char[1] { ' ' };
			string[] array3 = text2.Split(separator2, StringSplitOptions.RemoveEmptyEntries);
			if (array3.Length >= 3 && num > 0)
			{
				string text3 = array3[0].Trim();
				if (text3 != string.Empty)
				{
					BasePart.PartType partType = PartNameToType(text3);
					if (partType != 0)
					{
						float fun = float.Parse(array3[1], CultureInfo.InvariantCulture);
						float fear = float.Parse(array3[2], CultureInfo.InvariantCulture);
						m_partReactions[partType] = new PartReaction(fun, fear);
					}
				}
			}
			num++;
		}
	}

	private BasePart.PartType PartNameToType(string name)
	{
		BasePart.PartType result = BasePart.PartType.Unknown;
		foreach (GameObject part in m_parts)
		{
			BasePart.PartType partType = part.GetComponent<BasePart>().m_partType;
			if (partType.ToString() == name)
			{
				result = partType;
				break;
			}
		}
		return result;
	}
}
