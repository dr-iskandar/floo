using UnityEngine;
using System.Collections;

public class EssentialData : MonoBehaviour 
{
	public const string TAG_NONE = "none";
	public const string DEFAULT_FISH_SKIN = "nemo";
	public const string TAG_PLAYER = "Player";
	public const string TAG_ENEMY = "Enemy";
	public const float scaleFactor = 100.0f;
	public const string TAG_CURRENCY = "USD";

	private const string DEFAULT_USER_ID = "test_user";
	private const string DEFAULT_SECRET_KEY = "jambanpuri";
	private const string DEFAULT_DISPLAY_NAME = "FlooPlay";

	#region HARDWARE BACK BUTTON
	public static EnumData.PopupState popupState;
	public static EnumData.PanelState panelState;
	public static EnumData.SuddenPopupState suddenState;
	#endregion

	#region FIRST GAME
	public const string LINK_STORE_ANDROID = "https://play.google.com/store/apps/details?id=com.legends.floo";
	public const string LINK_STORE_IOS = "https://www.apple.com";
	#endregion

	public static Color colorBlueLight 	= new Color (0.207f, 0.286f, 0.686f);
	public static Color colorBlueDark 	= new Color (0.133f, 0.1607f, 0.298f);
	public static Color colorBlack 		= new Color (0.137f, 0.1215f, 0.1254f);

	private static EssentialData instance;

