using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayGameController : MonoBehaviour 
{
	
	public static DisplayGameController Instance;

	public MovementControl movementControl;
	public GameObject mainCameraUI;
	public GameObject quadParent;

	//local
	int fishUpdateCount = 0;
	int predatorUpdateCount = 0;
	int mapUpdateCount = 0;

	//The fish controller for the player
	private FishController playerFish;

	//The list of fish registered in the current state
	private List<FishController> registeredFish;
	//The list of food registered in the current state
	private List<FoodController> registeredFood;
	//This list of map feature registered in the current state
	private List<MapFeatureController> displayedMapFeatures;
	//This list of predator registered in the current state
	private List<PredatorController> registeredPredator;

	private FishData previousPlayerData;

	private FoodObjectPooling foodPool;
	private FishObjectPooling fishPool;
	private PredatorObjectPooling predatorPool;

	private BGMData usedBGMData;

	private bool isObjectPooled = false;
	private bool isPlayerFishLoaded = false;

	void Awake () 
	{
		Instance = this;
		registeredFish = new List<FishController>();
		registeredFood = new List<FoodController>();
		displayedMapFeatures = new List<MapFeatureController>();
		registeredPredator = new List<PredatorController>();
		isObjectPooled = false;
		isPlayerFishLoaded = false;

		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	public void PlayPlayerFishDeadAnimation()
	{
		playerFish.StartDeathAnimation ();
	}

	public void StartObjectPooling()
	{
		StartCoroutine(InitializePool());
	}

	IEnumerator InitializePool()
	{
		if(!isObjectPooled)
		{
			UIGameController.Instance.ShowNotificationMessage(LanguageManager.Instance.GetMessage("LOA0012"));
			yield return new WaitForSeconds(0.1f);

			foodPool = FoodObjectPooling.Instance;

			fishPool = FishObjectPooling.Instance;

			predatorPool = PredatorObjectPooling.Instance;

			isObjectPooled = true;
		}
		yield return null;

		GameController.Instance.StartListeningForSocketUpdates();
	}

	public void LoadPlayerFish(string skinName)
	{
		if (!isPlayerFishLoaded)
		{
			GameObject go = fishPool.GetGameObject(skinName);
			FishController control = go.GetComponent<FishController>();
			if (control == null)
			{
				control = go.AddComponent<FishController>();
			}

			control.SetTag(EssentialData.TAG_PLAYER);

			//Update Movement Control
			movementControl.gameObject.SetActive(true);
			movementControl.fishParentTransform = go.transform;
			movementControl.fishEye = control.fishEyeControl;

			playerFish = control;

			isPlayerFishLoaded = true;
		}
	}

	public void UpdatePlayer(FishData playerData, List<FoodData> listFoodData)
	{
		UpdateCamera(playerData.xPosition,playerData.yPosition);

		if (previousPlayerData == null ||
			previousPlayerData.level != playerData.level)
		{
			UpdateViewPort(playerData);
		}
		playerFish.SetData(playerData,true);

		//Update UI information
		UIGameController.Instance.UpdatePlayerPosition(playerData.xPosition, playerData.yPosition);
		UIGameController.Instance.UpdatePlayerLevel(playerData.level, playerData.experience);
		UIGameController.Instance.UpdatePlayerPoint(Mathf.RoundToInt( playerData.experience ));

		previousPlayerData = playerData;

		OcculsionManager.Instance.UpdateOcculstion(playerData);

		//Food Eaten Calculation
		float playerRadius = playerData.width /2;
		float foodRadius;

		try
		{
			for (int i=0; i < listFoodData.Count; i++)
			{
				foodRadius = listFoodData[i].width/2;

				//compare food pos and player pos
				float xDiff = Mathf.Abs( listFoodData[i].xPosition - playerData.xPosition);
				float yDiff = Mathf.Abs( listFoodData[i].yPosition - playerData.yPosition);
				float foodDistance = xDiff + yDiff;
				//if the distance is lesser than both radius added, it is eaten
				if (foodDistance <= (foodRadius + playerRadius))
				{
					//Search the GO, play animation with the gameobject and change its tag to eaten
					int idx = registeredFood.FindIndex(x=>x.foodId == listFoodData[i].foodID);
					if (idx >=0)
					{
						playerFish.fishLevelControl.EatingAnimationStart(registeredFood[idx].gameObject);
						registeredFood[idx].gameObject.tag = "Eaten";
					}
				}


			}
		}
		catch(System.Exception e)
		{
			Debug.Log("Food Animation Error - " + gameObject.name + " : " + e.ToString());
		}
		RandomBubbleController.Instance.AutomateRandomBubble (playerData);

		PlayBGMSound(playerData);

	}

	void PlayBGMSound(FishData playerData)
	{
		BGMData setBGMData = BGMData.Ingame;
		if (playerData.isTargetedByPredator)
		{
			setBGMData = BGMData.PredatorChase;
			//Debug.Log("Is targeted");
			UIGameController.Instance.ShowPanelPredatorRed ();
			UIGameController.Instance.AnimatePredatorRed ();
		}
		else if (playerData.isInPredatorRange == 1) //octopus
		{
			setBGMData = BGMData.PredatorArea;
			//Debug.Log("In Predator range");
			UIGameController.Instance.ShowPanelPredatorRed ();
		}
		else if (playerData.isInPredatorRange == 2) //shark
		{
			setBGMData = BGMData.PredatorAreaShark;
			//Debug.Log("In Predator range");
			UIGameController.Instance.ShowPanelPredatorShark ();
		}

		if (usedBGMData != setBGMData)
		{
			usedBGMData = setBGMData;
			SoundUtility.Instance.SetBGM(usedBGMData);
			UIGameController.Instance.HidePanelPredator ();
		}
	}



	public void UpdateFish(List<FishData> listOpponent, FishData playerFish)
	{
		for (int idx = 0; idx < listOpponent.Count; idx++)
		{
			FishData data = listOpponent[idx];
			int fIdx = registeredFish.FindIndex(x => x.Id.Equals( data.playerID));
			//Find the given data in the list. If it's not in the list, create new food for the data
			if (fIdx < 0)
			{
				GameObject go = fishPool.GetGameObject(data.fishSkin);

				FishController control = go.GetComponent<FishController>();
				control.SetTag(EssentialData.TAG_ENEMY);
				control.SetData(data, false);
				registeredFish.Add(control);
			}
			//When it's already in the list, just reset the data
			else
			{
				registeredFish[fIdx].SetData(data,false);
			}
		}

		fishUpdateCount += 1;
		//Clear food after 10 updates, to reduce procesing cost
		if (fishUpdateCount >= 5)
		{
			ClearFish(listOpponent, playerFish);
			fishUpdateCount = 0;
		}
	}
		
	public void UpdateFoods(List<FoodData> listFood)
	{
		for (int idx = 0; idx < listFood.Count; idx++)
		{
			FoodData data = listFood[idx];
			int fIdx = registeredFood.FindIndex(x => x.foodId.Equals( data.foodID));
			//Find the given data in the list. If it's not in the list, create new food for the data
			if (fIdx < 0)
			{
				GameObject go = foodPool.GetGameObject(data.foodKeyword);
				FoodController control = go.GetComponent<FoodController>();
				if (control == null)
				{
					control = go.AddComponent<FoodController>();
				}
				LayerController.Instance.SetFoodLayer(go);
				go.gameObject.tag = "Food";
				control.SetData(data);
				registeredFood.Add(control);
			}
			//When it's already in the list, just reset the data
			else
			{
				//registeredFood[fIdx].SetData(data);
			}
		}

		ClearFood(listFood);

	}

	public void UpdatePredators(List<PredatorData> listPredator)
	{
		for (int idx = 0; idx < listPredator.Count; idx++)
		{
			PredatorData data = listPredator[idx];
			int fIdx = registeredPredator.FindIndex(x => x.predatorId.Equals( data.id));
			//Find the given data in the list. If it's not in the list, create new predator
			if (fIdx < 0)
			{
				GameObject go = predatorPool.GetGameObject(data.assetKeyword);
				PredatorController control = go.GetComponent<PredatorController>();
				//reset animation before deploying
				control.SetAnimationToMove();

				LayerController.Instance.SetFishLayer(go, 10);
				if (control == null)
				{
					control = go.AddComponent<PredatorController>();
				}
				control.SetData(data);
				registeredPredator.Add(control);
//				Debug.Log ("predator id = "+data.id);
			}
			//When it's already in the list, just reset the data
			else
			{
				registeredPredator[fIdx].SetData(data);
			}
		}

		predatorUpdateCount += 1;
		//Clear food after 10 updates, to reduce procesing cost
		if (predatorUpdateCount >= 10)
		{
			ClearPredator(listPredator);
			predatorUpdateCount = 0;
		}
	}

	private bool overlayChange = false;

	public void ReturnFishOverlay()
	{
		if (overlayChange)
		{
			playerFish.fishLevelControl.fish.GetComponent<MeshRenderer> ().sortingOrder -= 50;
			playerFish.fishEyeControl.DecrementLayer(50);

			playerFish.transform.localScale = new Vector3 (1f,1f,1);
			playerFish.fishLevelControl.fish.transform.localEulerAngles = new Vector3 (0,0,90);

			overlayChange = false;
		}
	}

	public void SetParentPlayerFish(Transform parent)
	{
		if (!overlayChange)
		{
			playerFish.fishLevelControl.fish.GetComponent<MeshRenderer> ().sortingOrder += 50;
			playerFish.fishEyeControl.IncrementLayer (50);
			overlayChange = true;

			mainCameraUI.GetComponent<Camera>().orthographicSize = 0.8f;
			playerFish.transform.SetParent (parent);
		}

		playerFish.transform.localPosition = new Vector3 (0,0,0);
		playerFish.fishLevelControl.fish.transform.eulerAngles = Vector3.zero;

		int level = GetFishLevel();

		if (level == 6)
		{
			playerFish.transform.localScale = new Vector3 (0.7f,0.7f,0.7f);
		}
		else if (level == 7)
		{
			playerFish.transform.localScale = new Vector3 (0.54f,0.54f,0.54f);
		}
		else if (level == 8)
		{
			playerFish.transform.localScale = new Vector3 (0.45f,0.45f,0.45f);
		}
		else if (level == 9)
		{
			playerFish.transform.localScale = new Vector3 (0.38f,0.38f,0.38f);
		}
		else if (level == 10)
		{
			playerFish.transform.localScale = new Vector3 (0.34f,0.34f,0.34f);
		}
		else
		{
			playerFish.transform.localScale = new Vector3 (1,1,1);
		}
	}

	public void UpdateMapFeature(List<MapData> listMap)
	{
		for (int idx = 0; idx < listMap.Count; idx++)
		{
			MapData data = listMap[idx];
			int fIdx = displayedMapFeatures.FindIndex(x => x.mapFeatureId.Equals( data.mapFeatureId));
			//Find the given data in the list. If it's not in the list, display new map feature
			if (fIdx < 0)
			{
				Object prefab = AssetManager.Instance.GetPrefabByKeyword(data.mapFeatureKeyword);
				GameObject go = Instantiate(prefab) as GameObject;
				MapFeatureController control = go.AddComponent<MapFeatureController>();
				control.SetData(data);
				displayedMapFeatures.Add(control);
			}
			//When it's already in the list, just reset the data
			else
			{
				//displayedMapFeatures[fIdx].SetData(data);
			}
		}

		mapUpdateCount += 1;
		//Clear maps after 10 updates, to reduce procesing cost
		if (mapUpdateCount >= 10)
		{
			ClearMap(listMap);
			mapUpdateCount = 0;
		}
	}

	private void UpdateCamera(float x, float y)
	{
		float z = mainCameraUI.transform.position.z;
		mainCameraUI.transform.position = new Vector3 (x,y,z);
	}

	private void UpdateViewPort(FishData playerDaata)
	{
		FishParameterData data = DefaultParameterManager.Instance.GetFishParameterData(playerDaata.fishSkin, playerDaata.level);
		//Debug.Log("Fish level " + playerDaata.level);
		float cam = 0.8f;
		if (data != null)
		{
			cam = data.cameraSize;
		}

		mainCameraUI.GetComponent<Camera>().orthographicSize = cam;
	}

	public void UpdateBoostBar(float boostValue)
	{
		UIGameController.Instance.CalculateNosBar(boostValue);
	}

	public void ResetFishBuff()
	{
		playerFish.buffControl.SetNoBuff();
	}

	public int GetFishLevel()
	{
		return playerFish.fishLevelControl.GetCurrentLevel ();
	}

	public Vector2 GetPlayerPosition()
	{
		Vector2 result;
		try
		{
			result = new Vector2 (previousPlayerData.xPosition, previousPlayerData.yPosition);
		}
		catch 
		{
			result = new Vector2 (0,0);
		}
		return result;
	}

	public bool GetBoostStatus()
	{
		return previousPlayerData.isBoost;
	}

	public void PlayPointAnimation(int point)
	{
		if (playerFish.fishIndicator.ptsControl.enabled == false)
			playerFish.fishIndicator.ptsControl.enabled = true;
		playerFish.fishIndicator.ptsControl.AnimatePts(point);
	}

	#region Clear Unused UI element
	public void ClearAllPooledObject()
	{
		ClearFish(new List<FishData>());
		ClearFood(new List<FoodData>());
		ClearPredator(new List<PredatorData>());
	}

	void ClearFish(List<FishData> listFish)
	{
		//Clear foods that's not in the data list
		List<FishController> removeFish = new List<FishController>();

		//Loop trough all the registered food. Add to removed list if it's not in the data list
		for (int i = 0; i < registeredFish.Count; i++)
		{
			string id = registeredFish[i].Id;
			int idx = listFish.FindIndex(x=>x.playerID.Equals( id ));
			//When the registered food is nowhere to found in the list data, register it to be deleted
			if (idx < 0)
			{
				removeFish.Add(registeredFish[i]);
			}
		}

		//Remove all the fish in the remove list
		for (int i = 0; i < removeFish.Count; i++)
		{
			registeredFish.Remove(removeFish[i]);	
		}

		//Delete the game object
		for (int i = 0; i < removeFish.Count; i++)
		{
			//Remove unused fishes
			//removeFish[i].firstAssign = true;
			//Debug.Log ("First Assign = " + removeFish[i].firstAssign);
			fishPool.ReturnGameObject(removeFish[i].localData.fishSkin, removeFish[i].gameObject);
		}
	}

	void ClearFish(List<FishData> listFish, FishData playerFish)
	{
		//Clear foods that's not in the data list
		List<FishController> removeFish = new List<FishController>();

		//Loop trough all the registered food. Add to removed list if it's not in the data list
		for (int i = 0; i < registeredFish.Count; i++)
		{
			string id = registeredFish[i].Id;
			int idx = listFish.FindIndex(x=>x.playerID.Equals( id ));
			//When the registered food is nowhere to found in the list data, register it to be deleted
			if (idx < 0)
			{
				removeFish.Add(registeredFish[i]);
			}
		}

		//Remove all the fish in the remove list
		for (int i = 0; i < removeFish.Count; i++)
		{
			registeredFish.Remove(removeFish[i]);	
		}

		//Delete the game object
		for (int i = 0; i < removeFish.Count; i++)
		{
			//check first before removing
			bool isDead = CheckDeathFish(removeFish[i].localData, playerFish);
			if (isDead) 
			{
				//if dead, do animation etc then remove it after animation
				//removeFish[i].firstAssign = true;
				//Debug.Log ("First Assign = " + removeFish[i].firstAssign);
				removeFish[i].StartDeathAnimation(removeFish[i],fishPool);
			} 
			else
			{
				//Remove unused fishes
				//removeFish[i].firstAssign = true;
				//Debug.Log ("First Assign = " + removeFish[i].firstAssign);
				fishPool.ReturnGameObject(removeFish[i].localData.fishSkin, removeFish[i].gameObject);
			}
		}
	}
		
	private float deathWaitTime = 4f;

	public void SetPredatorToDead(string predatorID)
	{
		for (int i= 0; i< registeredPredator.Count; i++)
		{
			if (registeredPredator[i].predatorId == predatorID) 
			{
				registeredPredator [i].SetAnimationToDeath ();
				registeredPredator [i].SetPredatorDoingDeathAnim (true);
				registeredPredator [i].SetWaitTimeForAnimation(deathWaitTime);
			}
		}
	}


	void ClearPredator(List<PredatorData> listPredator)
	{
		//Clear foods that's not in the data list
		List<PredatorController> removeGameObject = new List<PredatorController>();

		//Loop trough all the registered food. Add to removed list if it's not in the data list
		for (int i = 0; i < registeredPredator.Count; i++)
		{
			string id = registeredPredator[i].predatorId;

			if (!registeredPredator [i].isdoingDeathAnim) 
			{
				int idx = listPredator.FindIndex (x => x.id.Equals (id));
				//When the registered food is nowhere to found in the list data, register it to be deleted
				if (idx < 0) {
					removeGameObject.Add (registeredPredator [i]);
				}
			}
		}

		//Remove all the fish in the remove list
		for (int i = 0; i < removeGameObject.Count; i++)
		{
			registeredPredator.Remove(removeGameObject[i]);
		}

		//Delete the game object
		for (int i = 0; i < removeGameObject.Count; i++)
		{
			//Remove unused fishes
			predatorPool.ReturnGameObject(removeGameObject[i].keyword, removeGameObject[i].gameObject);
		}
	}

	void ClearFood(List<FoodData> listFood)
	{
		//Clear foods that's not in the data list
		List<FoodController> removedFood = new List<FoodController>();

		//Loop trough all the registered food. Add to removed list if it's not in the data list
		for (int i = 0; i < registeredFood.Count; i++)
		{
			string id = registeredFood[i].foodId;
			int idx = listFood.FindIndex(x=>x.foodID.Equals( id ));
			//When the registered food is nowhere to found in the list data, register it to be deleted
			if (idx < 0)
			{
				removedFood.Add(registeredFood[i]);
			}
		}

		//Remove all the food in the remove list
		for (int i = 0; i < removedFood.Count; i++)
		{
			registeredFood.Remove(removedFood[i]);
		}

		//Delete the game object
		for (int i = 0; i < removedFood.Count; i++)
		{
			//removedFood[i].RemoveFood();
			//Return the game object to the pool
			foodPool.ReturnGameObject(removedFood[i].keyword, removedFood[i].gameObject);
		}
	}

	void ClearMap(List<MapData> listMap)
	{
		//Clear map feature that's not in the data list
		List<MapFeatureController> removed = new List<MapFeatureController>();

		//Loop trough all the displayed map. Add to removed list if it's not in the data list
		for (int i = 0; i < displayedMapFeatures.Count; i++)
		{
			string id = displayedMapFeatures[i].mapFeatureId;

			int idx = listMap.FindIndex(x=>x.mapFeatureId.Equals( id ));
			//When the displayed map is nowhere to found in the list data, register it to be removed
			if (idx < 0)
			{
				removed.Add(displayedMapFeatures[i]);
			}
		}

		//Remove all the displayed map in the remove list
		for (int i = 0; i < removed.Count; i++)
		{
			displayedMapFeatures.Remove(removed[i]);
		}

		//Delete the game object
		for (int i = 0; i < removed.Count; i++)
		{
			removed[i].RemoveMapFeature();
		}
	}

	private FishParameterData fishParamData;
	private bool CheckDeathFish(FishData enemyFish, FishData playerFish)
	{
		fishParamData = DefaultParameterManager.Instance.GetFishParameterData (playerFish.fishSkin,playerFish.level);
		float xMax = fishParamData.viewWidth /2;
		float yMax = fishParamData.viewHeight /2;

		float xDiff = Mathf.Abs(playerFish.xPosition - enemyFish.xPosition);
		float yDiff = Mathf.Abs(playerFish.yPosition - enemyFish.yPosition);

		if (xDiff < xMax && yDiff < yMax)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	#endregion
}
