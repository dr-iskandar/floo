using UnityEngine;

public enum FoodType
{
	Food = 1,
	Buff = 4
}

public class FoodData
{
	public string foodID;
	public float xPosition;
	public float yPosition;
	public float width;
	public float height;
	public int foodType;
	public string foodKeyword;
	public int colorCode;

	public void EnterDetail(string fdID, float xPos, float yPos, float w, float h, int fdType, string fdKeyword, int cCode)
	{
		foodID = fdID;
		xPosition = xPos;
		yPosition = yPos;
		width = w;
		height = h;
		foodType = fdType;
		foodKeyword = fdKeyword;
		colorCode = cCode;
	}

	public void DebugDetail()
	{
		string debug = string.Format ("Food Debug \nFoodID: {0}\nX Position: {1}\nY Position: {2}\nFood Type: {3}\nColor Code: {4}",
			foodID,xPosition,yPosition,foodType,colorCode);
		Debug.Log (debug);
	}
}