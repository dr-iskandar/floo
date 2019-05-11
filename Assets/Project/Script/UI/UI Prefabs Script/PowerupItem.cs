using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerupItem : MonoBehaviour
{
	#region VARAIBLE
	public Sprite imageSelected;
	public Sprite imageUnselected;

	public Image itemBackground;
	public Image itemGlow;
	public Image buffImage;

	public Text buffName;
	public Text buffPrice;

	public GameObject buffShellIcon;
	public Toggle toggItem;

	[SerializeField]
	private UIPopupPowerup popupBuffScript;

	[SerializeField]
	private PowerupData buffData;
	#endregion

	void Awake ()
	{
		popupBuffScript = MainMenuController.Instance.buffScript;
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

	public void SetItemData (PowerupData data) // data item buff
	{
		buffData = data;
		DefaultConditionItem ();

		if (!string.IsNullOrEmpty(buffData.powerupId))
		{
			buffName.text = buffData.powerupName;

			SetImageBuff (buffData.spritePowerUp);

//			buffShellIcon.SetActive (true);
			buffPrice.text = buffData.powerupPrice.ToString("N0");
		}
	}

	public void SetImageBuff (Sprite sprite)
	{
		buffImage.sprite = sprite;
		buffImage.gameObject.SetActive (true);
	}

	void DefaultConditionItem ()
	{
		buffName.text = "";
		buffPrice.text = "";
//		buffShellIcon.SetActive (false);
	}

	#endregion

	#region TOOGLE
	public void ItemSelectedPreview ()
	{
		if (!string.IsNullOrEmpty(buffData.powerupId))
		{
			popupBuffScript.BuffSelected (buffData);
		}
	}

	public void ItemSelected ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		ChangeItemFrame ();

		if (toggItem.isOn)
		{
			if (!string.IsNullOrEmpty(buffData.powerupId))
			{
				popupBuffScript.BuffSelected (buffData);
			}
		}
	}

	void ChangeItemFrame ()
	{
		if (toggItem.isOn)
		{
			itemBackground.sprite = imageSelected;
			itemGlow.gameObject.SetActive (true);
		}
		else
		{
			itemBackground.sprite = imageUnselected;
			itemGlow.gameObject.SetActive (false);
		}
	}

	#endregion

}
