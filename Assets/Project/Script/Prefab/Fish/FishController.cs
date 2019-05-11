using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
public class FishController : MonoBehaviour 
{
	
	[System.NonSerialized]public GameObject headHitBox;
	[System.NonSerialized]public FishTextMesh fishIndicator;
	[System.NonSerialized]public Transform tFishHead;
	[System.NonSerialized]public Transform tFishBodies;
	[System.NonSerialized]public GameObject fishHead;

	[System.NonSerialized]public FishData localData;
	private string fishId;

	//buff
	[System.NonSerialized]public BuffController buffControl;

	[System.NonSerialized]public IdleController fishIdleController;
	[System.NonSerialized]public FishLevelController fishLevelControl;
	[System.NonSerialized]public FishEyeController fishEyeControl;
	[System.NonSerialized]public FishHSVController fishHSVControl;
	//public MeshRenderersFish meshFishControl;
	public string Id
	{
		get
		{
			return fishId;
		}
	}

	private List<GameObject> bodyObjects = new List<GameObject>();
	private int localLevelIndicator;
	private float localExpIndicator;

	public void SetData(FishData data, bool isPlayer)
	{
		//Set The fish to it's layer
		LayerController.Instance.SetFishLayer(this.gameObject,data.level);

		//Fish HSV Determination
		SetFishHSV(data.colorCode);
		SetFishId(data.playerID);
		SetFishBodies(data);

		if (EssentialData.Instance.IsShowHeadCollider)
		{
			headHitBox.SetActive(true);
			headHitBox.GetComponent<MeshRenderer>().sortingOrder = 50;
		}
		else
		{
			headHitBox.SetActive(false);
		}
		//Show the fish
		if (EssentialData.Instance.IsShowFishHead)
		{
			fishHead.SetActive(true);
		}
		else
		{
			fishHead.SetActive(false);
		}

		if (!EssentialData.Instance.IsShowFishHead && !EssentialData.Instance.IsShowHeadCollider)
		{
			fishHead.gameObject.SetActive(true);
		}

		fishIndicator.SetFishName(data.playerName);
		fishIndicator.UpdateFishLevel(data.level.ToString());

		//Buff Effect
		switch (data.buffType)
		{
		case 2:
			buffControl.SetBuffExp ();
			break;
		case 3:
			buffControl.SetBuffInvisible (isPlayer);
			break;
		case 1:
			buffControl.SetBuffInvulnerable ();
			break;
		case 4:
			buffControl.SetBuffSpeed ();
			break;
		case 5:
			buffControl.SetBuffStarPower ();
			break;
		case 6:
			buffControl.SetBuffGold ();
			break;
		default:
			buffControl.SetNoBuff ();
			break;
		}
		SetUIExp (data, isPlayer);

		if (data.isBoost)
		{
			buffControl.ActivateNosEffect ();
			buffControl.SetBuffInvulnerable ();
		}
		else
		{
			buffControl.DeactivateNosEffect ();
		}

		//Set Fish Level & Exp for direct script check
		localLevelIndicator = data.level;
		localExpIndicator = data.experience;

		localData = data;
	}

	private bool expFirstAssign = true;
	//Get the maximum experience of the previous level
	private float GetPreviousLevelMaxExp(string fishSkin, int fishLevel)
	{
		float prevLevelMaxExp = 0;
		if (fishLevel > 1)
		{
			return DefaultParameterManager.Instance.GetFishParameterData(fishSkin, fishLevel - 1).maxExp;
		}
		return prevLevelMaxExp;
	}

	private void SetUIExp(FishData fish, bool isPlayer)
	{
		if (isPlayer) 
		{		
			float currentMaxExp = DefaultParameterManager.Instance.GetFishParameterData(fish.fishSkin, fish.level).maxExp;
			float prevMaxExp = GetPreviousLevelMaxExp(fish.fishSkin, fish.level);
			if (expFirstAssign) {
				
				fishIndicator.SetExpBarData (currentMaxExp - prevMaxExp);
				expFirstAssign = false;
				fishIndicator.ShowExpBar ();
			}
			if (fish.level != localLevelIndicator) {
				int prevLevelMaxExp = 0;

				fishIndicator.SetExpBarData (currentMaxExp - prevMaxExp);
			}
			if (fish.experience != localExpIndicator) {
				
				fishIndicator.CalculateFishExp (fish.experience - prevMaxExp);
			}
		}
		else
		{
			//do nothing if not player fish
		}
	}

