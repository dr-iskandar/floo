using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIPopupPowerup : MonoBehaviour 
{
	#region VARIABLE
	public UIPopupConfirmBuy popConfirmScript;

	public ScrollRect scrollRect;
	public GameObject parentGameobject;
	public GameObject powerupPrefab;
	public GameObject panelSelected;
	public Image selectedBuffImage;
	public Text selectedBuffName;
	public Text selectedBuffDesc;

	[Header("Btn Booster Pseudo")]
	public Sprite spriteNoBooster;
	public Sprite spriteBooster;

	public Image pseudoBtnImage;
	public Image pseudoBuffImage;

	public GameObject txtNoBooster;
	public GameObject frameEquipped;
	public Text pseudoBuffText;

	private int count;
	private PowerupData selectedData;
	private List <PowerupData> listBuff = new List<PowerupData>();
	private List <GameObject> listCreatedBuff = new List<GameObject>();
	#endregion

	void Start ()
	{
		HidePaneLSelected();
		GeneratePowerupPrefab ();
//		isBoosterIsInUse (false);
		UpperMenuGold.Instance.SetGold (EssentialData.Instance.PlayerData.gold);
	}

	void OnEnable ()
	{
		DefaultPopupCondition ();
	}

	#region SPAWN BUFF
	void ClearList (List <GameObject> listCreated)
	{		
		count = listCreated.Count;

		for (int i = 0; i < count; i++)
		{
			Destroy (listCreated[i]);
		}

		listCreated.Clear ();
	}

	void GeneratePowerupPrefab ()
	{
		ClearList (listCreatedBuff);

		listBuff = ShopDataManager.Instance.GetPoerUpData();

		for (int i = 0; i < listBuff.Count; i++)
		{
			GameObject go = Instantiate(powerupPrefab) as GameObject;
			PowerupItem buffPrefab = go.GetComponent<PowerupItem>();

			buffPrefab.SetParentToggleGrup (parentGameobject);
			buffPrefab.SetItemData (listBuff[i]);

			if (i == 0)
			{
				buffPrefab.ItemSelectedPreview ();
			}

			listCreatedBuff.Add (go);
		}
	}

	#endregion

	#region BUTTON
	public void BtnUseBuff ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		Invoke ("ExecuteBtnUse", 0.25f);
	}

	void ExecuteBtnUse ()
	{
		if (!string.IsNullOrEmpty (selectedData.powerupId))
		{
			popConfirmScript.InitPopConfirm (selectedData, ProceedConfirmBuff);
		}
	}

	#endregion

	#region UI FUNCT

	public void BuffSelected (PowerupData data)
	{		
		selectedData = data;

		if (!string.IsNullOrEmpty (selectedData.powerupId))
		{
			ShowPanelSelected();

			selectedBuffName.text = data.powerupName;
			selectedBuffDesc.text = data.powerupDescription;
			ChangeSelectedBuffImage (data.spritePowerUp);
		}
	}

	// button pseudo
	public void isBoosterIsInUse (bool isUseBuff)
	{
		if (isUseBuff)
		{
			txtNoBooster.SetActive (false);
			frameEquipped.SetActive (true);
			pseudoBtnImage.sprite = spriteBooster;

			pseudoBuffImage.gameObject.SetActive(true);
		}
		else // NO BOOSTER
		{
			txtNoBooster.SetActive (true);
			frameEquipped.SetActive (false);
			pseudoBtnImage.sprite = spriteNoBooster;

			pseudoBuffImage.gameObject.SetActive(false);
		}
	}

	// BUTTON BUFF IMAGE
	public void ChangeImageBothEquip (PowerupData data)
	{
		isBoosterIsInUse(true);
		ChangePseudoImage (data.spritePowerUp);

		MainMenuController.Instance.isBoosterIsInUse(true);
		MainMenuController.Instance.ChangeSelectedBuffImage (data.spritePowerUp);
	}

	#endregion

	#region UI
	void DefaultPopupCondition ()
	{
		scrollRect.verticalNormalizedPosition = 1.0f;
	}

	void ShowPanelSelected()
	{
		panelSelected.SetActive(true);
	}

	void HidePaneLSelected()
	{
		panelSelected.SetActive(false);
	}

	public void ChangeSelectedBuffImage (Sprite spriteBuff)
	{
		selectedBuffImage.sprite = spriteBuff;
	}

	public void ChangePseudoImage (Sprite selectedBuffSprite)
	{
		pseudoBuffImage.gameObject.SetActive(true);
		pseudoBuffImage.sprite = selectedBuffSprite;
	}

	public void UpdateTextPseudo (string time)
	{
		pseudoBuffText.text = time;
	}

	#endregion

	#region POPUP CONFIRM FUNCTION

	void ProceedConfirmBuff ()
	{
		if (!string.IsNullOrEmpty (selectedData.powerupId))
		{
			UIPopupAdditionalUtility.Instance.ShowLoadingPopup ();

			string apiName = "buy_item";
			string[] field = { "user_id", "secret_key", "item_id" };
			string[] value = { EssentialData.Instance.PlayerData.userId, EssentialData.Instance.PlayerData.secretKey, selectedData.powerupId};
			BackEndConnect.Instance.SendRequestToServer (CBBuyBuff, apiName, field, value, 3);
		}
	}

	void CBBuyBuff(APIResponse response)
	{
		Debug.Log(response.rawData);
		if (!response.isError)
		{
			var message = response.data ["buy_status"].ToString();
			if (message.Equals ("SUCCESS"))
			{
				string apiName = "change_equip";
				string[] field = { "user_id", "secret_key", "buff" };
				string[] value = {
					EssentialData.Instance.PlayerData.userId,
					EssentialData.Instance.PlayerData.secretKey,
					selectedData.buffCode
				};
				BackEndConnect.Instance.SendRequestToServer (CBEquipBuff, apiName, field, value, 3);
			}
			else if(message.Equals("INSUFFECIENT"))
			{
				UIPopupAdditionalUtility.Instance.InitPopGoldInsuffecient ();

			}
			else if(message.Equals("FAILED"))
			{
				UIPopupAdditionalUtility.Instance.InitPopNoticeConfirmClose (LanguageManager.Instance.GetMessage("POP0006"));
			}

			popConfirmScript.ClosePopupConfirm ();
			UIPopupAdditionalUtility.Instance.HideLoadingPopup ();
		}
	}


	void CBEquipBuff(APIResponse response)
	{
		Debug.Log(response.rawData);
		if (!response.isError)
		{
			var data = response.data["user_data"] as Dictionary<string,object>;
			PlayerData player = new PlayerData(data);
			EssentialData.Instance.PlayerData = player;

			UIPopupAdditionalUtility.Instance.InitPopNoticeConfirmClose (LanguageManager.Instance.GetMessage("POP0007"));
			// Transaction successful

			UpperMenuGold.Instance.SetGold (EssentialData.Instance.PlayerData.gold);

			ChangeImageBothEquip (selectedData);
			MainMenuController.Instance.CheckBoosterEquipped ();
		}
	}

	#endregion
}
