using UnityEngine;
using System.Collections;

public class FadeControllerAnemone : MonoBehaviour {

	public MeshRenderer[] objectsM;
	public SkinnedMeshRenderer[] objectsSM;
	public Material defaultMaterial;
	public Material alphaMaterial;

	private int sorting = 20;
	private GameObject fish;

	void OnDestroy()
	{
		StopAllCoroutines ();
	}

	void OnTriggerEnter(Collider other)
	{
		fish = other.gameObject;
		StopCoroutine ("FadeInText");
		StartCoroutine ("FadeOutText");
		if (other.tag == "Player") 
		{
			AlphaTransition (objectsM);
			AlphaTransition (objectsSM);
			StopCoroutine ("FadeInAlpha");
			StartCoroutine ("FadeOutAlpha");
		}
	}

	void OnTriggerExit(Collider other)
	{
		fish = other.gameObject;
		StopCoroutine ("FadeOutText");
		StartCoroutine ("FadeInText");
		if (other.tag == "Player") 
		{
			StopCoroutine ("FadeOutAlpha");
			StartCoroutine ("FadeInAlpha");
		}
	}


	void AlphaTransition(MeshRenderer[] objects)
	{
		for (int i = 0; i < objects.Length; i++)
		{
			objects [i].material = alphaMaterial;
			objects [i].sortingOrder = objects [i].sortingOrder + sorting;
		}
	}

	void AlphaTransition(SkinnedMeshRenderer[] objects)
	{
		for (int i = 0; i < objects.Length; i++)
		{
			objects [i].material = alphaMaterial;
			objects [i].sortingOrder = objects [i].sortingOrder + sorting;
		}
	}	

	void NonAlphaTransition(MeshRenderer[] objects)
	{
		for (int i = 0; i < objects.Length; i++)
		{
			objects [i].sortingOrder = objects [i].sortingOrder - sorting;
			objects [i].material = defaultMaterial;
		}
	}

	void NonAlphaTransition(SkinnedMeshRenderer[] objects)
	{
		for (int i = 0; i < objects.Length; i++)
		{
			objects [i].sortingOrder = objects [i].sortingOrder - sorting;
			objects [i].material = defaultMaterial;
		}
	}

	int loopCount = 20;
	float yieldCount = 0.02f;
	Color fadeColor = Color.white;
	IEnumerator FadeOutAlpha()
	{
		fadeColor = Color.white;
		alphaMaterial.color = fadeColor;

		for (int i = 0; i < loopCount; i++)
		{
			fadeColor.a -= 0.03f;
			alphaMaterial.color = fadeColor;
			yield return new WaitForSeconds(yieldCount);
		}
	}


	IEnumerator FadeInAlpha()
	{
		fadeColor.a = 0.6f;
		alphaMaterial.color = fadeColor;

		for (int i = 0; i < loopCount; i++)
		{
			fadeColor.a += 0.03f;
			alphaMaterial.color = fadeColor;
			yield return new WaitForSeconds(yieldCount);
		}
		NonAlphaTransition (objectsM);
		NonAlphaTransition (objectsSM);
	}

	void ChangeLayers(bool isAddition)
	{
		for (int i = 0; i < objectsM.Length; i++)
		{
			objectsM [i].material = alphaMaterial;
			if (isAddition)
				objectsM [i].sortingOrder = objectsM [i].sortingOrder + sorting;
			else
				objectsM [i].sortingOrder = objectsM [i].sortingOrder - sorting;
		}

		for (int i = 0; i < objectsSM.Length; i++)
		{
			objectsSM [i].material = alphaMaterial;			
			if (isAddition)
				objectsSM [i].sortingOrder = objectsSM [i].sortingOrder + sorting;
			else
				objectsSM [i].sortingOrder = objectsSM [i].sortingOrder - sorting;
		}
	}

	IEnumerator FadeOutText()
	{
		
		Color textFadeColor = fish.GetComponent<FishController>().fishIndicator.fishName.color;
		textFadeColor.a = 1;
		for (int i = 0; i < 20; i++)
		{
			textFadeColor.a -= 0.05f;
			if(fish != null)
			{
				fish.GetComponent<FishController>().fishIndicator.fishName.color = textFadeColor;
			}
			yield return new WaitForSeconds (0.02f);
		}

	}

	IEnumerator FadeInText()
	{
		Color textFadeColor = fish.GetComponent<FishController>().fishIndicator.fishName.color;
		textFadeColor.a = 0;
		for (int i = 0; i < 20; i++)
		{
			textFadeColor.a += 0.05f;
			if(fish != null)
			{
				fish.GetComponent<FishController>().fishIndicator.fishName.color = textFadeColor;
			}
			yield return new WaitForSeconds (0.02f);
		}
	}
}
