using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using ProjectMiniJSON;

namespace FlooMapEditor
{

	public class MapCreatorManager : MonoBehaviour 
	{
		private const string TAG_MAP_SAVE_FILENAME = "/floo_map_data.json";

		private static MapCreatorManager instance;

		public static MapCreatorManager Instance
		{
			get
			{
				return instance;
			}
		}

		public GameObject prefabMapSave;
		public Transform listMapParent;
		public MapCreatorPopUp popUpCreate;
		private List<MapEditorSaveData> savedMaps;

		void Awake()
		{
			instance = this;
		}

		void Start () 
		{
			savedMaps = new List<MapEditorSaveData>();
			LoadSavedMaps();
			DisplaySavedMaps();
			popUpCreate.ClosePopUp();
		}

		private void ClearMapPrefab()
		{
			int childCount = listMapParent.childCount;
			for (int i = childCount - 1; i >= 0; i--)
			{
				Destroy(listMapParent.GetChild(i).gameObject);
			}
		}

		private void SortSavedMaps()
		{
			savedMaps.Sort((x, y) => y.modifiedTimeStamp.CompareTo(x.modifiedTimeStamp));
		}

		public void DisplaySavedMaps()
		{
			ClearMapPrefab();
			SortSavedMaps();

			for (int i = 0; i < savedMaps.Count; i++)
			{
				GameObject go = Instantiate(prefabMapSave) as GameObject;
				go.transform.SetParent(listMapParent);
				go.transform.localScale = Vector3.one;

				go.GetComponent<MapEditorSave>().Init(savedMaps[i]);
			}
		}

		public void LoadSavedMaps()
		{
			FileInfo f = new FileInfo(Application.persistentDataPath + TAG_MAP_SAVE_FILENAME);

			if (f.Exists)
			{
				try
				{
					//Read the data here
					StreamReader sr = f.OpenText();
					string fileContent = sr.ReadToEnd();
					sr.Close();
					var savedMapList = Json.Deserialize(fileContent) as List<object>;
					savedMaps.Clear();
					for(int i=0; i<savedMapList.Count; i++)
					{
						Dictionary<string,object> rawData = savedMapList[i] as Dictionary<string,object>;
						MapEditorSaveData data = new MapEditorSaveData(rawData);
						savedMaps.Add(data);
					}
				}
				catch(System.Exception e)
				{
					Debug.Log("Message " + e.Message);
				}
			}
		}

		private void SaveMaps()
		{
			FileInfo f = new FileInfo(Application.persistentDataPath + TAG_MAP_SAVE_FILENAME);
			if (f.Exists)
			{
				f.Delete();
			}

			List<object> savedMapList = new List<object>();
			for (int i = 0; i < savedMaps.Count; i++)
			{
				savedMapList.Add(savedMaps[i].ToDictionary());
			}

			string jsonMap = Json.Serialize(savedMapList);
			StreamWriter sw = f.CreateText();
			sw.WriteLine(jsonMap);
			sw.Close();
		}

		public void CreateMap(string name, int width, int height)
		{
			MapEditorSaveData data = new MapEditorSaveData();
			data.mapName = name;
			data.width = width;
			data.height = height;
			string currentTime = DateTime.Now.ToString("yyyyMMddHHmmss");
			data.saveName = name + "_" + currentTime + ".json";
			data.modifiedTimeStamp = currentTime;
			data.createdDate = DateTime.Now.ToString("yyyy-MM-dd");
			data.modifiedDate = data.createdDate;
			savedMaps.Add(data);
			SaveMaps();
			//Create default map and save it with the save name
			EditorMapData basicMap = MapEditorUtility.CreateDefaultMap(data);
			MapEditorUtility.SaveMap(data.saveName, basicMap);

			DisplaySavedMaps();
		}

		public void UpdateSavedMap(MapEditorSaveData saveData)
		{
			string currentTime = DateTime.Now.ToString("yyyyMMddHHmmss");
			int index = savedMaps.FindIndex(x=>x.saveName == saveData.saveName);
			if (index >= 0)
			{
				//Map existed, proceed to change the modified time
				savedMaps[index].modifiedDate = DateTime.Now.ToString("yyyy-MM-dd");
				savedMaps[index].modifiedTimeStamp = currentTime;
			}

			SaveMaps();
		}

		public void EditMap(MapEditorSaveData saveData)
		{
			MapEditorManager.Instance.LoadAndDisplayMap(saveData);
		}
	}
}
