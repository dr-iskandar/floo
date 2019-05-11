using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishObjectPooling : GameObjectPoolManager 
{
	private static FishObjectPooling instance;

	public static FishObjectPooling Instance {
		get {
			if(instance == null)
			{
				GameObject go = new GameObject("Fish Object Pooling");
				instance = go.AddComponent<FishObjectPooling>();
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
		keywordList.Add("nemo");
		keywordPoolSize.Add(10);
		keywordList.Add("dory");
		keywordPoolSize.Add(10);
		keywordList.Add("angler");
		keywordPoolSize.Add(10);
		keywordList.Add("pine");
		keywordPoolSize.Add(10);
		keywordList.Add("orca");
		keywordPoolSize.Add(10);

		keywordList.Add("nemo_gold");
		keywordPoolSize.Add(3);
		keywordList.Add("dory_gold");
		keywordPoolSize.Add(3);
		keywordList.Add("angler_gold");
		keywordPoolSize.Add(3);

		keywordList.Add("nemo_silver");
		keywordPoolSize.Add(3);
		keywordList.Add("dory_silver");
		keywordPoolSize.Add(3);
		keywordList.Add("angler_silver");
		keywordPoolSize.Add(3);

		keywordList.Add("skeleton");
		keywordPoolSize.Add(5);

		keywordList.Add("snowy");
		keywordPoolSize.Add(5);

		keywordList.Add("rudolph");
		keywordPoolSize.Add(5);

		keywordList.Add("flower");
		keywordPoolSize.Add(5);

		keywordList.Add("dragon");
		keywordPoolSize.Add(5);




		for (int i = 0; i < keywordList.Count; i++)
		{
			CreatePool(keywordList[i], keywordPoolSize[i]);
		}

	}

	public override GameObject GetGameObject(string keyword)
	{
		if (poolDictionary.ContainsKey(keyword))
		{
			GameObject go = poolDictionary[keyword].GetGameObject();
			go.GetComponent<FishController> ().firstAssign = true;
			go.GetComponent<FishController> ().DeleteLevelUpAnim();
			go.GetComponent<FishController> ().ResetFishPositionAndText ();
			return go;
		}

		return null;
	}
}
