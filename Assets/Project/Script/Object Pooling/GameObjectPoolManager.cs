using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectPoolManager : MonoBehaviour 
{
	public bool isInitialize = false;

	public Dictionary<string, GameObjectPooling> poolDictionary = new Dictionary<string, GameObjectPooling>();

	public virtual void InitializePool()
	{
		
	}

	public void Init()
	{
		if(!isInitialize)
		{
			InitializePool();
			isInitialize = true;
		}
	}

	public void CreatePool(string keyword, int defaultSize)
	{
		GameObject go = new GameObject(keyword + " Pool");
		go.transform.SetParent(this.transform);
		GameObjectPooling pool = go.AddComponent<GameObjectPooling>();
		Object prefab = AssetManager.Instance.GetPrefabByKeyword(keyword);
		pool.PoolGameObject(prefab, defaultSize);

		poolDictionary.Add(keyword, pool);
	}

	public virtual GameObject GetGameObject(string keyword)
	{
		if (poolDictionary.ContainsKey(keyword))
		{
			return poolDictionary[keyword].GetGameObject();
		}

		return null;
	}

	public void ReturnGameObject(string keyword, GameObject go)
	{
		if (poolDictionary.ContainsKey(keyword))
		{
			poolDictionary[keyword].ReturnObjectToPool(go);
		}
	}

	public void ResetAllPool()
	{
		foreach(KeyValuePair<string, GameObjectPooling> entry in poolDictionary)
		{
			poolDictionary[entry.Key].ReturnAllObjectToPool();
		}
	}
}
