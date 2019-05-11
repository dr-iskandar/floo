using UnityEngine;
using System.Collections;

public class IdleController : MonoBehaviour {

	[System.NonSerialized]public Animator fishAnimator;
	[System.NonSerialized]public float idleExecuteTime = 0.5f;
	[System.NonSerialized]public Transform fishTransform;

	private int counter;
	private Vector3 tempfishPosition;

	void Start()
	{
		counter = 0;
	}

	void Update () 
	{
		CounterCounting ();
		CheckMovement ();
	}

	private float localTimeCountdown;

	void CheckMovement()
	{
		if (fishTransform.position == tempfishPosition)
		{
			localTimeCountdown += Time.deltaTime;
		}
		else
		{
			localTimeCountdown = 0;
		}

		if (localTimeCountdown > idleExecuteTime) 
		{
			fishAnimator.SetBool ("isMoving",false);
		}
		else 
		{
			fishAnimator.SetBool ("isMoving",true);
		}
	}

	void CounterCounting()
	{
		if (counter == 0)
		{
			tempfishPosition = fishTransform.position;
			counter++;
		}
		else 
		{
			counter = 0;
		}
	}
}
