using UnityEngine;
using System.Collections;

public class OcculsionManager : MonoBehaviour 
{
	private static OcculsionManager instance;

	public static OcculsionManager Instance
	{
		get
		{
			return instance;
		}
	}

	public Transform parentObstacle;
	public Transform parentHidingPlace;

	void Awake()
	{
		instance = this;
	}

	public void DisableAllMapFeature()
	{
		int obstacleCount = parentObstacle.childCount;
		for (int i = 0; i < obstacleCount; i++)
		{
			GameObject go = parentObstacle.GetChild(i).gameObject;
			go.SetActive(false);
		}

		int hidingCount = parentHidingPlace.childCount;
		for (int i = 0; i < hidingCount; i++)
		{
			GameObject go = parentHidingPlace.GetChild(i).gameObject;
			go.SetActive(false);
		}
	}

	public void UpdateOcculstion(FishData data)
	{
		FishParameterData fishParam = DefaultParameterManager.Instance.GetFishParameterData(data.fishSkin, data.level);
		float diagonalView = Vector2.Distance(Vector2.zero, new Vector2(fishParam.viewWidth, fishParam.viewHeight));
		float viewRadius = diagonalView / 2.0f;
		//Update occulsion of obstacle
		UpdateChidVisibility(parentObstacle, viewRadius + 1.0f);
		//Update occulsion of hiding place
		UpdateChidVisibility(parentHidingPlace, viewRadius + 2.0f);
		//Update occulsion of water ripple
		float rippleSize = TileGenerator.Instance.rippleSize;
		UpdateChidVisibility(TileGenerator.Instance.parentWaterRipple, viewRadius + rippleSize);
		//Update occulsion of tile
		UpdateChidVisibility(TileGenerator.Instance.parentTile, viewRadius + 1.5f);
	}

	void UpdateChidVisibility(Transform parent, float occulsionDistance)
	{
		int obstacleCount = parent.childCount;
		Vector2 cameraPosition = new Vector2(transform.position.x, transform.position.y);
		for (int i = 0; i < obstacleCount; i++)
		{
			GameObject go = parent.GetChild(i).gameObject;
			Vector3 position = go.transform.position;
			Vector2 objectPosition = new Vector2(position.x, position.y);
			if (Vector2.Distance(cameraPosition, objectPosition) <= occulsionDistance)
			{
				go.SetActive(true);
			}
			else
			{
				go.SetActive(false);
			}
		}
	}
}
