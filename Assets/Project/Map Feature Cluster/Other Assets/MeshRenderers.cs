using UnityEngine;
using System.Collections;

public class MeshRenderers : MonoBehaviour {

	public MeshRenderer[] meshRenderer;
	public SkinnedMeshRenderer[] skinMeshRenderer;
	public SpriteRenderer[] spriteRends;
	public Material[] defaultMat;

	private int sorting = 11;

	void Awake()
	{
		AlphaTransition (meshRenderer);
		AlphaTransition (skinMeshRenderer);
		AlphaTransition (spriteRends);
	}

	public void DisableAll()
	{
		for(int i = 0; i < meshRenderer.Length; i++)
		{
			meshRenderer [i].enabled = false;
		}

		for(int i = 0; i < skinMeshRenderer.Length; i++)
		{
			skinMeshRenderer [i].enabled = false;
		}

		for(int i = 0; i < spriteRends.Length; i++)
		{
			skinMeshRenderer [i].enabled = false;
		}
	}

	public void EnableAll()
	{
		for(int i = 0; i < meshRenderer.Length; i++)
		{
			meshRenderer [i].enabled = true;
		}

		for(int i = 0; i < skinMeshRenderer.Length; i++)
		{
			skinMeshRenderer [i].enabled = true;
		}

		for(int i = 0; i < spriteRends.Length; i++)
		{
			skinMeshRenderer [i].enabled = true;
		}
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

	void AlphaTransition(MeshRenderer[] objects)
	{
		try
		{
			for (int i = 0; i < objects.Length; i++)
			{
				objects [i].sortingOrder = objects [i].sortingOrder + sorting;
			}
		}
		catch(System.Exception e)
		{
			Debug.Log("Game object " + gameObject.name + " : " + e.ToString());
		}
	}

	void AlphaTransition(SkinnedMeshRenderer[] objects)
	{
		try
		{
			for (int i = 0; i < objects.Length; i++)
			{
				objects [i].sortingOrder = objects [i].sortingOrder + sorting;
			}
		}
		catch(System.Exception e)
		{
			Debug.Log("Game object " + gameObject.name + " : " + e.ToString());
		}
	}	

	void AlphaTransition(SpriteRenderer[] objects)
	{
		try
		{
			for (int i = 0; i < objects.Length; i++)
			{
				objects [i].sortingOrder = objects [i].sortingOrder + sorting;
			}
		}
		catch(System.Exception e)
		{
			Debug.Log("Game object " + gameObject.name + " : " + e.ToString());
		}
	}

	private void ChangeAllColor(Color newColor)
	{
		ChangeColor (meshRenderer, newColor);
		ChangeColor (skinMeshRenderer, newColor);
		ChangeColor (spriteRends, newColor);
	}

	void ChangeColor(MeshRenderer[] objects, Color newColor)
	{
		try
		{
			for (int i = 0; i < objects.Length; i++)
			{
				for (int j = 0; j < objects [i].materials.Length; j++)
				{
					objects [i].materials[j].color = newColor;
				}
			}
		}
		catch(System.Exception e)
		{
			Debug.Log("Game object " + gameObject.name + " : " + e.ToString());
		}
	}

	void ChangeColor(SkinnedMeshRenderer[] objects, Color newColor)
	{
		try
		{
			for (int i = 0; i < objects.Length; i++)
			{
				for (int j = 0; j < objects [i].materials.Length; j++)
				{
					objects [i].materials[j].color = newColor;
				}
			}
		}
		catch(System.Exception e)
		{
			Debug.Log("Game object " + gameObject.name + " : " + e.ToString());
		}
	}

	void ChangeColor(SpriteRenderer[] objects, Color newColor)
	{
		try
		{
			for (int i = 0; i < objects.Length; i++)
			{
				for (int j = 0; j < objects [i].materials.Length; j++)
				{
					objects [i].materials[j].color = newColor;
				}
			}
		}
		catch(System.Exception e)
		{
			Debug.Log("Game object " + gameObject.name + " : " + e.ToString());
		}
	}


	int loopCount = 20;
	float yieldCount = 0.02f;
	Color fadeColor = Color.white;
	IEnumerator FadeOutAlpha()
	{
		for (int j = 0; j < defaultMat.Length; j++)
		{
			fadeColor = Color.white;
			ChangeAllColor (fadeColor);
			//defaultMat[j].color = fadeColor;

			for (int i = 0; i < loopCount; i++)
			{
				fadeColor.a -= 0.035f;
				ChangeAllColor (fadeColor);
				//defaultMat[j].color = fadeColor;
				yield return new WaitForSeconds(yieldCount);
			}
		}

	}
	IEnumerator FadeInAlpha()
	{
		for (int j = 0; j < defaultMat.Length; j++) 
		{
			fadeColor = Color.white;
			fadeColor.a = 0.3f;
			ChangeAllColor (fadeColor);
			//defaultMat[j].color = fadeColor;

			for (int i = 0; i < loopCount; i++) {
				fadeColor.a += 0.035f;
				ChangeAllColor (fadeColor);
				//defaultMat[j].color = fadeColor;
				yield return new WaitForSeconds (yieldCount);
			}
		}
	}
}
