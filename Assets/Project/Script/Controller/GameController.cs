using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using ProjectMiniJSON;

public class GameController : MonoBehaviour 
{
	private static GameController instance;

	public static GameController Instance
	{
		get
		{
			return instance;
		}
	}

	#region Variables
	[System.NonSerialized] public bool connected;
	[System.NonSerialized] public bool isAlive;
	[System.NonSerialized] public string playerID;
	[System.NonSerialized] public static bool animationReady;
	[System.NonSerialized] public int controllerType = 1;
	[System.NonSerialized] public Vector2 localPos;
	public GameObject playerSphere;

	[HideInInspector]
	public FishData playerFishData;


	List<FishData> listOpponentData;
	List<FoodData> listFoodData;
	List<PredatorData> listPredatorData;
	List<MapData> listMapData;

	//Scale between backend and client UI position, width, and height
	float scaleFactor = 100.0f;

	private bool isFirstUpdate = false;
	#endregion

	private Dictionary<string,System.Action<Dictionary<string,object>> > eventDictionary;

	void Awake()
	{
		instance = this;
		controllerType = EssentialData.Instance.LoadControllerType ();
	}

	void Start()
	{
		LoadData ();
		isAlive = true;
		playerID = EssentialData.Instance.PlayerData.userId;
		//Debug.Log ("PID = "+playerID);
		connected = false;
		listOpponentData = new List<FishData>();
		listFoodData = new List<FoodData> ();
		listMapData = new List<MapData> ();
		listPredatorData = new List<PredatorData>();

		//Initialize the default parameter
		DefaultParameterManager.Instance.LoadDefaultParameterJSON();

		Application.runInBackground = true;

		eventDictionary = new Dictionary<string, System.Action<Dictionary<string, object>>>();

		eventDictionary.Add(APITag.socketTagUpdate, UpdateGame);
		eventDictionary.Add(APITag.socketTagDead, PlayerDead);
		eventDictionary.Add(APITag.socketTagPredatorDead, PredatorDead);

		StartJoinNewGame();
	}

	public void ReJoinGame()
	{
		DisplayGameController.Instance.ReturnFishOverlay ();
		DisplayGameController.Instance.ClearAllPooledObject();
		UIGameController.Instance.CloseAllPop();
		StartJoinNewGame();
		UIGameController.Instance.CanvasOverlay.SetActive (true);
	}

	void StartJoinNewGame()
	{
		animationReady = false;
		connected = false;
		isAlive = true;
		UIGameController.Instance.ShowConnectingMessage();
		StopCoroutine("UpdateLoop");
		StartCoroutine(BackEndConnect.Instance.EstablishConnection ());
		StartCoroutine(FirstConnection());
		//DisplayGameController.Instance.movementControl.SetPointerZero ();

		#if (UNITY_ANDROID || UNITY_IOS)
		if (!EssentialData.Instance.PlayerData.noAds) {
			switch (Application.platform) {
			case RuntimePlatform.IPhonePlayer:
				AdsUtility.instance.LoadInterstitialAds ();
				break;
			case RuntimePlatform.Android:
				AdsUtility.instance.LoadInterstitialAds ();
				break;
			default:
				break;
			}
		}
		#endif
	}

	void JoinGame(string reply)
	{
		//Debug.Log ("Reply = "+ reply);
		var jsonData = Json.Deserialize (reply) as Dictionary<string,object>;
		string tag = jsonData[APITag.socketTag].ToString();
		string status = jsonData[APITag.socketStatus].ToString();

		var rawData = jsonData [APITag.socketData] as Dictionary<string,object>;

		//User Data Compile
		string userData = rawData[APITag.socketUserData].ToString();
		FishData playerFish = CompileUserData(userData);
		playerFishData = playerFish;

		isFirstUpdate = false;

		Debug.Log ("Joined game " + playerFish.playerName);

		UIGameController.Instance.isPlayAgainAlreadyPressed = false;
		UIGameController.Instance.CanvasOverlay.SetActive (true);

		TileGenerator.Instance.LoadAndShowMap();
	}

	void PlayerDead(Dictionary<string,object> jsonData)
	{
		StartCoroutine(DeathWait (jsonData));
	}

