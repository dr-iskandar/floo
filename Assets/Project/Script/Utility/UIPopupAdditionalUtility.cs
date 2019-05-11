using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIPopupAdditionalUtility : MonoBehaviour
{
	public UIPopupNotice popNoticeScript;
	public GameObject popupLoading;
	public GameObject popupExitgame;
	public Text txtPopupExitgame;
	public Text txtBtnQuit;
	public Text txtBtnCancel;

	private System.Action delegateOK;

	private static UIPopupAdditionalUtility instance;

	public static UIPopupAdditionalUtility Instance {
		get 
		{
			if (instance == null)
			{
				Object prefabCanvasPop = Resources.Load ("Prefab/CanvasAdditional");
				GameObject go = Instantiate (prefabCanvasPop) as GameObject;

				instance = go.GetComponent <UIPopupAdditionalUtility>();
				DontDestroyOnLoad(go);
			}

			return instance;
		}
	}

	#region LOADING

	public void ShowLoadingPopup ()
	{
		popupLoading.SetActive (true);
	}

	public void HideLoadingPopup ()
	{
		popupLoading.SetActive (false);
	}

	#endregion

	#region EXIT GAME

	public void ShowPopupExitgame ()
	{
		txtPopupExitgame.text = LanguageManager.Instance.GetMessage ("POP0002"); // "Are You sure want to Exit Floo?"
		LanguageManager.Instance.SetMessageToText (txtBtnQuit ,"BTN0005");
		LanguageManager.Instance.SetMessageToText (txtBtnCancel ,"BTN0004");
		popupExitgame.SetActive (true);

		EssentialData.popupState = EnumData.PopupState.on;
	}

	public void HidePopupExitgame ()
	{
		txtPopupExitgame.text = "";
		popupExitgame.SetActive (false);

		EssentialData.popupState = EnumData.PopupState.off;
	}

	public void BtnExitGameConfirm ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);
		Invoke ("ExecuteConfirmExitQuit", 0.25f);
	}

	public void BtnExitGameCancel ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
		Invoke ("HidePopupExitgame", 0.25f);
	}

	void ExecuteConfirmExitQuit ()
	{
		Application.Quit();
	}
	#endregion

	// FOR NORMAL USE. WITH BUTTON CLOSE. USE THIS FUNCT
	// NEED FUNCTION delegate OK
	// PopNoticeScript.InitPopNotice (string message, delegateOK, delegateCloseCancel);
	#region NORMAL NOTICE
	public void InitPopNoticeNormal (string message, System.Action actOk = null)
	{
		LanguageManager.Instance.SetMessageToText (popNoticeScript.txtBtnOk,"BTN0002");

		popNoticeScript.InitPopNotice (message, BtnNoticeConfirmClose);

		if (actOk != null)
		{
			delegateOK = actOk;
		}

		EssentialData.popupState = EnumData.PopupState.on;
	}

	public void InitPopNoticeConfirmClose (string message, System.Action actOk = null)
	{
		LanguageManager.Instance.SetMessageToText (popNoticeScript.txtBtnOk,"BTN0002");

		popNoticeScript.InitPopSuddenNotice (message, BtnNoticeConfirmClose);

		if (actOk != null)
		{
			delegateOK = actOk;
		}

		EssentialData.popupState = EnumData.PopupState.on;
	}

	public void InitPopNoticeCloseOnly (string message)
	{
		popNoticeScript.InitPopNoticeCloseOnly (message);

		EssentialData.popupState = EnumData.PopupState.on;
	}

	#endregion

	#region SPECIFIC NOTICE
	public void InitPopDisconnected ()
	{
		LanguageManager.Instance.SetMessageToText (popNoticeScript.txtBtnOk,"BTN0002");

		string message = LanguageManager.Instance.GetMessage ("GBL0001");
		popNoticeScript.InitPopSuddenNotice (message, BtnDisconnectRetry);

		EssentialData.suddenState = EnumData.SuddenPopupState.on;
	}

	public void InitPopMaintenance ()
	{
		LanguageManager.Instance.SetMessageToText (popNoticeScript.txtBtnOk,"BTN0002");

		string message = LanguageManager.Instance.GetMessage("GBL0002");
		popNoticeScript.InitPopSuddenNotice (message, BtnOkMaintenance);

		EssentialData.suddenState = EnumData.SuddenPopupState.on;
	}

	public void InitPopGoldInsuffecient ()
	{
		LanguageManager.Instance.SetMessageToText (popNoticeScript.txtBtnOk,"BTN0002");

		string message = LanguageManager.Instance.GetMessage("POP0005");
		popNoticeScript.InitPopNotice (message, BtnOkInsuffecient);

		EssentialData.popupState = EnumData.PopupState.on;
	}
	#endregion

	#region DELEGATE FUNCTION
	void BtnNoticeConfirmClose ()
	{
		if(delegateOK != null)
		{
			delegateOK ();
		}

		HideLoadingPopup ();
		ClosePopupNoticeAdditional ();
	}

	void BtnDisconnectRetry () // send api to check connectivity
	{
		APIGetUserData ();
		ShowLoadingPopup ();
	}

	void BtnOkMaintenance ()
	{
		Debug.Log ("OK to exit game?");
		ClosePopupNoticeAdditional ();
		SceneManager.LoadScene("Loading");
	}

	void BtnOkInsuffecient ()
	{		
		UpperMenuGold.Instance.BtnAddGold ();
		ClosePopupNoticeAdditional ();
	}

	#endregion

	#region UI
	void ClosePopupNoticeAdditional ()
	{
		popNoticeScript.ClosePopupNotice();
		EssentialData.popupState = EnumData.PopupState.off;
	}
	#endregion


	#region API
	void APIGetUserData()
	{
		string apiName = "get_user_data";
		string[] fieldName = {APITag.userId,APITag.secretKey};
		string[] input = {EssentialData.Instance.PlayerData.userId, EssentialData.Instance.PlayerData.secretKey};
		int totalInput = 2;
		BackEndConnect.Instance.SendRequestToServer (CBGetUserData,apiName,fieldName,input,totalInput);
	}

	void CBGetUserData(APIResponse response)
	{
		Debug.Log("RETRY : Get User Data " + response.rawData);
		if (!response.isError)
		{
			PlayerData player = new PlayerData(response.data);
			EssentialData.Instance.PlayerData = player;

			UpperMenuGold.Instance.SetGold (EssentialData.Instance.PlayerData.gold);
			ClosePopupNoticeAdditional ();
			HideLoadingPopup ();
		}
		else
		{
			Debug.Log("Fail get user data " + response.errorMessage + " can't connect w/ server");

			HideLoadingPopup ();
		}
	}
	#endregion
}
