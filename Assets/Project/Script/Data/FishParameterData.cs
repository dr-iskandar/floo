using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishParameterData
{
	public int level;
	public float headSize;		//2X radius
	public List<float> bodiesSize;	//List bodies 
	public float totalSize;		//The total size of the fish head + bodies
	public float cameraSize;	//The ortographic camera size for each level
	public float viewWidth;
	public float viewHeight;
	public float maxExp;

	private const string TAG_LEVEL = "level";
	private const string TAG_HEAD = "head";
	private const string TAG_BODIES = "bodies";
	private const string TAG_CAMERA_SIZE = "camera_size";
	private const string TAG_VIEW_WIDTH = "view_width";
	private const string TAG_VIEW_HEIGHT = "view_height";
	private const string TAG_MAX_EXP = "max_exp";

	public FishParameterData()
	{
		
	}

	public FishParameterData(Dictionary<string,object> rawData)
	{
		level = JsonUtility.GetInt(rawData, TAG_LEVEL);
		headSize = 2.0f * JsonUtility.GetFloat(rawData, TAG_HEAD) / EssentialData.scaleFactor ;		//The given data is radius. Needs to times it by 2 to get size
		totalSize = headSize;
		bodiesSize = new List<float>();
		viewWidth = JsonUtility.GetFloat(rawData,TAG_VIEW_WIDTH) / EssentialData.scaleFactor;
		viewHeight = JsonUtility.GetFloat(rawData,TAG_VIEW_HEIGHT) / EssentialData.scaleFactor;
		var listBody = rawData[TAG_BODIES] as List<object>;
		for (int i = 0; i < listBody.Count; i++)
		{
			float size = float.Parse(listBody[i].ToString()) * 2.0f / EssentialData.scaleFactor ;
			bodiesSize.Add(size);
			totalSize += size;
		}

		cameraSize = JsonUtility.GetFloat(rawData, TAG_CAMERA_SIZE);
		maxExp = JsonUtility.GetInt (rawData, TAG_MAX_EXP);
	}
}
