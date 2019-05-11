using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ProjectMiniJSON;

public class AchievementDataManager : MonoBehaviour
{
	private static AchievementDataManager instance;

	public static AchievementDataManager Instance {
		get {
			if (instance == null) {
				GameObject go = new GameObject ("Achievement Data Manager");
				instance = go.AddComponent<AchievementDataManager> ();
				DontDestroyOnLoad (go);
			}
			return instance;
		}
	}

	private const string SAVE_NAME = "achievement_data.json";
	private const string BUILD_IN_FILE = "Text/achievement_data";

	private const string TAG_DATA = "data";

	private List<AchievementData> listAchievementData = new List<AchievementData> ();

	private bool isDataLoaded = false;

	public void SaveAchievementDataJSON (string jsonText)
	{
		string path = Application.persistentDataPath + "/" + SAVE_NAME;
		try {
			FileInfo f = new FileInfo (path);
			if (f.Exists) {
				f.Delete ();
			}

			StreamWriter sw = f.CreateText ();
			string encryptedText = AES.Encrypt (jsonText, false);
			sw.WriteLine (encryptedText);
			sw.Close ();
		} catch (System.Exception e) {
			Debug.Log ("Error save achievement data " + e.ToString ());
		}
	}

	public void LoadAchievementDataJSON ()
	{
		if (isDataLoaded) {
			return;
		}

		isDataLoaded = true;
		string path = Application.persistentDataPath + "/" + SAVE_NAME;
		string jsonText = "";
		try {
			FileInfo f = new FileInfo (path);
			if (f.Exists) {
				//When default parameter save file exist, load data from there
				StreamReader sr = f.OpenText ();
				string encryptedText = sr.ReadToEnd ();
				sr.Close ();
				jsonText = AES.Decrypt (encryptedText, false);
			} else {
				TextAsset textAsset = Resources.Load (BUILD_IN_FILE) as TextAsset;
				jsonText = textAsset.text;
			}
		} catch (System.Exception e) {
			Debug.Log ("Fail read achievement_data.json data");
			return;
		}

		SetAchievementData (jsonText);
	}

	void SetAchievementData (string jsonText)
	{
		var jsonData = Json.Deserialize (jsonText) as Dictionary<string,object>;
		List<object> data = null;
		if (NetworkConfig.IsUsingEncryption) 
		{
			var enc = jsonData [TAG_DATA] as string;
			data = Json.Deserialize(AES.Decrypt(enc)) as List<object>;
		}
		else
			data = jsonData[TAG_DATA] as List<object>;

		listAchievementData.Clear ();

		AchievementData achData = new AchievementData ();
		for (int i = 0; i < data.Count; i++) {
			
			var rawData = data [i] as Dictionary<string,object>;
			string currId = JsonUtility.GetString (rawData, "achievement_id");
			string id = currId.Substring (0, 1);
			if (!achData.achievementId.Equals (id)) {
				
				if (!achData.achievementId.Equals (""))
					listAchievementData.Add (achData);
				
				achData = new AchievementData ();
				achData.achievementId = id.Substring (0, 1);
				achData.achievementUnlockMethod = JsonUtility.GetInt (rawData, "unlock_method");
			}
			achData.allAchievementDescription.Add (JsonUtility.GetString (rawData, "description"));
			achData.allAchievementId.Add (currId);
			achData.allAchievementName.Add (JsonUtility.GetString (rawData, "title"));
			achData.allAchievementProgressMax.Add (JsonUtility.GetLong (rawData, "unlock_amount"));

			int rewardType = JsonUtility.GetInt (rawData, "reward_type");
			if (rewardType == 1)
				achData.allAchievementReward.Add (JsonUtility.GetInt (rawData, "amount"));
			else
				achData.allAchievementReward.Add (JsonUtility.GetString (rawData, "skin_name"));
			achData.isClaimed.Add (false);
		}

	}

	//Get list of powerups data from shop
	public List<AchievementData> GetAchievementData ()
	{
		LoadAchievementDataJSON ();

		for (int i = 0; i < listAchievementData.Count; i++) {
			AchievementData data = listAchievementData [i];
			for (int j = 0; j < data.allAchievementId.Count; j++) {
				if (EssentialData.Instance.PlayerData.achievements.Contains (data.allAchievementId [j])) 
				{
					data.isProgressDone = true;
					data.clearedStars = j;
					data.isClaimed [j] = true;
					data.achievementName = data.allAchievementName [j];
					data.achievementDescription = data.allAchievementDescription[j];
				}
				if (!data.isClaimed [j]) {
					data.achievementId = data.allAchievementId[j];
					data.clearedStars = j;
					data.achievementName = data.allAchievementName[j];
					data.achievementDescription = data.allAchievementDescription[j];
					data.isProgressDone = false;
					switch (data.achievementUnlockMethod) {
					case 1:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.totalFishKilled) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.totalFishKilled;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					case 2:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.highestFishKilled) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.highestFishKilled;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					case 3:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.longestPlayTime) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.longestPlayTime;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					case 4:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.totalPlayTime) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.totalPlayTime;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					case 5:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.highestBigFishEaten) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.highestBigFishEaten;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					case 6:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.totalBigFishEaten) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.totalBigFishEaten;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					case 7:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.highestFoodEaten) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.highestFoodEaten;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					case 8:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.totalFoodEaten) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.totalFoodEaten;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					case 9:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.totalBuffEaten) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.totalBuffEaten;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					case 10:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.totalEscape) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.totalEscape;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					case 11:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.killByUrchin) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.killByUrchin;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					case 12:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.killByPlayer) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.killByPlayer;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					case 13:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.predatorKilled) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.predatorKilled;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					case 14:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.invisibleKill) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.invisibleKill;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					case 15:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.usedBoost) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.usedBoost;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					case 16:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.maxLevel) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.maxLevel;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					case 17:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.skinCount) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.skinCount;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					case 18:
						if (data.allAchievementProgressMax [j] <= EssentialData.Instance.PlayerData.shareCount) {
							data.isProgressDone = true;
						}
						data.achievementProgressCurr = EssentialData.Instance.PlayerData.shareCount;
						data.achievementProgressMax = data.allAchievementProgressMax [j];
						break;

					default:
						data.isProgressDone = false;
						data.achievementProgressCurr = 0;
						data.achievementProgressMax = 1;
						break;
					}

					if (data.allAchievementReward [j] is int) {
						data.isGoldReward = true;
						int.TryParse(data.allAchievementReward [j].ToString(), out data.achievementRewardValue);
					} else {
						data.isGoldReward = false;
						data.achievementRewardSkin = data.allAchievementReward [j].ToString();
					}


					break;
				}
			}
		}

		return listAchievementData;
	}
}
