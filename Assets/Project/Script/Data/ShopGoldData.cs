using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ShopGoldData
{
	public string goldName;
	public string goldId;
	public float goldPrice;

	public string skuId;
	public string codeIAP;

	public Sprite spriteIAP;
	public bool isSprite;

	private const string TAG_PACKAGE_NAME = "item_package_name";
	private const string TAG_AMOUNT = "amount";
	private const string TAG_ITEM_ID = "item_id";
	private const string TAG_PRICE = "price";
	private const string TAG_ITEM_NAME = "item_name";
	private const string TAG_SKIN_NAME = "skin_name";

	public ShopGoldData()
	{
		
	}

	public ShopGoldData(Dictionary<string,object> rawData)
	{
		skuId = JsonUtility.GetString(rawData, TAG_PACKAGE_NAME);
		goldId = JsonUtility.GetString(rawData, TAG_ITEM_ID);

		goldPrice = JsonUtility.GetFloat(rawData, TAG_PRICE);

		goldName = JsonUtility.GetString(rawData, TAG_ITEM_NAME);
		codeIAP = JsonUtility.GetString(rawData, TAG_SKIN_NAME);
		if (codeIAP.Contains("no_ads") || codeIAP.Contains("xmas_bundle_2016"))
		{
			spriteIAP = ImageUtility.CreateSpriteFromObject (AssetManager.Instance.GetPrefabByKeyword(codeIAP));
			isSprite = true;
		}
		else
		{
			// use 'codeIAP' to generate prefab and put under parent.
//			goldName = JsonUtility.GetInt(rawData, TAG_AMOUNT) + " GOLD";
			//spriteIAP = ImageUtility.CreateSpriteFromObject (AssetManager.Instance.GetPrefabByKeyword(codeIAP));
			isSprite = false;
		}
	}
}
