using UnityEngine;
using System.Collections;

public class FadeControllerSeaweed : MonoBehaviour {
	public MeshRenderer baseSeaweed;
	public SkinnedMeshRenderer[] objectsOne;
	public SkinnedMeshRenderer[] objectsTwo;
	public Material defaultMaterial;
	public Material alphaMaterial;
	public Material defaultMaterialTwo;
	public Material alphaMaterialTwo;

	private GameObject fishes;
	private int sorting = 20;

	void OnDestroy()
	{
		StopAllCoroutines ();
	}

	void OnTriggerEnter(Collider other)
	{
		fishes = other.gameObject;
		StopCoroutine ("FadeInText");
		StartCoroutine ("FadeOutText");

		if (other.tag == "Player") 
		{
			AlphaTransition (objectsOne, 1);
			AlphaTransition (objectsTwo, 2);
			baseSeaweed.material = alphaMaterial;

			StopCoroutine ("FadeInAlpha");
			StartCoroutine ("FadeOutAlpha");
		}
	}

	void OnTriggerExit(Collider other)
	{
		fishes = other.gameObject;
		StopCoroutine ("FadeOutText");
		StartCoroutine ("FadeInText");

		if (other.tag == "Player") 
		{
			StopCoroutine ("FadeOutAlpha");
			StartCoroutine ("FadeInAlpha");
		}
	}


	void AlphaTransition(SkinnedMeshRenderer[] objects, int materialSet)
	{
		if (materialSet == 1) {
			for (int i = 0; i < objects.Length; i++) {
				objects [i].material = alphaMaterial;
				objects [i].sortingOrder = objects [i].sortingOrder + sorting;
			}
		} else {
			for (int i = 0; i < objects.Length; i++) {
				objects [i].material = alphaMaterialTwo;
				objects [i].sortingOrder = objects [i].sortingOrder + sorting;
			}
		}

	}

	void NonAlphaTransition(SkinnedMeshRenderer[] objects, int materialSet)
	{
		if (materialSet == 1) {
			for (int i = 0; i < objects.Length; i++) {
				objects [i].material = defaultMaterial;
				objects [i].sortingOrder = objects [i].sortingOrder - sorting;
			}
		} else {
			for (int i = 0; i < objects.Length; i++) {
				objects [i].material = defaultMaterialTwo;
				objects [i].sortingOrder = objects [i].sortingOrder - sorting;
			}
		}
	}

	int loopCount = 20;
	float yieldCount = 0.02f;
	Color fadeColor = Color.white;
	IEnumerator FadeOutAlpha()
	{
		fadeColor = Color.white;
		alphaMaterial.color = fadeColor;
		alphaMaterialTwo.color = fadeColor;
		for (int i = 0; i < loopCount; i++)
		{
			fadeColor.a -= 0.03f;
			alphaMaterial.color = fadeColor;
			alphaMaterialTwo.color = fadeColor;
			yield return new WaitForSeconds(yieldCount);
		}

	}

	IEnumerator FadeInAlpha()
	{
		fadeColor.a = 0.6f;
		alphaMaterial.color = fadeColor;
		alphaMaterialTwo.color = fadeColor;
		for (int i = 0; i < loopCount; i++)
		{
			fadeColor.a += 0.03f;
			alphaMaterial.color = fadeColor;
			alphaMaterialTwo.color = fadeColor;
			yield return new WaitForSeconds(yieldCount);
		}
		NonAlphaTransition (objectsOne,1);
		NonAlphaTransition (objectsTwo,2);
		baseSeaweed.material = defaultMaterial;
	}

	IEnumerator FadeOutText()
	{
		Color textFadeColor = fishes.GetComponent<FishController>().fishIndicator.fishName
			.color;
		textFadeColor.a = 1;
		for (int i = 0; i < 20; i++)
		{
			textFadeColor.a -= 0.05f;
			if(fishes != null)
			{
				fishes.GetComponent<FishController>().fishIndicator.fishName
				.color = textFadeColor;
			}
			yield return new WaitForSeconds (0.02f);
		}

	}

	IEnumerator FadeInText()
	{
		Color textFadeColor = fishes.GetComponent<FishController>().fishIndicator.fishName
			.color;
		textFadeColor.a = 0;
		for (int i = 0; i < 20; i++)
		{
			textFadeColor.a += 0.05f;
			if(fishes != null)
			{
				fishes.GetComponent<FishController>().fishIndicator.fishName
				.color = textFadeColor;
			}
			yield return new WaitForSeconds (0.02f);
		}
	}
}
