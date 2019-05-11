using UnityEngine;
using System.Collections;

public class MeshRenderersFish : MonoBehaviour {

	[System.NonSerialized]public MeshRenderer fishMeshRenderer;
	[System.NonSerialized]public MeshRenderer fishText;

	//Local HSV Container, no HSV by default
	private float H = 0;
	private float S = 1;
	private float V = 1;

	public void ResetFishHSV()
	{
		for (int i = 0; i < fishMeshRenderer.materials.Length; i++)
		{
			fishMeshRenderer.materials[i].SetFloat ("_HueShift", H);
			fishMeshRenderer.materials[i].SetFloat ("_Sat", S);
			fishMeshRenderer.materials[i].SetFloat ("_Val",V);
		}
	}

	public void SetFishHSV(float hue, float saturation, float value)
	{
		//store the HSV
		H = hue;
		S = saturation;
		V = value;

		if (fishMeshRenderer.materials [0].GetFloat ("_HueShift") != hue)
			for (int i = 0; i < fishMeshRenderer.materials.Length; i++)
			{
				fishMeshRenderer.materials[i].SetFloat ("_HueShift", H);
				fishMeshRenderer.materials[i].SetFloat ("_Sat", S);
				fishMeshRenderer.materials[i].SetFloat ("_Val",V);
			}
	}

	public void DisableAll()
	{
		fishMeshRenderer.enabled = false;
		fishText.enabled = false;
	}

	public void EnableAll()
	{
		fishMeshRenderer.enabled = true;
		fishText.enabled = true;
	}

	void OnDestroy()
	{
		StopAllCoroutines ();
	}

	public void InvulnerableEffectStart()
	{
		//Debug.Log ("invul start");
		HSVPlay();
	}

	public void InvulnerableEffectStop()
	{
		if (HSVIsPlaying)
		{
			//stop the animation
			StopHSVPlay();
		}
	}

	private void StopHSVPlay()
	{
		HSVIsPlaying = false;
		StopCoroutine ("HSVAnim");
		ResetFishHSV();
	}

	private void HSVPlay()
	{
		//play The HSV of all Materials in temp
		StartCoroutine("HSVAnim");
	}

	bool HSVIsPlaying = false;
	float yieldCount = 0.1f;
	int incremental = 50;
	float HSVvalue;
	float tempValue = 1;
	bool isUp;

	IEnumerator HSVAnim()
	{
		HSVvalue = fishMeshRenderer.materials[0].GetFloat("_HueShift");
		HSVIsPlaying = true;
		while(HSVIsPlaying)
		{
			for (int i = 0; i < fishMeshRenderer.materials.Length; i++)
			{
				fishMeshRenderer.materials[i].SetFloat ("_HueShift", HSVvalue);
				fishMeshRenderer.materials[i].SetFloat ("_Val",tempValue);
				if (isUp) 
				{
					tempValue += 1;
					if (tempValue > 2)
						isUp = false;
				}
				else
				{
					tempValue -= 1;
					if (tempValue < 2)
						isUp = true;
				}
			}
			HSVvalue += incremental;
			yield return new WaitForSeconds(yieldCount);
		}
	}
}
