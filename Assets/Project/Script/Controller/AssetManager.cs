using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProjectMiniJSON;

public class AssetManager : MonoBehaviour 
{
	private static AssetManager instance;

	public static AssetManager Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject go = new GameObject("Asset Manager");
				instance = go.AddComponent<AssetManager>();
				instance.InitializeDictionaryData();
			}
			return instance;
		}
	}

	private Dictionary<string,string> assetDictionary;
	private Dictionary<string,Object> prefabPool;

	void OnDestroy()
	{
		instance = null;
	}

	private void InitializeDictionaryData()
	{
		//TODO Read JSON data here and return the keyword to prefab dictionary here
		assetDictionary = new Dictionary<string, string>();
		prefabPool = new Dictionary<string, Object>();

		string assetTextPath = "Text/game_asset";
		TextAsset textAsset = Resources.Load(assetTextPath) as TextAsset;
		string jsonMessage = textAsset.text;
		var jsonData = Json.Deserialize(jsonMessage) as Dictionary<string,object>;
		var assetData = jsonData["data"] as List<object>;

		for (int i = 0; i < assetData.Count; i++)
		{
			var data = assetData[i] as Dictionary<string,object>;
			string assetCode = JsonUtility.GetString(data,"asset_code");
			string assetPath = JsonUtility.GetString(data, "asset_path");
			assetDictionary.Add(assetCode, assetPath);
		}
	}

	//This function will return the prefab of the obstacle to instantiated
	public Object GetPrefabByKeyword(string keyword)
	{
		Object resultPrefab = null;

		//Check for the keyword in prefab pool
		if(prefabPool.ContainsKey(keyword))
		{
			resultPrefab = prefabPool[keyword];
			return resultPrefab;
		}

		//Check the dictionary for the keyword
		string usedKeyword = "default";
		string prefabPath = assetDictionary[usedKeyword];
		if (assetDictionary.ContainsKey(keyword))
		{
			prefabPath = assetDictionary[keyword];
			usedKeyword = keyword;
		}
		else
		{
			Debug.Log("Keyword not found : " + keyword);
			keyword = usedKeyword;
		}

		resultPrefab = Resources.Load(prefabPath);
		//When the result is still null
		if (resultPrefab == null)
		{
			//Asset not found load the default again
			Debug.Log("Prefab path doesn't exist : " + prefabPath);
			usedKeyword = "default";
			prefabPath = assetDictionary["default"];
			resultPrefab = Resources.Load(prefabPath);
		}

		if(!prefabPool.ContainsKey(usedKeyword))
		{
			prefabPool.Add(usedKeyword, resultPrefab);
		}
		return resultPrefab;
	}
}
