using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using ProjectMiniJSON;

public class DailyLoginDataManager : MonoBehaviour 
{
	private static DailyLoginDataManager instance;

	public static DailyLoginDataManager Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject go = new GameObject("Daily Login Data Manager");
				instance = go.AddComponent<DailyLoginDataManager>();
//				DontDestroyOnLoad(go);
			}
			return instance;
		}
	}

	private const string SAVE_NAME = "dailyLogin_data.json";
	private const string BUILD_IN_FILE = "Text/dailyLogin_data";

	private const string TAG_DATA = "data";
	private const string TAG_DAILY = "daily_data";
	private const string TAG_DAILY_ID = "daily_login_id";
	private const string TAG_REWARD_TYPE = "reward_type";

	private List<DailyRewardData> listDailyNormal = new List<DailyRewardData>();
	private List<DailyRewardData> listDailyEvent = new List<DailyRewardData>();

	private bool isDataLoaded = false;

	#region DAILY LOGIN DATA
	public void SaveDailyLoginDataJSON(string jsonText)
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
			Debug.Log("Error save Daily data " + e.ToString());
		}
	}

	public void LoadDailyLoginDataJSON()
	{
		if (isDataLoaded)
		{
			return;
		}

		listDailyNormal.Clear();
		listDailyEvent.Clear();

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
			Debug.Log("Fail read dailyLogin_data.json data");
			return;
		}

		SetDailyLoginData(jsonText);
	}

	void SetDailyLoginData(string jsonText)
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

		var daily = data[TAG_DAILY] as List<object>;

		for (int i = 0; i < daily.Count; i++)
		{
			var dailyType = daily[i] as Dictionary<string,object>;

			string dailyID = JsonUtility.GetString(dailyType, TAG_DAILY_ID);

			var rewardType = dailyType[TAG_REWARD_TYPE] as List<object>;

			for (int j = 0; j < rewardType.Count; j++)
			{				
				var rawData = rewardType[j] as Dictionary<string,object>;
				DailyRewardData dailyData = new DailyRewardData(rawData);

				if (dailyID.Equals("default"))
				{
					listDailyNormal.Add(dailyData);
//					Debug.Log (j + " deafaut manger");
				}
				else
				{
					listDailyEvent.Add(dailyData);
//					Debug.Log (j + " event manger");
				}
			}
		}
		isDataLoaded = true;
	}

	//Get list of powerups data from shop
	public List<DailyRewardData> GetDailyDataNormal()
	{
		LoadDailyLoginDataJSON();

		return listDailyNormal;
	}
		
	public List<DailyRewardData> GetDailyDataEvent()
	{
		LoadDailyLoginDataJSON();

		return listDailyEvent;
	}
	#endregion

}