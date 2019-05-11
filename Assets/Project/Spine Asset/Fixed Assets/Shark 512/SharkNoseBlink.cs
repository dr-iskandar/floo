using UnityEngine;
using System.Collections;

public class SharkNoseBlink : MonoBehaviour {

	private float waitTime = 0.5f;

	void Start()
	{
		StartCoroutine (Blink());
	}

	IEnumerator Blink()
	{
		gameObject.GetComponent<SpriteRenderer>().enabled = false;
		yield return new WaitForSeconds (waitTime);
		gameObject.GetComponent<SpriteRenderer>().enabled = true;
		yield return new WaitForSeconds (waitTime);
		StartCoroutine (Blink());
	}
}
