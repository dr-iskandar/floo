using UnityEngine;
using System.Collections;

public class FadeControllerSpine : MonoBehaviour {
	public MeshRenderer spineMeshAsset;
	private int sorting = 20;

	void Awake()
	{
		//is this still needed or just do it via script outside this?
		IncreaseLayer();
	}

	void OnDestroy()
	{
		StopAllCoroutines ();
	}

	public void TriggerEnter()
	{
		StopCoroutine ("FadeInAlpha");
		StartCoroutine ("FadeOutAlpha");
	}

	public void TriggerExit()
	{
		StopCoroutine ("FadeOutAlpha");
		StartCoroutine ("FadeInAlpha");
	}

	int loopCount = 20;
	float yieldCount = 0.02f;
	Color fadeColor = Color.white;
	IEnumerator FadeOutAlpha()
	{
		fadeColor = Color.white;
		for (int i = 0; i < spineMeshAsset.materials.Length; i++)
		{
			fadeColor.a -= 0.035f;
			spineMeshAsset.materials [i].color = fadeColor;
			yield return new WaitForSeconds (yieldCount);
		}
	}

	IEnumerator FadeInAlpha()
	{
		fadeColor.a = 0.3f;
		for (int i = 0; i < spineMeshAsset.materials.Length; i++)
		{
			fadeColor.a += 0.035f;
			spineMeshAsset.materials [i].color = fadeColor;
			yield return new WaitForSeconds (yieldCount);
		}
	}

	public void DisableAll()
	{
		spineMeshAsset.enabled = false;
	}

	public void EnableAll()
	{
		spineMeshAsset.enabled = true;
	}

	private void IncreaseLayer()
	{
		spineMeshAsset.sortingOrder += sorting;
	}
}
