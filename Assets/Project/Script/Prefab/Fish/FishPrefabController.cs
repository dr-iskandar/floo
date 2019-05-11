using UnityEngine;
using System.Collections;
using Spine;
using Spine.Unity;

public class FishPrefabController : MonoBehaviour {

	//name of the ish. Will import the fish bsed on name
	private string fishVariant;
	public string fishName;
	public bool silver;
	public bool gold;
	//Get All The parts of the fish
	#region Fish Parts
	[System.NonSerialized]public GameObject fishHead;
	[System.NonSerialized]public GameObject shineEffect;
	[System.NonSerialized]public GameObject hitBox;
	[System.NonSerialized]public GameObject playerName;
	[System.NonSerialized]public GameObject fishShadow;
	[System.NonSerialized]public GameObject theHeadObject;
	[System.NonSerialized]public GameObject theFish;
	[System.NonSerialized]public GameObject theFishEyes;
	[System.NonSerialized]public GameObject buffParent;
	[System.NonSerialized]public GameObject ingamePlayerName;
	[System.NonSerialized]public GameObject fishBodies;
	#endregion

	//private Vector3 fishScale = new Vector3 (0.049f, 0.049f, 0.049f);

	void Awake()
	{
		LoadResources ();
		SetParentingBond ();
		ReferObjects ();
	}

	private void ReferObjects()
	{
		//Fish Controller Refer
		FishController fishControl = gameObject.GetComponent<FishController>();
		fishControl.headHitBox = hitBox;
		fishControl.fishIndicator = ingamePlayerName.GetComponent<FishTextMesh>();
		fishControl.tFishHead = fishHead.transform;
		fishControl.tFishBodies = fishBodies.transform;
		fishControl.fishHead = theHeadObject;
		fishControl.buffControl = theHeadObject.GetComponent<BuffController> ();
		fishControl.fishIdleController = gameObject.GetComponent<IdleController> ();
		fishControl.fishLevelControl = theHeadObject.GetComponent<FishLevelController> ();
		fishControl.fishEyeControl = theFishEyes.GetComponent<FishEyeController> ();
		fishControl.fishHSVControl = fishHead.GetComponent<FishHSVController> ();

		//Idle Controller Refer
		IdleController idleControl = gameObject.GetComponent<IdleController>();
		idleControl.fishAnimator = theFish.GetComponent<Animator> ();
		idleControl.fishTransform = gameObject.transform;

		//Mesh Renderers Fish Refer
		MeshRenderersFish renderFish = fishHead.GetComponent<MeshRenderersFish>();
		renderFish.fishMeshRenderer = theFish.GetComponent<MeshRenderer> ();
		renderFish.fishText = playerName.GetComponent<MeshRenderer> ();

		//Fish HSV Controller Refer
		FishHSVController fishHSVControl = fishHead.GetComponent<FishHSVController>();
		fishHSVControl.fish = fishHead.GetComponent<MeshRenderersFish> ();
		fishHSVControl.fishEye = theFishEyes.GetComponent<FishEyeController>();
		fishHSVControl.blinkEffect = shineEffect;

		//Shadow Controller Refer
		ShadowController shadowControl = fishShadow.GetComponent<ShadowController>();
		shadowControl.fishTransform = theFish.transform;

		//Buff Controller Refer
		BuffController buffControl = theHeadObject.GetComponent<BuffController>();
		buffControl.BuffAnimation = buffParent.transform.GetChild (0).GetComponent<SkeletonAnimation> ();
		buffControl.fishRenderers = fishHead.GetComponent<MeshRenderersFish> ();
		buffControl.buffParentTransform = buffParent.transform;
		buffControl.buffAnimationObject = buffParent.transform.GetChild (0).gameObject;
		buffControl.nosBuffGameObject = buffParent.transform.GetChild (1).gameObject;

		//Fish Level Controller Refer - Fish Skin Name per level Previously included
		FishLevelController fishLevelControl = theHeadObject.GetComponent<FishLevelController>();
		fishLevelControl.fishControllerTransform = gameObject.transform;
		fishLevelControl.fish = theFish.GetComponent<SkeletonAnimator> ();
		fishLevelControl.fishAnimator = theFish.GetComponent<Animator> ();
		fishLevelControl.fishRend = fishHead.GetComponent<MeshRenderersFish> ();
		fishLevelControl.mouthAnim = theFish.transform.GetChild (0).GetComponent<MouthAnimationController> ();

	}

