using UnityEngine;
using System.Collections;

public class FadePredatorController : MonoBehaviour {

	public GameObject[] attachments;
	public Material materialused;

	private int loopCount = 10;
	public float waitTime  =0.25f;

	public void FadeOut()
	{
		Debug.Log ("fade out shark called");
		StartCoroutine (AnimFadeOut());
	}

	public void Reappear()
	{
		UnityEngine.Color oldColor = materialused.color;
		materialused.color = new UnityEngine.Color(oldColor.r,oldColor.g,oldColor.b,1f);
	}

	IEnumerator AnimFadeOut()
	{
		float token = 1;
		UnityEngine.Color oldColor = materialused.color;

		for (int i=0; i < loopCount ; i++)
		{
			Debug.Log ("loop");
			yield return new WaitForSeconds (waitTime);
			materialused.color = new UnityEngine.Color(oldColor.r,oldColor.g,oldColor.b,token);
			token -= 0.15f;
		}
		yield return new WaitForSeconds (2f);
		//Reappear ();
	}
}