	IEnumerator DeathWait(Dictionary<string,object> jsonData)
	{
		UIGameController.Instance.CanvasOverlay.SetActive (false);

		DisplayGameController.Instance.PlayPlayerFishDeadAnimation ();

		SoundUtility.Instance.PlaySFX(SFXData.SfxDiePlayer);
		yield return new WaitForSeconds (1f);

		DisplayGameController.Instance.ResetFishBuff ();

		isAlive = false;
		CancelInvoke("MovementLoop");
		StopCoroutine("UpdateLoop");

		BackEndConnect.Instance.backendWebSocket.Close();

		string status = jsonData[APITag.socketStatus].ToString();

		var rawData = jsonData [APITag.socketData] as Dictionary<string,object>;
		long playTime = JsonUtility.GetLong(rawData, APITag.socketDeadPlayTime);
		int playerEaten = JsonUtility.GetInt(rawData, APITag.socketDeadPlayerKilled);
		int gold = JsonUtility.GetInt(rawData, APITag.socketDeadGold);
		int level = JsonUtility.GetInt(rawData, APITag.socketDeadLevel);

		long totalSecond = playTime / 1000;
		long totalMinute = totalSecond / 60;
		long playHour = totalMinute / 60;

		long playSecond = totalSecond % 60;
		long playMinute = totalMinute % 60;
		string displayedPlayTime = playHour.ToString().PadLeft(2,'0') + ":" + playMinute.ToString().PadLeft(2,'0') + ":" + playSecond.ToString().PadLeft(2,'0');

		UIGameController.Instance.SetResultName(EssentialData.Instance.PlayerDisplayName);
		UIGameController.Instance.SetResultEat(playerEaten);
		UIGameController.Instance.SetResultGold(gold);
		UIGameController.Instance.SetResultLevel(level);
		UIGameController.Instance.SetResultTime(displayedPlayTime);

		UIGameController.Instance.OpenPopResult();

		#if (UNITY_ANDROID || UNITY_IOS)
		if (!EssentialData.Instance.PlayerData.noAds) {
			switch (Application.platform) {
			case RuntimePlatform.IPhonePlayer:
				AdsUtility.instance.ShowInterstitialAds ();
				break;
			case RuntimePlatform.Android:
				AdsUtility.instance.ShowInterstitialAds ();
				break;
			default:
				break;
			}
		}
		#endif
	}

	private bool isPressedOnce = false;

	public void BackToMenu()
	{
		if (!isPressedOnce)
		{
			FishObjectPooling.Instance.ResetAllPool();
			FoodObjectPooling.Instance.ResetAllPool();
			PredatorObjectPooling.Instance.ResetAllPool();
			MapFeatureObjectPooling.Instance.ResetAllPool();

			Invoke ("GoToMenu", 0.5f);

			isPressedOnce = true;
		}
	}

	void GoToMenu ()
	{
		UIPopupAdditionalUtility.Instance.ShowLoadingPopup ();

		EssentialData.QuitGameLoop++;

		SceneManager.LoadScene ("MainMenu");

		isPressedOnce = false;
	}

	void DisconnectedFromSocket()
	{
		UIPopupAdditionalUtility.Instance.InitPopDisconnected();
		BackToMenu();
	}

	void PredatorDead(Dictionary<string,object> jsonData)
	{
		//string status = jsonData[APITag.socketStatus].ToString();
//		Debug.Log(Json.Serialize(jsonData));
		var rawPredatorDead = jsonData [APITag.socketData] as string ;
	
		//get data which predator is dead
		string[] dataArray = rawPredatorDead.Split(',');

		//compile data
		string predatorId = dataArray[0];
		float posX = float.Parse(dataArray[1]);
		float posY = float.Parse(dataArray[2]);

		DisplayGameController.Instance.SetPredatorToDead (predatorId);
	}

	void PlaceDeadFish(string data)
	{
		//"$PLAYER_ID,#COIN_TYPE,#STATUS"
		//data here
		string[] dataArray = data.Split(',');
		string playerID = dataArray[0];
		string coinType = dataArray [1];
		string status = dataArray [2];

	}

