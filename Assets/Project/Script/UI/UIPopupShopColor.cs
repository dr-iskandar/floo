using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIPopupShopColor : MonoBehaviour 
{
	#region VARIABLES

	public GameObject parentColor;
	public GameObject prefabColor;
	public ScrollRect scrollRectColor;

	private ShopSkinData selectedskin;
//	[SerializeField]
	private HSVData selectedHSVData;
	private List <HSVData> listColor = new List <HSVData>();
	private List <GameObject> listCreatedObj = new List<GameObject>();

	#endregion

	public void InitPopupColor (ShopSkinData data)
	{
		selectedHSVData = new HSVData ();
		selectedskin = data;
		this.gameObject.SetActive (true);

		GeneratePrefabColor ();
	}

	#region SPAWN BUFF

	void ClearList (List <GameObject> listCreated)
	{		
		int count = listCreated.Count;

		for (int i = 0; i < count; i++)
		{
			Destroy (listCreated[i]);
		}

		listCreated.Clear ();
	}

	void GeneratePrefabColor ()
	{
		ClearList (listCreatedObj);

		listColor = DefaultParameterManager.Instance.GetListHSVColorData();

		for (int i = 0; i < listColor.Count; i++)
		{
			if (selectedskin.skinCode == listColor[i].fishSkin)
			{
				GameObject go = Instantiate (prefabColor) as GameObject;
				ColorSelectItem itemPrefab = go.GetComponent <ColorSelectItem>();

				itemPrefab.SetParentToggleGrup (parentColor);
				itemPrefab.SetImageItem (selectedskin.spriteColorSprite);
				itemPrefab.InitColorSelection (listColor[i]);
				itemPrefab.InitDelegateColor (SetSelectingColor);
		
				listCreatedObj.Add (go);
			}
		}
	}

	#endregion

	#region BUTTON
	public void BtnColorDone ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		Invoke ("ProceedColorSelected", 0.25f);
	}

	public void BtnColorClose ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);

		Invoke ("ClosePopup", 0.25f);
	}

	#endregion

	#region FUNT 
	void SetSelectingColor (HSVData data)
	{
		selectedHSVData = data;
//		Debug.Log ("selectedColor : " + selectedHSVData.colorHSVCodeName );

		if (!string.IsNullOrEmpty(selectedHSVData.colorHSVCodeId))
		{
			MainmenuFishColorManager.Instance.StoreFishHSVData (selectedHSVData);
		}
	}

	void ProceedColorSelected ()
	{
		if (!string.IsNullOrEmpty(selectedHSVData.colorHSVCodeId))
		{
			UIPopupAdditionalUtility.Instance.ShowLoadingPopup ();

			string apiName = "change_skin";
			string[] field = { "user_id", "secret_key", "skin", "color_code" };
			string[] value = { EssentialData.Instance.PlayerData.userId, EssentialData.Instance.PlayerData.secretKey, selectedskin.skinCode, selectedHSVData.colorHSVCodeId};
			BackEndConnect.Instance.SendRequestToServer (CBChangeSkin, apiName, field, value, 4);

//			Debug.Log ("-DONE- Proceed Change selected color (" + selectedHSVData.colorHSVCodeId + ") " + selectedHSVData.colorHSVCodeName);
		}
		else
		{
			ClosePopup ();
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

			MainmenuFishColorManager.Instance.CreateMenuFish (EssentialData.Instance.PlayerData.equippedSkin);
			MainmenuFishColorManager.Instance.ExecuteFishColorChange ();
			UIShopController.Instance.popupShopScript.InitPanelSkin ();

			ClosePopup ();
		}
		else
		{
			UIPopupAdditionalUtility.Instance.HideLoadingPopup ();
			Debug.Log ("error buy. cheat attempt");
		}
	}

	void ClosePopup ()
	{
		scrollRectColor.verticalNormalizedPosition = 1.0f;
		this.gameObject.SetActive (false);

		selectedHSVData = new HSVData ();
	}

	#endregion

}
