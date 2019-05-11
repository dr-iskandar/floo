using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LeaderboardData
{
	public string leaderboardId;
	public string leaderboardPlayerName;

	public int leaderboardType; // 0 nothing 1 poin 2 time 3 eat
	public int rankingNumber;

	public long totalTimePlay;
	public long totalPoint;
	public long totalEat;

	public string skinCode;
	public int colorCode;

	public Sprite spriteFish;

	private const string TAG_ITEM_ID = "user_id";
	private const string TAG_ITEM_NICKNAME = "display_name";
	private const string TAG_ITEM_RANK = "rank";
	private const string TAG_SKIN_ID = "skin";
	private const string TAG_COLORCODE = "color_code";

	private const string TAG_LV1 = "_lv1";

	public LeaderboardData ()
	{
	}

	public LeaderboardData (Dictionary<string,object> rawData)
	{
		leaderboardId = JsonUtility.GetString(rawData, TAG_ITEM_ID);
		leaderboardPlayerName = JsonUtility.GetString(rawData, TAG_ITEM_NICKNAME);

		rankingNumber = JsonUtility.GetInt(rawData, TAG_ITEM_RANK);
		colorCode = JsonUtility.GetInt(rawData, TAG_COLORCODE);

		skinCode = JsonUtility.GetString(rawData, TAG_SKIN_ID);

		spriteFish = ImageUtility.CreateSpriteFromObject( AssetManager.Instance.GetPrefabByKeyword(skinCode + TAG_LV1) );
	}
}