	void UpdateGame(Dictionary<string,object> jsonData)
	{
		if (!isFirstUpdate)
		{
			UIGameController.Instance.HideMessage();
			isFirstUpdate = true;
			animationReady = true;
		}
		//Debug.Log ("Reply = " + reply);
		if (!isAlive)
			return;
		
		string status = jsonData[APITag.socketStatus].ToString();

		var rawData = jsonData [APITag.socketData] as Dictionary<string,object>;
		var rawOpponents = rawData [APITag.socketOpponents] as List<object>;
		var rawPredators = rawData[APITag.socketPredators] as List<object>;

		//User Data Compile
		string userData = rawData[APITag.socketUserData].ToString();
		FishData playerFish = CompileUserData(userData);

		//coin managing system
		try 
		{
			var coins = rawData[APITag.socketCoin] as List<object>;

			for(int i =0; i< coins.Count; i++)
			{
				CompileCoins(coins[i].ToString());
			}

		}
		catch 
		{
			//Debug.Log ("No coin given");
			//no coins given
		}


		//place all opponents
		listOpponentData.Clear();
		for(int i = 0; i < rawOpponents.Count; i++)
		{
			PlaceOpponent (rawOpponents[i].ToString());
		}

		//Place all predator
		listPredatorData.Clear();
		for (int i = 0; i < rawPredators.Count; i++)
		{
			PlacePredator(rawPredators[i].ToString());
		}

//		Debug.Log ("Predator raw data = " + Json.Serialize(rawPredators));

		//dont show warning if there is no predator
		if (rawPredators.Count < 1) 
		{
			WarningHandler.Instance.isSharkAround = false;
		}

		//process data given per socket send
		DisplayGameController.Instance.UpdateFish(listOpponentData,playerFish);
		DisplayGameController.Instance.UpdatePredators(listPredatorData);
		DisplayGameController.Instance.UpdatePlayer(playerFish,listFoodData);
		DisplayGameController.Instance.UpdateBoostBar(playerFish.boostValue);

		//Update Food if there is data given
		if (rawData.ContainsKey(APITag.socketFoods))
		{
			var rawFoods = rawData [APITag.socketFoods] as List<object>;
			//place all foods
			listFoodData.Clear();
			for (int i = 0; i < rawFoods.Count; i++)
			{
				PlaceFood (rawFoods [i].ToString ());
			}
			DisplayGameController.Instance.UpdateFoods(listFoodData);
		}
	}

	//before addtional functions

	#region Additional Functions

	void LoadData()
	{
		//TODO: Load Data here;
		//load the data given by api or in the player pref, save it here
	}

	IEnumerator FirstConnection()
	{
		if (BackEndConnect.Instance.socketReady) {
			string joinData = EssentialData.Instance.PlayerData.userId + "," + EssentialData.Instance.PlayerDisplayName + "," + EssentialData.Instance.PlayerData.secretKey + "," + controllerType;
			BackEndConnect.Instance.SendSocket (APITag.socketTagJoin, joinData);
			while (!connected) {
				string reply = BackEndConnect.Instance.backendWebSocket.RecvString ();

				if (reply != null) {
					connected = true;
					UIGameController.Instance.ShowJoinGameMessage();
					JoinGame (reply);
				}
				if (BackEndConnect.Instance.backendWebSocket.Error != null) {
					//Debug.Log ("Error: " + BackEndConnect.Instance.backendWebSocket.Error);
					DisconnectedFromSocket();
					break;
				}
				yield return new WaitForSeconds(0.01f);
			}
		} else {
			yield return new WaitForSeconds (0.1f);
			StartCoroutine (FirstConnection ());
		}
	}

	public void StartListeningForSocketUpdates()
	{
		//Change player fish first
		DisplayGameController.Instance.LoadPlayerFish(playerFishData.fishSkin);
		//Close All Pop ups
		UIGameController.Instance.CloseAllPop();

		//Clear Update loop
		BackEndConnect.Instance.ClearBuffer();
		//Start socket receiver from backend
		StartCoroutine("UpdateLoop");
		//Start Sending Movement to backend
		InvokeRepeating("MovementLoop", 0.05f, 0.05f);
		//set several things to be zero
		DisplayGameController.Instance.movementControl.SetPointerZero ();
	}

	IEnumerator UpdateLoop()
	{
		bool establishConnection = true;
		while (establishConnection) 
		{
			string reply = BackEndConnect.Instance.ReceiveMessage();
			if (reply != null) {
				connected = true;

				var jsonData = Json.Deserialize (reply) as Dictionary<string,object>;
				string tag = jsonData[APITag.socketTag].ToString();

				if (eventDictionary.ContainsKey(tag))
				{
					eventDictionary[tag](jsonData);
				}
			}
			if (BackEndConnect.Instance.backendWebSocket.Error != null) {
				//Debug.LogError ("Error: " + BackEndConnect.Instance.backendWebSocket.Error);
				DisconnectedFromSocket();
				break;
			}

			yield return 0;
		}
		BackEndConnect.Instance.backendWebSocket.Close ();
	}

