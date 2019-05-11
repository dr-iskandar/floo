using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UIShopController : MonoBehaviour 
{
	private static UIShopController instance;

	public static UIShopController Instance
	{
		get
		{
			return instance;
		}
	}

	#region VARIABLE
	public GameObject canvasShop;
	public UIShopPanelController popupShopScript;

	public Text txtSkin;
	public Text txtGold;
	public Toggle toggSkin;
	public Toggle toggGold;
	public GameObject panelSkin;
	public GameObject panelGold;

	private bool isOnceGold = false;
	private bool isOnceSkin = false;
	#endregion

	void Awake()
	{
		instance = this;
	}

	public void InitShop ()
	{
		SetTabDefault ();
		SwitchTabPanel ();		
	}

	#region UI FUNCT
	public void OpenCanvasShop ()
	{
		canvasShop.SetActive (true);
	}

	public void CloseCanvasShop ()
	{
		canvasShop.SetActive (false);
		isOnceGold = false;
		isOnceSkin = false;
	}

	void ExecuteClose ()
	{
//		CloseCanvasShop ();
		MainMenuController.Instance.CanvasDefaultCondition ();
	}
	#endregion

	#region BUTTON
	public void BackToMain ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);

		Invoke ("ExecuteClose", 0.25f);
	}

	public void SwitchTogGold ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		if (toggGold.isOn)
		{
//			toggGold.isOn = true;
//			Debug.Log ("#pres gold ");
			SwitchTabPanel ();
//			Debug.Log ("#SwitchTogGold: isOnceGold " + isOnceGold + " isOnceSkin " + isOnceSkin );
		}
	}

	public void SwitchTogSkin ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		if (toggSkin.isOn)
		{
//			toggSkin.isOn = true;
//			Debug.Log ("#pres kin ");
			SwitchTabPanel ();
//			Debug.Log ("#SwitchTogSkin: isOnceGold " + isOnceGold + " isOnceSkin " + isOnceSkin );
		}
	}

	#endregion

	#region TOGGLE
	void SetTabDefault ()
	{
		panelGold.SetActive (false);
		panelSkin.SetActive (false);
	}

	void OpenTabGold ()
	{
//		toggGold.transform.SetAsLastSibling ();
//		toggSkin.transform.SetAsFirstSibling ();

		panelGold.SetActive (true);
		panelSkin.SetActive (false);

//		txtGold.color = EssentialData.colorBlueLight;
//		txtSkin.color = Color.white;
	}

	void OpenTabSkin ()
	{
//		toggSkin.transform.SetAsLastSibling ();
//		toggGold.transform.SetAsFirstSibling ();

		panelGold.SetActive (false);
		panelSkin.SetActive (true);

//		txtSkin.color = EssentialData.colorBlueLight;
//		txtGold.color = Color.white;
	}

	void SwitchTabPanel ()
	{
		if (toggGold.isOn && !isOnceGold)
		{
			OpenTabGold ();
			popupShopScript.InitPanelGold ();

			isOnceGold = true;
			isOnceSkin = false;
//			Debug.Log ("- gold: " + toggGold.isOn + " isOnceGold " + isOnceGold + " isOnceSkin " + isOnceSkin );
		}
		else if (toggSkin.isOn && !isOnceSkin)
		{
			OpenTabSkin ();
			popupShopScript.InitPanelSkin ();

			isOnceGold = false;
			isOnceSkin = true;

//			Debug.Log ("- skin: " + toggSkin.isOn + " isOnceGold " + isOnceGold + " isOnceSkin " + isOnceSkin );
		}
	}

	#endregion

}
