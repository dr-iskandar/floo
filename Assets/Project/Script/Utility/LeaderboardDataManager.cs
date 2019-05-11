using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ProjectMiniJSON;

public class LeaderboardDataManager : MonoBehaviour 
{
	private static LeaderboardDataManager instance;

	public static LeaderboardDataManager Instance {
		get {
			if (instance == null) {
				GameObject go = new GameObject ("Leaderboard Data Manager");
				instance = go.AddComponent<LeaderboardDataManager> ();
				DontDestroyOnLoad (go);
			}
			return instance;
		}
	}

	private const string SAVE_NAME = "leaderboard_data.json";
	private const string BUILD_IN_FILE = "Text/leaderboard_data";

	private const string TAG_DATA = "data";
	private const string TAG_LEADERBOARD_POINT = "scoreboard";
	private const string TAG_LEADERBOARD_TIMEPLAY = "timeboard";
	private const string TAG_LEADERBOARD_FISHKILL = "killboard";

	private List<LeaderboardData> listLeaderboardDataPoint = new List<LeaderboardData> ();
	private List<LeaderboardData> listLeaderboardDataTime = new List<LeaderboardData> ();
	private List<LeaderboardData> listLeaderboardDataEat = new List<LeaderboardData> ();

	private bool isDataLoaded = false;

	public void SaveLeaderboardDataJSON (string jsonText)
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
			Debug.Log ("Error save leaderboard data " + e.ToString ());
		}
	}

	public void LoadLeaderboardDataJSON ()
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
			Debug.Log ("Fail read Leaderboard_data.json data");
			return;
		}

		SetLeaderboardData (jsonText);
	}

	void SetLeaderboardData (string jsonText)
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

		//Load leaderboard point
		listLeaderboardDataPoint.Clear();
		var leaderboardPoint = data[TAG_LEADERBOARD_POINT] as List<object>;
		for (int i = 0; i < leaderboardPoint.Count; i++)
		{
			var rawData = leaderboardPoint[i] as Dictionary<string,object>;
			LeaderboardData pointLeaderboardData = new LeaderboardData(rawData);

			pointLeaderboardData.leaderboardType = 1;
			pointLeaderboardData.totalPoint = JsonUtility.GetLong (rawData, "score");

			listLeaderboardDataPoint.Add(pointLeaderboardData);
		}

		//Load leaderboard timeplay
		listLeaderboardDataTime.Clear();
		var leaderboardTimeplay = data[TAG_LEADERBOARD_TIMEPLAY] as List<object>;
		for (int i = 0; i < leaderboardTimeplay.Count; i++)
		{
			var rawData = leaderboardTimeplay[i] as Dictionary<string,object>;
			LeaderboardData timeplayLeaderboardData = new LeaderboardData(rawData);

			timeplayLeaderboardData.leaderboardType = 2;
			timeplayLeaderboardData.totalTimePlay = JsonUtility.GetLong (rawData, "playtime");

			listLeaderboardDataTime.Add(timeplayLeaderboardData);
		}

		//Load leaderboard eat / fish kill
		listLeaderboardDataEat.Clear();
		var leaderboardFishKill = data[TAG_LEADERBOARD_FISHKILL] as List<object>;
		for (int i = 0; i < leaderboardFishKill.Count; i++)
		{
			var rawData = leaderboardFishKill[i] as Dictionary<string,object>;
			LeaderboardData eatLeaderboardData = new LeaderboardData(rawData);

			eatLeaderboardData.leaderboardType = 3;
			eatLeaderboardData.totalEat = JsonUtility.GetLong (rawData, "fish_killed");

			listLeaderboardDataEat.Add(eatLeaderboardData);
		}
	}

	public List<LeaderboardData> GetLeaderboardDataPoint ()
	{
		LoadLeaderboardDataJSON();
		return listLeaderboardDataPoint;
	}

	public List<LeaderboardData> GetLeaderboardDataTimeplay ()
	{
		LoadLeaderboardDataJSON();
		return listLeaderboardDataTime;
	}

	public List<LeaderboardData> GetLeaderboardDataEat ()
	{
		LoadLeaderboardDataJSON();
		return listLeaderboardDataEat;
	}

}
