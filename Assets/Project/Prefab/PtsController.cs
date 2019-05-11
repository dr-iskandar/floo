using UnityEngine;
using System.Collections;

public class PtsController : MonoBehaviour {

	public int sortingOrder; 
	private MeshRenderer meshrender;
	private TextMesh textMesh;

	int totalPoint = 0;
	bool scoreEnabled = false;
	float delayPoint = 2f;
	void Awake()
	{
		meshrender = gameObject.GetComponent<MeshRenderer>();
		textMesh = gameObject.GetComponent<TextMesh>();
		meshrender.sortingOrder = sortingOrder;
		meshrender.enabled = false;
	}

	void Update()
	{
		if (scoreEnabled) 
		{
//			Debug.Log ("delay point = " + delayPoint);
			delayPoint -= Time.deltaTime;
			if (delayPoint < 0) 
			{
				scoreEnabled = false;
				StartCoroutine (FadeOutText ());

			}
		}	
	}

	public void AnimatePts(int ptsValue)
	{
		if (totalPoint == 0) 
		{
			meshrender.enabled = true;
			StartCoroutine (MoveText ());
			StartCoroutine (FadeInText());
			scoreEnabled = true;
		}
		delayPoint = 3f;
		totalPoint += ptsValue;
		textMesh.text = "+ " + totalPoint;
//		Debug.Log ("total point = " + totalPoint);

	}

	int loopCount = 10;
	float waitCount = 0.01f;
	IEnumerator MoveText()
	{
		float height = 0.6f;
		float targetHeight = 1f;
		float diff = (targetHeight - height) / (loopCount * 2);
		for (int i=0; i< loopCount * 2; i++)
		{
			height += diff;
			gameObject.transform.localPosition = new Vector2 (gameObject.transform.localPosition.x, height);
			yield return new WaitForSeconds (waitCount * 2);
		}
		gameObject.transform.localPosition = new Vector2 (gameObject.transform.localPosition.x, targetHeight);
	}

	IEnumerator FadeOutText()
	{
		Color textFadeColor = textMesh.color;
		textFadeColor.a = 1;
		for (int i = 0; i < loopCount; i++)
		{
			textFadeColor.a -= 0.1f;
			if(textMesh != null)
			{
				textMesh.color = textFadeColor;
			}
			yield return new WaitForSeconds (waitCount);
		}
		meshrender.enabled = false;
		totalPoint = 0;
	}

	IEnumerator FadeInText()
	{
		Color textFadeColor = textMesh.color;
		textFadeColor.a = 0;
		for (int i = 0; i < loopCount; i++)
		{
			textFadeColor.a += 0.1f;
			if(textMesh != null)
			{
				textMesh.color = textFadeColor;
			}
			yield return new WaitForSeconds (waitCount);
		}
	}
}
