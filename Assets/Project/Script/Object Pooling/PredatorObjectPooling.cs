using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PredatorObjectPooling : GameObjectPoolManager 
{
	private static PredatorObjectPooling instance;
	
	public static PredatorObjectPooling Instance {
		get {
			if(instance == null)
			{
				GameObject go = new GameObject("Predator Object Pooling");
				instance = go.AddComponent<PredatorObjectPooling>();
				DontDestroyOnLoad(go);
				instance.Init();
			}
			return instance;
		}
	}

	public override void InitializePool()
	{
		List<string> keywordList = new List<string>();
		List<int> keywordPoolSize = new List<int>();
		keywordList.Add("squid");
		keywordPoolSize.Add(5);
		keywordList.Add("shark");
		keywordPoolSize.Add(5);
		for (int i = 0; i < keywordList.Count; i++)
		{
			CreatePool(keywordList[i], keywordPoolSize[i]);
		}
	}
}
