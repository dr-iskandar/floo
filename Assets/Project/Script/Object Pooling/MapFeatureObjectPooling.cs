using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapFeatureObjectPooling : GameObjectPoolManager 
{
	private static MapFeatureObjectPooling instance;
	
	public static MapFeatureObjectPooling Instance {
		get {
			if(instance == null)
			{
				GameObject go = new GameObject("Map Feature Object Pooling");
				instance = go.AddComponent<MapFeatureObjectPooling>();
				DontDestroyOnLoad(go);
				instance.Init();
			}
			return instance;
		}
	}
	
	public override void InitializePool()
	{
		List<string> mapFeatureList = new List<string>();
		List<int> mapFeaturePoolSize = new List<int>();
//		mapFeatureList.Add("mf_hide_01");		//Anemone
//		mapFeaturePoolSize.Add(10);
//		mapFeatureList.Add("mf_hide_02");		//Seaweed
//		mapFeaturePoolSize.Add(10);
//		mapFeatureList.Add("mf_hide_03");		//Coral Pink
//		mapFeaturePoolSize.Add(10);
//		mapFeatureList.Add("mf_hide_04");		//Coral Purple
//		mapFeaturePoolSize.Add(10);
//		mapFeatureList.Add("mf_hide_05");		//Coral Yellow
//		mapFeaturePoolSize.Add(10);
//		mapFeatureList.Add("mf_hide_c");
//		mapFeaturePoolSize.Add(2);
		mapFeatureList.Add("mf_hide_i");
		mapFeaturePoolSize.Add(3);
		mapFeatureList.Add("mf_hide_l");
		mapFeaturePoolSize.Add(2);
		mapFeatureList.Add("mf_hide_plus");
		mapFeaturePoolSize.Add(2);
		mapFeatureList.Add("mf_hide_square");
		mapFeaturePoolSize.Add(2);
		mapFeatureList.Add("mf_kill_01");		//Sea Urchin
		mapFeaturePoolSize.Add(5);
//		mapFeatureList.Add("mf_obs_01");		//Coral Obstacle
//		mapFeaturePoolSize.Add(10);

		for (int i = 0; i < mapFeatureList.Count; i++)
		{
			CreatePool(mapFeatureList[i], mapFeaturePoolSize[i]);
		}
	}
}
