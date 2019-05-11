using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopGoldItem : MonoBehaviour 
{
	#region VARAIBLE
	public Sprite imageSelected;
	public Sprite imageUnselected;

	public Image itemBackground;
	public Image itemGlow;
	public Image itemImage;

	public Text goldName;
	public Text goldPrice;
	public Text txtBtnBuy;

	public Toggle toggItem;

	public GameObject iconShell;
	public Transform parentSpineUIObj;

	[SerializeField]
	private UIShopPanelController panelScript;

	[SerializeField]
	private ShopGoldData goldData;
	#endregion

	void Start () 
	{
		panelScript = UIShopController.Instance.popupShopScript;
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

	public void SetItemData (ShopGoldData data) // data item buff
	{
		DataDefault ();
		SetTextBtnBuy ();
		goldData = data;

		goldName.text = goldData.goldName;

		string goldPriceString = goldData.goldPrice.ToString() + " " + EssentialData.TAG_CURRENCY;
		goldPrice.text = goldPriceString;

		if (goldData.isSprite)
		{
			SetImageItem (goldData.spriteIAP);
		}
		else
		{
			Object prefab = AssetManager.Instance.GetPrefabByKeyword(goldData.codeIAP);
			GameObject go = Instantiate(prefab) as GameObject;

			go.transform.SetParent(parentSpineUIObj);
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
			go.transform.localScale = Vector3.one;
		}
	}

	#endregion

	#region SET UI
	void DataDefault ()
	{
		goldName.text = "";
		goldPrice.text = "";
		itemImage.gameObject.SetActive (false);
//		iconShell.SetActive (false);
	}

	public void SetImageItem (Sprite sprite)
	{
		itemImage.gameObject.SetActive (true);
		itemImage.sprite = sprite;
	}

	public void	SetTextBtnBuy ()
	{
		LanguageManager.Instance.SetMessageToText(txtBtnBuy, "BTN0009");
	}

	#endregion

	#region TOOGLE
	public void ItemSelected ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		ChangeItemFrame ();

		if (toggItem.isOn)
		{
			panelScript.ItemGoldSelected (goldData);
		}
	}

	public void ItemBtnBuyPress ()
	{
		if (toggItem.isOn)
		{
			ItemSelected ();
		}
		else
		{
			toggItem.isOn = true;
		}
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
