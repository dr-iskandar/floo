using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpperMenuGold : MonoBehaviour 
{
	private static UpperMenuGold instance;

	public static UpperMenuGold Instance
	{
		get
		{
			return instance;
		}
	}

	public Text txtGoldShell;

	void Awake()
	{
		instance = this;
	}

	#region Set UI
	public void SetGold (long integerLongGold)
	{
		string goldString = integerLongGold.ToString ("N0");
		txtGoldShell.text = goldString;
	}

	#endregion

	#region BUTTON
	public void BtnAddGold ()
	{
//		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		Invoke ("OpenShop", 0.25f);
	}


	void OpenShop ()
	{
		if (UIShopController.Instance.canvasShop.activeSelf)
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
		}
		else //outside
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

			MainMenuController.Instance.OpenPanelShop ();
		}
	}

	#endregion
}
