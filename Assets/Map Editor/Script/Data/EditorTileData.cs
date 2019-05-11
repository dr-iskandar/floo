using UnityEngine;
using System.Collections.Generic;
using ProjectMiniJSON;

namespace FlooMapEditor
{
	public enum TileOptionType
	{
		Grid = 1,
		MapFeature = 2,
		Spawner = 3,
		Predator = 4
	}

	[System.Serializable]
	public class EditorTileData
	{
		public string assetCode;
		public string assetKeyword;

		public float angle;

		public float mapXPos;
		public float mapYPos;

		public float width;
		public float height;

		public int tileOptionType;

		private const string TAG_ASSET_CODE = "asset_code";
		private const string TAG_ANGLE = "angle";
		private const string TAG_X_POSITION = "x_position";
		private const string TAG_Y_POSITION = "y_position";


		public EditorTileData()
		{
			
		}

		public EditorTileData(Dictionary<string,object> rawData)
		{
			assetCode = JsonUtility.GetString(rawData, TAG_ASSET_CODE);
			angle = JsonUtility.GetFloat(rawData, TAG_ANGLE);
			mapXPos = JsonUtility.GetFloat(rawData, TAG_X_POSITION);
			mapYPos = JsonUtility.GetFloat(rawData, TAG_Y_POSITION);
		}

		public virtual Dictionary<string,object> ToDictionary()
		{
			Dictionary<string,object> result = new Dictionary<string, object>();
			result[TAG_ASSET_CODE] = assetCode;
			result[TAG_ANGLE] = angle;
			result[TAG_X_POSITION] = mapXPos;
			result[TAG_Y_POSITION] = mapYPos;
			return result;
		}
	}
}
