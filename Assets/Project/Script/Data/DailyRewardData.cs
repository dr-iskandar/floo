using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DailyRewardData
{
	public string dailyDay;
	public bool isGoldReward;
	public string dailyRewardInfo;

	private const string TAG_ITEM_DAY = "day";
	private const string TAG_ITEM_GOLD = "is_gold";
	private const string TAG_ITEM_VALUE = "amount";

	public DailyRewardData ()
	{

	}

	public DailyRewardData (Dictionary<string,object> rawData)
	{
		dailyDay 		= JsonUtility.GetString(rawData, TAG_ITEM_DAY);
		isGoldReward 	= JsonUtility.GetBool(rawData, TAG_ITEM_GOLD);
		dailyRewardInfo	= JsonUtility.GetString(rawData, TAG_ITEM_VALUE);
	}
}
