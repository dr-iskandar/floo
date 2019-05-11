using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using ProjectMiniJSON;

public class LanguageManager : MonoBehaviour 
{
	private static LanguageManager instance;

	public static LanguageManager Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject go = new GameObject("Language Manager");
				instance = go.AddComponent<LanguageManager>();
				DontDestroyOnLoad(go);
//				instance.InitializeLanguageLibrary();
				instance.LoadLanguageDataJsonInRuntime();
			}
			return instance;
		}
	}

	private Dictionary<string,LanguageData> languageDictionary;

	#region LIBRARY
	//Initialize language dictionary. Read data from json text
	void InitializeLanguageLibrary()
	{
		int IndexofCurrLangId = GetIndexOfSavedLanguage ();

		string codeLanguage = listLanguage[IndexofCurrLangId].languageCode;

		string languageDictionaryPath = "Text/game_message_" + codeLanguage;

		Debug.Log ("languageDictionaryPath " + languageDictionaryPath );

		//TODO : This is dummy. The json text needed to be downloaded from backend
//		string languageDictionaryPath = "Text/game_message_en";
		TextAsset textAsset = Resources.Load(languageDictionaryPath) as TextAsset;
		string jsonMessage = textAsset.text;
		var jsonData = Json.Deserialize(jsonMessage) as Dictionary<string,object>;
		var langData = jsonData["data"] as List<object>;

		languageDictionary = new Dictionary<string, LanguageData>();
		for (int i = 0; i < langData.Count; i++)
		{
			var rawData = langData[i] as Dictionary<string,object>;
			LanguageData data = new LanguageData(rawData);
			languageDictionary.Add(data.messageCode, data);
		}
	}

	LanguageData GetLanguageDataFromDictionary(string messageCode)
	{
		if (languageDictionary.ContainsKey(messageCode))
		{
			return languageDictionary[messageCode];
		}

		return new LanguageData();
	}

	public string GetMessage(string messageCode)
	{
		return GetLanguageDataFromDictionary(messageCode).messageValue;
	}

	public void SetMessageToText(Text uiText, string messageCode)
	{
		LanguageData data = GetLanguageDataFromDictionary(messageCode);
		uiText.text = data.messageValue;
		uiText.fontSize = data.messageSize;
	}

	public void SetMessageToText(TextMesh textMesh, string messageCode)
	{
		LanguageData data = GetLanguageDataFromDictionary(messageCode);
		textMesh.text = data.messageValue;
		textMesh.fontSize = data.messageSize;
	}

	#endregion

	#region LIST LANGUAGE
	private List<MultiLanguageData> listLanguage = new List<MultiLanguageData>();

	public void LoadLanguageDataJsonInRuntime ()
	{
		LoadListLanguageData ();
		InitializeLanguageLibrary();
	}

	public List<MultiLanguageData> LoadListLanguageData ()
	{
		MockMultiData ();

		return listLanguage;
	}

	public int GetIndexOfSavedLanguage ()
	{
		int currIdx = EssentialData.Instance.LoadLanguageId();
		int indexOfCurrId = listLanguage.FindIndex(id => id.languageId == currIdx);

		return indexOfCurrId;
	}

	void ClearList ()
	{
		listLanguage.Clear ();
	}

	void MockMultiData ()
	{
		Debug.Log ("-- mockk language list");
		ClearList ();

		MultiLanguageData multi = new MultiLanguageData();
		multi.languageId = 1;
		multi.languageCode = "en";
		multi.languageName = "English";
		listLanguage.Add (multi);

		multi = new MultiLanguageData();
		multi.languageId = 2;
		multi.languageCode = "id";
		multi.languageName = "Indonesia";
		listLanguage.Add (multi);

		multi = new MultiLanguageData();
		multi.languageId = 5;
		multi.languageCode = "th";
		multi.languageName = "Thai";
		listLanguage.Add (multi);

		multi = new MultiLanguageData();
		multi.languageId = 6;
		multi.languageCode = "vn";
		multi.languageName = "Vietnamese";
		listLanguage.Add (multi);

	}

	#endregion

//	#region API 
//	void APIListLanguageData()
//	{
//		string apiName = "get_achievement_data";
//
//		BackEndConnect.Instance.SendRequestToServer (CBListLanguageData, apiName);
//	}
//
//	void CBListLanguageData(APIResponse response)
//	{
//		if (!response.isError)
//		{
//			Debug.Log("list language Data " + response.rawData);
//			 // AchievementDataManager.Instance.SaveAchievementDataJSON(response.rawData);
//		}
//	}
//	#endregion

}
