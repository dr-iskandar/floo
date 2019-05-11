using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using ProjectMiniJSON;

public class NewsDataManager : MonoBehaviour 
{
	private static NewsDataManager instance;

	public static NewsDataManager Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject go = new GameObject("News Data Manager");
				instance = go.AddComponent<NewsDataManager>();
				DontDestroyOnLoad(go);
			}
			return instance;
		}
	}

	private const string SAVE_NAME = "news_data.json";
	private const string BUILD_IN_FILE = "Text/news_data";

	private const string TAG_DATA = "data";
	private const string TAG_NEWS = "news";

	private List<NewsData> listNews = new List<NewsData>();

	private bool isDataLoaded = false;

	public void SaveNewsDataJSON(string jsonText)
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
			Debug.Log("Error save News data " + e.ToString());
		}
	}

	public void LoadNewsDataJSON()
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
			Debug.Log("Fail read News_data.json data");
			return;
		}

		SetNewsData(jsonText);
	}

	void SetNewsData(string jsonText)
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

		//Load news data
		listNews.Clear();
		var news = data[TAG_NEWS] as List<object>;
		for (int i = 0; i < news.Count; i++)
		{
			var rawData = news[i] as Dictionary<string,object>;
			NewsData newsData = new NewsData(rawData);
			listNews.Add(newsData);
		}
	}

	//Get list of powerups data from shop
	public List<NewsData> GetNewsData()
	{
		LoadNewsDataJSON();

		return listNews;
	}

}
