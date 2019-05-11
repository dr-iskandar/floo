using UnityEngine;
using System.Collections;

public class MapFeatureController : MonoBehaviour 
{
	public string mapFeatureId;

	public void SetData(MapData data)
	{
		SetId(data.mapFeatureId);
		SetParent(data);
		SetPosition(data);
		SetSize(data.width, data.height);
	}

	public void SetId(string id)
	{
		mapFeatureId = id;
	}

	public void RemoveMapFeature()
	{
		Destroy(this.gameObject);
	}

	void SetParent(MapData data)
	{
		switch (data.mapFeatureType)
		{
			case (int)MapFeatureType.HidingPlace:
				LayerController.Instance.SetSeaweedLayer(this.gameObject);
				break;
			default:
				LayerController.Instance.SetObstacleLayer(this.gameObject);
				break;

		}
	}

	public void SetPosition(MapData data)
	{
		float zPos = 0.0f;
		gameObject.transform.localPosition = new Vector3(data.mapXPos, data.mapYPos, zPos);
	}

	public void SetSize(float width, float height)
	{
		gameObject.transform.localScale = new Vector3(width, height,1.0f);
	}
}
