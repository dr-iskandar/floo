using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace FlooMapEditor
{
	public class EditorTileManager : MonoBehaviour 
	{
		private static EditorTileManager instance;

		public static EditorTileManager Instance
		{
			get
			{
				return instance;
			}
		}

		// LINE 
		public Material lineMat;
		public bool drawBorderlineRuntime = true;

		// Parent for contents
		public Transform gridParent;
		public Transform mapFeatureParent;
		public Transform spawnerParent;

		// Map Feature Spawn Detection
		public MapEditorFeatureSpawnDetection featureSpawnDetector;

		// Spawner Spawn Detection
		public MapEditorSpawnerSpawnDetection spawnerSpawnDetection;

		private float gridSize = 1.0f;

		private EditorMapData referenceData = null;

		// Use this for initialization
		void Awake () 
		{
			instance = this;
		}

		void Start()
		{
			HideEditorHighlight();
			DisableMapFeatureSpawnDetection();
		}

		#region Enable disable Map Feature
		public void EnableMapFeatureSpawnDetection()
		{
			featureSpawnDetector.Activate();
		}

		public void DisableMapFeatureSpawnDetection()
		{
			featureSpawnDetector.Deactive();
		}

		public void EnableMapFeatureCollider()
		{
			int count = mapFeatureParent.childCount;
			for (int i = 0; i < count; i++)
			{
				mapFeatureParent.GetChild(i).gameObject.GetComponent<Collider>().enabled = true;
			}
		}

		public void DisableMapFeatureCollider()
		{
			int count = mapFeatureParent.childCount;
			for (int i = 0; i < count; i++)
			{
				mapFeatureParent.GetChild(i).gameObject.GetComponent<Collider>().enabled = false;
			}
		}
		#endregion

		#region Enable Disable Spawner
		public void EnableSpawnerSpawnDetection()
		{
			spawnerSpawnDetection.Activate();
		}

		public void DisableSpawnerSpawnDetection()
		{
			spawnerSpawnDetection.Deactive();
		}

		public void EnableSpawnerCollider()
		{
			int count = spawnerParent.childCount;
			for (int i = 0; i < count; i++)
			{
				spawnerParent.GetChild(i).gameObject.GetComponent<Collider>().enabled = true;
			}
		}

		public void DisableSpawnerCollider()
		{
			int count = spawnerParent.childCount;
			for (int i = 0; i < count; i++)
			{
				spawnerParent.GetChild(i).gameObject.GetComponent<Collider>().enabled = false;
			}
		}
		#endregion

		public void GenerateMap(EditorMapData mapData)
		{
			ClearTiles();
			referenceData = mapData;
			//Generate grid
			GenerateTile(mapData.listTile);
			//Generate map feature and establish the spawn detection
			GenerateMapFeature(mapData.listFeature);
			featureSpawnDetector.SetMapData(mapData);
			//Generate spawner and establish the spawn detection
			GenerateSpawner(mapData.listSpawner);
			spawnerSpawnDetection.SetMapData(mapData);
		}

		public void GenerateTile(List<EditorTileData> listTile)
		{
			for (int i = 0; i < listTile.Count; i++)
			{
				EditorTileData referenceData = listTile[i];
				Object prefab = AssetManager.Instance.GetPrefabByKeyword(referenceData.assetCode);
				GameObject go = Instantiate(prefab) as GameObject;
				go.transform.SetParent(gridParent);
				go.transform.localScale = Vector3.one;
				go.transform.localPosition = new Vector3(referenceData.mapXPos, referenceData.mapYPos, 0);
				go.transform.localEulerAngles = new Vector3(0, 0, referenceData.angle);

				bool isEditable = true;
				//Make so the border and corner can not be edited
				if (referenceData.assetCode.Equals(MapEditorUtility.DEFAULT_TILE_BORDER) ||
				   referenceData.assetCode.Equals(MapEditorUtility.DEFAULT_TILE_CORNER))
				{
					isEditable = false;
				}

				//If this is not the corner or border asset attach script so the gameobject can be edited
				go.AddComponent<MapEditorTileController>().Init(referenceData.assetCode, isEditable);
			}
		}

		void GenerateMapFeature(List<EditorMapFeatureData> features)
		{
			for (int i = 0; i < features.Count; i++)
			{
				EditorMapFeatureData referenceData = features[i];
				Object prefab = AssetManager.Instance.GetPrefabByKeyword(referenceData.assetCode);
				GameObject go = Instantiate(prefab) as GameObject;
				go.transform.SetParent(mapFeatureParent);

				MapEditorFeatureController mfController = go.AddComponent<MapEditorFeatureController>();
				mfController.Init(referenceData);
			}
		}

		void GenerateSpawner(List<EditorSpawnerData> spawners)
		{
			for (int i = 0; i < spawners.Count; i++)
			{
				EditorSpawnerData referenceData = spawners[i];
				Object prefab = AssetManager.Instance.GetPrefabByKeyword(referenceData.assetCode);
				GameObject go = Instantiate(prefab) as GameObject;
				go.transform.SetParent(spawnerParent);

				MapEditorSpawnerController controller = go.AddComponent<MapEditorSpawnerController>();
				controller.Init(referenceData);
			}
		}

		#region Saving data related method
		public EditorMapData RegenerateMapData()
		{
			referenceData.listTile = RegenerateListTileData();
			referenceData.listFeature = RegenerateMapFeatureData();
			referenceData.listSpawner = RegenerateSpawnerData();
			return referenceData;
		}

		private List<EditorTileData> RegenerateListTileData()
		{
			int count = gridParent.childCount;
			List<EditorTileData> tiles = new List<EditorTileData>();
			for (int i = 0; i < count; i++)
			{
				MapEditorTileController controller = gridParent.GetChild(i).gameObject.GetComponent<MapEditorTileController>();
				tiles.Add(controller.GenerateData());
			}
			return tiles;
		}

		private List<EditorMapFeatureData> RegenerateMapFeatureData()
		{
			int count = mapFeatureParent.childCount;
			List<EditorMapFeatureData> mapFeatures = new List<EditorMapFeatureData>();
			for (int i = 0; i < count; i++)
			{
				MapEditorFeatureController controller = mapFeatureParent.GetChild(i).gameObject.GetComponent<MapEditorFeatureController>();
				mapFeatures.Add(controller.GenerateData());
			}
			return mapFeatures;
		}

		private List<EditorSpawnerData> RegenerateSpawnerData()
		{
			int count = spawnerParent.childCount;
			List<EditorSpawnerData> spawnersData = new List<EditorSpawnerData>();
			for (int i = 0; i < count; i++)
			{
				MapEditorSpawnerController controller = spawnerParent.GetChild(i).gameObject.GetComponent<MapEditorSpawnerController>();
				spawnersData.Add(controller.GenerateData());
			}
			return spawnersData;
		}
		#endregion

		public void ClearTiles()
		{
			int count = gridParent.childCount;
			for (int i = count - 1; i >= 0; i--)
			{
				Destroy(gridParent.GetChild(i).gameObject);
			}

			count = mapFeatureParent.childCount;
			for (int i = count - 1; i >= 0; i--)
			{
				Destroy(mapFeatureParent.GetChild(i).gameObject);
			}

			count = spawnerParent.childCount;
			for (int i = count - 1; i >= 0; i--)
			{
				Destroy(spawnerParent.GetChild(i).gameObject);
			}

		}

		#region LINEs
		public void HideEditorHighlight()
		{
			MapEditorHighliterController.Instance.HideHighlight();
		}

		public void ShowEditorHighlight(Vector3 position, float radius = 0.5f)
		{
			MapEditorHighliterController.Instance.ShowHighlight(position,radius);
		}

		public void DrawAllGridLine (MapEditorSaveData data)
		{	
			GL.Clear(true, true, Color.blue);

			// each (wi & he) is +1 line
			lineMat.SetPass(0);

			int mapWidth = data.width;
			int mapHeight = data.height;

			GL.PushMatrix ();

			GL.Begin(GL.LINES);
			GL.Color(new Color(lineMat.color.r, lineMat.color.g, lineMat.color.b, lineMat.color.a));

			for (int x = 0; x <= mapWidth; x++)
			{	
				GL.Vertex3( x*gridSize, 0, 0);
				GL.Vertex3( x *gridSize , mapHeight*gridSize, 0);
			}

			for (int y = 0; y <= mapHeight; y++)
			{
				GL.Vertex3( 0, y*gridSize, 0);
				GL.Vertex3( mapWidth *gridSize , y*gridSize, 0);
			}

			GL.End();
			GL.PopMatrix ();
		}

		#endregion


	}
}
