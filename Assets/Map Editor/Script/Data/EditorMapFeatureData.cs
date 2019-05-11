using UnityEngine;
using System.Collections.Generic;

namespace FlooMapEditor
{
	public enum MapFeatureTypeEditor
	{
		HidingPlace = 1,
		Obstacle = 2,
		InstantKill = 3,
		Portal = 4
	}

	public class EditorMapFeatureData : EditorTileData
	{
		public int mapFeatureType;

		public float radius;

		// portal :
		public string portalTag;
		// portal entrance only :
		public int portalLevelMin;
		public int portalLevelMax;

		private const string TAG_ASSET_CODE = "asset_code";
		private const string TAG_ANGLE = "angle";
		private const string TAG_X_POSITION = "x_position";
		private const string TAG_Y_POSITION = "y_position";
		private const string TAG_MAP_FEATURE_TYPE = "map_feature_type";
		private const string TAG_RADIUS = "radius";

		public EditorMapFeatureData()
		{

		}

		public EditorMapFeatureData(Dictionary<string,object> rawData)
		{
			assetCode = JsonUtility.GetString(rawData, TAG_ASSET_CODE);
			angle = JsonUtility.GetFloat(rawData, TAG_ANGLE);
			mapXPos = JsonUtility.GetFloat(rawData, TAG_X_POSITION);
			mapYPos = JsonUtility.GetFloat(rawData, TAG_Y_POSITION);
			mapFeatureType = JsonUtility.GetInt(rawData, TAG_MAP_FEATURE_TYPE);
			radius = JsonUtility.GetFloat(rawData, TAG_RADIUS);
		}

		public Dictionary<string,object> ToDictionary()
		{
			Dictionary<string,object> result = new Dictionary<string, object>();
			result[TAG_ASSET_CODE] = assetCode;
			result[TAG_ANGLE] = angle;
			result[TAG_X_POSITION] = mapXPos;
			result[TAG_Y_POSITION] = mapYPos;
			result[TAG_MAP_FEATURE_TYPE] = mapFeatureType;
			result[TAG_RADIUS] = radius;
			return result;
		}
	}
}