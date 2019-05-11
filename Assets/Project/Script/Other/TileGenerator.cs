using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileGenerator : MonoBehaviour 
{
	private static TileGenerator instance;

	public static TileGenerator Instance
	{
		get
		{
			return instance;
		}
	}

	public Transform parentTile;
	public Transform parentWaterRipple;
	public GameObject waterRipplePrefab;
	public float rippleSize = 2.0f;

	private bool isTileLoaded = false;

	// Use this for initialization
	void Awake () 
	{
		instance = this;
		isTileLoaded = false;
	}

	public void LoadAndShowMap()
	{
		StartCoroutine(GenerateMapTilesAndFeature());
	}

	IEnumerator GenerateMapTilesAndFeature()
	{
		if (!isTileLoaded)
		{
			//TODO Load map given by data here
			string mapName = "simple_map";
			TextAsset textAsset = Resources.Load("Text/" + mapName) as TextAsset;
			string jsonText = textAsset.text;
			var jsonData = ProjectMiniJSON.Json.Deserialize(jsonText) as Dictionary<string,object>;

			FlooMapEditor.EditorMapData mapData = new FlooMapEditor.EditorMapData(jsonData);

			UIGameController.Instance.ShowNotificationMessage(LanguageManager.Instance.GetMessage("LOA0009"));
			yield return new WaitForSeconds(0.1f);
			//Load all tiles here
			for (int i = 0; i < mapData.listTile.Count; i++)
			{
				FlooMapEditor.EditorTileData tileData = mapData.listTile[i];
				Object prefab = AssetManager.Instance.GetPrefabByKeyword(tileData.assetCode);
				GameObject go = Instantiate(prefab) as GameObject;
				go.transform.SetParent(parentTile);
				go.transform.localPosition = new Vector3(tileData.mapXPos, tileData.mapYPos, 0);
				go.transform.localEulerAngles = new Vector3(0, 0, tileData.angle);
				go.transform.localScale = Vector3.one;
			}

			UIGameController.Instance.ShowNotificationMessage(LanguageManager.Instance.GetMessage("LOA0010"));
			yield return new WaitForSeconds(0.1f);
			//Load all map features here
			for (int i = 0; i < mapData.listFeature.Count; i++)
			{
				FlooMapEditor.EditorMapFeatureData mapFeature = mapData.listFeature[i];
				GameObject go = MapFeatureObjectPooling.Instance.GetGameObject(mapFeature.assetCode);

				if (mapFeature.mapFeatureType == (int)FlooMapEditor.MapFeatureTypeEditor.HidingPlace)
				{
					LayerController.Instance.SetSeaweedLayer(go);
				}
				else
				{
					LayerController.Instance.SetObstacleLayer(go);
				}

				go.transform.localPosition = new Vector3(mapFeature.mapXPos, mapFeature.mapYPos, 0);
				go.transform.localEulerAngles = new Vector3(0, 0, mapFeature.angle);
				go.transform.localScale = new Vector3(mapFeature.radius * 2, mapFeature.radius * 2, 1.0f);

				HidingPlaceClusterManager hidingCluster = go.GetComponent<HidingPlaceClusterManager>();
				if (hidingCluster != null)
				{
					//Stabilize the hiding cluster
					hidingCluster.StabilizeHidingPlaces();
					hidingCluster.Init(true);
				}
			}

			UIGameController.Instance.ShowNotificationMessage(LanguageManager.Instance.GetMessage("LOA0011"));
			yield return new WaitForSeconds(0.1f);

			//Place water ripple here
			for (float x = -1.0f; x < mapData.width + 1.0f; x += rippleSize)
			{
				for (float y = -1.0f; y < mapData.height + 1.0f; y += rippleSize)
				{
					GameObject rippleObj = Instantiate(waterRipplePrefab) as GameObject;
					rippleObj.transform.SetParent(parentWaterRipple);
					rippleObj.transform.localPosition = new Vector3(x, y, 0);
					rippleObj.transform.localScale = new Vector3(rippleSize, rippleSize, 1.0f);
				}
			}


			OcculsionManager.Instance.DisableAllMapFeature();
			isTileLoaded = true;
		}
		DisplayGameController.Instance.StartObjectPooling();
		
		yield return true;
	}

}
