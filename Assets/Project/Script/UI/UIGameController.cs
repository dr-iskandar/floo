using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Globalization;

public class UIGameController : MonoBehaviour 
{
	private static UIGameController instance;

	public static UIGameController Instance
	{
		get
		{
			return instance;
		}
	}

	#region VARIABLES

	[Header("Canvas Overlay")]
	public FadingSpineObject spineNOSAnim;
	public GameObject CanvasOverlay;
	public GameObject gamePopUp;
	public Slider loadingbarSlider;

	//controller
	public MovementControl movementCtrl;

	public Text serverPositionText;
	public Text playerExp;
	public Text gameMessage;
	
	public GameObject popupBG;
	public GameObject popupOption;
	public GameObject popupQuitgame;
	public GameObject panelPredatorRed;

	public Text txtPlayerPoint;
	// opt
	public Toggle toggSFX;
	public Toggle toggBGM;
	public Text txtStatusSFX;
	public Text txtStatusBGM;

	[Header("Canvas Camera")]
	public GameObject popupResult;
	public Text txtResultPoint;
	public Text txtResultLevel;
	public Text txtResultName;
	public Text txtResultEat;
	public Text txtResultTime; // time format HH:mm:ss
	public Text txtResultGold; // shell
	public Transform fishLayerParent;

	// NOS
	[Header("NOS")]
	public Button btnNOS;
	public Image imageNOS;

	[Header("Predator Frame")]
	public Material sharkFrameMat;
	public GameObject frameImage;

	private float minFillValueNOS = 0.3f; // 30 /100
	private bool isAllowedNos = false;

	private float valueNosMax = 100;
	private float nosFillValue;
	private float nosFillValueSaved = 0;
	//calculate
	private float valueNosOld;
	private float valueNosNew;
	private float changeValue = 0;
	private float timeLapse = 0;
	private int loop;

	#endregion

	void Start ()
	{
		#if UNITY_EDITOR
		timeLapse = Time.deltaTime * 1f;
		#else
		timeLapse = Time.deltaTime;
		#endif

		CloseAllPop ();
		loadingbarSlider.value = 0;
	}

	void Awake()
	{
		instance = this;

		#if UNITY_EDITOR
		QualitySettings.vSyncCount = 0;  // VSync must be disabled
		Application.targetFrameRate = 30;
		#else
		Application.targetFrameRate = 30;
		#endif

	}

	#region Update UI


	public void UpdatePlayerLevel(int level, float experience)
	{
		playerExp.text = "LEVEL : " + level + " EXP : " + experience;
	}

	public void UpdatePlayerPosition(float px, float py)
	{
		serverPositionText.text = "(x,y):(" + px + ","+ py + ")";
	}

	public void UpdateLoadingBarSlide (float percentage)
	{
		loadingbarSlider.value = percentage;
	}

	public void ShowConnectingMessage()
	{
		gamePopUp.SetActive(true);
		LanguageManager.Instance.SetMessageToText(gameMessage,"LOA0001");
	}

	public void ShowJoinGameMessage()
	{
		gamePopUp.SetActive(true);
		LanguageManager.Instance.SetMessageToText(gameMessage,"LOA0002");
	}

	public void ShowNotificationMessage(string message)
	{
		gamePopUp.SetActive(true);
		gameMessage.text = message;
	}

	public void HideMessage()
	{
		gamePopUp.SetActive(false);
	}

	// additional
	public void UpdatePlayerPoint (int point)
	{
		int diff = point - int.Parse (txtPlayerPoint.text, NumberStyles.AllowThousands) ;
		if (diff > 0) 
		{
			DisplayGameController.Instance.PlayPointAnimation(diff);
		}

		string pointString = point.ToString("N0");
		txtPlayerPoint.text = pointString;
	}

	public void SetResultPlayerPoint ()
	{
		txtResultPoint.text = txtPlayerPoint.text;
	}

	public void SetResultLevel (int level)
	{
		string levelString = level.ToString("N0");
		txtResultLevel.text = levelString;
	}

	public void SetResultName (string name)
	{
		txtResultName.text = name;
	}

	public void SetResultEat (int eat)
	{
		string eatString = eat.ToString("N0");
		txtResultEat.text = eatString;
	}

	public void SetResultTime (string time)
	{
		txtResultTime.text = time;
	}

	public void SetResultGold (int gold)
	{
		string goldString = "+" + gold.ToString("N0");
		txtResultGold.text = goldString;
	}
	#endregion

	#region BUTTON

