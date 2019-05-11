using UnityEngine;
using System.Collections;

public class FoodController : MonoBehaviour 
{
	public Material[] colorMaterial;
	[HideInInspector]
	public string foodId;
	public string keyword;

	public void SetData(FoodData data)
	{
		SetFoodId(data.foodID);
		SetPosition(data.xPosition, data.yPosition);
		SetSize(data.width, data.height);
		SetColor();
		keyword = data.foodKeyword;
	}

	private void SetColor()
	{
		if (colorMaterial != null && colorMaterial.Length > 0)
		{
			int number = Random.Range(0, 50);
			int colorIndex = number % colorMaterial.Length;
			this.GetComponent<Renderer>().material = colorMaterial[colorIndex];
		}
	}

	public void SetFoodId(string id)
	{
		foodId = id;
	}

	public void RemoveFood()
	{
		Destroy(this.gameObject);
	}

	public void SetPosition(float x, float y)
	{
		gameObject.transform.localPosition = new Vector2(x, y);
	}

	public void SetSize(float width, float height)
	{
		gameObject.transform.localScale = new Vector2(width, height);
	}

}
