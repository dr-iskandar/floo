using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using ProjectMiniJSON;
using System;

public class UIShopPanelController : MonoBehaviour 
{
	#region VARIABLES
	public UIPopupConfirmBuy popConfirmScript;
	public UIPopupShopColor popSkinColor;

	public GameObject parentShopSkin;
	public GameObject parentShopGold;
	public GameObject prefabShopSkin;
	public GameObject prefabShopGold;

	public ScrollRect scrollRectSkin;
	public ScrollRect scrollRectGold;
	// SKIN SELECTED
	public GameObject panelSkinSelected;
	public GameObject frameSelectedMaxSkin;
	public GameObject btnSelectedSkinUse;
	public GameObject btnSelectedSkinBuy;
	public GameObject btnSelectedSkinColor;

	public Image selectedSkinImageMin;
	public Image selectedSkinImageMax;

	public Text txtSelectedSkinInuse;
	public Text txtSelectedSkinName;
	public Text txtSelectedSkinDesc;

	[SerializeField]
	private List <ShopSkinData> listSkin = new List <ShopSkinData>();
	[SerializeField]
	private List <ShopGoldData> listGold = new List <ShopGoldData>();
	[SerializeField]
	private ShopSkinData selectedSkin;
	[SerializeField]
	private ShopGoldData selectedGold;

	// Spawn
	private int count;
	private List <GameObject> listCreatedSkin = new List<GameObject>();
	private List <GameObject> listCreatedGold = new List<GameObject>();

	[SerializeField]
	private bool isListUpdatedSkin;
	[SerializeField]
	private bool isListUpdatedGold;
	#endregion

	void Start()
	{
		/*UM_InAppPurchaseManager.Client.OnServiceConnected += OnBillingConnectFinishedAction;
		UM_InAppPurchaseManager.Client.OnPurchaseFinished += OnPurchaseFlowFinishedAction;
		UM_InAppPurchaseManager.Client.Connect ();*/
	}

	void OnDestroy()
	{
		/*UM_InAppPurchaseManager.Client.OnServiceConnected -= OnBillingConnectFinishedAction;
		UM_InAppPurchaseManager.Client.OnPurchaseFinished -= OnPurchaseFlowFinishedAction;
	*/}

	#region INIT
	public void InitPanelRefresh ()
	{
		isListUpdatedSkin = false;
		isListUpdatedGold = false;

		selectedSkin = new ShopSkinData ();

		DefaultPanelCondition ();
		DefaultSkinSelectedCondition ();
		UpperMenuGold.Instance.SetGold (EssentialData.Instance.PlayerData.gold);
	}

	public void InitPanelSkin ()
	{
		InitPanelRefresh ();

		if (!isListUpdatedSkin)
		{
			GeneratePrefabSkin ();
//			Debug.Log ("init skin");
		}
	}

	public void InitPanelGold ()
	{
		InitPanelRefresh ();

		if (!isListUpdatedGold)
		{
			GeneratePrefabGold ();
//			Debug.Log ("init gold");
		}
	}


	#endregion

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

	void GeneratePrefabGold ()
	{
		ClearList (listCreatedGold);

		listGold = ShopDataManager.Instance.GetGoldData ();

		for (int i = 0; i < listGold.Count; i++)
		{
			if (!EssentialData.Instance.PlayerData.noAds || !(listGold[i].codeIAP.Contains("no_ads"))) {
				GameObject go = Instantiate (prefabShopGold) as GameObject;
				ShopGoldItem goldPrefab = go.GetComponent <ShopGoldItem>();

				goldPrefab.SetParentToggleGrup (parentShopGold);
				goldPrefab.SetItemData (listGold[i]);

				listCreatedGold.Add (go);
			}
		}

		isListUpdatedGold = true;
	}