	private void SetParentingBond()
	{
		theFishEyes.transform.SetParent (theFish.transform);

		buffParent.transform.SetParent (theHeadObject.transform);
		theFish.transform.SetParent (theHeadObject.transform);

		shineEffect.transform.SetParent (fishHead.transform);
		hitBox.transform.SetParent (fishHead.transform);
		playerName.transform.SetParent (fishHead.transform);
		fishShadow.transform.SetParent (fishHead.transform);
		theHeadObject.transform.SetParent (fishHead.transform);
		ingamePlayerName.transform.SetParent (fishHead.transform);

		fishBodies.transform.SetParent (gameObject.transform);
		fishHead.transform.SetParent (gameObject.transform);
	}

	private void LoadResources()
	{
		//determine Silver/Gold or none
		if (silver) {
			shineEffect = Instantiate ((GameObject)Resources.Load ("Prefab/Fishes/Fish Parts/Silver Effect", typeof(GameObject)));
			fishVariant = fishName + "Silver";

			theFishEyes = Instantiate((GameObject)Resources.Load("Prefab/Fishes/Fish Unique/"+fishName+"/" + fishVariant + " Eyes Parent",typeof(GameObject)));
			theFishEyes.GetComponent<FishEyeController> ().isGoldOrSilver = true;
		} else if (gold) {
			shineEffect = Instantiate ((GameObject)Resources.Load ("Prefab/Fishes/Fish Parts/Gold Effect", typeof(GameObject)));
			fishVariant = fishName + "Gold";

			theFishEyes = Instantiate((GameObject)Resources.Load("Prefab/Fishes/Fish Unique/"+fishName+"/" + fishVariant + " Eyes Parent",typeof(GameObject)));
			theFishEyes.GetComponent<FishEyeController> ().isGoldOrSilver = true;
		} else {
			shineEffect = Instantiate ((GameObject)Resources.Load ("Prefab/Fishes/Fish Parts/No Effect", typeof(GameObject)));
			fishVariant = fishName;

			theFishEyes = Instantiate((GameObject)Resources.Load("Prefab/Fishes/Fish Unique/"+fishName+"/" + fishVariant + " Eyes Parent",typeof(GameObject)));
			theFishEyes.GetComponent<FishEyeController> ().isGoldOrSilver = false;
		}

		//The object, uses fish name in load process
		theFish = Instantiate((GameObject)Resources.Load("Prefab/Fishes/Fish Unique/"+fishName+"/"+ fishVariant,typeof(GameObject)));

		theHeadObject = Instantiate( (GameObject)Resources.Load("Prefab/Fishes/Fish Unique/"+fishName+"/" + fishName + " Head Object",typeof(GameObject)));

		fishHead = Instantiate((GameObject)Resources.Load("Prefab/Fishes/Fish Parts/Fish Head",typeof(GameObject)));

		hitBox = Instantiate((GameObject)Resources.Load("Prefab/Fishes/Fish Parts/Hit Box",typeof(GameObject)));
		playerName = Instantiate((GameObject)Resources.Load("Prefab/Fishes/Fish Parts/Player Name",typeof(GameObject)));
		fishShadow = Instantiate((GameObject)Resources.Load("Prefab/Fishes/Fish Parts/Fish Shadow",typeof(GameObject)));
		buffParent = Instantiate((GameObject)Resources.Load("Prefab/Fishes/Fish Parts/Buff Parent",typeof(GameObject)));
		ingamePlayerName = Instantiate((GameObject)Resources.Load("Prefab/Fishes/Fish Parts/Ingame Player Name",typeof(GameObject)));
		fishBodies = Instantiate((GameObject)Resources.Load("Prefab/Fishes/Fish Parts/Fish Bodies",typeof(GameObject)));
	}
}
