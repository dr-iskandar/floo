using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PowerupData
{
	public string powerupName;
	public string powerupId;
	public string powerupDescription;
	public float powerupPrice;

	public bool isDataBought; 	// if already bought
	public bool isUsed;

	public Sprite spritePowerUp;

	public string buffCode;

	private const string TAG_ITEM_NAME = "item_name";
	private const string TAG_ITEM_DESCRIPTION = "description";
	private const string TAG_ITEM_ID = "item_id";
	private const string TAG_PRICE = "price";
	private const string TAG_BUFF_ID = "buff_id";

	private const string TAG_PREVIEW = "_preview";

	public PowerupData()
	{
		
	}

	public PowerupData(Dictionary<string,object> rawData)
	{
		powerupName = JsonUtility.GetString(rawData, TAG_ITEM_NAME);
		powerupId = JsonUtility.GetString(rawData, TAG_ITEM_ID);
		powerupDescription = JsonUtility.GetString(rawData, TAG_ITEM_DESCRIPTION);
		powerupPrice = JsonUtility.GetFloat(rawData, TAG_PRICE);
		buffCode = JsonUtility.GetString(rawData, TAG_BUFF_ID);
		Object o = AssetManager.Instance.GetPrefabByKeyword(buffCode + TAG_PREVIEW);
		spritePowerUp = ImageUtility.CreateSpriteFromObject(o);
		
	}
}