	public void SetFishHSV(int colorCode)
	{
		FishHSVData data =  HSVDataControl.Instance.GetFishHSV (colorCode);

		fishHSVControl.SetFishHSV (data);

	}

	public void SetMainMenuFishHSV(int colorCode)
	{
		FishHSVData data =  HSVDataControl.Instance.GetFishHSV (colorCode);
		fishHSVControl.SetFishHSV (data);
	}

	public void SetFishId(string id)
	{
		fishId = id;
	}

	public void RemoveFish()
	{
		Destroy(this.gameObject);
	}

	public void SetPosition(float x, float y, float angle)
	{
		try
		{
			gameObject.transform.localPosition = new Vector2(x, y);
			fishHead.transform.localEulerAngles = new Vector3(0, 0, angle);

			headHitBox.transform.localEulerAngles = new Vector3(0, 0, angle);
		}
		catch(System.Exception e)
		{
			Debug.Log("Error angle " + angle);
		}
	}

	public void SetFishBodies(FishData data)
	{
		//Set The position of the head
		SetPosition(data.xPosition, data.yPosition, data.fishAngle);
		//Set The size of the head
		SetFishSize(data.width, data.height);
		//Set Fish Level According to the level given in data
		SetFishLevel(data.level);

		if (EssentialData.Instance.IsShowBody)
		{
			//Set Bodies position and size
			for (int i = 0; i < data.bodies.Count; i++)
			{
				FishBodyData bodyData = data.bodies[i];

				if (i >= bodyObjects.Count)
				{
					//Create new body object for the fish
					GameObject go = Instantiate(headHitBox) as GameObject;
					go.name = "Fish " + data.playerID + " Body " + (i + 1);
					go.transform.SetParent(tFishBodies);
					go.transform.localPosition = new Vector3(bodyData.xPosition, bodyData.yPosition, 0);
					go.transform.localScale = new Vector3(bodyData.width, bodyData.height, 1.0f);
					go.SetActive(true);
					bodyObjects.Add(go);
				}
				else
				{
					bodyObjects[i].transform.position = new Vector3(bodyData.xPosition, bodyData.yPosition, 0);
					bodyObjects[i].transform.localScale = new Vector3(bodyData.width, bodyData.height, 1.0f);

				}
			}
		}
	}
	[System.NonSerialized]public bool firstAssign = true;
	public void SetFishLevel(int fishLevel)
	{
		//check skin level
		if (fishLevel == fishLevelControl.GetCurrentLevel())
		{
			firstAssign = false;
			//do nothing if same skin level
		} 
		else 
		{
			//change if incorrect
			fishLevelControl.SkinLevel(fishLevel);
			fishEyeControl.SetEyeLevel (fishLevel);

//			if (fishLevelControl.GetCurrentLevel() > fishLevel)
//			{
//				//don't play level up if it is level down skin change
//				firstAssign = false;
//			}
//			else if (fishLevel == 1)
//			{
//				//do nothing if it is level 1 skin
//				firstAssign = false;
//			}
//			else if (firstAssign)
//			{
//				//if the fish is firstly assigned no need level up
//				firstAssign = false;
//			}
//			else 
//			{
//				fishLevelControl.LevelUpAnimationStart ();
//			}
				
			if (firstAssign) 
			{
				firstAssign = false;
			} 
			else 
			{
				fishLevelControl.LevelUpAnimationStart ();
			}
		}
	
	}

	public void DeleteLevelUpAnim()
	{
		fishLevelControl.DestroyLeveLUp ();
	}