	public static EssentialData Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject go = new GameObject("Essential Data");
				instance = go.AddComponent<EssentialData>();
				instance.GenerateDefaultPlayerData();
				DontDestroyOnLoad(go);
			}
			return instance;
		}
	}

	public string PlayerDisplayName
	{
		get
		{
			return playerData.displayName;
		}
		set
		{
			playerData.displayName = value;
		}
	}

	private bool isShowFishHead = true;

	public bool IsShowFishHead
	{
		get
		{
			return isShowFishHead;
		}
	}

	private bool isShowHeadCollider = false;

	public bool IsShowHeadCollider
	{
		get
		{
			return isShowHeadCollider;
		}
	}

	private bool isShowBody = false;

	public bool IsShowBody
	{
		get
		{
			return isShowBody;
		}
	}

	private PlayerData playerData;

	public PlayerData PlayerData
	{
		get
		{
			return playerData;
		}
		set
		{
			playerData = value;
		}
	}

	void GenerateDefaultPlayerData()
	{
		PlayerData data = new PlayerData();
		data.userId = DEFAULT_USER_ID;
		data.secretKey = DEFAULT_SECRET_KEY;
		data.displayName = DEFAULT_DISPLAY_NAME;
		data.gold = 0;
		data.equippedBuff = TAG_NONE;
		data.equippedSkin = DEFAULT_FISH_SKIN;
		data.collectedSkins.Add(DEFAULT_FISH_SKIN);

		playerData = data;
	}

	public void GoToLink (string link)
	{
		Debug.Log(link);
		Application.OpenURL(link);
	}

	#region Data Saves
	private const string TAG_USER_ID = "user_id";

	public void SaveUserId()
	{
		PlayerPrefs.SetString(TAG_USER_ID, playerData.userId);
	}

	public string LoadUserId()
	{
		string userId = "";
		if(PlayerPrefs.HasKey(TAG_USER_ID))
		{
			userId = PlayerPrefs.GetString(TAG_USER_ID);
		}

		return userId;
	}
	#endregion

	#region Sound Saves
	private const string TAG_BGM_SETTING = "isBGM";
	private const string TAG_SFX_SETTING = "isSFX";

	public void SaveSettingBGM(bool settingBGM)
	{
		PlayerPrefs.SetInt(TAG_BGM_SETTING, (settingBGM ? 1 : 0));
	}

	public bool LoadSettingBGM()
	{
		if(PlayerPrefs.HasKey(TAG_BGM_SETTING))
		{
			return (PlayerPrefs.GetInt(TAG_BGM_SETTING) != 0);
		}else{
			SaveSettingBGM (true);
			return (PlayerPrefs.GetInt(TAG_BGM_SETTING) != 0);
		}
	}

	public void SaveSettingSFX(bool settingSFX)
	{
		PlayerPrefs.SetInt(TAG_SFX_SETTING, (settingSFX ? 1 : 0));
	}

	public bool LoadSettingSFX()
	{
		if(PlayerPrefs.HasKey(TAG_SFX_SETTING))
		{
			return (PlayerPrefs.GetInt(TAG_SFX_SETTING) != 0);
		}else{
			SaveSettingSFX (true);
			return (PlayerPrefs.GetInt(TAG_SFX_SETTING) != 0);
		}
	}
	#endregion

	#region Controller Type Saves
	private const string TAG_CONTROLLER_TYPE = "controller_type";	// 1 joystick - 2 touch

	public void SaveControllerType(int controllerType)
	{
		PlayerPrefs.SetInt(TAG_CONTROLLER_TYPE, controllerType);
	}

	public int LoadControllerType()
	{
		int controllerType = 1;
		if(PlayerPrefs.HasKey(TAG_CONTROLLER_TYPE))
		{
			controllerType = PlayerPrefs.GetInt(TAG_CONTROLLER_TYPE);
		}

		return controllerType;
	}
	#endregion

	#region FIRST GAME
	private const string TAG_FIRST_TIME = "isFirstTimeOpenGame";

	public void SaveStatusFirstOpen (bool isFirstTime)
	{
		PlayerPrefs.SetInt(TAG_FIRST_TIME, (isFirstTime ? 1 : 0));
	}

	public bool LoadStatusFirstOpen()
	{
		if(PlayerPrefs.HasKey(TAG_FIRST_TIME))
		{
			return (PlayerPrefs.GetInt(TAG_FIRST_TIME) != 0);
		}else{
			SaveStatusFirstOpen (false);
			return (PlayerPrefs.GetInt(TAG_FIRST_TIME) != 0);
		}
	}
	#endregion

	#region DAILYLOGIN
	private const string TAG_OPEN_DAILYLOGIN = "isAlreadyShowDailyLogin";

	public void SaveStatusOpenDaily (bool isAlreadyOpenDaily)
	{
		PlayerPrefs.SetInt(TAG_OPEN_DAILYLOGIN, (isAlreadyOpenDaily ? 1 : 0));
	}

	public bool LoadStatusOpenDaily ()
	{
		if(PlayerPrefs.HasKey(TAG_OPEN_DAILYLOGIN))
		{
			return (PlayerPrefs.GetInt(TAG_OPEN_DAILYLOGIN) != 0);
		}else{
			SaveStatusOpenDaily (false);
			return (PlayerPrefs.GetInt(TAG_OPEN_DAILYLOGIN) != 0);
		}
	}
	#endregion

	#region NEWS
	public static bool isAlreadyShowNews = false;

	private const string TAG_ALLOW_SHOW_NEWS = "isAllowShowNews";

	public void SaveStatusAllowShowNews (bool isAllowNews)
	{
		PlayerPrefs.SetInt(TAG_ALLOW_SHOW_NEWS, (isAllowNews ? 1 : 0));
//		Debug.Log("is allow " + LoadStatusAllowShowNews ());
	}

	public bool LoadStatusAllowShowNews ()
	{
		if(PlayerPrefs.HasKey(TAG_ALLOW_SHOW_NEWS))
		{
			return (PlayerPrefs.GetInt(TAG_ALLOW_SHOW_NEWS) != 0);
		}else{
			SaveStatusAllowShowNews (true);
			return (PlayerPrefs.GetInt(TAG_ALLOW_SHOW_NEWS) != 0);
		}
	}
	#endregion


	#region RATE US
	private const string TAG_OPEN_LINK_RATEUS = "isAlreadyRating";
	private const string TAG_DECLINE_LINK_RATEUS = "isDeclineToRating";
	public static int QuitGameLoop = 0;

	//user go to rating app store
	public void SaveStatusAlreadyRating (bool isAlreadyRating)
	{
		PlayerPrefs.SetInt(TAG_OPEN_LINK_RATEUS, (isAlreadyRating ? 1 : 0));
	}

	public bool LoadStatusAlreadyRating ()
	{
		if(PlayerPrefs.HasKey(TAG_OPEN_LINK_RATEUS))
		{
			return (PlayerPrefs.GetInt(TAG_OPEN_LINK_RATEUS) != 0);
		}else{
			SaveStatusAlreadyRating (false);
			return (PlayerPrefs.GetInt(TAG_OPEN_LINK_RATEUS) != 0);
		}
	}
	//user STATUS decline to rating... if decline = true, set to 3 counter
	public void SaveStatusDeclineRating (bool isDeclineRating)
	{
		PlayerPrefs.SetInt(TAG_DECLINE_LINK_RATEUS, (isDeclineRating ? 1 : 0));
	}

	public bool LoadStatusDeclineRating ()
	{
		if(PlayerPrefs.HasKey(TAG_DECLINE_LINK_RATEUS))
		{
			return (PlayerPrefs.GetInt(TAG_DECLINE_LINK_RATEUS) != 0);
		}else{
			SaveStatusDeclineRating (false);
			return (PlayerPrefs.GetInt(TAG_DECLINE_LINK_RATEUS) != 0);
		}
	}

	#endregion

	#region LANGUAGE SAVED
	private const string TAG_LANGUAGE_SELECT_INDEX = "indexDeviceLanguage";

	public void SaveLanguageId (int LanguageId)
	{
		PlayerPrefs.SetInt(TAG_LANGUAGE_SELECT_INDEX, LanguageId);
//		Debug.Log ("save LanguageId " + LanguageId);
	}

	public int LoadLanguageId ()
	{
		int LanguageId = 1;
		if(PlayerPrefs.HasKey(TAG_LANGUAGE_SELECT_INDEX))
		{
			LanguageId = PlayerPrefs.GetInt(TAG_LANGUAGE_SELECT_INDEX);
		}

		return LanguageId;
	}
	#endregion

}
