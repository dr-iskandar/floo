using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPopupRateUs : MonoBehaviour 
{
	public GameObject popRateUs;
	public Text txtMessage;
	public Text txtBtnOk;
	public Text txtBtnDecline;

	public void InitPopRateUs ()
	{
		SetText ();
		ShowPopupRateUs ();
	}

	#region UI
	void SetText ()
	{
		DefaultText ();
		txtMessage.text = LanguageManager.Instance.GetMessage("POP0009");
		LanguageManager.Instance.SetMessageToText(txtBtnOk,"BTN0017");
		LanguageManager.Instance.SetMessageToText(txtBtnDecline,"BTN0018");
	}

	void DefaultText ()
	{
		txtMessage.text = "";
		txtBtnOk.text = "";
		txtBtnDecline.text = "";
	}

	void ShowPopupRateUs ()
	{
		popRateUs.gameObject.SetActive (true);

		EssentialData.popupState = EnumData.PopupState.on;
	}

	void HidePopupRateUs ()
	{
		DefaultText ();
		popRateUs.gameObject.SetActive (false);

		EssentialData.popupState = EnumData.PopupState.off;
	}
	#endregion

	#region BUTTON
	public void BtnRateOk ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
		Invoke ("ExecuteRatingNow", 0.25f);
	}

	public void BtnRateDecline ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);
		Invoke ("DeclineRatingNow", 0.25f);
	}
	#endregion

	#region FUNCT
	void ExecuteRatingNow ()
	{
		SetStatusGoToRating (true);
		SetStatusDeclineRating (false);

		HidePopupRateUs ();

		UIPopupAdditionalUtility.Instance.InitPopNoticeCloseOnly (LanguageManager.Instance.GetMessage ("POP0010"));

		#if UNITY_ANDROID
		EssentialData.Instance.GoToLink (EssentialData.LINK_STORE_ANDROID);
		#elif UNITY_IPHONE
		EssentialData.Instance.GoToLink (EssentialData.LINK_STORE_IOS);
		#endif
	}

	public void DeclineRatingNow ()
	{
		SetStatusGoToRating (false);
		SetStatusDeclineRating (true);

		HidePopupRateUs ();
	}

	void SetStatusGoToRating (bool isAlreadyRating)
	{
		EssentialData.Instance.SaveStatusAlreadyRating (isAlreadyRating);
	}

	void SetStatusDeclineRating (bool isAlreadyDecline)
	{
		EssentialData.Instance.SaveStatusDeclineRating (isAlreadyDecline);
	}
	#endregion
}
