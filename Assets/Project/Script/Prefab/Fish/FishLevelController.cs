using UnityEngine;
using System.Collections;
using Spine;
using Spine.Unity;

public class FishLevelController : MonoBehaviour {

	[System.NonSerialized]public Transform fishControllerTransform;
	[System.NonSerialized]public SkeletonAnimator fish;
	[System.NonSerialized]public Animator fishAnimator;
	[System.NonSerialized]public MeshRenderersFish fishRend;
	[System.NonSerialized]public MouthAnimationController mouthAnim;
	private int fishLevel;
	private bool isEating = false;
	#region Skin Name
	//public string mouthBoneName;

	public string SkinLevelOneName;
	public string SkinLevelTwoName;
	public string SkinLevelThreeName;
	public string SkinLevelFourName;
	public string SkinLevelFiveName;
	public string SkinLevelSixName;
	public string SkinLevelSevenName;
	public string SkinLevelEightName;
	public string SkinLevelNineName;
	public string SkinLevelTenName;

	private const string babyStayAnimation = "baby_stay";
	private const string babyMoveAnimation = "baby_move";
	private const string teenStayAnimation = "teen_stay";
	private const string teenMoveAnimation = "teen_move";
	private const string adultStayAnimation = "adult_stay";
	private const string adultMoveAnimation = "adult_move";
	#endregion

	void Start()
	{
		fishLevel = 1;
		SkinLevel (1);
	}

	public void EatingTest()
	{
		if (isEating) {
			//if still eating then dont re play any animation
		} else {
			StartCoroutine (EatAnimation(fish.skeleton));
		}
	}

	public void EatingAnimationStart (GameObject food)
	{
		if (food.tag == "Eaten") 
		{
			//if food is eaten already then dont do anything stupid
		} 
		else 
		{
			if (isEating) {
				//if still eating then dont re play any animation
			} else {
				StartCoroutine ("EatAnimation", fish.skeleton);
			}
		}
	}

	IEnumerator EatAnimation(Skeleton fishSkeleton)
	{
		isEating = true;

		//eating anim here
		mouthAnim.PlayFishMouthAnimation();

		yield return new WaitForSeconds (0.5f);
		isEating = false;
	}

	public void IncrementLayer()
	{
		//TODO: increment layer based on spine
		fish.GetComponent<MeshRenderer> ().sortingOrder += 1; 
	}


	public int GetCurrentLevel()
	{
		return fishLevel;
	}

	//CALL WHEN NEEDED ONLY - will reset the animation due to change skin
	public void SkinLevel(int localFishLevel)
	{
		//destroy any ;evel up left behind (if any)
		if (lvlUp != null)
			Destroy (lvlUp);

		string name;

		switch (localFishLevel) 
		{
		case 1:
			name = SkinLevelOneName;
			break;
		case 2:
			name = SkinLevelTwoName;
			break;
		case 3: 
			name = SkinLevelThreeName;
			break;
		case 4:
			name = SkinLevelFourName;
			break;
		case 5:
			name = SkinLevelFiveName;
			break;
		case 6: 
			name = SkinLevelSixName;
			break;
		case 7: 
			name = SkinLevelSevenName;
			break;
		case 8:
			name = SkinLevelEightName;
			break;
		case 9:
			name = SkinLevelNineName;
			break;
		case 10:
			name = SkinLevelTenName;
			break;
		default:
			name = SkinLevelOneName;
			//Debug.LogError ("Error - Invalid Level given");
			break;
		}
		fish.skeleton.SetSkin (name);
		fish.skeleton.SetToSetupPose ();

		mouthAnim.ResetMouthPosition ();

		fishLevel = localFishLevel;
		AdjustAnimation ();
	}

	public void AdjustAnimation()
	{
		//Debug.Log ("Adjust Animation Called - Fish Level " + fishLevel);
		switch (fishLevel) 
		{
		case 1:
		case 2:
		case 3:
			fishAnimator.Play (GetAnimName(1));
			break;
		case 4:
		case 5:
		case 6:
			fishAnimator.Play (GetAnimName(2));
			break;
		case 7:
		case 8:
		case 9:
		case 10:
			fishAnimator.Play (GetAnimName(3));
			break;
		default:
			//Debug.Log ("Error - Fish Level Animation Play error");
			break;
		}
	}

	private string GetAnimName(int fishCategory)
	{
		bool isMoving = fishAnimator.GetBool ("isMoving");
		switch (fishCategory) 
		{
		case 1:
			if (isMoving)
				return babyMoveAnimation;
			else
				return babyStayAnimation;
			break;
		case 2:
			if (isMoving)
				return teenMoveAnimation;
			else
				return teenStayAnimation;
			break;
		case 3:
			if (isMoving)
				return adultMoveAnimation;
			else
				return adultStayAnimation;
			break;
		default:
			//Debug.LogError ("GetAnimName Error - Wrong Fish Category");
			return null;
			break;
		}
	}

	GameObject lvlUp;

	public void LevelUpAnimationStart()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxLevelUp);
		StartCoroutine (LevelUpAnim());
	}

	public void DestroyLeveLUp()
	{
		if (lvlUp != null)
			Destroy (lvlUp);
	}

	private IEnumerator LevelUpAnim()
	{
		Vector3 bubbleScale = gameObject.transform.lossyScale;
		//Player dead Animation
		GameObject levelUp = (GameObject)Resources.Load("prefab/Level Up Prefab",typeof(GameObject));
		lvlUp = Instantiate (levelUp) as GameObject;
		//calc scale
		Vector3 newScale = new Vector3(lvlUp.transform.localScale.x * bubbleScale.x , lvlUp.transform.localScale.y * bubbleScale.y);
		lvlUp.transform.localScale = newScale;
		//lvlUp.transform.position = new Vector3 (fishControllerTransform.transform.position.x - 0.18f, fishControllerTransform.transform.position.x - 0.75f);
		lvlUp.transform.SetParent (fishControllerTransform);

		//lvlUp.transform.SetParent (fish.skeleton.FindBone("root"));

		lvlUp.transform.localPosition = new Vector3 (0, -0.15f - ((fishLevel -1 )* 0.02f));
		//lvlUp.transform.localPosition = new Vector3 (0,0);
		yield return new WaitForSeconds (0.1f);
		if (lvlUp.GetComponent<MeshRenderer> () != null)
		{
			lvlUp.GetComponent<MeshRenderer> ().enabled = true;
			yield return new WaitForSeconds (5f);
		}
		

		if (lvlUp != null) 
		{
			Destroy (lvlUp);
		}	
	}

}
