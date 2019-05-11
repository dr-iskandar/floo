using Spine;
using Spine.Unity;
using UnityEngine;
using System.Collections;

public class PredatorController : MonoBehaviour 
{
	[HideInInspector]
	public string predatorId;
	[HideInInspector]
	public string keyword;
	[HideInInspector]
	public bool isdoingDeathAnim;

	public GameObject skeletonAnim;

	public FadePredatorController fadePredatorController;


	public void SetData(PredatorData data)
	{
		SetPredatorId(data.id);
		SetPosition(data.xPosition, data.yPosition);
		SetSize(data.width, data.height);
		SetAngle(data.angle);
		keyword = data.assetKeyword;
	}

	public void SetPredatorId(string id)
	{
		predatorId = id;
	}

	public void SetPosition(float x, float y)
	{
		gameObject.transform.localPosition = new Vector2(x, y);
	}

	public void SetSize(float width, float height)
	{
		gameObject.transform.localScale = new Vector2(width, height);
	}

	public void SetAngle(float angle)
	{
		gameObject.transform.localEulerAngles = new Vector3(0, 0, angle);
	}

	public void SetAnimationToMove()
	{
		gameObject.transform.GetComponentInChildren<Animator> ().Play ("Move");;
		skeletonAnim.SetActive (false);
		fadePredatorController.Reappear();
	}

	public void SetAnimationToDeath()
	{
		gameObject.transform.GetComponentInChildren<Animator> ().Play ("Death");
		gameObject.transform.GetComponentInChildren<Animator> ().Play ("Dead");
		skeletonAnim.SetActive(true);
		skeletonAnim.GetComponent<SkeletonAnimation> ().state.SetAnimation (0,"animation",false);
		fadePredatorController.FadeOut();
	}

	public void SetPredatorDoingDeathAnim(bool isDoingDeathAnimation)
	{
		isdoingDeathAnim = isDoingDeathAnimation;
	}
		
	public void SetWaitTimeForAnimation(float waitTime)
	{
		StartCoroutine (SetBoolTimerFalse(waitTime));
	}

	IEnumerator SetBoolTimerFalse(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		isdoingDeathAnim = false;
	}
}

