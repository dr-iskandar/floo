using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ProjectMiniJSON;

namespace FlooMapEditor
{
	public class MapEditorUtility
	{
		public const string DEFAULT_TILE = "g_00";
		public const string DEFAULT_TILE_BORDER = "g_border";
		public const string DEFAULT_TILE_CORNER = "g_corner";
		private const float TILE_RADIUS = 0.5f;

		public static EditorMapData CreateDefaultMap(MapEditorSaveData data)
		{
			EditorMapData basicMap = new EditorMapData();
			basicMap.mapName = data.mapName;
			basicMap.width = data.width;
			basicMap.height = data.height;
			//Create the default width x height tile with 0 angle
			//Also create it with the default asset for empty tile
			for (int x = 0; x < data.width; x++)
			{
				for (int y = 0; y < data.height; y++)
				{
					EditorTileData tileData = new EditorTileData();
					tileData.angle = 0;
					tileData.assetCode = DEFAULT_TILE;		//HARDCODED This is empty tile
					tileData.mapXPos = x + TILE_RADIUS;
					tileData.mapYPos = y + TILE_RADIUS;
					basicMap.listTile.Add(tileData);
				}
			}
			//Create the border for maps
			//Create TOP Border
			for (int x = 0; x < data.width; x++)
			{
				EditorTileData tileData = new EditorTileData();
				tileData.angle = 0;
				tileData.assetCode = DEFAULT_TILE_BORDER;		
				tileData.mapXPos = x + TILE_RADIUS;
				tileData.mapYPos = data.height + TILE_RADIUS;
				basicMap.listTile.Add(tileData);
			}

			//Create Bottom Border
			for (int x = 0; x < data.width; x++)
			{
				EditorTileData tileData = new EditorTileData();
				tileData.angle = 180;
				tileData.assetCode = DEFAULT_TILE_BORDER;		
				tileData.mapXPos = x + TILE_RADIUS;
				tileData.mapYPos = -TILE_RADIUS;
				basicMap.listTile.Add(tileData);
			}

			//Create Left Border
			for (int y = 0; y < data.height; y++)
			{
				EditorTileData tileData = new EditorTileData();
				tileData.angle = 90;
				tileData.assetCode = DEFAULT_TILE_BORDER;		
				tileData.mapXPos = -TILE_RADIUS;
				tileData.mapYPos = y + TILE_RADIUS;
				basicMap.listTile.Add(tileData);
			}

			//Create Right Border
			for (int y = 0; y < data.height; y++)
			{
				EditorTileData tileData = new EditorTileData();
				tileData.angle = 270;
				tileData.assetCode = DEFAULT_TILE_BORDER;		
				tileData.mapXPos = data.width + TILE_RADIUS;
				tileData.mapYPos = y + TILE_RADIUS;
				basicMap.listTile.Add(tileData);
			}

			//Create corner border for the map
			for (int x = 0; x < 2; x++)
			{
				for (int y = 0; y < 2; y++)
				{
					EditorTileData tileData = new EditorTileData();
					tileData.assetCode = DEFAULT_TILE_CORNER;
					if (x == 0 && y == 0)		//LEFT BOTTOM CORNER
					{
						tileData.angle = 90;
						tileData.mapXPos = -TILE_RADIUS;
						tileData.mapYPos = -TILE_RADIUS;
					}
					else if (x == 0 && y == 1) 	//LEFT TOP CORNER
					{
						tileData.angle = 0;
						tileData.mapXPos = -TILE_RADIUS;
						tileData.mapYPos = data.height + TILE_RADIUS;
					}
					else if (x == 1 && y == 0)	//RIGHT BOTTOM CORNER
					{
						tileData.angle = 180;
						tileData.mapXPos = data.width + TILE_RADIUS;
						tileData.mapYPos = -TILE_RADIUS;
					}
					else
					{
						tileData.angle = 270;	//RIGHT TOP CORNER
						tileData.mapXPos = data.width + TILE_RADIUS;
						tileData.mapYPos = data.height + TILE_RADIUS;
					}

					basicMap.listTile.Add(tileData);
				}
			}

			//Basic map doesn't have map feature nor spawners
			return basicMap;
		}

		public static bool SaveMap(string saveName, EditorMapData mapData)
		{
			string path = Application.persistentDataPath + "/" + saveName;
			FileInfo f = new FileInfo(path);
			if (f.Exists)
			{
				f.Delete();
			}

			try
			{
				StreamWriter sw = f.CreateText();
				string jsonText = Json.Serialize(mapData.ToDictionary());
				sw.WriteLine(jsonText);
				sw.Close();
				return true;
			}
			catch(System.Exception e)
			{
				Debug.Log("Error write file " + e.Message);
				return false;
			}
		}

		public static EditorMapData LoadMap(string saveName)
		{
			string path = Application.persistentDataPath + "/" + saveName;
			FileInfo f = new FileInfo(path);
			if (!f.Exists)
			{
				Debug.Log("File not found");
				return null;
			}

			try
			{
				StreamReader sr = f.OpenText();
				string jsonMessage = sr.ReadToEnd();
				var rawData = Json.Deserialize(jsonMessage) as Dictionary<string,object>;
				EditorMapData data = new EditorMapData(rawData);
				return data;
			}
			catch(System.Exception e)
			{
				Debug.Log("Fail Load Map " + e.Message);
				return null;
			}
		}
	}
}