	public void ResetFishPositionAndText()
	{
		//size and position adjust
		if (fishLevelControl.fish.GetComponent<MeshRenderer> ().sortingOrder > 50)
			fishLevelControl.fish.GetComponent<MeshRenderer> ().sortingOrder -= 50;
		if (fishEyeControl.GetOrderLayer() > 50)
			fishEyeControl.DecrementLayer(50);
		gameObject.transform.localScale = new Vector3 (1f,1f,1);
		fishLevelControl.fish.transform.localEulerAngles = new Vector3 (0,0,90);

		//text adjust
		fishIndicator.shadowName.gameObject.SetActive (true);
		fishIndicator.fishLvl.gameObject.SetActive (true);
		fishIndicator.expBarGameObj.gameObject.SetActive (true);
		fishIndicator.shadowLvl.gameObject.SetActive (true);
		//fishIndicator.fishName.color = Color.white;
		//fishIndicator.shadowName.color = Color.white;
	}

	//Set fish size by it's width and height
	public void SetFishSize(float width, float height)
	{
		tFishHead.localScale = new Vector3(width, height,1.0f);

		BoxCollider collider = this.gameObject.GetComponent<BoxCollider>();
		if (collider != null)
		{
			collider.size = new Vector3(width, height, 1.0f);
		}
	}

	//Set fish size by it's level
	public void SetFishSize(int level)
	{
		float scale = 0;
		switch (level)
		{
		case 1:
			scale = FishTag.ScaleLevelOne;
			break;
		case 2:
			scale = FishTag.ScaleLevelTwo;		
			break;
		case 3:
			scale = FishTag.ScaleLevelThree;
			break;
		case 4:
			scale = FishTag.ScaleLevelFour;
			break;
		case 5:
			scale = FishTag.ScaleLevelFive;
			break;
		case 6:
			scale = FishTag.ScaleLevelSix;
			break;
		case 7:
			scale = FishTag.ScaleLevelSeven;
			break;
		case 8:
			scale = FishTag.ScaleLevelEight;
			break;
		case 9:
			scale = FishTag.ScaleLevelNine;
			break;
		case 10:
			scale = FishTag.ScaleLevelTen;
			break;
		default:
			Debug.LogError ("Invalid fish level! - Size");
			break;
		}
		gameObject.transform.localScale = new Vector3 (scale,scale,gameObject.transform.localScale.z);
	}

	public void ResetAlpha()
	{
		//fishIndicator.fishName.color = Color.white;
		//fishIndicator.shadowName.color = Color.white;
	}

	IEnumerator FadeOutText()
	{
//		Debug.Log ("Fade Out Text");

		Color textFadeColor = fishIndicator.fishName.color;
		Color textFadeColorShadow = fishIndicator.shadowName.color;
		textFadeColor.a = 1;
		for (int i = 0; i < 20; i++)
		{
			fishIndicator.shadowName.gameObject.SetActive (false);
			fishIndicator.fishLvl.gameObject.SetActive (false);
			fishIndicator.expBarGameObj.gameObject.SetActive (false);
			fishIndicator.shadowLvl.gameObject.SetActive (false);
			textFadeColor.a -= 0.05f;
			fishIndicator.fishName.color = textFadeColor;
			fishIndicator.shadowName.color = textFadeColorShadow;
			yield return new WaitForSeconds (0.02f);
		}
		ResetAlpha ();
	}

	IEnumerator FadeInText()
	{
//		Debug.Log ("Fade In Text");
		
		ResetAlpha ();
		Color textFadeColor = fishIndicator.fishName.color;
		Color textFadeColorShadow = fishIndicator.shadowName.color;
		textFadeColor.a = 0;
		for (int i = 0; i < 20; i++)
		{
			fishIndicator.shadowName.gameObject.SetActive (true);
			fishIndicator.fishLvl.gameObject.SetActive (true);
			fishIndicator.expBarGameObj.gameObject.SetActive (true);
			fishIndicator.shadowLvl.gameObject.SetActive (true);
			textFadeColor.a += 0.05f;
			fishIndicator.fishName.color = textFadeColor;
			fishIndicator.shadowName.color = textFadeColorShadow;
			yield return new WaitForSeconds (0.02f);
		}
		//fishIndicator.shadowName.gameObject.SetActive (true);
		//fishIndicator.fishLvl.gameObject.SetActive (true);
		//fishIndicator.expBarGameObj.gameObject.SetActive (true);
		//fishIndicator.shadowLvl.gameObject.SetActive (true);
	}

