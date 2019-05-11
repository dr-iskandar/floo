using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerData
{
	private const string TAG_GOLD = "gold";
	private const string TAG_USER_ID = "user_id";
	private const string TAG_SECRET_KEY = "secret_key";
	private const string TAG_DISPLAY_NAME = "display_name";
	private const string TAG_EQUIPPED_SKIN = "equipped_skin";
	private const string TAG_EQUIPPED_BUFF = "equipped_buff";
	private const string TAG_SKINS = "skins";
	private const string TAG_BUFFS = "buffs";
	private const string TAG_ACHIEVEMENTS = "achievements";
	private const string TAG_COLOR_CODE = "color_code";
	private const string TAG_NO_ADS = "no_ads";


	public const string NONE = "none";

	public string displayName;
	public int gold;
	public string userId;
	public string secretKey;
	public string equippedSkin;
	public string equippedBuff;
	public long startBuffTime;
	public long endBuffTime;
	public List<string> collectedSkins = new List<string>();
	public List<string> collectedBuffs = new List<string>();
	public List<string> achievements = new List<string> ();
	public int totalFishKilled;
	public long totalPlayTime;
	public int timePlayed;
	public long longestPlayTime;
	public int highestFishKilled;
	public int highestBigFishEaten;
	public int totalBigFishEaten;
	public int highestFoodEaten;
	public int totalFoodEaten;
	public int totalBuffEaten;
	public int totalEscape;
	public int killByPlayer;
	public int killByUrchin;
	public string colorCode;

	public int invisibleKill;
	public int maxLevel;
	public int predatorKilled;
	public int usedBoost;
	public int shareCount;
	public int skinCount;

	public bool noAds;

	public PlayerData()
	{
		
	}

	public PlayerData(Dictionary<string,object> rawData)
	{
		gold = JsonUtility.GetInt(rawData,TAG_GOLD);
		userId = JsonUtility.GetString(rawData, TAG_USER_ID);
		displayName = JsonUtility.GetString(rawData, TAG_DISPLAY_NAME);
		secretKey = JsonUtility.GetString(rawData, TAG_SECRET_KEY);
		equippedSkin = JsonUtility.GetString(rawData, TAG_EQUIPPED_SKIN);

		equippedBuff = JsonUtility.GetString(rawData, TAG_EQUIPPED_BUFF);
		colorCode = JsonUtility.GetString (rawData, TAG_COLOR_CODE);

		try {
			var buffObj = rawData[TAG_EQUIPPED_BUFF] as Dictionary<string,object>;
			equippedBuff = JsonUtility.GetString(buffObj, "buff_id");
			var startObj = buffObj["start_time"] as Dictionary<string,object>;
			string startTimeString = JsonUtility.GetString(startObj, "$numberLong");
			long.TryParse(startTimeString, out startBuffTime);
			var endObj = buffObj["end_time"] as Dictionary<string,object>;
			string endTimeString = JsonUtility.GetString(endObj, "$numberLong");
			long.TryParse(endTimeString, out endBuffTime);


		} catch (Exception ex) {
			equippedBuff = "none";
		}

		try {
			var listBuff = rawData[TAG_BUFFS] as List<object>;
		} catch (Exception ex) {
			
		}

		var listSkin = rawData[TAG_SKINS] as List<object>;
		for (int i = 0; i < listSkin.Count; i++)
		{
			collectedSkins.Add(listSkin[i].ToString());
		}

		var listAchievements = rawData [TAG_ACHIEVEMENTS] as List<object>;
		for (int i = 0; i < listAchievements.Count; i++) {
			achievements.Add (listAchievements [i].ToString ());
		}

		try {
			noAds = JsonUtility.GetBool(rawData, TAG_NO_ADS);
		} catch (Exception ex) {
			noAds = false;
		}

		try {
			totalFishKilled = JsonUtility.GetInt(rawData, "total_fish_killed");
		} catch (Exception ex) {
			totalFishKilled = 0;
		}

		try {
			totalPlayTime = JsonUtility.GetLong(rawData, "total_play_time");
		} catch (Exception ex) {
			totalPlayTime = 0;
		}

		try {
			timePlayed = JsonUtility.GetInt(rawData, "time_played");
		} catch (Exception ex) {
			timePlayed = 0;
		}

		try {
			longestPlayTime = JsonUtility.GetLong(rawData, "longest_play_time");
		} catch (Exception ex) {
			longestPlayTime = 0;
		}

		try {
			totalFoodEaten = JsonUtility.GetInt(rawData, "total_food_eaten");
		} catch (Exception ex) {
			totalFoodEaten = 0;
		}

		try {
			highestFishKilled = JsonUtility.GetInt(rawData, "highest_fish_killed");
		} catch (Exception ex) {
			highestFishKilled = 0;
		}

		try {
			totalBuffEaten = JsonUtility.GetInt(rawData, "total_buff_eaten");
		} catch (Exception ex) {
			totalBuffEaten = 0;
		}

		try {
			highestBigFishEaten = JsonUtility.GetInt(rawData, "total_big_fish_eaten");
		} catch (Exception ex) {
			highestBigFishEaten = 0;
		}

		try {
			totalBigFishEaten = JsonUtility.GetInt(rawData, "total_big_fish_eaten");
		} catch (Exception ex) {
			totalBigFishEaten = 0;
		}

		try {
			totalEscape = JsonUtility.GetInt(rawData, "total_escape");
		} catch (Exception ex) {
			totalEscape = 0;
		}

		try {
			killByPlayer = JsonUtility.GetInt(rawData, "kill_by_player");
		} catch (Exception ex) {
			killByPlayer = 0;
		}

		try {
			killByUrchin = JsonUtility.GetInt(rawData, "kill_by_urchin");
		} catch (Exception ex) {
			killByUrchin = 0;
		}

		try {
			highestFoodEaten = JsonUtility.GetInt(rawData, "highest_food_eaten");
		} catch (Exception ex) {
			highestFoodEaten = 0;
		}

		try {
			predatorKilled = JsonUtility.GetInt(rawData, "predator_killed");
		} catch (Exception ex) {
			predatorKilled = 0;
		}

		try {
			invisibleKill = JsonUtility.GetInt(rawData, "invisible_kill");
		} catch (Exception ex) {
			invisibleKill = 0;
		}

		try {
			usedBoost = JsonUtility.GetInt(rawData, "used_boost");
		} catch (Exception ex) {
			usedBoost = 0;
		}

		try {
			maxLevel = JsonUtility.GetInt(rawData, "max_level");
		} catch (Exception ex) {
			maxLevel = 0;
		}

		try {
			shareCount = JsonUtility.GetInt(rawData, "share_count");
		} catch (Exception ex) {
			shareCount = 0;
		}

		try {
			skinCount = collectedSkins.Count;
		} catch (Exception ex) {
			skinCount = 0;
		}
	}

}