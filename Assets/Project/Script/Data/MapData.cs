using UnityEngine;

public enum MapFeatureType
{
	HidingPlace = 1,
	Obstacle = 2,
	InstantKill = 3
}

public class MapData
{
	public string mapFeatureId;
	public float mapXPos;
	public float mapYPos;
	public float width;
	public float height;
	public int mapFeatureType;
	public string mapFeatureKeyword;

	public void DebugDetail()
	{
		string debug = string.Format ("Map Data Debug \nX Position: {0}\nY Position: {1}\nMap Feature Type: {2}\nMap Feature Keyword: {3}",
			mapXPos,mapYPos,mapFeatureType,mapFeatureKeyword);
		Debug.Log (debug);
	}
}