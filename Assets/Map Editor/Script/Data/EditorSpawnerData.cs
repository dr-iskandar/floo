using UnityEngine;
using System.Collections.Generic;

namespace FlooMapEditor
{
	public enum SpawnerTypeEditor
	{
		Food = 1,
		PowerUp = 2
	}
	public enum SpawnerMethodEditor
	{
		Point = 1,
		Line = 2
	}

	public enum BuffTypeEditor
	{
		Invulnerable = 1,
		Experience = 2,
		Invisible = 3,
		Speed = 4,
		StarPower = 5,
		Gold = 6
	}

	public class SpawnItemData
	{
		public int buffType;

		private const string TAG_BUFF_TYPE = "buff_type";

		public SpawnItemData()
		{
			
		}

		public SpawnItemData(Dictionary<string,object> rawData)
		{
			buffType = JsonUtility.GetInt(rawData, TAG_BUFF_TYPE);
		}

		public Dictionary<string,object> ToDictionary()
		{
			Dictionary<string,object> result = new Dictionary<string, object>();
			result[TAG_BUFF_TYPE] = buffType;
			return result;
		}
	}

	public class EditorSpawnerData : EditorTileData
	{
		public int spawnerType;		// 1. Food 2. Buff
		public int spawnerMethod;	// 1. Point 2. Line

		public float radius; 		// the unit from backend (1/100 unit)
		public int angle;

		public int spawnInterval; // seconds
		public int spawnRate;

		public int maxSpawn;

		public Vector2 spawnStartPos;
		public Vector2 spawnEndPos;

		public List<SpawnItemData> spawnItem = new List<SpawnItemData>();

		private const string TAG_ASSET_CODE = "asset_code";
		private const string TAG_SPAWNER_TYPE = "spawner_type";
		private const string TAG_SPAWN_METHOD = "spawn_method";
		private const string TAG_RADIUS = "radius";
		private const string TAG_ANGLE = "angle";
		private const string TAG_INTERVAL = "spawn_interval";
		private const string TAG_SPAWN_RATE = "spawn_rate";
		private const string TAG_MAX = "max_spawn";
		private const string TAG_POSITION = "spawner_position";
		private const string TAG_ITEM = "spawn_item";

		public EditorSpawnerData()
		{
			
		}

		public EditorSpawnerData(Dictionary<string,object> rawData)
		{
			assetCode = JsonUtility.GetString(rawData, TAG_ASSET_CODE);
			spawnerType = JsonUtility.GetInt(rawData, TAG_SPAWNER_TYPE);
			spawnerMethod = JsonUtility.GetInt(rawData, TAG_SPAWN_METHOD);
			radius = JsonUtility.GetFloat(rawData, TAG_RADIUS);
			angle = JsonUtility.GetInt(rawData, TAG_ANGLE);
			spawnInterval = JsonUtility.GetInt(rawData, TAG_INTERVAL);
			spawnRate = JsonUtility.GetInt(rawData, TAG_SPAWN_RATE);
			maxSpawn = JsonUtility.GetInt(rawData, TAG_MAX);

			string position = JsonUtility.GetString(rawData, TAG_POSITION);
			string[] points = position.Split(';');
			for (int i = 0; i < points.Length; i++)
			{
				string[] axis = points[i].Split(',');
				Vector2 pos = new Vector2(float.Parse(axis[0]), float.Parse(axis[1]));
				if (i == 0)
				{
					spawnStartPos = pos;
				}
				else
				{
					spawnEndPos = pos;
				}
			}

			spawnItem = new List<SpawnItemData>();
			if (rawData.ContainsKey(TAG_ITEM))
			{
				var listItemObject = rawData[TAG_ITEM] as List<object>;
				for (int i = 0; i < listItemObject.Count; i++)
				{
					var itemDictionary = listItemObject[i] as Dictionary<string,object>;
					SpawnItemData itemData = new SpawnItemData(itemDictionary);
					spawnItem.Add(itemData);
				}
			}
		}

		public override Dictionary<string, object> ToDictionary()
		{
			Dictionary<string,object> result = new Dictionary<string, object>();
			result[TAG_ASSET_CODE] = assetCode;
			result[TAG_SPAWNER_TYPE] = spawnerType;
			result[TAG_SPAWN_METHOD] = spawnerMethod;
			result[TAG_RADIUS] = radius;
			result[TAG_ANGLE] = angle;
			result[TAG_INTERVAL] = spawnInterval;
			result[TAG_SPAWN_RATE] = spawnRate;
			result[TAG_MAX] = maxSpawn;
			string position = spawnStartPos.x + "," + spawnStartPos.y + ";" + spawnEndPos.x + "," + spawnEndPos.y;
			result[TAG_POSITION] = position;
			List<object> items = new List<object>();
			for (int i = 0; i < spawnItem.Count; i++)
			{
				items.Add(spawnItem[i].ToDictionary());
			}
			result[TAG_ITEM] = items;
			return result;
		}
	}
}