	void MovementLoop()
	{
		string data;
		if (controllerType == 1)
			data = CompileStringMovement (playerID,(playerSphere.transform.localPosition.x /150).ToString(),(playerSphere.transform.localPosition.y /150).ToString());
		else
			data = CompileStringMovement (playerID,UIGameController.Instance.GetWorldPoint().x.ToString(),UIGameController.Instance.GetWorldPoint().y.ToString());
		BackEndConnect.Instance.SendSocket (APITag.socketTagMovement, data);
		//Debug.Log (data);
	}
		
	void PlaceOpponent(string opponentData)
	{
		//Debug.Log ("Opponent = " + opponentData);
		string[] opponentDetails = opponentData.Split(';');
		string[] opponentArray = opponentDetails[0].Split (',');
		FishData fish = new FishData();
		fish.playerID = opponentArray [0];
		fish.playerName = opponentArray [1];
		fish.xPosition = float.Parse (opponentArray [2]) / scaleFactor;
		fish.yPosition = float.Parse (opponentArray [3]) / scaleFactor;
		fish.fishAngle = float.Parse (opponentArray [4]);
		fish.colorCode = int.Parse (opponentArray [5]);
		fish.fishSkin = opponentArray [6];
		fish.level = int.Parse (opponentArray [7]);
		//Read fish width and height from default parameter
		FishParameterData levelData = DefaultParameterManager.Instance.GetFishParameterData(fish.fishSkin, fish.level);
		if (levelData != null)
		{
			fish.width = levelData.totalSize;
			fish.height = levelData.totalSize;
		}

		fish.buffType = int.Parse(opponentArray[8]);
		fish.buffTime = int.Parse(opponentArray[9]);
		fish.isBoost = int.Parse(opponentArray[10]) == 1 ? true : false;

		if (opponentDetails.Length > 1)
		{
			for (int i = 1; i < opponentDetails.Length; i++)
			{
				FishBodyData bodyData = new FishBodyData();
				string[] rawBodyData = opponentDetails[i].Split(',');
				bodyData.xPosition = float.Parse(rawBodyData[0]) / scaleFactor;
				bodyData.yPosition = float.Parse(rawBodyData[1]) / scaleFactor;
				bodyData.width = float.Parse(rawBodyData[2]) / scaleFactor;
				bodyData.height = float.Parse(rawBodyData[3]) / scaleFactor;
				fish.bodies.Add(bodyData);
			}
		}

		if (playerID != opponentArray [0])
			listOpponentData.Add (fish);
	}

	void PlaceFood(string foodData)
	{
		//Debug.Log ("Food = " + foodData);
		string[] foodArray = foodData.Split (',');
		FoodData food = new FoodData ();

		food.foodID = foodArray [0];
		food.xPosition = float.Parse (foodArray [1]) / scaleFactor;
		food.yPosition = float.Parse (foodArray [2]) / scaleFactor;

		food.height = food.width = DefaultParameterManager.Instance.FoodSize;

		food.foodType = int.Parse (foodArray [3]);
		food.foodKeyword = foodArray[4].ToString();
		if (string.IsNullOrEmpty(food.foodKeyword))
		{
			food.foodKeyword = "food_pellet";
		}

		listFoodData.Add (food);
	}

	void PlacePredator(string predatorData)
	{
		//Debug.Log (predatorData);
		PredatorData predator = new PredatorData();
		string[] predators = predatorData.Split(',');
		predator.id = predators[0].ToString();
		predator.xPosition = float.Parse(predators[1]) / scaleFactor;
		predator.yPosition = float.Parse(predators[2]) / scaleFactor;
		predator.angle = float.Parse(predators[3]);
		predator.assetKeyword = predators[4];
		predator.targetUserId = predators[5];
		predator.height = predator.width = DefaultParameterManager.Instance.GetPredatorSize(predator.assetKeyword);

		listPredatorData.Add(predator);

		if (predator.assetKeyword == "shark")
		{
			WarningHandler.Instance.SetSharkPosition (predator.xPosition, predator.yPosition);
			WarningHandler.Instance.isSharkAround = true;
		}
		else
		{
			WarningHandler.Instance.isSharkAround = false;
		}
	}

	void PlaceMapFeature(string mapData)
	{
		MapData map = new MapData();
		string[] mapFeature = mapData.Split(',');
		map.mapFeatureId = mapFeature[0].ToString();
		map.mapXPos = float.Parse(mapFeature [1]) / scaleFactor;
		map.mapYPos = float.Parse(mapFeature [2]) / scaleFactor;
		map.width = float.Parse(mapFeature[3]) / scaleFactor;
		map.height = float.Parse(mapFeature[4]) / scaleFactor;
		map.mapFeatureType = int.Parse(mapFeature [5]);
		map.mapFeatureKeyword = mapFeature [6];

		listMapData.Add (map);
	}

