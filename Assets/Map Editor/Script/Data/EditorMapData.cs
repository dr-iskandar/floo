using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FlooMapEditor
{
	public class EditorMapData 
	{
		public string mapName;
		//The width and height of the map
		public int width;
		public int height;
		//The list of all tiles
		public List<EditorTileData> listTile;
		//The list of all map feature
		public List<EditorMapFeatureData> listFeature;
		//The list of all spawner
		public List<EditorSpawnerData> listSpawner;

		private const string TAG_NAME = "map_name";
		private const string TAG_WIDTH = "map_width";
		private const string TAG_HEIGHT = "map_height";
		private const string TAG_TILES = "tiles";
		private const string TAG_LINE = "borderline";
		private const string TAG_FEATURE = "map_feature";
		private const string TAG_SPAWNER = "spawner";


		public EditorMapData()
		{
			listTile = new List<EditorTileData>();
			listFeature = new List<EditorMapFeatureData>();
			listSpawner = new List<EditorSpawnerData>();
		}

		public EditorMapData(Dictionary<string,object> rawData)
		{
			mapName = JsonUtility.GetString(rawData, TAG_NAME);
			width = JsonUtility.GetInt(rawData, TAG_WIDTH);
			height = JsonUtility.GetInt(rawData, TAG_HEIGHT);
			//Load tiles from given dictionary
			listTile = new List<EditorTileData>();
			if (rawData.ContainsKey(TAG_TILES))
			{
				var tiles = rawData[TAG_TILES] as List<object>;
				for (int i = 0; i < tiles.Count; i++)
				{
					var rawTile = tiles[i] as Dictionary<string,object>;
					EditorTileData tileData = new EditorTileData(rawTile);
					listTile.Add(tileData);
				}
			}
			//Load map features from given dictionary
			listFeature = new List<EditorMapFeatureData>();
			if (rawData.ContainsKey(TAG_FEATURE))
			{
				var features = rawData[TAG_FEATURE] as List<object>;
				for (int i = 0; i < features.Count; i++)
				{
					var rawFeature = features[i] as Dictionary<string,object>;
					EditorMapFeatureData featureData = new EditorMapFeatureData(rawFeature);
					listFeature.Add(featureData);
				}
			}

			//Load saved spawner data
			listSpawner = new List<EditorSpawnerData>();
			if (rawData.ContainsKey(TAG_SPAWNER))
			{
				var spawners = rawData[TAG_SPAWNER] as List<object>;
				for (int i = 0; i < spawners.Count; i++)
				{
					var rawSpanwer = spawners[i] as Dictionary<string,object>;
					EditorSpawnerData spawnerData = new EditorSpawnerData(rawSpanwer);
					listSpawner.Add(spawnerData);
				}
			}
		}

		public Dictionary<string,object> ToDictionary()
		{
			Dictionary<string,object> result = new Dictionary<string, object>();
			result[TAG_NAME] = mapName;
			result[TAG_WIDTH] = width;
			result[TAG_HEIGHT] = height;
			List<object> tiles = new List<object>();
			foreach (EditorTileData data in listTile)
			{
				tiles.Add(data.ToDictionary());
			}
			result[TAG_TILES] = tiles;

			List<object> features = new List<object>();
			foreach (EditorMapFeatureData data in listFeature)
			{
				features.Add(data.ToDictionary());
			}
			result[TAG_FEATURE] = features;

			List<object> spawners = new List<object>();
			foreach (EditorSpawnerData data in listSpawner)
			{
				spawners.Add(data.ToDictionary());
			}
			result[TAG_SPAWNER] = spawners;

			List<object> lines = new List<object>();
			result[TAG_LINE] = lines;

			return result;
		}
	}
}
