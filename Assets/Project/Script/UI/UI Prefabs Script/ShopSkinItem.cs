using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopSkinItem : MonoBehaviour 
{
	#region VARAIBLE
	public Sprite imageSelected;
	public Sprite imageUnselected;

	public Image itemBackground;
	public Image itemGlow;
	public Image itemImage;

	public Text skinName;
	public Text skinPrice;

	public Toggle toggItem;

	public GameObject frameInUse;
	public GameObject iconShell;

	public GameObject btnBuy;
	public Text txtBtnBuy;

	public Text txtFrameInuse;

	private UIShopPanelController panelScript;
	[SerializeField]
	private ShopSkinData skinData;
	#endregion

	void Awake () 
	{
		panelScript = UIShopController.Instance.popupShopScript;

		DataDefault ();
	}

	#region SET DATA
	public void SetParentToggleGrup (GameObject parent)
	{		
		toggItem.group = parent.GetComponent<ToggleGroup>();

		transform.SetParent (parent.transform);
		transform.localPosition = Vector3.zero;
		transform.localEulerAngles = Vector3.zero;
		transform.localScale = Vector3.one;
	}

	void DataDefault ()
	{
		skinName.gameObject.SetActive (false);
		itemImage.gameObject.SetActive (false);

		frameInUse.SetActive (false);
		iconShell.SetActive (false);
		skinPrice.gameObject.SetActive (false);

		SetTextBtnBuy ();
		SetTextFrameInuse ();
	}

	public void ItemIsBought ()
	{
		btnBuy.SetActive (false);
		iconShell.SetActive (false);

		skinPrice.gameObject.SetActive (false);
	}

	public void SetItemData (ShopSkinData data)
	{
		skinData = data;

		skinName.text = skinData.skinName;

		SetImageItem (data.spriteMinLevel);

		skinName.gameObject.SetActive (true);
		itemImage.gameObject.SetActive (true);

		if (!skinData.isDataBought) // is not bought yet
		{
			iconShell.SetActive (skinData.isPayWithGold);

			skinPrice.gameObject.SetActive (true);
			skinPrice.text = skinData.skinPrice.ToString("N0");
		}
		else
		{
			ItemIsBought ();
		}

		SetIFrameInUseActive (skinData.isInUse);

		if (skinData.isInUse)
		{
			SendSkinData ();
		}
	}

	#endregion

	#region SET UI

	public void SetIFrameInUseActive (bool isActive)
	{
		frameInUse.SetActive (isActive);
	}

	public void SetImageItem (Sprite sprite)
	{
		itemImage.sprite = sprite;
	}

	public void	SetTextBtnBuy ()
	{
		LanguageManager.Instance.SetMessageToText(txtBtnBuy, "BTN0009");
	}

	public void	SetTextFrameInuse ()
	{
		LanguageManager.Instance.SetMessageToText(txtFrameInuse, "TXT0009");
	}

	#endregion

	#region TOOGLE
	public void ItemSelected ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		ChangeItemFrame ();

		if (toggItem.isOn)
		{
			SendSkinData ();
		}
	}

	public void ItemBtnBuyPress ()
	{
		toggItem.isOn = true;
//		ItemSelected ();
		panelScript.BtnPanelSkinBuy (skinData);
	}

	void SendSkinData ()
	{
		if(skinData.skinCode == EssentialData.Instance.PlayerData.equippedSkin)
		{
			int codeId = int.Parse(EssentialData.Instance.PlayerData.colorCode);
			MainmenuFishColorManager.Instance.ChangeFishColorPreview (codeId);
		}
		else
		{
			MainmenuFishColorManager.Instance.ResetFishColorPreview ();
		}

		panelScript.ItemSkinSelected (skinData);
	}

	void ChangeItemFrame ()
	{
		if (toggItem.isOn)
		{
			itemBackground.sprite = imageSelected;
		}
		else
		{
			itemBackground.sprite = imageUnselected;
		}
	}

	#endregion

}
