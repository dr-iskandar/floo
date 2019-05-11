using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PredatorParameterData
{
	public string skinName;
	public float size;

	private const string TAG_SKIN = "skin";
	private const string TAG_RADIUS = "radius";

	public PredatorParameterData(Dictionary<string,object> rawData)
	{
		skinName = JsonUtility.GetString(rawData, TAG_SKIN);
		size = 2 * JsonUtility.GetFloat(rawData, TAG_RADIUS) / EssentialData.scaleFactor;
	}
}
