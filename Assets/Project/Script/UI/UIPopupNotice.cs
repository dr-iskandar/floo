using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPopupNotice : MonoBehaviour
{
	#region VARIABLE
	public Text txtNotice;
	public Text txtBtnOk;
	public GameObject btnClose;
	public GameObject btnOk;

	private System.Action delegateOK; // DELEGATE FUNCTION FOR BUTTON OK
	private System.Action delegateClose; // DELEGATE FUNCTION FOR BUTTON CLOSE / CANCEL
	#endregion

	void Awake ()
	{
		LanguageManager.Instance.SetMessageToText (txtBtnOk ,"BTN0002");
	}

	#region INIT

	public void InitPopNotice (string message, System.Action actOK, System.Action actExit = null)
	{
		btnClose.SetActive (true);
		btnOk.SetActive (true);
		this.gameObject.SetActive (true);

		txtNotice.text = message;
		SetDelegate (actOK, actExit);
	}

	/// <summary>
	/// Init Pop sudden notice. for Maintenance / Disconnect.
	/// </summary>
	public void InitPopSuddenNotice (string message, System.Action actOK)
	{
		btnClose.SetActive (false);
		btnOk.SetActive (true);
		this.gameObject.SetActive (true);

		txtNotice.text = message;
		SetDelegate (actOK);
	}

	/// <summary>
	/// Init Pop NoticeCloseOnly. With only close button no ok button.
	/// </summary>
	public void InitPopNoticeCloseOnly (string message)
	{
		btnClose.SetActive (true);
		btnOk.SetActive (false);
		this.gameObject.SetActive (true);

		txtNotice.text = message;
	}

	void SetDelegate (System.Action actOk, System.Action actCancel = null)
	{
		if (actOk != null)
		{
			delegateOK = actOk;
		}
		if (actCancel != null)
		{
			delegateClose = actCancel;
		}
	}

	#endregion

	#region BUTTON
	public void BtnNoticeOk ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		if(delegateOK != null)
		{
			Invoke ("ExecuteDelegateOk", 0.25f);
		}
	}

	public void BtnNoticeExit ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);

		Invoke ("ClosePopupNotice", 0.25f);

		if(delegateClose != null)
		{
			Invoke ("ExecuteDelegateClose", 0.25f);
		}
	}

	#endregion

	#region UI
	public void ClosePopupNotice ()
	{
		txtNotice.text = "";
		this.gameObject.SetActive (false);
	}

	void ExecuteDelegateOk ()
	{
		delegateOK ();
	}

	void ExecuteDelegateClose ()
	{
		delegateClose ();
	}
	#endregion
}
