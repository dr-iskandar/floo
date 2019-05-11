using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using ProjectMiniJSON;

public class DefaultParameterManager : MonoBehaviour 
{
	private static DefaultParameterManager instance;

	public static DefaultParameterManager Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject go = new GameObject("Default Parameter Manager");
				instance = go.AddComponent<DefaultParameterManager>();
				DontDestroyOnLoad(go);
			}
			return instance;
		}
	}

	private const string SAVE_NAME = "default_parameter.json";
	private const string BUILD_IN_DEFAULT_PARAMETER = "Text/default_parameter";

	private const string TAG_FOOD_SIZE = "food_size";
	private const string TAG_DATA = "data";
	private const string TAG_FISH_SIZE = "fish_size";
	private const string TAG_PREDATOR_SIZE = "predator_size";
	private const string TAG_SKIN = "skin";
	private const string TAG_LEVELS = "levels";
	private const string TAG_COLOR_CODE = "color_code";

	private float foodSize = 5.0f;

	public float FoodSize
	{
		get
		{
			return foodSize;
		}
	}

//	private Dictionary<string, List<string>> skinColorData;
	private List<HSVData> listHSVColorData = new List<HSVData>();

	private Dictionary<string,List<FishParameterData>> skinLevelsData;
	private List<PredatorParameterData> predatorParameter;

	public void SaveDefaultParameterJSON(string jsonText)
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
			Debug.Log("Error save default parameter " + e.ToString());
		}
	}

	public void LoadDefaultParameterJSON()
	{
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
				TextAsset textAsset = Resources.Load(BUILD_IN_DEFAULT_PARAMETER) as TextAsset;
				jsonText = textAsset.text;
			}
		}
		catch(System.Exception e)
		{
			Debug.Log("Fail read default_parameter.json data");
			return;
		}

		listHSVColorData = new List<HSVData>();
		SetDefaultParameterData(jsonText);
	}

	void SetDefaultParameterData(string jsonText)
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

		foodSize = 2.0f * JsonUtility.GetFloat(data, TAG_FOOD_SIZE) / EssentialData.scaleFactor;
		//Load fishes parameter data
		skinLevelsData = new Dictionary<string, List<FishParameterData>>();
//		skinColorData = new Dictionary<string, List<string>> ();

		var skins = data[TAG_FISH_SIZE] as List<object>;
		for (int i = 0; i < skins.Count; i++)
		{
			var skinData = skins[i] as Dictionary<string,object>;
			string skinName = JsonUtility.GetString(skinData, TAG_SKIN);
			var levelsData = skinData[TAG_LEVELS] as List<object>;
			List<FishParameterData> fishParameters = new List<FishParameterData>();
			for (int fi = 0; fi < levelsData.Count; fi++)
			{
				var fishLevelData = levelsData[fi] as Dictionary<string,object>;
				FishParameterData fishLevel = new FishParameterData(fishLevelData);
				fishParameters.Add(fishLevel);
			}

			skinLevelsData.Add(skinName, fishParameters);

//			List<string> colorData = new List<string> ();

			var colors = skinData[TAG_COLOR_CODE] as List<object>;
			for (int ci = 0; ci < colors.Count; ci++) 
			{
//				colorData.Add (colors [ci].ToString());

				HSVData colorHSVData = new HSVData();

				string colorString = colors [ci].ToString();
				colorHSVData.colorHSVCodeId = colorString;
				colorHSVData.fishSkin = skinName;

				listHSVColorData.Add (colorHSVData);
//				Debug.Log ("skiname " + skinName + ", " + colorString + " ~ " + listHSVColorData.Count);
			}

			//Debug.Log (skinName);
//			skinColorData.Add (skinName, colorData);
		}

		//Load Predator Parameter Data
		predatorParameter = new List<PredatorParameterData>();
		var predators = data[TAG_PREDATOR_SIZE] as List<object>;
		for (int i = 0; i < predators.Count; i++)
		{
			var rawPredatorData = predators[i] as Dictionary<string,object>;
			PredatorParameterData predatorData = new PredatorParameterData(rawPredatorData);
			predatorParameter.Add(predatorData);
		}
	}

//	public List<string> GetColorData(string skinName) 
//	{
//		return skinColorData [skinName];
//	}


	public List<HSVData> GetListHSVColorData ()
	{
		return listHSVColorData;
	}

	public float GetPredatorSize(string predatorName)
	{
		int idx = predatorParameter.FindIndex(x => x.skinName.Equals(predatorName));
		if (idx >= 0)
		{
			return predatorParameter[idx].size;
		}
		return 0;
	}

	public FishParameterData GetFishParameterData(string skinName, int level)
	{
		if (skinLevelsData.ContainsKey(skinName))
		{
			int idx = skinLevelsData[skinName].FindIndex(x => x.level == level);

			if (idx >= 0)
			{
				return skinLevelsData[skinName][idx];
			}
			else
			{
				Debug.Log("Skin " + skinName + " has no level " + level);
				return null;
			}
		}
		else
		{
			Debug.Log("Skin Name Not found in default parameter " + skinName);

			return null;
		}
	}
}
