using UnityEngine;
using System.Collections;

public class FishMechanismController : MonoBehaviour {

	public FishController fishController;
	public Animator fishAnimator;
	private int counter;
	private int localLoop = 10;
	private int detectTurn = 10;

//	private float waitTime = 300;

	private float startAngle; // vector Y. local fish not the direction angle

	private float headAngleMod;
	private float lastAngleSaved;

	private float loopCurrAngle;
	private float loopTargetAngle;

	private float inputV = 0;

	private int angleTurnRepeat = 0;

	private bool isAngleMinus = false;
	private bool isNormalize = true;

	//loop
	private float timeLapse = 0;
	private int loopLapseLocal;
	private float changeValuePrivate;

	void Awake()
	{
//		#if UNITY_EDITOR
//		QualitySettings.vSyncCount = 0;  // VSync must be disabled
//		Application.targetFrameRate = 30;
//		#else
//		Application.targetFrameRate = 30;
//		#endif

		counter = 0;
	}

	void Start ()
	{
		#if UNITY_EDITOR
		timeLapse = Time.deltaTime * 0.2f;
		#else
		timeLapse = Time.deltaTime;
		#endif
	}


	void Update()
//	void FixedUpdate()
	{
//		if (waitTime < 0)
		if (GameController.animationReady)
		{
			if (counter == 0) 
			{
				startAngle = fishController.localData.fishAngle;
				counter++;
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
//		else
//		{
////			Debug.Log ("counting down ...");
//			waitTime--;
//		}

		if (Input.GetKeyDown("space"))
		{
			fishAnimator.Play("munch");
		}


	}

	void ApplyTailAnimation (float tailAnimationAngle)
	{
		inputV = (tailAnimationAngle / 90) * -1;

		fishAnimator.SetFloat ("turnPoint", inputV);
	}

	void UpdateAnimation(float currAngle, float targetAngle)
	{
		// FIND HEAD ANGLE RESULT
		float HeadAngleResult = 0;
		float AnglePlus = 0;
		float AngleMinus = 0;

		if (currAngle > targetAngle)
		{
			AnglePlus = (360 - currAngle) + targetAngle;
			AngleMinus = currAngle - targetAngle;
		}
		else
		{
			AnglePlus = targetAngle - currAngle;
			AngleMinus = (360 - targetAngle) + currAngle;
		}

		if (AnglePlus <= AngleMinus)
		{
			
			HeadAngleResult = AnglePlus;
		}
		else
		{
			HeadAngleResult = -AngleMinus;
		}

		// debug
		bool ctr = false;
		if (!ctr && (currAngle != targetAngle))
		{
//			Debug.Log (" > " + currAngle + " ~ " +targetAngle + " = " + HeadAngleResult);
//			Debug.Log (" > head: " + HeadAngleResult);
			ctr = true;
		}
		else
			ctr = false;

		// PLAY ANIMATION
		if (HeadAngleResult <= detectTurn && HeadAngleResult >= -detectTurn) //(-10~10)
		{
			IncreaseAngleWhenTurn (HeadAngleResult);
		}
		else if (HeadAngleResult >= detectTurn)
		{
			// left 
			isAngleMinus = false;
			isNormalize = false;

			IncreaseAngleWhenTurn (HeadAngleResult);

		}
		else if (HeadAngleResult <= -detectTurn) 
		{
			// right
			isAngleMinus = true;
			isNormalize = false;

			IncreaseAngleWhenTurn (HeadAngleResult);
		}
	}


	void IncreaseAngleWhenTurn (float headAngle)
	{
		// if turnAngle more than 28' (value >= 28 or value <= -28) as / FPS
		// will have a chance to make a SHARP TURN. 

	// fOR 5 degree / 30 fps
		// FINAL SHARP TURN is complete when turnAngle keep repeat until 90 degree.
		// --- for 20 fps. 30-40 degree/sharpturn. 90'-40 = 50 (4 times * 12.5f)
		// --- for 30 fps. 40-60 degree/sharpturn. 90'-60 = 30 (3 times * 10f)
		// each STEPs WILL ADD THE turnAngle until 90'.

	// fOR 2 degree / 30 fps = 0 ~ 28(MAX) sharp turn need >= 24 times 3 or 4 to smoothing

		if ((headAngle >= 38f) || (headAngle <= -38f)) 
//		if ((headAngle >= 25f) || (headAngle <= -25f)) 
		{
			isNormalize = false;

			angleTurnRepeat++;

			if (headAngleMod == 0)
			{
				headAngleMod = headAngle;
			}
		}
		else if ((headAngle > -38f) && (headAngle < 38f)) 
//		else if ((headAngle > -25f) && (headAngle < 25f)) 
		{
			angleTurnRepeat = 0;

			if (headAngle <= detectTurn && headAngle >= -detectTurn) //[-10~10]
			{
				if (!isNormalize)
				{
//					Debug.Log ("Las " + lastAngleSaved + " > 0 , normalize ");

					NormalizingAngle (lastAngleSaved);
				}
			}
			else // -38 ~ -10  &&  10~38
			{
//				Debug.Log ("Las " + lastAngleSaved + " > " + headAngle + " , play animation normally");

				ChangingLoopAngle (lastAngleSaved, headAngle);
			}
		}

		// more than 1
		if (angleTurnRepeat > 1)
		{
			
//			if (lastAngleSaved >= 38f && lastAngleSaved <= 90) // LEFT ANIM DIRECTION
			if (lastAngleSaved > 0 && lastAngleSaved <= 90) // LEFT ANIM DIRECTION
			{
				if (!isAngleMinus) // LEFT Controller
				{
					if (headAngle >= 38f && headAngle <= 90) // LEFT Result
					{
						headAngleMod += 12.5f;

						if (headAngleMod > 90)
						{
							headAngleMod = 90;
						}
					}
				}
				else // if minus = True / RIGHT controller
				{
					float distAngle = lastAngleSaved - headAngle;

					if (distAngle >= 45)
					{
						float increaseAngle = distAngle / 2.5f;

						headAngleMod -= increaseAngle;

//						Debug.Log ("    dist: " + distAngle + " incre " + increaseAngle + " Mod " + headAngleMod);
					}
					else 
					{
						headAngleMod = 0;

//						Debug.Log ("    dist: " + distAngle + " Mod " + headAngleMod);
					}

				}

				// apply animation with stack angles
				ChangingLoopAngle (lastAngleSaved, headAngleMod);
//				Debug.Log (".Las " + lastAngleSaved + " >> .Mod (" + headAngleMod + ") " + headAngle + ".bf | rep : " + angleTurnRepeat + " isMin " + isAngleMinus);

			}

//			else if (lastAngleSaved >= -90 && lastAngleSaved <= -38f) // RIGHT ANIM DIRection
			else if (lastAngleSaved >= -90 && lastAngleSaved < 0)
			{
				if (isAngleMinus) // Right Controller
				{
					if (headAngle >= -90 && headAngle <= -38f) // Right Result
					{	
						headAngleMod -= 12.5f;

						if (headAngleMod < -90)
						{
							headAngleMod = -90;
						}
					}
				}
				else // if minus = False / LEFT Controller
				{
					float distAngle = headAngle - lastAngleSaved;

					if (distAngle >= 45)
					{
						float increaseAngle = distAngle / 2.5f;

						headAngleMod += increaseAngle;

//						Debug.Log ("    dist: " + distAngle + " incre " + increaseAngle + " Mod " + headAngleMod);
					}
					else 
					{
						headAngleMod = 0;

//						Debug.Log ("    dist: " + distAngle);
					}
				}

				// apply animation with stack angles
				ChangingLoopAngle (lastAngleSaved, headAngleMod);
//				Debug.Log (".Las " + lastAngleSaved + " >> .Mod (" + headAngleMod + ") " + headAngle + ".bf | rep : " + angleTurnRepeat + " isMin " + isAngleMinus);
			}
				
			/*
			if (headAngle >= 38f && headAngle <= 90)
			{
				if (!isAngleMinus)
				{
					headAngleMod += 12.5f;

					if (headAngleMod > 90)
					{
						headAngleMod = 90;
					}
				}
				else // isminus = true
				{
					float distAngle = lastAngleSaved - headAngle;
					Debug.Log ("dist Angle : " + distAngle);

					if (distAngle < -45)
					{
						headAngleMod -= 25f;
					}
					else 
					{
						headAngleMod = 0;
					}
				}

				// apply animation with stack angles
				ChangingLoopAngle (lastAngleSaved, headAngleMod);
			}

			else if (headAngle >= -90 && headAngle <= -38f)
			{
				if (isAngleMinus)
				{
					headAngleMod -= 12.5f;

					if (headAngleMod < -90)
					{
						headAngleMod = -90;
					}
				}
				else // isminus = false
				{
					float distAngle = headAngle - lastAngleSaved;
					Debug.Log ("dist Angle : " + distAngle);

					if (distAngle > 45)
					{
						headAngleMod += 25f;
					}
					else 
					{
						headAngleMod = 0;
					}
				}

				// apply animation with stack angles
				ChangingLoopAngle (lastAngleSaved, headAngleMod);
			}
	*/

			// apply animation with stack angles
//			ChangingLoopAngle (lastAngleSaved, headAngleMod);
//			Debug.Log (".Las " + lastAngleSaved + " >> .Mod (" + headAngleMod + ") " + headAngle + ".bf | rep : " + angleTurnRepeat + " isMin " + isAngleMinus);

		}
		else if (angleTurnRepeat == 1)
		{
			if (headAngle >= 50 && headAngle <= 90)
			{
				headAngleMod = 40;

//				Debug.Log (".Las " + lastAngleSaved + " >> .Mod (" + headAngleMod + ") " + headAngle + ".bf | rep : " + angleTurnRepeat + " isMin " + isAngleMinus);

				ChangingLoopAngle (lastAngleSaved, headAngleMod);
			}
			else if (headAngle >= -90 && headAngle <= -50)
			{
				headAngleMod = -40;

//				Debug.Log (".Las " + lastAngleSaved + " >> .Mod (" + headAngleMod + ") " + headAngle + ".bf | rep : " + angleTurnRepeat + " isMin " + isAngleMinus);

				ChangingLoopAngle (lastAngleSaved, headAngleMod);
			}
			else
			{
//				Debug.Log (".Las " + lastAngleSaved + " >> " + headAngle + " | rep : " + angleTurnRepeat + " isMin " + isAngleMinus);

				ChangingLoopAngle (lastAngleSaved, headAngle);
			}
		}
	}

	void NormalizingAngle (float headAngle)
	{
		if (isNormalize == false)
		{
			angleTurnRepeat = 0;
			headAngleMod = 0;

			// play idle animation -- funct slowly reduce to 0.
			ChangingLoopAngle (headAngle, 0);

			isNormalize = true;
		}
	}

	#region LOOP

	void OnDisable() 
	{
		CancelInvoke ("ChangePerLoop");
	}

	void ChangingLoopAngle (float currAngle, float targetAngle)
	{		
		float loopLapse = 0;
		float changeValue = 0;

		loopCurrAngle = currAngle;
		loopTargetAngle = targetAngle;

		loopLapse = 15;
		loopLapseLocal = (int)loopLapse;
		changeValue = targetAngle - currAngle;

//		Debug.Log ("TimeDetla " + timeLapse + " - loop " + loopLapse);

		changeValue = changeValue / loopLapse;
		changeValuePrivate = changeValue;

		if(this.gameObject.activeSelf)
		{
			Invoke("ChangePerLoop", timeLapse);			
		}
	}

	void ChangePerLoop()
	{
		loopLapseLocal--;

		if (loopLapseLocal > 0)
		{
			loopCurrAngle += changeValuePrivate;

			ApplyTailAnimation (loopCurrAngle);

			if(this.gameObject.activeSelf)
			{
				Invoke("ChangePerLoop", timeLapse);
			}

			lastAngleSaved = loopCurrAngle;

//			Debug.Log ("      # " + loopLapseLocal + " (" + loopCurrAngle  + ")");
		}
		else 
		{
			ApplyTailAnimation (loopTargetAngle);

			lastAngleSaved = loopTargetAngle;

//			Debug.Log ("      $ " + loopLapseLocal + " (" + loopTargetAngle + ")");
		}
	}

	#endregion
}