	void CompileCoins(string coins)
	{
		string[] dataCoin = coins.Split (',');

		string playerID = dataCoin [0];
		float posX = float.Parse (dataCoin [1]);
		float posY = float.Parse (dataCoin [2]);
		bool isPredator = int.Parse (dataCoin [3]) == 1 ? true : false;

		//call coin animation here
		if (posX != null && posY != null)
		{
			try
			{
				StartCoroutine(CoinAnimation (posX,posY, isPredator));
			}
			catch
			{
				
			}
		}
	}

	IEnumerator CoinAnimation(float posX, float posY, bool isPredator)
	{
		int scale = 1;
		if (isPredator) 
		{
			yield return new WaitForSeconds (1);
			scale = 2; 
		}
		GameObject coinGainAnim = Instantiate ((GameObject)Resources.Load("Prefab/Coin Gain Effect", typeof(GameObject)));
		coinGainAnim.transform.position = new Vector2 (posX/scaleFactor,posY/scaleFactor);
		coinGainAnim.transform.localScale = coinGainAnim.transform.localScale * scale;

		yield return new WaitForSeconds (5f);
		Destroy (coinGainAnim);
	}

	FishData CompileUserData(string userData)
	{	
		FishData playerFish = new FishData ();

		string[] playerData = userData.Split(';');

		string[] playerArray = playerData[0].Split(',');

		playerFish.playerID = playerArray[0];
		playerFish.playerName = playerArray [1];
		playerFish.xPosition = float.Parse (playerArray [2]) / scaleFactor;
		playerFish.yPosition = float.Parse (playerArray [3]) / scaleFactor;
		playerFish.fishAngle = float.Parse (playerArray [4]);
		playerFish.colorCode = int.Parse (playerArray [5]);
		playerFish.fishSkin = playerArray [6];
		playerFish.level = int.Parse (playerArray [7]);

		FishParameterData levelData = DefaultParameterManager.Instance.GetFishParameterData(playerFish.fishSkin, playerFish.level);
		if (levelData != null)
		{
			playerFish.width = levelData.totalSize;
			playerFish.height = levelData.totalSize;
		}

		playerFish.experience = float.Parse (playerArray [8]);
		playerFish.buffType = int.Parse(playerArray[9]);
		playerFish.buffTime = float.Parse(playerArray[10]);
		playerFish.isInPredatorRange = int.Parse (playerArray [11]);// == 1 ? true : false;
		playerFish.isTargetedByPredator = int.Parse(playerArray[12]) == 1 ? true : false;
		playerFish.isBoost = int.Parse(playerArray[13]) == 1 ? true : false;
		playerFish.boostValue = float.Parse (playerArray [14]);

		//playerFish.isTargetedByPredator = true;
		//playerFish.isInPredatorRange = true;

		if (playerData.Length > 1)
		{
			for (int i = 1; i < playerData.Length; i++)
			{
				FishBodyData bodyData = new FishBodyData();
				string[] rawBodyData = playerData[i].Split(',');
				bodyData.xPosition = float.Parse(rawBodyData[0]) / scaleFactor;
				bodyData.yPosition = float.Parse(rawBodyData[1]) / scaleFactor;
				bodyData.width = float.Parse(rawBodyData[2]) / scaleFactor;
				bodyData.height = float.Parse(rawBodyData[3]) / scaleFactor;
				playerFish.bodies.Add(bodyData);
			}
		}

		localPos = new Vector2(playerFish.xPosition, playerFish.yPosition);

		if (playerID != playerArray [0])
			Debug.LogError ("ERROR - ID does not match");


		return playerFish;
	}



	public void ChangeControllerType()
	{
		if (controllerType == 1)
			EssentialData.Instance.SaveControllerType (2);
		else
			EssentialData.Instance.SaveControllerType (1);
	}

	string CompileStringMovement(string playerID, string directionX, string directionY)
	{
		return playerID + "," + directionX + "," + directionY;
	}

	const string glyphs= "abcdefghijklmnopqrstuvwxyz0123456789";
	string RandomizeString(int count = 16)
	{
		string myString = "";
		int charAmount = count;
		for(int i=0; i<charAmount; i++)
		{
			myString += glyphs[Random.Range(0, glyphs.Length)];
		}
		return myString;
	}

	#endregion
}
