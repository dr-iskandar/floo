using UnityEngine;
using System.Collections.Generic;

public class FishBodyData
{
	public float xPosition;
	public float yPosition;
	public float width;
	public float height;
}

public class FishData
{
	public string playerID;
	public string playerName;
	public float xPosition;
	public float yPosition;
	public float fishAngle;
	public int colorCode;
	public string fishSkin;
	public int level;
	public float width;
	public float height;
	public int buffType;
	public float buffTime;
	public int isInPredatorRange;
	public bool isTargetedByPredator;
	public bool isBoost;

	//player fish only
	public float experience;
	public float topY;
	public float bottomY;
	public float leftX;
	public float rightX;
	public float viewWidth;
	public float viewHeight;
	public float boostValue;

	public List<FishBodyData> bodies = new List<FishBodyData>();

//	public void EnterOpponentDetail(string pID, string pName, float xPos, float yPos, float fAngle, int cCode, int fType, int lvl, float w, float h)
//	{
//		playerID= pID;
//		playerName = pName;
//		xPosition = xPos;
//		yPosition = yPos;
//		fishAngle = fAngle;
//		colorCode = cCode;
//		fishSkin = fType;
//		level = lvl;
//		width = w;
//		height = h;
//	}
//
//	public void EnterPlayerDetail(string pID, string pName, float xPos, float yPos, float fAngle, int cCode, int fType, int lvl, float w, float h, float exp, float tY, float bY, float lX, float rX,
//		float vw, float vh)
//	{
//		playerID= pID;
//		playerName = pName;
//		xPosition = xPos;
//		yPosition = yPos;
//		fishAngle = fAngle;
//		colorCode = cCode;
//		fishSkin = fType;
//		level = lvl;
//		width = w;
//		height = h;
//		experience = exp;
//		topY = tY;
//		bottomY = bY;
//		rightX = rX;
//		leftX = lX;
//		viewWidth = vw;
//		viewHeight = vh;
//	}

	public void DebugDetail(bool isPlayer)
	{
		string debug;
		if (isPlayer) {
			debug = string.Format ("Player Debug:\nPlayerID: {0}\nPlayerName: {1}\nX Position: {2}\nY Position: {3}\nFish Angle: {4}\nColor Code: {5}\nFish Type: {6}\nLevel: {7}\nWidth: {8}\nHeight: {9}\nExperience: {10}\nTop Y: {11}\nBottom Y: {12}\nRight X: {13}\nLeft X: {14}",
				playerID, playerName, xPosition, yPosition, fishAngle, colorCode, fishSkin, level, width, height, experience, topY, bottomY, leftX, rightX);
		} else {
			debug = string.Format ("Player Debug:\nPlayerID: {0}\nPlayerName: {1}\nX Position: {2}\nY Position: {3}\nFish Angle: {4}\nColor Code: {5}\nFish Type: {6}\nLevel: {7}\nWidth: {8}\nHeight: {9}",
				playerID, playerName, xPosition, yPosition, fishAngle, colorCode, fishSkin, level, width, height);
		}
		Debug.Log (debug);
	}

}