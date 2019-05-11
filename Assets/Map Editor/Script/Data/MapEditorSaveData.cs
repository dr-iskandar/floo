using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProjectMiniJSON;

namespace FlooMapEditor
{
	public class MapEditorSaveData
	{
		public string saveName;
		public string mapName;
		public int width;
		public int height;
		public string createdDate;
		public string modifiedDate;
		public string modifiedTimeStamp;

		private const string TAG_SAVE_NAME = "save_name";
		private const string TAG_MAP_NAME = "map_name";
		private const string TAG_WIDTH = "width";
		private const string TAG_HEIGHT = "height";
		private const string TAG_CREATE_DATE = "create_date";
		private const string TAG_MODIFY_DATE = "modify_date";
		private const string TAG_MODIFY_TIMESTAMP = "modify_timestamp";

		public MapEditorSaveData()
		{
			
		}

		public MapEditorSaveData(Dictionary<string,object> rawData)
		{
			saveName = JsonUtility.GetString(rawData, TAG_SAVE_NAME);
			mapName = JsonUtility.GetString(rawData, TAG_MAP_NAME);
			width = JsonUtility.GetInt(rawData, TAG_WIDTH);
			height = JsonUtility.GetInt(rawData, TAG_HEIGHT);
			createdDate = JsonUtility.GetString(rawData, TAG_CREATE_DATE);
			modifiedDate = JsonUtility.GetString(rawData, TAG_MODIFY_DATE);
			modifiedTimeStamp = JsonUtility.GetString(rawData, TAG_MODIFY_TIMESTAMP);
		}

		public Dictionary<string,object> ToDictionary()
		{
			Dictionary<string,object> result = new Dictionary<string, object>();
			result[TAG_SAVE_NAME] = saveName;
			result[TAG_MAP_NAME] = mapName;
			result[TAG_WIDTH] = width;
			result[TAG_HEIGHT] = height;
			result[TAG_CREATE_DATE] = createdDate;
			result[TAG_MODIFY_DATE] = modifiedDate;
			result[TAG_MODIFY_TIMESTAMP] = modifiedTimeStamp;

			return result;
		}
	}
}