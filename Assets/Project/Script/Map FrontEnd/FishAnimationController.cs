using UnityEngine;
using System.Collections;

public class FishAnimationController : MonoBehaviour {

	public FishController fishController;
	//public Animator fishAnimator;
	public Animation fishAnimation;
	private int counter;
	private int localLoop = 1;
	private float startAngle;
	private int detectTurn = 1;

	public float fishSpeed;
	private float timeCounter;
	private string animName = "iban_test";

	private float localTimeCounter;
	private float idleExecuteTime = 1;

	void Awake()
	{
		counter = 0;
		fishAnimation [animName].time = 2.5f;
		fishAnimation [animName].speed = fishSpeed;
	}


	void Start()
	{
		//fishAnimation ["dash_fullspeed"].time = 1f;
	}

	//update main loop for how many times per frame should this script run
	void Update()
	{
		if (GameController.animationReady)
		{
			if (counter == 0) 
			{
				startAngle = fishController.localData.fishAngle;
				counter ++;
			} 
			else if (counter > localLoop) 
			{
				UpdateAnimation (startAngle, fishController.localData.fishAngle);
				counter = 0;
			} 
			else 
			{
				counter++;
			}
		}
		else
		{
			//wait until its ready
		}
	}

	void UpdateAnimation(float startAngle, float endAngle)
	{
		CalculateTime();
		int inc = 0;
		float dif = endAngle - startAngle;
		if (dif < detectTurn && dif > -detectTurn)
		{
			//idle
			AdjustTime();
			//Debug.Log ("Update Animation - Idle " + inc);
		}
		else if(dif > detectTurn)
		{
			//left
			fishAnimation [animName].speed = -fishSpeed;
			fishAnimation.Play (animName);
			//Debug.Log ("Update Animation - Left " + inc);

		}
		else if (dif < - detectTurn) 
		{
			//right
			fishAnimation [animName].speed = fishSpeed;
			fishAnimation.Play (animName);
			//Debug.Log ("Update Animation - Right " + inc);
		}
		inc++;
	}

	void CalculateTime()
	{
		if (timeCounter > 5 && fishAnimation [animName].time == 0) 
		{
			//do nothing	
		}
		else 
		{
			timeCounter = fishAnimation [animName].time;
		}
		fishAnimation [animName].time = timeCounter;
	}
	int smoothAnim = 0;
	void AdjustTime()
	{

		//check local time, if more than executed time, play anim
		if (localTimeCounter > idleExecuteTime)
		{
			//play anim
			//fishAnimation.Play ("dash_fullspeed");
			//Debug.Log("play anim dash fullspeed");
			//make a countdown not to play other animation while this is played
			localTimeCounter = 0;
		}
		else if (fishAnimation [animName].time > 2.4f && fishAnimation [animName].time < 2.7f)
		{
			//Debug.Log ("adjust time - Counting Local Time");
			fishAnimation [animName].time = 2.5f;
			fishAnimation [animName].speed = 0;
			localTimeCounter += Time.deltaTime;
		}
		else 
		{
			if (smoothAnim == 0)
			{
				localTimeCounter = 0;
				if (fishAnimation [animName].time < 2.4f)
				{
					//Debug.Log ("else 1");
					fishAnimation [animName].speed = fishSpeed * 1.5f;
					fishAnimation.Play (animName);
				}
				else if (fishAnimation[animName].time > 2.7f)
				{
					//Debug.Log ("else 2");
					fishAnimation [animName].speed = -fishSpeed * 1.5f;
					fishAnimation.Play (animName);
				}
				else 
				{
					//Debug.Log ("else 3");
					//fishAnimation [animName].time = 0;
				}
				//fishAnimation.Play (animName);
				smoothAnim = 0;
			}
			else 
			{
				localTimeCounter = 0;
				smoothAnim++;
			}


		}
	}
}