	public void BtnOption ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		Invoke ("OpenPopOption", 0.25f);
	}

	public void BtnNosActive ()
	{
		// no button effect
		ValidateButtonNos ();
	}

	#endregion

	#region POP BUTTON
	public void BtnClosePopup ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);

		Invoke ("CloseAllPop", 0.25f);
	}

	public void BtnShareResult ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		Invoke ("FunctShareResult", 0.25f);
	}

	public void BtnPlayAgain ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnPlay);
		DisplayGameController.Instance.movementControl.SetPointerZero ();
		ShowConnectingMessage();
		Invoke ("FunctPlayAgain", 0.25f);
	}

	public void BtnQuitGameResult ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);

		FunctQuitGame ();
	}

	public void BtnQuitGameOption ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);

		Invoke ("OpenPopQuitgame", 0.25f);
	}

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

	#endregion

	#region POP CONFIRM
	public void BtnConfirmQuit ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);
		FunctQuitGame ();
	}

	public void BtnConfirmCancel ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		Invoke ("CloseAllPop", 0.25f);
	}

	#endregion

	#region POP
	public void CloseAllPop ()
	{
		EssentialData.panelState = EnumData.PanelState.off;
		EssentialData.popupState = EnumData.PopupState.off;

		popupBG.SetActive (false);

		popupResult.SetActive (false);
		popupOption.SetActive (false);
		popupQuitgame.SetActive (false);
	}

	void OpenPopOption ()
	{
		CloseAllPop ();

		EssentialData.panelState = EnumData.PanelState.on;

		popupBG.SetActive (true);
		popupOption.SetActive (true);

		SetSoundLocalData ();
		LabelChangeToggle (toggBGM.isOn, txtStatusBGM);
		LabelChangeToggle (toggSFX.isOn , txtStatusSFX);
	}

	public void OpenPopResult ()
	{
		SetFishLayerPopupResult ();

		CloseAllPop ();

		SetResultPlayerPoint ();
//		CanvasOverlay.SetActive (false);
		popupResult.SetActive (true);

		EssentialData.popupState = EnumData.PopupState.on;
	}

	void OpenPopQuitgame ()
	{
		CloseAllPop ();

		popupBG.SetActive (true);
		popupQuitgame.SetActive (true);

		EssentialData.panelState = EnumData.PanelState.on;
		EssentialData.popupState = EnumData.PopupState.on;
	}

	public void HidePanelPredator ()
	{
		panelPredatorRed.SetActive (false);
		panelPredatorRed.GetComponent<Animator>().enabled = false;
	}

	public void ShowPanelPredatorRed ()
	{
		//change color to octo frame color
		frameImage.GetComponent<Image>().material = null;
		panelPredatorRed.SetActive (true);
	}

	public void ShowPanelPredatorShark ()
	{
		//change color to shark frame color
		Material frameCpy = new Material(sharkFrameMat);
		frameImage.GetComponent<Image>().material = frameCpy;
		panelPredatorRed.SetActive (true);
	}

	public void AnimatePredatorRed ()
	{
		panelPredatorRed.GetComponent<Animator>().enabled = true;
	}

	#endregion

	#region FUNCTIONs
	void LabelChangeToggle (bool status, Text label)
	{
		label.text = (status) ? LanguageManager.Instance.GetMessage ("TOG0006") : LanguageManager.Instance.GetMessage ("TOG0007");					// ON / OFF : multi language
	}

	void UtilityChangeSFX (bool isTrueSFX)
	{
//		Debug.Log ("tog SFX: " + isTrueSFX);
		SoundUtility.Instance.SetSettingSFX(isTrueSFX);
	}
	void UtilityChangeBGM (bool isTrueBGM)
	{
//		Debug.Log ("tog BGM: " + isTrueBGM);
		SoundUtility.Instance.SetSettingBGM(isTrueBGM);
	}

	void SetSoundLocalData ()
	{
		toggSFX.isOn = EssentialData.Instance.LoadSettingSFX (); // TRUE NYALA
		toggBGM.isOn = EssentialData.Instance.LoadSettingBGM ();

//		Debug.Log ("toggSFX" + toggSFX.isOn + EssentialData.Instance.LoadSettingSFX ());
//		Debug.Log ("toggBGM" + toggBGM.isOn + EssentialData.Instance.LoadSettingBGM ());
	}

	void FunctShareResult ()
	{
		/*if (!Utilities.SocialManager.Instance.IsLoggedInFB())
		{
//			Debug.Log ("Share via FB.. tapi belum login");
			Utilities.SocialManager.Instance.LoginFB(FunctShareResult);
		}
		else
		{
//			Debug.Log ("Share via FB.. ");
			Utilities.SocialManager.Instance.ShareFB("I have score " + txtPlayerPoint.text);
		}*/
	}

	public bool isPlayAgainAlreadyPressed = false;
	void FunctPlayAgain ()
	{
		if (!isPlayAgainAlreadyPressed)
		{
			if (CanvasOverlay.activeSelf)
			{
				spineNOSAnim.HideNosAnimation ();
			}
			else
			{
				spineNOSAnim.CloseNosAnimation ();
			}

			HidePanelPredator ();
//			CanvasOverlay.SetActive (true);
			GameController.Instance.ReJoinGame();

			isPlayAgainAlreadyPressed = true;
		}
	}

	void FunctQuitGame ()
	{
//		CanvasOverlay.SetActive (true);
		GameController.Instance.BackToMenu();
	}

	public void SetFishLayerPopupResult ()
	{
		DisplayGameController.Instance.SetParentPlayerFish (fishLayerParent);
	}

	public Vector3 GetWorldPoint()
	{
		return movementCtrl.worldPoint;
	}

	#endregion

	#region UI NOS
	/// <summary>
	/// Value is around 0 ~ 1.  (1 = bar full blue, 0 = empty)
	/// </summary>
	public void UpdateImageFillValue (float value)
	{
		imageNOS.fillAmount = value;

		if (!DisplayGameController.Instance.GetBoostStatus ())
		{
			if (value <= 0.015f) 
			{
				imageNOS.fillAmount = value = 0;
			}

			if (CanvasOverlay.activeSelf)
			{
				spineNOSAnim.HideNosAnimation();
			}
			else
			{
				spineNOSAnim.CloseNosAnimation ();
			}
		}

//		Debug.Log ("value " + value + " fill: " +imageNOS.fillAmount);
	}

	void ValidateButtonNos ()
	{
//		Debug.Log ("NOS pressed. (fillAmount < minValue)=> (" + imageNOS.fillAmount +
//			" < " + minFillValueNOS + ") #isAllowedNos: " + isAllowedNos);
		
		// If FILL VALUE is less than VALUE Minimum, NOS will not burst. (can be moved)
		if (imageNOS.fillAmount < minFillValueNOS)
		{
			isAllowedNos = false;
		}
		else
		{
			isAllowedNos = true;
		}

		if (isAllowedNos)
		{
			SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
			ExecuteNOS ();
		}
		else
		{
			SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);
		}
	}

	void ExecuteNOS ()
	{
//		Debug.Log ("Send Boost to Backend");
		string data = GameController.Instance.playerID;
		BackEndConnect.Instance.SendSocket (APITag.socketTagBoost, data);

		spineNOSAnim.ShowNosAnimation ();
	}
	#endregion

	#region ANIMATE NOS
	public void SetNosMaxFill (float nowFillMax)
	{
		valueNosMax = nowFillMax;
	}

	public void SetNosMinValidation (float nowMinFill)
	{
		minFillValueNOS = nowMinFill;
	}

	public void ResetNosToZero ()
	{
		nosFillValueSaved = 1;
		nosFillValue = 1;
		AnimateNos (nosFillValueSaved, nosFillValue);
	}

	/// <summary>
	/// Calculates the nos bar. valueExpCurr (0 ~ MAX) => (0 ~ 100)
	/// </summary>
	public void CalculateNosBar (float valueExpCurr)
	{
		if (valueExpCurr >= 0)
		{
			valueExpCurr = valueNosMax - valueExpCurr;

			nosFillValue = Mathf.InverseLerp(0, valueNosMax, valueExpCurr);

			if (nosFillValue < 0)
			{
				nosFillValue = 0;
			}

			AnimateNos (nosFillValueSaved, nosFillValue);
		}
	}

	void AnimateNos (float savedValue, float newValue)
	{
		valueNosOld = savedValue;
		valueNosNew = newValue;

		loop = 10;

		changeValue = newValue - savedValue;
		changeValue = changeValue / loop;

		if (imageNOS.gameObject.activeSelf)
		{
			Invoke("ChangePerLoop", timeLapse);			
		}
	}

	void ChangePerLoop()
	{
		loop--;

		if (loop > 0)
		{
			valueNosOld += changeValue;

			ApplyNosFillChanges (valueNosOld);

			if(imageNOS.gameObject.activeSelf)
			{
				Invoke("ChangePerLoop", timeLapse);
			}

			nosFillValueSaved = valueNosOld;
//			Debug.Log ("      # " + loop + " (" + valueNosOld  + ")");
		}
		else 
		{
			ApplyNosFillChanges (valueNosNew);

			nosFillValueSaved = valueNosNew;
//			Debug.Log ("      $ " + loop + " (" + valueNosNew + ")");
		}
	}

	void ApplyNosFillChanges (float value)
	{
		value = 1 - value;
		UpdateImageFillValue (value);
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
	// panel : option
	// pop : result, quit
	public void PhisicalButton ()
	{
//		Debug.Log ("b4: panel " + EssentialData.panelState + ", pop "+ EssentialData.popupState);

		if (EssentialData.suddenState == EnumData.SuddenPopupState.off)
		{
			if (EssentialData.popupState == EnumData.PopupState.on && EssentialData.panelState == EnumData.PanelState.off)
			{
				SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);

				BtnQuitGameResult ();
			}
			else if (EssentialData.panelState == EnumData.PanelState.on)
			{
				SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);

				if (EssentialData.popupState == EnumData.PopupState.on) // pop quit game
				{
					OpenPopOption ();
				}
				else
				{
					CloseAllPop ();
				}
			}
			else // if no pop / panel on. open popup exit.
			{
				SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
				OpenPopOption ();
			}
		}
		else
		{
			if (EssentialData.popupState == EnumData.PopupState.on)
			{
				UIPopupAdditionalUtility.Instance.HidePopupExitgame ();
			}
			else
			{
				Debug.Log ("open canvas additional pop quit, EssentialData.suddenState " + EssentialData.suddenState);
				UIPopupAdditionalUtility.Instance.ShowPopupExitgame ();
			}
		}
//		Debug.Log ("after: panel " + EssentialData.panelState + ", pop "+ EssentialData.popupState);
	}
	#endregion

}
