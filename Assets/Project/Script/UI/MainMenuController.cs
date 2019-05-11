using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour 
{
	private static MainMenuController instance;

	public static MainMenuController Instance
	{
		get
		{
			return instance;
		}
	}

	#region VARAIBLES
	public UIPopupRateUs rateScript;
	public UIPopupPowerup buffScript;
	public UIPopupNews newsScript;
	public UIPopupDailyLogin dailyScript;
	public GameObject popupBooster;
	public GameObject popupOption;
	public GameObject popupNickname;
	public GameObject popupCredit;
	public GameObject popupExitGame;
	public ScrollRect scrollRectCredit;
	public Button btnPlay;

	[Header("Canvas Others")]
	public GameObject canvasBGOther;

	[Header("Btn Booster")]
	public Sprite spriteNoBooster;
	public Sprite spriteBooster;

	public Image imageBoosterBtn;
	public Image imageSelectedBuff;

	public GameObject txtNoBooster;
	public GameObject frameEquipped;
	public Text txtSelectedBuff;

	[Header("Pop Option")]
	public Toggle toggSFX;
	public Toggle toggBGM;
	public Toggle toggContrJoystick;
	public Toggle toggContrTouch;

	public Text txtStatusSFX;
	public Text txtStatusBGM;
	public Dropdown dropLanguage;
	public Sprite spriteFB;
	public Sprite spriteFBConnected;
	public Button btnFacebook;
	public Image imgBtnFacebook;
	public Text txtFacebook;

	private List<MultiLanguageData> listLanguageDropdown = new List<MultiLanguageData>();

	public GameObject PopUpLogin;

	[Header("Pop Nickname")]
	public Sprite spriteNickNormal;
	public Sprite spriteNickWarning;
	public Image imageFrameNick;

	public InputField playerName;
	public Text warningText;
	public Text displayNickname; // menu

	private List <PowerupData> listBuff = new List<PowerupData>();
	private string namePlayer;
	private int remainSeconds;
	private double timeBuff;

	private int loopCounterPopupRating = 1;
	#endregion

	void Awake()
	{
		instance = this;
		Utilities.SocialManager.Instance.InitFB();
		DefaultParameterManager.Instance.LoadDefaultParameterJSON ();
		ShopDataManager.Instance.LoadShopDataJSON();
		AchievementDataManager.Instance.LoadAchievementDataJSON ();
		UpdateDropdownList ();
	}

	void Start()
	{
		SoundUtility.Instance.SetBGM(BGMData.MainMenu);
		// SoundUtility.Instance.SetBGM(BGMData.MainMenuChristmas);

		InitializeData();

		btnPlay.interactable = false;
		Invoke ("EnableBtnPlay", 0.4f);

		dailyScript.InitGetDailyLogin();
	}

	void OnApplicationPause (bool pauseStatus)
	{
		// if Application is not paused / from pause (temp exit the apps) to pause state.
		if (!pauseStatus)
		{
			CancelInvoke("ChangePerLoop");
			CheckBoosterEquipped ();
		}
	}

	public void InitializeData()
	{
		CanvasDefaultCondition ();
		ResetPopupNickname ();
		ChangeDisplayNickname ();
		namePlayer = playerName.text;

		// Mock List data
		SetDropdownList ();

		GeneratePlayerFishMenu ();

		isBoosterIsInUse (false);
		CheckBoosterEquipped ();

		CheckPopupRatingStatus ();

		ShowCurrentLanguageSelected ();

		Screen.sleepTimeout = SleepTimeout.SystemSetting;
	}

	void GeneratePlayerFishMenu ()
	{
		if (!string.IsNullOrEmpty (EssentialData.Instance.PlayerData.equippedSkin))
		{

			MainmenuFishColorManager.Instance.CreateMenuFish (EssentialData.Instance.PlayerData.equippedSkin);

			if (!string.IsNullOrEmpty (EssentialData.Instance.PlayerData.colorCode))
			{
				UIPopupAdditionalUtility.Instance.ShowLoadingPopup ();

				int codeId = int.Parse (EssentialData.Instance.PlayerData.colorCode);
				MainmenuFishColorManager.Instance.ChangeFishColorMenu (codeId);
			}
		}
	}

	#region RATE US
	void CheckPopupRatingStatus ()
	{
//		Debug.Log ("= Rating " +EssentialData.Instance.LoadStatusAlreadyRating() + " Decline " + EssentialData.Instance.LoadStatusDeclineRating() );
		if (!EssentialData.Instance.LoadStatusAlreadyRating ()) // if not yet rating. Open popup rating
		{
			if (EssentialData.Instance.LoadStatusDeclineRating ()) 
			{
				loopCounterPopupRating = 3;
			}
			else
			{
				loopCounterPopupRating = 2;
			}

//			Debug.Log ("== loopCounterPopupRating " + loopCounterPopupRating +" QuitGameLoop "+ EssentialData.QuitGameLoop);
			if (loopCounterPopupRating == EssentialData.QuitGameLoop)
			{
				// open popup rating
				rateScript.InitPopRateUs ();
				EssentialData.QuitGameLoop = 0;
			}
		}
	}
	#endregion

	#region UI FUNCT
	// canvas
	public void CanvasDefaultCondition ()
	{
		EssentialData.panelState = EnumData.PanelState.off;

		CloseAllPopup ();
		canvasBGOther.SetActive (false);
		popupBooster.SetActive (false);
		UIShopController.Instance.CloseCanvasShop ();
		UIAchievementController.Instance.CloseCanvasAchievement ();
		UILeaderboardController.Instance.CloseCanvasLeaderboard ();
		UpperMenuGold.Instance.SetGold (EssentialData.Instance.PlayerData.gold);
	}

	public void OpenPanelShop ()
	{
		CanvasDefaultCondition ();

		EssentialData.panelState = EnumData.PanelState.on;

		canvasBGOther.SetActive (true);
		UIShopController.Instance.InitShop ();
		UIShopController.Instance.OpenCanvasShop ();
	}

	void OpenPanelShopAds ()
	{
		OpenPanelShop ();
	}

	public void OpenPanelShopInvicible ()
	{
		OpenPanelShop ();
	}

	public void OpenPanelAchievement ()
	{
		CanvasDefaultCondition ();

		EssentialData.panelState = EnumData.PanelState.on;

		canvasBGOther.SetActive (true);
		UIAchievementController.Instance.OpenCanvasAchievement ();
		UIAchievementController.Instance.InitAchievement ();
	}

	public void OpenPanelLeaderboard ()
	{
		CanvasDefaultCondition ();

		EssentialData.panelState = EnumData.PanelState.on;

		canvasBGOther.SetActive (true);
		UILeaderboardController.Instance.OpenCanvasLeaderboard ();
		UILeaderboardController.Instance.InitLeaderboard ();
	}

	void OpenPanelBooster ()
	{
		CanvasDefaultCondition ();

		EssentialData.panelState = EnumData.PanelState.on;

		canvasBGOther.SetActive (true);
		popupBooster.SetActive (true);
	}

	// Boosters
	public void UpdateSelectedBuffText (string buffMsg)
	{
		txtSelectedBuff.text = buffMsg;
	}

	public void ChangeSelectedBuffImage (Sprite selectedBuffSprite)
	{
		imageSelectedBuff.sprite = selectedBuffSprite;
	}

	public void isBoosterIsInUse (bool isUseBuff)
	{
		if (isUseBuff)
		{
			txtNoBooster.SetActive (false);
			frameEquipped.SetActive (true);
			imageBoosterBtn.sprite = spriteBooster;
			imageSelectedBuff.gameObject.SetActive (true);

		}
		else // NO BOOSTER
		{
			txtNoBooster.SetActive (true);
			frameEquipped.SetActive (false);
			imageBoosterBtn.sprite = spriteNoBooster;
			imageSelectedBuff.gameObject.SetActive (false);
		}
	}

	void ConditionBtnFacebookIsConnect ()
	{
		/*if (Utilities.SocialManager.Instance.IsLoggedInFB ())	// if already login fb
		{
			btnFacebook.interactable = false;
			imgBtnFacebook.sprite = spriteFBConnected;
			txtFacebook.text = LanguageManager.Instance.GetMessage ("BTN0014");
		}
		else
		{
			btnFacebook.interactable = true;
			imgBtnFacebook.sprite = spriteFB;
			txtFacebook.text = LanguageManager.Instance.GetMessage ("BTN0013");
		}*/
	}

	#endregion

	#region BUTTON

	public void PlayGame()
	{
		Invoke ("ExecuteBtnPlay", 0.25f);
	}

	public void BtnLeaderboard ()
	{
//		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
		Invoke ("OpenPanelLeaderboard", 0.25f);
	}

	public void BtnAchievement ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
		Invoke ("OpenPanelAchievement", 0.25f);
	}
		
	public void BtnShop ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
		Invoke ("OpenPanelShop", 0.25f);
	}

	public void BtnBooster ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
		Invoke ("OpenPanelBooster", 0.25f);
	}

	public void BtnOption ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
		Invoke ("OpenPopupOption", 0.25f);

		ConditionBtnFacebookIsConnect ();

		SetSoundLocalData ();
		SetControllerLocalData ();
		LabelChangeToggle (toggBGM.isOn, txtStatusBGM);
		LabelChangeToggle (toggSFX.isOn , txtStatusSFX);
	}

	public void BtnShopInvicible ()
	{
		if (UIShopController.Instance.toggSkin.isOn)
		{
			SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
		}
		else
		{
			UIShopController.Instance.toggGold.isOn = false;
			UIShopController.Instance.toggSkin.isOn = true;
		}

		Invoke ("OpenPanelShopInvicible", 0.25f);
	}

	public void BtnNoAds ()
	{
		if (UIShopController.Instance.toggGold.isOn)
		{
			SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
		}
		else
		{
			UIShopController.Instance.toggGold.isOn = true;
			UIShopController.Instance.toggSkin.isOn = false;
		}

		Invoke ("OpenPanelShopAds", 0.25f);
	}

	public void BtnLog ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		Debug.Log ("Log");
		//		Invoke ("OpenPanelLeaderboard", 0.25f);
