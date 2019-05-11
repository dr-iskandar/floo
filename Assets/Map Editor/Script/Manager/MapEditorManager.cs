using UnityEngine;
using System.Collections;

namespace FlooMapEditor
{
	public class MapEditorManager : MonoBehaviour 
	{
		private static MapEditorManager instance;

		public static MapEditorManager Instance
		{
			get
			{
				return instance;
			}
		}

		public Camera gameCamera;
		public EditorTileManager tileManager;
		public MapCreatorManager creatorManager;
		public MapEditorMenu mapEditorMenu;

		private string editedMapName;
		private bool isMapLoaded = false;
		private float mapWidth;
		private float mapHeight;
		private MapEditorSaveData referenceSaveData;

		void Awake()
		{
			instance = this;
			creatorManager.gameObject.SetActive(true);
			mapEditorMenu.gameObject.SetActive(false);
			editedMapName = "";
		}

		void Start()
		{
			
		}

		void Update()
		{
			if (isMapLoaded)
			{
				if (Input.GetKeyDown(KeyCode.A))
				{
					gameCamera.transform.Translate(Vector3.left);
				}
				else if (Input.GetKeyDown(KeyCode.D))
				{
					gameCamera.transform.Translate(Vector3.right);
				}
				else if (Input.GetKeyDown(KeyCode.W))
				{
					gameCamera.transform.Translate(Vector3.up);
				}
				else if (Input.GetKeyDown(KeyCode.S))
				{
					gameCamera.transform.Translate(Vector3.down);
				}
			}
		}

		void LateUpdate()
		{
			float x = Mathf.Clamp(gameCamera.transform.position.x, 0, mapWidth);
			float y = Mathf.Clamp(gameCamera.transform.position.y, 0, mapHeight);
			gameCamera.transform.position = new Vector3(x,y,0);
		}

		public void LoadAndDisplayMap(MapEditorSaveData saveData)
		{
			string saveName = saveData.saveName;
			try
			{
				EditorMapData mapData = MapEditorUtility.LoadMap(saveName);
				tileManager.GenerateMap(mapData);
				mapWidth = mapData.width;
				mapHeight = mapData.height;
				creatorManager.gameObject.SetActive(false);
				mapEditorMenu.ShowMainMenu(mapData.mapName);
				editedMapName = saveName;
				referenceSaveData = saveData;
				isMapLoaded = true;
			}
			catch(System.Exception e)
			{
				Debug.Log("Unable to load map " + e.StackTrace);
			}
		}

		public void SaveMap()
		{
			EditorMapData mapData = tileManager.RegenerateMapData();
			MapEditorUtility.SaveMap(editedMapName, mapData);
			creatorManager.UpdateSavedMap(referenceSaveData);
			mapEditorMenu.ShowMessage();
		}

		public void CloseMapEditorMenu()
		{
			tileManager.ClearTiles();
			creatorManager.gameObject.SetActive(true);
			mapEditorMenu.HideMainMenu();
			creatorManager.LoadSavedMaps();
			creatorManager.DisplaySavedMaps();
		}
	}
}