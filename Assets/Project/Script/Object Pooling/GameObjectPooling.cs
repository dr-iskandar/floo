using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectPooling : MonoBehaviour 
{
	private List<GameObject> usedObject;
	private List<GameObject> unUsedObject;
	private Object referencePrefab;

	public void PoolGameObject(Object prefab, int poolSize)
	{
		usedObject = new List<GameObject>();
		unUsedObject = new List<GameObject>();
		referencePrefab = prefab;

		for (int i = 0; i < poolSize; i++)
		{
			GameObject go = Instantiate(prefab) as GameObject;
			go.name = prefab.name + " " + i.ToString().PadLeft(3, '0');
			go.SetActive(false);
			go.transform.SetParent(this.transform);
			unUsedObject.Add(go);
		}
	}

	public GameObject GetGameObject()
	{
		GameObject go = null;

		while(unUsedObject.Count > 0)
		{
			go = unUsedObject[0];
			unUsedObject.RemoveAt(0);
			if(go!=null)
			{
				break;
			}
		}

		if(go == null)
		{
			go = Instantiate(referencePrefab) as GameObject;
			go.name = referencePrefab.name + " " + usedObject.Count.ToString().PadLeft(3, '0');
		}

		usedObject.Add(go);
		go.SetActive(true);

		return go;
	}

	public void ReturnAllObjectToPool()
	{
		while(usedObject.Count > 0)
		{
			GameObject go = usedObject[0];
			ReturnObjectToPool(go);
		}
	}

	public void ReturnObjectToPool(GameObject go)
	{
		try
		{
			int index = usedObject.FindIndex(x => x == go);
			if (index >= 0)
			{
				usedObject.RemoveAt(index);
				go.SetActive(false);
				go.transform.SetParent(this.transform);
				unUsedObject.Add(go);
			}
			else
			{
				Debug.Log(go.name + " is not registered in the pool");
			}
		}
		catch(System.Exception e)
		{

		}
	}
}
