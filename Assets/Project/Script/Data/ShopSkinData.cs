using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProjectMiniJSON;

[System.Serializable]
public class ShopSkinData
{
	public string skinName;
	public string skinId;
	public string skinDescription;
	public float skinPrice;

	public bool isInUse;
	public bool isDataBought; 	// if already bought
	public bool isPayWithGold;

	public string skinCode;

	public Sprite spriteMinLevel;
	public Sprite spriteMaxLevel;
	public Sprite spriteColorSprite;

	private const string TAG_ITEM_ID = "item_id";
	private const string TAG_ITEM_NAME = "item_name";
	private const string TAG_ITEM_DESCRIPTION = "description";
	private const string TAG_SKIN_ID = "skin_id";
	private const string TAG_PRICE = "price";

	private const string TAG_LV1 = "_lv1";
	private const string TAG_LV10 = "_lv10";
	private const string TAG_COLORSPRITE = "_colorSprite";

	public ShopSkinData()
	{
		
	}

	public ShopSkinData(Dictionary<string,object> rawData)
	{
		skinName = JsonUtility.GetString(rawData, TAG_ITEM_NAME);
		skinId = JsonUtility.GetString(rawData, TAG_ITEM_ID);
		skinDescription = JsonUtility.GetString(rawData, TAG_ITEM_DESCRIPTION);
		skinPrice = JsonUtility.GetFloat(rawData, TAG_PRICE);
		isPayWithGold = true;

		skinCode = JsonUtility.GetString(rawData, TAG_SKIN_ID);
		spriteMinLevel = ImageUtility.CreateSpriteFromObject( AssetManager.Instance.GetPrefabByKeyword(skinCode + TAG_LV1) );
		spriteMaxLevel = ImageUtility.CreateSpriteFromObject( AssetManager.Instance.GetPrefabByKeyword(skinCode + TAG_LV10) );
		spriteColorSprite = ImageUtility.CreateSpriteFromObject( AssetManager.Instance.GetPrefabByKeyword(skinCode + TAG_COLORSPRITE) );

	}

}
