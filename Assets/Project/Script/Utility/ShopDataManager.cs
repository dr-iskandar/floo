using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using ProjectMiniJSON;

public class ShopDataManager : MonoBehaviour 
{
	private static ShopDataManager instance;

	public static ShopDataManager Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject go = new GameObject("Shop Data Manager");
				instance = go.AddComponent<ShopDataManager>();
				DontDestroyOnLoad(go);
			}
			return instance;
		}
	}

	private const string SAVE_NAME = "shop_data.json";
	private const string BUILD_IN_FILE = "Text/shop_data";

	private const string TAG_DATA = "data";
	private const string TAG_SHOP_GOLD = "shop_gold";
	private const string TAG_SHOP_SKIN = "shop_skin";
	private const string TAG_SHOP_BUFF = "shop_buff";

	private List<PowerupData> listShopBuff = new List<PowerupData>();
	private List<ShopGoldData> listShopGold = new List<ShopGoldData>();
	private List<ShopSkinData> listShopSkin = new List<ShopSkinData>();

	private bool isDataLoaded = false;

	public void SaveShopDataJSON(string jsonText)
	{
		string path = Application.persistentDataPath + "/" + SAVE_NAME;
		try
		{
			FileInfo f = new FileInfo(path);
			if (f.Exists)
			{
				f.Delete();
			}

			StreamWriter sw = f.CreateText();
			string encryptedText = AES.Encrypt(jsonText,false);
			sw.WriteLine(encryptedText);
			sw.Close();
		}
		catch(System.Exception e)
		{
			Debug.Log("Error save shop data " + e.ToString());
		}
	}

	public void LoadShopDataJSON()
	{
		if (isDataLoaded)
		{
			return;
		}

		isDataLoaded = true;
		string path = Application.persistentDataPath + "/" + SAVE_NAME;
		string jsonText = "";
		try
		{
			FileInfo f = new FileInfo(path);
			if(f.Exists)
			{
				//When default parameter save file exist, load data from there
				StreamReader sr = f.OpenText();
				string encryptedText = sr.ReadToEnd();
				sr.Close();
				jsonText = AES.Decrypt(encryptedText,false);
			}
			else
			{
				TextAsset textAsset = Resources.Load(BUILD_IN_FILE) as TextAsset;
				jsonText = textAsset.text;
			}
		}
		catch(System.Exception e)
		{
			Debug.Log("Fail read shop_data.json data");
			return;
		}
			
		SetShopData(jsonText);
	}

	void SetShopData(string jsonText)
	{
		var jsonData = Json.Deserialize(jsonText) as Dictionary<string,object>;
		Dictionary<string,object> data = null;
		if (NetworkConfig.IsUsingEncryption) 
		{
			var enc = jsonData [TAG_DATA] as string;
			data = Json.Deserialize(AES.Decrypt(enc)) as Dictionary<string,object>;
		}
		else
			data = jsonData[TAG_DATA] as Dictionary<string,object>;

		//Load shop gold data
		listShopGold.Clear();
		var shopGolds = data[TAG_SHOP_GOLD] as List<object>;
		for (int i = 0; i < shopGolds.Count; i++)
		{
			var rawData = shopGolds[i] as Dictionary<string,object>;
			ShopGoldData sGoldData = new ShopGoldData(rawData);
			listShopGold.Add(sGoldData);
		}

		//Load Shop Skin
		listShopSkin.Clear();
		var shopSkins = data[TAG_SHOP_SKIN] as List<object>;
		for (int i = 0; i < shopSkins.Count; i++)
		{
			var rawData = shopSkins[i] as Dictionary<string,object>;
			ShopSkinData sSkinData = new ShopSkinData(rawData);
			listShopSkin.Add(sSkinData);
		}

		//Load Shop Buff
		listShopBuff.Clear();
		var shopBuffs = data[TAG_SHOP_BUFF] as List<object>;
		for (int i = 0; i < shopBuffs.Count; i++)
		{
			var rawData = shopBuffs[i] as Dictionary<string,object>;
			PowerupData puData = new PowerupData(rawData);
			listShopBuff.Add(puData);
		}
	}

	//Get list of powerups data from shop
	public List<PowerupData> GetPoerUpData()
	{
		LoadShopDataJSON();
		for (int i = 0; i < listShopBuff.Count; i++)
		{
			string buffCode = listShopBuff[i].buffCode;
			//Check is buff being used
			if (buffCode.Equals(EssentialData.Instance.PlayerData.equippedBuff))
			{
				listShopBuff[i].isUsed = true;
			}
			else
			{
				listShopBuff[i].isUsed = false;
			}

			//Check is buff being bought
			if (EssentialData.Instance.PlayerData.collectedBuffs.Contains(buffCode))
			{
				listShopBuff[i].isDataBought = true;
			}
			else
			{
				listShopBuff[i].isDataBought = false;
			}
		}

		return listShopBuff;
	}

	public List<ShopSkinData> GetSkinData()
	{
		LoadShopDataJSON();
		for (int i = 0; i < listShopSkin.Count; i++)
		{
			string skinCode = listShopSkin[i].skinCode;
			//Check is skin being used
			if (skinCode.Equals(EssentialData.Instance.PlayerData.equippedSkin))
			{
				listShopSkin[i].isInUse = true;
			}
			else
			{
				listShopSkin[i].isInUse = false;
			}

			//Check is buff being bought
			if (EssentialData.Instance.PlayerData.collectedSkins.Contains(skinCode))
			{
				listShopSkin[i].isDataBought = true;
			}
			else
			{
				listShopSkin[i].isDataBought = false;
			}
		}

		return listShopSkin;
	}

	public List<ShopGoldData> GetGoldData() 
	{
		return listShopGold;
	}
}