//		Invoke ("ExecuteConfirmChangeLanguage", 0.25f);
	}

	public void BtnMenuNickname ()
	{
		OpenPopupNickname ();
	}

	#endregion

	#region BUTTON FUNCT
	void ExecuteBtnPlay ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnPlay);

		namePlayer = playerName.text;
		SceneManager.LoadScene("Main Game");
		/*
		if (!string.IsNullOrEmpty(namePlayer))
		{
			if (namePlayer.Equals(EssentialData.Instance.PlayerDisplayName))
			{
				SceneManager.LoadScene("Main Game");
			}
		}
		else
		{
			UIPopupAdditionalUtility.Instance.InitPopNoticeConfirmClose (LanguageManager.Instance.GetMessage ("POP0003"));
			// "Please Fill the Name First"
		}*/
	}

	void OpenPopupOption ()
	{
		CloseAllPopup ();
		popupCredit.SetActive (false);

		EssentialData.popupState = EnumData.PopupState.on;

		popupOption.SetActive (true);
	}

	void OpenPopupCredit ()
	{
		CloseAllPopup ();

		EssentialData.popupState = EnumData.PopupState.on;

		popupCredit.SetActive (true);
		scrollRectCredit.verticalNormalizedPosition = 1.0f;
	}

	void OpenPopupNickname ()
	{
		CloseAllPopup ();

		EssentialData.popupState = EnumData.PopupState.on;

		ResetPopupNickname ();
		ChangeDisplayNickname ();
		popupNickname.SetActive (true);
	}

	void OpenPopupExitGame ()
	{
		CloseAllPopup ();

		EssentialData.popupState = EnumData.PopupState.on;

		popupExitGame.SetActive (true);
	}

	// NEWS
	public void OpenPopupNews ()
	{
		CloseAllPopup ();
//		EssentialData.popupState = EnumData.PopupState.on;
		newsScript.InitPopupNews();
	}

	#endregion

	#region POPUP BUTTON
	public void BtnClosePopup ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);
		Invoke ("CloseAllPopup", 0.25f);
	}

	public void BtnClosePanelBooster ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);
		Invoke ("CanvasDefaultCondition", 0.25f);
	}

	public void BtnLoginFacebook ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		PopUpLogin.SetActive (true);

		//UIPopupAdditionalUtility.Instance.ShowLoadingPopup ();

		//Utilities.SocialManager.Instance.LoginFB(ConditionBtnFacebookIsConnect);
	}

	public void BtnCredit ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		Invoke("OpenPopupCredit", 0.25f);
	}

	public void BtnCreditClose ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);

		Invoke ("OpenPopupOption", 0.25f);
	}

	public void BtnNews ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		Invoke("OpenPopupNews", 0.25f);
	}

	public void BtnRestorePurchase()
	{
		//TODO: restore purchase here
	}

	// SFX BGM
	public void SwitchTogSFX ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		UtilityChangeSFX (toggSFX.isOn);
		LabelChangeToggle (toggSFX.isOn , txtStatusSFX);
	}

	public void SwitchTogBGM ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		UtilityChangeBGM (toggBGM.isOn);
		LabelChangeToggle (toggBGM.isOn , txtStatusBGM);
	}

	// CONTROLLER
	public void SwitchTogCtrJoystick ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		if (toggContrJoystick.isOn)
		{
			EssentialData.Instance.SaveControllerType (1);
		}
	}
	public void SwitchTogCtrTouch ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		if (toggContrTouch.isOn)
		{
			EssentialData.Instance.SaveControllerType (2);
		}
	}

	// dropdown
	public void SwitchDropdown ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		UtilityChangeLanguage (dropLanguage);
	}

	// nickame
	public void BtnInputFValidateNick ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		Invoke ("ExecuteBtnDoneNickname", 0.25f);
	}

	void ExecuteBtnDoneNickname ()
	{
		namePlayer = playerName.text;

		if (string.IsNullOrEmpty(namePlayer))
		{
			warningText.gameObject.SetActive(true);
			WriteWarningText (LanguageManager.Instance.GetMessage ("TXT0003")); // "Please Fill Player Name"
			return;
		}
		else
		{
			if (namePlayer.Equals(EssentialData.Instance.PlayerDisplayName))
			{
				CloseAllPopup ();
			}
			else
			{
				//Name Has Been changed. Hit API to CHECK name if avaiable.
				ValidateNewNickName ();
			}
		}
	}

	public void BtnConfirmExitQuit ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);
		Invoke ("ExecuteConfirmExitQuit", 0.25f);
	}

	public void BtnConfirmExitCancel ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
		Invoke ("CloseAllPopup", 0.25f);
	}

	#endregion

	#region POPUP FUNCTION
	void ExecuteConfirmExitQuit ()
	{
		Application.Quit();
	}

	void EnableBtnPlay ()
	{
		btnPlay.interactable = true;
	}

	void CloseAllPopup ()
	{
		EssentialData.popupState = EnumData.PopupState.off;

		popupOption.SetActive (false);
		popupNickname.SetActive (false);
//		popupCredit.SetActive (false);
		popupExitGame.SetActive (false);
	}

	// POP NICKNAME
	void ResetPopupNickname ()
	{
		imageFrameNick.sprite = spriteNickNormal;
		warningText.text = "";
		displayNickname.text = LanguageManager.Instance.GetMessage ("TXT0002");
		playerName.text = "";

		// There is a bug with charater limit in inputfield
		playerName.characterLimit = 11;
		playerName.text = EssentialData.Instance.PlayerDisplayName;
		playerName.characterLimit = 10;
	}
		
	void ChangeDisplayNickname ()
	{
		if (!string.IsNullOrEmpty (EssentialData.Instance.PlayerDisplayName))
		{
			displayNickname.text = playerName.text;
		}
	}

	void ValidateNewNickName ()
	{
		APIChangeName(namePlayer);
	}

	public void WriteWarningText (string textWarning)
	{
		warningText.text = textWarning;
		imageFrameNick.sprite = spriteNickWarning;
	}
	#endregion

	#region POP OPTION
	void LabelChangeToggle (bool status, Text label)
	{
		label.text = (status) ? LanguageManager.Instance.GetMessage ("TOG0006") : LanguageManager.Instance.GetMessage ("TOG0007");					// ON / OFF : multi language
	}

	void UtilityChangeSFX (bool isTrueSFX)
	{
		SoundUtility.Instance.SetSettingSFX(isTrueSFX);
	}
	void UtilityChangeBGM (bool isTrueBGM)
	{
		SoundUtility.Instance.SetSettingBGM(isTrueBGM);
	}

	void SetSoundLocalData ()
	{
		toggSFX.isOn = EssentialData.Instance.LoadSettingSFX (); // TRUE NYALA
		toggBGM.isOn = EssentialData.Instance.LoadSettingBGM ();
	}

	void SetControllerLocalData ()
	{
		if (EssentialData.Instance.LoadControllerType() == 1)
		{
			toggContrJoystick.isOn = true;
		}
		else
		{
			toggContrTouch.isOn = true;
		}
	}

	#endregion

	#region ANIMATE BOOSTER

	public void CheckBoosterEquipped ()
	{
		double nowTick = (DateTime.UtcNow - new DateTime (1970, 1, 1)).TotalMilliseconds;
		timeBuff = EssentialData.Instance.PlayerData.endBuffTime - nowTick;
//		Debug.Log ("nowTick:  " + nowTick + " endBuffTime:  " + EssentialData.Instance.PlayerData.endBuffTime + " timeBuff: " + timeBuff);

		if (timeBuff < 0)
		{
			SendBoosterisNone ();
		}
		else
		{
			listBuff = ShopDataManager.Instance.GetPoerUpData();

			int idx = listBuff.FindIndex (m => m.buffCode == EssentialData.Instance.PlayerData.equippedBuff);
	//		Debug.Log ("idx = " + idx + " " +  EssentialData.Instance.PlayerData.equippedBuff + EssentialData.Instance.PlayerData.equippedBuff);

			if (idx > -1)	// more than -1
			{
				buffScript.ChangeImageBothEquip (listBuff [idx]);

				AnimateTimer ();
			}
		}
	}

	void AnimateTimer ()
	{	
		TimeSpan tspan = TimeSpan.FromMilliseconds (timeBuff);
		float totalSec = (float)tspan.TotalSeconds; 
		remainSeconds = Mathf.FloorToInt(totalSec);

		if (frameEquipped.gameObject.activeSelf)
		{
			Invoke("ChangePerLoop", 1);					
		}
	}

	void ChangePerLoop ()
	{
		remainSeconds --;

		if (remainSeconds > 0)
		{
			int minutes = Mathf.FloorToInt(remainSeconds / 60);
			int seconds = Mathf.FloorToInt(remainSeconds - minutes * 60);

			string times = "<b>" + minutes + "</b>m <b>" + seconds + "</b>s";
			UpdateTextTimer (times);

			if (frameEquipped.gameObject.activeSelf)
			{
				Invoke("ChangePerLoop", 1);			
			}
		}
		else if (remainSeconds == 5)
		{
			CancelInvoke("ChangePerLoop");
			AnimateTimer ();
		}
		else if (remainSeconds == 0)
		{
			CancelInvoke("ChangePerLoop");
			UIPopupAdditionalUtility.Instance.ShowLoadingPopup ();

			SendBoosterisNone ();
		}
	}

	void SendBoosterisNone ()
	{
		string apiName = "change_equip";
		string[] field = { "user_id", "secret_key", "buff" };
		string[] value = {
			EssentialData.Instance.PlayerData.userId,
			EssentialData.Instance.PlayerData.secretKey,
			"none"
		};
		BackEndConnect.Instance.SendRequestToServer (CBResetBuff, apiName, field, value, 3);
	}
	void CBResetBuff (APIResponse response)
	{
		Debug.Log(response.rawData);
		if (!response.isError)
		{
			EssentialData.Instance.PlayerData.equippedBuff = "none";
			EssentialData.Instance.PlayerData.startBuffTime = 0;
			EssentialData.Instance.PlayerData.endBuffTime = 0;

			CancelInvoke("ChangePerLoop");
//			CheckBoosterEquipped ();

			buffScript.isBoosterIsInUse (false);
			isBoosterIsInUse (false);
		}

		UIPopupAdditionalUtility.Instance.HideLoadingPopup ();
	}

	void UpdateTextTimer (string times)
	{
		UpdateSelectedBuffText (times);
		buffScript.UpdateTextPseudo (times);
	}

	#endregion

	#region DROPDOWN
	void ResetDropdown ()
	{
		dropLanguage.options.Clear ();
	}

	void UpdateDropdownList ()
	{
		listLanguageDropdown = LanguageManager.Instance.LoadListLanguageData();
	}

	void SetDropdownList ()
	{
		ResetDropdown ();

		string tempString = "";

		for (int i = 0 ; i < listLanguageDropdown.Count; i++)
		{
			tempString = LanguageManager.Instance.GetMessage ("LAN000" + listLanguageDropdown[i].languageId);

			dropLanguage.options.Add (new Dropdown.OptionData (tempString));
		}
	}

	void ShowCurrentLanguageSelected ()
	{
		int indexOfCurrId = LanguageManager.Instance.GetIndexOfSavedLanguage();
			
		dropLanguage.captionText.text = dropLanguage.options[indexOfCurrId].text;
		dropLanguage.value = indexOfCurrId;
//		Debug.Log("set " +dropLanguage.value);
	}

	void UtilityChangeLanguage (Dropdown dropData)
	{
//		Debug.Log ("index Dropdon [" + dropData.value + "] - " + listLanguageDropdown[dropData.value].languageId
//			+ " - " + listLanguageDropdown[dropData.value].languageName + " - " + listLanguageDropdown[dropData.value].languageCode);
		
		int currIdx 	= LanguageManager.Instance.GetIndexOfSavedLanguage();
		int selectedIdx = dropData.value;

		if (popupOption.activeSelf)
		{
			if (currIdx != selectedIdx)
			{
				UIPopupAdditionalUtility.Instance.InitPopNoticeNormal (LanguageManager.Instance.GetMessage ("POP0011") 
					+ listLanguageDropdown[dropData.value].languageName +  "?" , ExecuteConfirmChangeLanguage);
			}
		}

	}

	void ExecuteConfirmChangeLanguage ()
	{
		UIPopupAdditionalUtility.Instance.ShowLoadingPopup();

		int selectedlangId = listLanguageDropdown[dropLanguage.value].languageId;
		EssentialData.Instance.SaveLanguageId (selectedlangId);

		LanguageManager.Instance.LoadLanguageDataJsonInRuntime();

		SceneManager.LoadScene("MainMenu");
	}


	#endregion

	#region API
	void APIChangeName(string chosenName)
	{
		UIPopupAdditionalUtility.Instance.ShowLoadingPopup();

		string apiName = "change_name";
		string[] fieldName = {APITag.userId,APITag.secretKey,APITag.displayName};
		string[] input = {EssentialData.Instance.PlayerData.userId, EssentialData.Instance.PlayerData.secretKey, 
			chosenName};
		int totalInput = 3;
		BackEndConnect.Instance.SendRequestToServer (CBChangeName,apiName,fieldName,input,totalInput);
	}

	void CBChangeName(APIResponse response)
	{
		Debug.Log("Change name " + response.rawData);
		if (!response.isError)
		{
			EssentialData.Instance.PlayerDisplayName = playerName.text;

			UIPopupAdditionalUtility.Instance.HideLoadingPopup();

			ChangeDisplayNickname ();
			CloseAllPopup ();
		}
		else
		{
			UIPopupAdditionalUtility.Instance.HideLoadingPopup();

			WriteWarningText (LanguageManager.Instance.GetMessage ("TXT0005"));
			// Sorry, this Nickname Already Exist"
		}
	}

	void APIGetUserData()
	{
		Debug.Log ("get user data called in main menu");
		string apiName = "get_user_data";
		string[] fieldName = {APITag.userId,APITag.secretKey};
		string[] input = {EssentialData.Instance.PlayerData.userId, EssentialData.Instance.PlayerData.secretKey};
		int totalInput = 2;
		BackEndConnect.Instance.SendRequestToServer (CBGetUserData,apiName,fieldName,input,totalInput);
	}

	void CBGetUserData(APIResponse response)
	{
		Debug.Log("Get User Data " + response.rawData);
		if (!response.isError)
		{
			PlayerData player = new PlayerData(response.data);
			EssentialData.Instance.PlayerData = player;

			UpperMenuGold.Instance.SetGold (EssentialData.Instance.PlayerData.gold);
		}
		else
		{
			Debug.Log("Fail get user data " + response.errorMessage);
		}
	}

	#endregion

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			PhisicalButton ();
		}
	}

	#region BACK BUTTON
	public void PhisicalButton ()
	{
//		Debug.Log ("b4: panel " + EssentialData.panelState + ", pop "+ EssentialData.popupState);

		if (EssentialData.suddenState == EnumData.SuddenPopupState.off)
		{
			if (EssentialData.popupState == EnumData.PopupState.on
				&& EssentialData.panelState == EnumData.PanelState.off)
			{
				SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);

				if (popupCredit.activeSelf)
				{
					BtnCreditClose ();
				}
				else if (rateScript.popRateUs.activeSelf)
				{
					rateScript.DeclineRatingNow ();
				}
				else if (newsScript.popupNews.activeSelf)
				{
					newsScript.HidePopupNews ();
				}
				else if (dailyScript.popupDailyLogin.activeSelf)
				{
					if (dailyScript.isShowingReward)
					{
						dailyScript.BtnCloseDailyLoginPreview();
					}
					else
					{
						dailyScript.HidePopupDailyLogin();
					}
				}
				else
				{
					CloseAllPopup ();
				}
			}
			// IF PANEL (Booster, leaderboard, achievement, shop is opened)
			else if (EssentialData.panelState == EnumData.PanelState.on)
			{
				SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);

				if (EssentialData.popupState == EnumData.PopupState.on)
				{
					buffScript.popConfirmScript.ClosePopupConfirm ();
				}
				else
				{
					CanvasDefaultCondition ();
				}
			}
			else // if no pop / panel on. open popup exit.
			{
				SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
				OpenPopupExitGame ();
			}
		}
		else
		{
			if (EssentialData.popupState == EnumData.PopupState.on)
			{
				SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);
				UIPopupAdditionalUtility.Instance.HidePopupExitgame ();
			}
			else
			{
				Debug.Log ("open canvas additional pop quit, EssentialData.suddenState " + EssentialData.suddenState);
				SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
				UIPopupAdditionalUtility.Instance.ShowPopupExitgame ();
			}
		}
//		Debug.Log ("after: panel " + EssentialData.panelState + ", pop "+ EssentialData.popupState);
	}
	#endregion
}
	
