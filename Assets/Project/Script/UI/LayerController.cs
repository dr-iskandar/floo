using UnityEngine;
using System.Collections;

public class LayerController : MonoBehaviour 
{
	private static LayerController instance;

	public static LayerController Instance
	{
		get
		{
			return instance;
		}
	}

	public Transform LayerSeaweed;
	public Transform layerFood;
	public Transform layerObstacle;
	public Transform[] Layers;

	void Awake()
	{
		instance = this;
	}

	public void SetFishLayer(GameObject target, int fishLevel)
	{
		fishLevel--;
		target.transform.SetParent (Layers[fishLevel]);
		target.transform.position = new Vector3(target.transform.position.x,
			target.transform.position.y, Layers [fishLevel].position.z);
	}

	public void SetSeaweedLayer(GameObject target)
	{
		target.transform.SetParent (LayerSeaweed);
		target.transform.position = new Vector3(target.transform.position.x,
			target.transform.position.y, LayerSeaweed.position.z);
	}

	public void SetObstacleLayer(GameObject target)
	{
		target.transform.SetParent(layerObstacle);
	}

	public void SetFoodLayer(GameObject target)
	{
		target.transform.SetParent(layerFood);
	}
}
