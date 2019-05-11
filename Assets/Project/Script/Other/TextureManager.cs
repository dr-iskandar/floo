using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TextureManager : MonoBehaviour {
	private static TextureManager instance;
	public static TextureManager Instance {
		get {
			if (instance == null) {
				GameObject go = new GameObject ("TextureManager");
				instance = go.AddComponent<TextureManager> ();
			}
		return instance;
		}
	}

	private Shader shaderUsed;
	private Texture2D mainTexture;
	private Texture2D secondTexture;
	private Texture2D thirdTexture;

	private void RedefineShader ()
	{
		shaderUsed = Shader.Find ("Mobile/Diffuse");
	}

	public void SetOneTexture(GameObject hero, string keywordA)
	{
		RedefineShader ();
		mainTexture = RetrieveTexture (keywordA);
		SetTexture(hero.transform.GetChild(0).GetComponent<Renderer>(),mainTexture);
	}

	public void SetTwoTexture(GameObject hero, string keywordA, string keywordB)
	{
		RedefineShader ();
		if (keywordA [0].ToString () == APITag.heroMeshIndicator) {
			mainTexture = RetrieveTexture (keywordA);
			secondTexture = RetrieveTexture (keywordB);
		} else {
			mainTexture = RetrieveTexture (keywordB);
			secondTexture = RetrieveTexture (keywordA);
		}
		SetTexture(hero.transform.GetChild(0).GetComponent<Renderer>(),mainTexture);
		SetTexture(hero.transform.GetChild(1).GetComponent<Renderer>(),secondTexture);
	}

	public void SetThreeTexture(GameObject hero, string keywordA, string keywordB, string keywordC)
	{
		RedefineShader ();
		if (keywordA [0].ToString () == APITag.heroMeshIndicator) 
		{
			mainTexture = RetrieveTexture (keywordA);
			if (keywordB [0].ToString () == APITag.horseMeshIndicator) {
				secondTexture = RetrieveTexture (keywordB);
				thirdTexture = RetrieveTexture (keywordC);
			} else {
				secondTexture = RetrieveTexture (keywordC);
				thirdTexture = RetrieveTexture (keywordB);
			}
		} 
		else if (keywordB [0].ToString () == APITag.heroMeshIndicator) 
		{
			mainTexture = RetrieveTexture (keywordB);
			if (keywordA [0].ToString () == APITag.horseMeshIndicator) {
				secondTexture = RetrieveTexture (keywordA);
				thirdTexture = RetrieveTexture (keywordC);
			} else {
				secondTexture = RetrieveTexture (keywordC);
				thirdTexture = RetrieveTexture (keywordA);
			}
		} 
		else 
		{
			mainTexture = RetrieveTexture (keywordC);
			if (keywordA [0].ToString () == APITag.horseMeshIndicator) {
				secondTexture = RetrieveTexture (keywordA);
				thirdTexture = RetrieveTexture (keywordB);
			} else {
				secondTexture = RetrieveTexture (keywordB);
				thirdTexture = RetrieveTexture (keywordA);
			}
		}

		SetTexture(hero.transform.GetChild(0).GetComponent<Renderer>(),mainTexture);
		SetTexture(hero.transform.GetChild(1).GetComponent<Renderer>(),secondTexture);
		SetTexture(hero.transform.GetChild(2).GetComponent<Renderer>(),thirdTexture);
	}

	private void SetTexture(Renderer rend, Texture tex)
	{
		rend.sharedMaterial.SetTexture("_MainTex",tex);
		rend.sharedMaterial.shader = shaderUsed;
	}

	public Texture2D RetrieveTexture (string savedImageName)
	{
		string temp = PlayerPrefs.GetString (savedImageName);
		int width = PlayerPrefs.GetInt (savedImageName + "_w");
		int height = PlayerPrefs.GetInt (savedImageName + "_h");
		byte[] byteArray = Convert.FromBase64String (temp);
		Texture2D tex = new Texture2D (width, height);
		tex.LoadImage (byteArray);
		return tex;
	}


}
