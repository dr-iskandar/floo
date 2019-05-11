using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPopupConfirmBuy : MonoBehaviour 
{
	#region VARIABLE
	public GameObject goldShell;
	public Image imageConfirm;
	public Text txtConfirm;
	public Text txtConfirmPrice;
	public Text txtBtnBuyConfirm;
	public Transform parentSpineHolder;

	private string stringPurchase;

	private System.Action delegateBuy; // DELEGATE FUNCTION FOR BUTTON BUY
	private System.Action delegateExit; // DELEGATE FUNCTION FOR BUTTON EXIT
	#endregion

	void Awake () 
	{
		stringPurchase = LanguageManager.Instance.GetMessage ("POP0004");

		LanguageManager.Instance.SetMessageToText (txtBtnBuyConfirm ,"BTN0009");
	}

	#region INIT
	// BUFF
	public void InitPopConfirm (PowerupData buffData, System.Action actBuy, System.Action actExit = null)
	{
		this.gameObject.SetActive (true);

		SetDelegate (actBuy, actExit);

		SetConfirmPopup (buffData);

		EssentialData.popupState = EnumData.PopupState.on;
	}
	// GOLD -- no GOLD SHELL
	public void InitPopConfirm (ShopGoldData goldShopData, System.Action actBuy, System.Action actExit = null)
	{
		this.gameObject.SetActive (true);

		SetDelegate (actBuy, actExit);

		SetConfirmPopup (goldShopData);

		EssentialData.popupState = EnumData.PopupState.on;
	}
	// SKIN
	public void InitPopConfirm (ShopSkinData skinShopData, System.Action actBuy, System.Action actExit = null)
	{
		this.gameObject.SetActive (true);

		SetDelegate (actBuy, actExit);

		SetConfirmPopup (skinShopData);

		EssentialData.popupState = EnumData.PopupState.on;
	}

	#endregion

	#region SET DATA

	void SetConfirmPopup (PowerupData confirmData)
	{
		goldShell.SetActive (true);

		txtConfirm.text = stringPurchase + " " + confirmData.powerupName + "?";

		string goldString = confirmData.powerupPrice.ToString ("N0");
		txtConfirmPrice.text = goldString;

		SetConfirmImage (confirmData.spritePowerUp);
	}
	// GOLD -- no GOLD SHELL
	void SetConfirmPopup (ShopGoldData confirmData)
	{
		goldShell.SetActive (false);

		txtConfirm.text = stringPurchase + " " + confirmData.goldName + "?";

		string goldString = confirmData.goldPrice.ToString() + " " + EssentialData.TAG_CURRENCY;
		txtConfirmPrice.text = goldString;

		if (confirmData.isSprite)
		{
			SetConfirmImage (confirmData.spriteIAP);
		}
		else
		{
			imageConfirm.gameObject.SetActive (false);

			Object prefab = AssetManager.Instance.GetPrefabByKeyword(confirmData.codeIAP);
			GameObject go = Instantiate(prefab) as GameObject;

			go.transform.SetParent(parentSpineHolder);
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
			go.transform.localScale = new Vector3 (1.2f,1.2f,1.2f);
		}
	}
	// SKIN
	void SetConfirmPopup (ShopSkinData confirmData)
	{
		goldShell.SetActive (confirmData.isPayWithGold);

		txtConfirm.text = stringPurchase + " " + confirmData.skinName + "?";

		string goldString = confirmData.skinPrice.ToString ("N0");
		txtConfirmPrice.text = goldString;

		SetConfirmImage (confirmData.spriteMinLevel);
	}

	void SetDelegate (System.Action actBuy, System.Action actExit = null)
	{
		if (actBuy != null)
		{
			delegateBuy = actBuy;
		}
		if (actExit != null)
		{
			delegateExit = actExit;
		}
	}

	#endregion

	#region UI
	public void SetConfirmImage (Sprite sp)
	{
		imageConfirm.gameObject.SetActive (true);
		imageConfirm.sprite = sp;
	}

	void ResetConfirm ()
	{
		goldShell.SetActive (false);
		txtConfirm.text = "";
		txtConfirmPrice.text = "";
		imageConfirm.sprite = null;

		foreach (Transform child in parentSpineHolder)
		{
			Destroy(child.gameObject);
		}
	}
	#endregion

	#region BUTTON
	public void BtnConfrimBuy ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		if(delegateBuy != null)
		{
			Invoke ("ExecuteDelegateBuy", 0.25f);
		}
	}

	public void BtnConfirmExit ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);

		Invoke ("ClosePopupConfirm", 0.25f);

		if(delegateExit != null)
		{
			Invoke ("ExecuteDelegateExit", 0.25f);
		}
	}

	public void ClosePopupConfirm ()
	{
		ResetConfirm ();
		this.gameObject.SetActive (false);
		EssentialData.popupState = EnumData.PopupState.off;
	}
	#endregion

	void ExecuteDelegateBuy ()
	{
		delegateBuy ();
	}

	void ExecuteDelegateExit ()
	{
		delegateExit ();
	}

}
