using UnityEngine;
using System.Collections;

public class FishEyeController : MonoBehaviour {

	public SpriteRenderer fishIrisLeft;
	public SpriteRenderer fishIrisRight;
	public SpriteRenderer fishEyeballLeft;
	public SpriteRenderer fishEyeballRight;
	public Sprite[] fishIris;
	public Sprite[] fishEyeball;
	public Vector2[] fishEyePosition;

	//Fish ID for clamp movement; 1 = Nemo, 2 = Dory, 3 = Angler
	public int fishID;
	private int localFishLevel = 1;

	public bool isGoldOrSilver;

	public void SetEyeLevel(int fishLevel)
	{
		localFishLevel = fishLevel;
		fishLevel--;

		//setting eye position
		Vector2 rightEyePos = new Vector2 (fishEyePosition[fishLevel].x * -1 , fishEyePosition[fishLevel].y);
		fishEyeballLeft.transform.localPosition = fishEyePosition[fishLevel];
		fishEyeballRight.transform.localPosition = rightEyePos;

		//setting eye sprites
		fishIrisLeft.sprite = fishIris [fishLevel];
		fishIrisRight.sprite = fishIris [fishLevel];
		fishEyeballLeft.sprite = fishEyeball [fishLevel];
		fishEyeballRight.sprite = fishEyeball [fishLevel];

		//checks whether fisheyelashes exist or not
		if (gameObject.GetComponent<FishEyeLashesController>() != null)
		{
			//Eyelashes Found
			gameObject.GetComponent<FishEyeLashesController>().SetEyeLashes(fishLevel);
		}
	}

	public void SetEyeMovement(float xValue, float yValue, Vector3 rotationValue)
	{
		float clampValue = 2f; // 2f by default
		switch (fishID) 
		{
		case 1:
			//This is Nemo clamp modification
			clampValue = NemoClamp();
			break;
		case 2:
			//this is Dory Clamp modifcation
			clampValue = DoryClamp();
			break;
		case 3:
			//this is Angler Clamp modifcation
			clampValue = AnglerClamp();
			break;
		case 4:
			//this is Skeleton Clamp modifcation
			clampValue = SkeletonClamp();
			break;
		case 5:
			clampValue = 0; //pinefish does not need this clamp
			break;
		case 6:
			//orca clamp modification
			clampValue = OrcaClamp ();
			break;
		case 7:
			clampValue = RudolfClamp ();
			break;
		case 8:
			clampValue = SnowyClamp ();
			break;
		case 9:
			clampValue = FlowerClamp ();
			break;
		case 10:
			clampValue = DragonClamp ();
			break;
		default:
			break;
		}

		//default operation
		fishEyeballLeft.transform.eulerAngles =  rotationValue;
		fishEyeballRight.transform.eulerAngles = rotationValue;
		fishIrisLeft.transform.eulerAngles = Vector3.zero;
		fishIrisRight.transform.eulerAngles = Vector3.zero;
		fishIrisLeft.transform.localPosition = Vector2.ClampMagnitude(new Vector2 (xValue, yValue),clampValue);
		fishIrisRight.transform.localPosition = Vector2.ClampMagnitude(new Vector2 (xValue, yValue),clampValue);
	}

	public int GetOrderLayer()
	{
		return fishIrisLeft.sortingOrder;
	}

	public void DisableEyes()
	{
		fishIrisLeft.enabled = false;
		fishIrisRight.enabled = false;
		fishEyeballLeft.enabled = false;
		fishEyeballRight.enabled = false;
	}
	
	public void EnableEyes()
	{
		fishIrisLeft.enabled = true;
		fishIrisRight.enabled = true;
		fishEyeballLeft.enabled = true;
		fishEyeballRight.enabled = true;
	}

	public void IncrementLayer()
	{
		fishIrisLeft.sortingOrder += 1;
		fishIrisRight.sortingOrder += 1;
		fishEyeballLeft.sortingOrder += 1;
		fishEyeballRight.sortingOrder += 1;
	}

	public void IncrementLayer(int incrementValue)
	{
		fishIrisLeft.sortingOrder += incrementValue;
		fishIrisRight.sortingOrder += incrementValue;
		fishEyeballLeft.sortingOrder += incrementValue;
		fishEyeballRight.sortingOrder += incrementValue;
	}


	public void DecrementLayer(int incrementValue)
	{
		fishIrisLeft.sortingOrder -= incrementValue;
		fishIrisRight.sortingOrder -= incrementValue;
		fishEyeballLeft.sortingOrder -= incrementValue;
		fishEyeballRight.sortingOrder -= incrementValue;
	}

