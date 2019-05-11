using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodObjectPooling : GameObjectPoolManager 
{
	private static FoodObjectPooling instance;
	
	public static FoodObjectPooling Instance {
		get {
			if(instance == null)
			{
				GameObject go = new GameObject("Food Object Pooling");
				instance = go.AddComponent<FoodObjectPooling>();
				DontDestroyOnLoad(go);
				instance.Init();
			}
			return instance;
		}
	}

	public override void InitializePool()
	{
		List<string> foodList = new List<string>();
		List<int> foodPoolSize = new List<int>();
		foodList.Add("food_pellet");
		foodPoolSize.Add(50);
		foodList.Add("buff_1");
		foodPoolSize.Add(10);
		foodList.Add("buff_2");
		foodPoolSize.Add(10);
		foodList.Add("buff_3");
		foodPoolSize.Add(10);
		foodList.Add("buff_4");
		foodPoolSize.Add(10);
		foodList.Add("buff_5");
		foodPoolSize.Add(10);
		foodList.Add("buff_6");
		foodPoolSize.Add(10);

		for (int i = 0; i < foodList.Count; i++)
		{
			CreatePool(foodList[i], foodPoolSize[i]);
		}
	}
}