	void GeneratePrefabSkin ()
	{
		ClearList (listCreatedSkin);

		listSkin = ShopDataManager.Instance.GetSkinData();

		for (int i = listSkin.Count-1; i >= 0; i--)
		{
			GameObject go = Instantiate (prefabShopSkin) as GameObject;
			ShopSkinItem skinPrefab = go.GetComponent <ShopSkinItem>();

			skinPrefab.SetParentToggleGrup (parentShopSkin);

			if (listSkin[i] != null)
			{
				skinPrefab.SetItemData (listSkin[i]);
			}
		
			listCreatedSkin.Add (go);
		}

		isListUpdatedSkin = true;
	}

	#endregion

	#region BUTTON
	public void BtnPanelSkinUse () // pure button
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		Invoke ("ExecuteBtnPanelSkinUse", 0.25f);
	}

	public void BtnPanelSkinBuy () // pure button 
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		Invoke ("ExecuteBtnPanelSkinBuy", 0.25f);
	}

	public void BtnPanelSkinBuy (ShopSkinData data) // function not button
	{
		selectedSkin = data;

		Invoke ("ExecuteBtnPanelSkinBuy", 0.25f);
	}

	public void BtnPanelSkinColor () // pure button 
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		Invoke ("ExecuteBtnPanelSkinColor", 0.25f);
	}

	#endregion

	#region BUTTON FUNCT
	void ExecuteBtnPanelSkinUse ()
	{
		if (!string.IsNullOrEmpty(selectedSkin.skinCode))
		{
			Debug.Log ("USE " + selectedSkin.skinName);
			ProcessChangeSkin ();
		}
	}

	void ExecuteBtnPanelSkinBuy ()
	{
		if (!string.IsNullOrEmpty(selectedSkin.skinCode))
		{
			Debug.Log ("BUY " + selectedSkin.skinName);
			popConfirmScript.InitPopConfirm (selectedSkin, ProcessBuySkin);
		}
	}

	void ExecuteBtnPanelSkinColor ()
	{
		if (!string.IsNullOrEmpty(selectedSkin.skinCode))
		{
			Debug.Log ("selectedSkin " + selectedSkin.skinCode);

			popSkinColor.InitPopupColor (selectedSkin);
		}
	}
	#endregion

	#region TOOGLE SELECTED
	public void ItemGoldSelected (ShopGoldData data)
	{
		selectedGold = data;

		if (!string.IsNullOrEmpty(selectedGold.skuId))
		{
			Debug.Log ("Gold open pop up. " + selectedGold.goldName);

			popConfirmScript.InitPopConfirm (selectedGold, ProcessBuyGold);
		}
	}

	// SKIN 
	public void ItemSkinSelected (ShopSkinData data)
	{
		DefaultBtnSelected ();

		selectedSkin = data;

		if (!string.IsNullOrEmpty(selectedSkin.skinCode))
		{
			panelSkinSelected.SetActive (true);

			frameSelectedMaxSkin.SetActive (true);

			txtSelectedSkinName.text = data.skinName;
//			txtSelectedSkinDesc.text = data.skinDescription; // revisi 0.6 - desc is gone.

			SetSelectedSkinImageMin (data.spriteMinLevel);
			SetSelectedSkinImageMax (data.spriteMaxLevel);

			if (data.isInUse)
			{
//				txtSelectedSkinInuse.gameObject.SetActive (true);
				btnSelectedSkinColor.SetActive (true);
			}
			else
			{
//				txtSelectedSkinInuse.gameObject.SetActive (false);

				if (data.isDataBought) // already bought
				{
					btnSelectedSkinUse.SetActive (true);
					btnSelectedSkinColor.SetActive (true);
				}
				else
				{
					btnSelectedSkinBuy.SetActive (true);
				}
			}

		}
	}

	public void SetSelectedSkinImageMin (Sprite spriteImageMin)
	{
		selectedSkinImageMin.sprite = spriteImageMin;
	}

	public void SetSelectedSkinImageMax (Sprite spriteImageMax)
	{
		selectedSkinImageMax.sprite = spriteImageMax;
	}

	#endregion

	#region UI FUNCT
	void DefaultPanelCondition ()
	{
		scrollRectGold.verticalNormalizedPosition = 1.0f;
		scrollRectSkin.verticalNormalizedPosition = 1.0f;
	}

	// skin selected
	void DefaultSkinSelectedCondition ()
	{
		panelSkinSelected.SetActive (false);
	}

	void DefaultBtnSelected ()
	{
		btnSelectedSkinUse.SetActive (false);
		btnSelectedSkinColor.SetActive (false);
		btnSelectedSkinBuy.SetActive (false);
	}

	#endregion

	#region FUNCT API Execute BUY
	// GOLD 
	void ProcessBuyGold ()
	{
		if (!string.IsNullOrEmpty (selectedGold.skuId))
		{
			UIPopupAdditionalUtility.Instance.ShowLoadingPopup ();
			Debug.Log ("CONFRIM BUY GOLD with API & IAP " + selectedGold.skuId);

//			UM_InAppPurchaseManager.Client.OnServiceConnected += OnBillingConnectFinishedAction;
//			UM_InAppPurchaseManager.Client.OnPurchaseFinished += OnPurchaseFlowFinishedAction;
//			UM_InAppPurchaseManager.Client.Connect ();

			//UM_InAppPurchaseManager.Client.Purchase (selectedGold.skuId);
		}
	}

	// SKIN 
	void ProcessBuySkin ()
	{
		if (!string.IsNullOrEmpty (selectedSkin.skinId))
		{
			UIPopupAdditionalUtility.Instance.ShowLoadingPopup ();

			string apiName = "buy_item";
			string[] field = { "user_id", "secret_key", "item_id" };
			string[] value = { EssentialData.Instance.PlayerData.userId, EssentialData.Instance.PlayerData.secretKey, selectedSkin.skinId};
			BackEndConnect.Instance.SendRequestToServer (CBBuySkin, apiName, field, value, 3);
		}
	}

	void CBBuySkin(APIResponse response)
	{
		Debug.Log(response.rawData);
		if (!response.isError)
		{
			var message = response.data ["buy_status"].ToString();
			if (message.Equals ("SUCCESS"))
			{
	//			var jsonData = Json.Deserialize(response.data) as Dictionary<string,object>;
				var data = response.data["user_data"] as Dictionary<string,object>;
				PlayerData player = new PlayerData(data);
				EssentialData.Instance.PlayerData = player;

				UIPopupAdditionalUtility.Instance.InitPopNoticeConfirmClose (LanguageManager.Instance.GetMessage("POP0007"));
				// Transaction successful
				InitPanelSkin ();
			}
			else if(message.Equals("INSUFFECIENT"))
			{
				UIPopupAdditionalUtility.Instance.InitPopGoldInsuffecient ();

			}
			else if(message.Equals("FAILED"))
			{
				UIPopupAdditionalUtility.Instance.InitPopNoticeConfirmClose (LanguageManager.Instance.GetMessage("GBL0003"));
				// Something goes wrong with Server.
			}

			popConfirmScript.ClosePopupConfirm ();
			UIPopupAdditionalUtility.Instance.HideLoadingPopup ();
		}
		else
		{
			Debug.Log ("error " + response.rawData);
			popConfirmScript.ClosePopupConfirm ();
			UIPopupAdditionalUtility.Instance.HideLoadingPopup ();
					
		}
	}

	void ProcessChangeSkin() 
	{
		if (!string.IsNullOrEmpty (selectedSkin.skinId))
		{
			UIPopupAdditionalUtility.Instance.ShowLoadingPopup ();

			string apiName = "change_skin";
			string[] field = { "user_id", "secret_key", "skin", "color_code" };
			string[] value = { EssentialData.Instance.PlayerData.userId, EssentialData.Instance.PlayerData.secretKey, selectedSkin.skinCode, "0"};
			BackEndConnect.Instance.SendRequestToServer (CBChangeSkin, apiName, field, value, 4);
		}
	}

	void CBChangeSkin(APIResponse response)
	{
		Debug.Log(response.rawData);
		if (!response.isError)
		{
			//			var jsonData = Json.Deserialize(response.data) as Dictionary<string,object>;
			var data = response.data["user_data"] as Dictionary<string,object>;
			PlayerData player = new PlayerData(data);
			EssentialData.Instance.PlayerData = player;

			InitPanelSkin ();
			MainmenuFishColorManager.Instance.CreateMenuFish (EssentialData.Instance.PlayerData.equippedSkin);
			MainmenuFishColorManager.Instance.ResetFishColorChange ();

			popConfirmScript.ClosePopupConfirm ();
		}
	}

	#endregion

	#region IAP
	/*private void OnBillingConnectFinishedAction (UM_BillingConnectionResult result) 
	{
//		UM_InAppPurchaseManager.Client.OnServiceConnected -= OnBillingConnectFinishedAction;

		if(result.isSuccess) 
		{
			Debug.Log("Connected to billing service");
		}
		else 
		{
			Debug.Log("Failed to connect");

			UpperMenuGold.Instance.SetGold (EssentialData.Instance.PlayerData.gold);
			popConfirmScript.ClosePopupConfirm ();
			UIPopupAdditionalUtility.Instance.HideLoadingPopup ();
		}
	}

	private void OnPurchaseFlowFinishedAction (UM_PurchaseResult result) 
	{
//		UM_InAppPurchaseManager.Client.OnPurchaseFinished -= OnPurchaseFlowFinishedAction;
		Debug.Log("PURCHASE FINISH");
		if(result.isSuccess) 
		{			
			Debug.Log(result.Google_PurchaseInfo.OriginalJson);
			Debug.Log(result.Google_PurchaseInfo.Signature);
			string apiName = "buy_item";

			BackEndConnect.Instance.SendVerifyRequestToServer (CBBuyGold,result.Google_PurchaseInfo.OriginalJson,result.Google_PurchaseInfo.Signature,EssentialData.Instance.PlayerData.userId, selectedGold.goldId, EssentialData.Instance.PlayerData.secretKey);
		}
		else 
		{
			Debug.Log("Failed to flow finish");
			UIPopupAdditionalUtility.Instance.InitPopNoticeConfirmClose (LanguageManager.Instance.GetMessage("GBL0003"));

			UpperMenuGold.Instance.SetGold (EssentialData.Instance.PlayerData.gold);
			popConfirmScript.ClosePopupConfirm ();
			UIPopupAdditionalUtility.Instance.HideLoadingPopup ();
		}
	}*/

	void CBBuyGold(APIResponse response)
	{
//		try 
//		{
//			Debug.Log(response.rawData);
//			if (!response.isError)
//			{
//				var data = response.data["buy_status"].ToString ();
//				Debug.Log (data);
//				if(data=="SUCCESS")
//				{
//					Debug.Log("SUCCESS");
//				}
//				else
//				{
//					Debug.Log("fail");
//
//					UpperMenuGold.Instance.SetGold (EssentialData.Instance.PlayerData.gold);
//					popConfirmScript.ClosePopupConfirm ();
//					UIPopupAdditionalUtility.Instance.HideLoadingPopup ();
//				}
//			}
//		} 
//		catch (Exception ex )
//		{
//			
//		}

		APIGetUserData ();
	}

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
		Debug.Log("Get User Data " + response.rawData);
		if (!response.isError)
		{
			PlayerData player = new PlayerData(response.data);
			EssentialData.Instance.PlayerData = player;

			UIPopupAdditionalUtility.Instance.InitPopNoticeConfirmClose (LanguageManager.Instance.GetMessage("POP0007"));
			// Transaction successful

			UpperMenuGold.Instance.SetGold (EssentialData.Instance.PlayerData.gold);

			popConfirmScript.ClosePopupConfirm ();
			UIPopupAdditionalUtility.Instance.HideLoadingPopup ();
		}
		else
		{
			Debug.Log("Fail get user data " + response.errorMessage);

			UIPopupAdditionalUtility.Instance.InitPopNoticeConfirmClose (LanguageManager.Instance.GetMessage("GBL0003"));

			UpperMenuGold.Instance.SetGold (EssentialData.Instance.PlayerData.gold);
			popConfirmScript.ClosePopupConfirm ();
			UIPopupAdditionalUtility.Instance.HideLoadingPopup ();
		}
	}
	#endregion
}
