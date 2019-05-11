using UnityEngine;
using System.Collections;

public class RandomBubbleController : MonoBehaviour {

	private GameObject bubblePrefab;
	private FishParameterData fishParamData;
	private static RandomBubbleController instance;

	public static RandomBubbleController Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject go = new GameObject("Random Bubble Controller");
				instance = go.AddComponent<RandomBubbleController>();
				DontDestroyOnLoad(go);
			}
			return instance;
		}
	}

	void Awake()
	{
		bubblePrefab = (GameObject)Resources.Load("prefab/Bubble Prefab",typeof(GameObject));
	}

	/// <summary>
	/// Call This Function when you want to generate a random bubble for the designated Player Fish
	/// </summary>
	private IEnumerator GenerateRandomBubble(FishData playerFish)
	{
		fishParamData = DefaultParameterManager.Instance.GetFishParameterData (playerFish.fishSkin,playerFish.level);
		float xRandom = fishParamData.viewWidth /2;
		float yRandom = fishParamData.viewHeight /2;

		xRandom = Random.Range (-xRandom, xRandom);
		yRandom = Random.Range (-yRandom, yRandom);

		GameObject bubble = Instantiate (bubblePrefab) as GameObject;
		bubble.transform.position = new Vector3 (playerFish.xPosition + xRandom, playerFish.yPosition + yRandom,1);
		float randomScale = Random.Range (0.015f,0.075f);
		bubble.transform.localScale = new Vector3 (randomScale,randomScale);
		bubble.transform.SetParent (gameObject.transform);
		yield return new WaitForSeconds (0.3f);
		bubble.GetComponent<MeshRenderer> ().enabled = true;
		StartCoroutine(TimelyDelete(bubble));
	}

	//Wait time for another random generate bubble
	private float timeElapse = 6f;
	private float localTimeLapse = 0;
	/// <summary>
	/// Allowed to be Called Many Times, will calculate then generate random bubble with predetermined values
	/// </summary>
	public void AutomateRandomBubble(FishData playerFish)
	{
		if (localTimeLapse <= 0) 
		{
			//if zero then generate random bubble
			StartCoroutine(GenerateRandomBubble(playerFish));
			localTimeLapse = Random.Range(1,timeElapse);

			SoundUtility.Instance.PlaySFX (SFXData.SfxBubble);
		}
		else 
		{
			//else reduce via delta time
			localTimeLapse -= Time.deltaTime;
		}
	}

	private float deleteTime = 4f;
	private IEnumerator TimelyDelete(GameObject go)
	{
		yield return new WaitForSeconds (deleteTime);
		Destroy(go);
	}
}