	//bool isEnteringObstacle = false;



	public void TriggerTextFadeIn()
	{
		//if (!isEnteringObstacle) 
		//{
			//isEnteringObstacle = true;
			StopCoroutine (FadeOutText());
			StartCoroutine (FadeInText());
		//}
	}

	public void TriggerTextFadeOut()
	{
		//if (isEnteringObstacle)
		//{
			//isEnteringObstacle = false;
			StopCoroutine (FadeInText());
			StartCoroutine (FadeOutText());
		//}
	}

	public void SetTag(string fishTag)
	{
		gameObject.tag = fishTag;
		if (fishTag == EssentialData.TAG_PLAYER) {
			//Set order layer this line MUST be run only ONCE per game
			fishLevelControl.IncrementLayer();
			fishEyeControl.IncrementLayer();
			buffControl.IncrementLayer ();
			fishEyeControl.EnableEyes();
		} else if (fishTag == EssentialData.TAG_ENEMY) {
			//Disable Eyes animation
			fishEyeControl.DisableEyes();
		}
	}

	public void StartDeathAnimation(FishController removeFish, FishObjectPooling fishPool)
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxDie);
		StartCoroutine (DeathAnimation(removeFish, fishPool));
	}

	public void StartDeathAnimation()
	{
//		Debug.Log ("Please make a validation, that IF fish object is active." +
//			"\nCoroutine couldn't be started because the the game object 'Fish Prefab Nemo Gold 001' is inactive!");
		StartCoroutine (DeathAnimation());
	}


	int count = 6;
	float delay = 0.001f;
	IEnumerator DeathAnimation(FishController removeFish,FishObjectPooling fishPool)
	{
		Vector3 deathScale = fishHead.transform.lossyScale;
		float increment = 0.04f;
		for (int i = 0; i < count/2; i++)
		{
			gameObject.transform.localScale += new Vector3 (increment,increment);
			yield return new WaitForSeconds (delay/2);
		}
		float decrement = 0.18f;
		for (int i = 0; i < count; i++)
		{
			gameObject.transform.localScale -= new Vector3 (decrement,decrement);
			yield return new WaitForSeconds (delay);
		}

		gameObject.transform.localScale = new Vector3 (0,0);

		//Player dead Animation
		GameObject deadEffect = (GameObject)Resources.Load("prefab/Death Prefab",typeof(GameObject));
		GameObject death = Instantiate (deadEffect) as GameObject;
		//calc scale
		Vector3 newScale = new Vector3(death.transform.localScale.x * deathScale.x , death.transform.localScale.y * deathScale.y);
		death.transform.localScale = newScale;
		death.transform.position = fishHead.transform.position;

		yield return new WaitForSeconds (2f);
		Destroy (death);
		gameObject.transform.localScale = new Vector3(1,1,1);

		fishPool.ReturnGameObject(removeFish.localData.fishSkin, removeFish.gameObject);
	}

	IEnumerator DeathAnimation()
	{
		Vector3 deathScale = fishHead.transform.lossyScale;
		float increment = 0.04f;
		for (int i = 0; i < count/2; i++)
		{
			gameObject.transform.localScale += new Vector3 (increment,increment);
			yield return new WaitForSeconds (delay/2);
		}
		float decrement = 0.18f;
		for (int i = 0; i < count; i++)
		{
			gameObject.transform.localScale -= new Vector3 (decrement,decrement);
			yield return new WaitForSeconds (delay);
		}

		gameObject.transform.localScale = new Vector3 (0,0);

		//Player dead Animation
		GameObject deadEffect = (GameObject)Resources.Load("prefab/Death Prefab",typeof(GameObject));
		GameObject death = Instantiate (deadEffect) as GameObject;
		//calc scale
		Vector3 newScale = new Vector3(death.transform.localScale.x * deathScale.x , death.transform.localScale.y * deathScale.y);
		death.transform.localScale = newScale;
		death.transform.position = fishHead.transform.position;

		yield return new WaitForSeconds (2f);
		Destroy (death);
		//gameObject.transform.localScale = new Vector3(1,1,1);
	}
}