	public void SetEyePositionZero()
	{
		fishIrisLeft.transform.localPosition = new Vector2 (0,0);
		fishIrisRight.transform.localPosition = new Vector2 (0,0);
	}

	public void SetEyesHSV(float hue, float saturation, float value)
	{
		if (!isGoldOrSilver)
		{
			if (fishIrisRight.materials [0].GetFloat ("_HueShift") != hue) 
			{
				for (int i = 0; i < fishIrisRight.materials.Length; i++) {
					fishIrisLeft.materials [i].SetFloat ("_HueShift", hue);
					fishIrisLeft.materials [i].SetFloat ("_Sat", saturation);
					fishIrisLeft.materials [i].SetFloat ("_Val", value);
				}

				for (int i = 0; i < fishIrisRight.materials.Length; i++) {
					fishIrisRight.materials [i].SetFloat ("_HueShift", hue);
					fishIrisRight.materials [i].SetFloat ("_Sat", saturation);
					fishIrisRight.materials [i].SetFloat ("_Val", value);
				}
			}
		}
		//TODO: eye itself want to have HSV or not?
	}

	#region Clamp Methods per fish

	private float NemoClamp()
	{
		switch (localFishLevel) 
		{
		case 1:
			return 0.8f;
			break;
		case 2:
		case 3:
			return 0.6f;
			break;
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
			return 0.5f;
			break;
		default:
			return 0.5f;
			break;
		}
	}
	private float DoryClamp()
	{
		switch (localFishLevel) 
		{
		case 1:
		case 2:
		case 3:
		case 4:
			return 0.6f;
			break;
		case 5:
		case 6:
			return 0.45f;
			break;
		case 7:
		case 8:
			return 0.35f;
			break;
		case 9:
		case 10:
			return 0.3f;
			break;
		default:
			return 0.4f;
			break;
		}
	}
	private float AnglerClamp()
	{
		switch (localFishLevel) 
		{
		case 1:
		case 2:
		case 3:
		case 4:
			return 0.68f;
			break;
		case 5:
		case 6:
			return 0.5f;
			break;
		case 7:
		case 8:
			return 0.4f;
			break;
		case 9:
		case 10:
			return 0.35f;
			break;
		default:
			return 0.4f;
			break;
		}
	}

	private float SkeletonClamp()
	{
		switch (localFishLevel) 
		{
		case 1:
			return 1.8f;
			break;
		case 2:
			return 1.6f;
			break;
		case 3:
			return 1.5f;
			break;
		case 4:
			return 1.3f;
			break;
		case 5:
			return 1.2f;
			break;
		case 6:
		case 7:
			return 1.1f;
			break;
		case 8:
		case 9:
		case 10:
			return 1f;
			break;
		default:
			return 1f;
			break;
		}
	}

	private float OrcaClamp()
	{
		switch (localFishLevel) 
		{
		case 1:
			return 0.8f;
			break;
		case 2:
			return 0.65f;
			break;
		case 3:
			return 0.6f;
			break;
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
			return 0.4f;
			break;
		default:
			return 0.4f;
			break;
		}
	}

	private float RudolfClamp()
	{
		switch (localFishLevel) 
		{
		case 1:
			return 0.8f;
			break;
		case 2:
			return 0.7f;
			break;
		case 3:
			return 0.6f;
			break;
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
			return 0.5f;
			break;
		default:
			return 0.5f;
			break;
		}
	}

	private float SnowyClamp()
	{
		switch (localFishLevel) 
		{
		case 1:
		case 2:
			return 0.6f;
			break;
		case 3:
		case 4:
			return 0.5f;
			break;
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
			return 0.4f;
			break;
		default:
			return 0.4f;
			break;
		}
	}

	private float FlowerClamp()
	{
		switch (localFishLevel) 
		{
		case 1:
		case 2:
			return 0.6f;
			break;
		case 3:
		case 4:
			return 0.5f;
			break;
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
			return 0.4f;
			break;
		default:
			return 0.4f;
			break;
		}
	}

	private float DragonClamp()
	{
		switch (localFishLevel) 
		{
		case 1:
			return 0.6f;
			break;
		case 2:
			return 0.65f;
			break;
		case 3:
			return 0.55f;
			break;
		case 4:
		case 5:
			return 0.65f;
			break;
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
			return 0.6f;
			break;
		default:
			return 0.6f;
			break;
		}
	}
	#endregion
}
