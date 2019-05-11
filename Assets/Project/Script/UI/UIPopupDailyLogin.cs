using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIPopupDailyLogin : MonoBehaviour 
{
	public UIPreviewReward layerPreview;
	public GameObject popupDailyLogin;
	public GameObject titleDailyLogin;
	public GameObject titleDailyEvent;

	public GameObject prefabDailyLogin;
	public Transform[] parentDailyLogin;

//	public Button btnDoneDaily;
	public bool isShowingReward = false;

	private bool isEventExist = false;
	private int claimedDailyDayDefault = -1;
	private int claimedDailyDayEvent = -1;

	private int count;
	private List <DailyRewardData> listDaily = new List<DailyRewardData>();
	private List <GameObject> listCreated = new List<GameObject>();


	#region INIT
	public void InitGetDailyLogin ()
	{
		APIDailyClaimData ();
		layerPreview.popupDailyPreview.SetActive (false);
	}

	// normal
	public void InitPopupDailyNormal ()
	{
		SetDefault ();
		ShowPopupDailyLogin ();
		GenerateDailyLoginPrefab ();
		// AFTER ANIMATION CLOSE POP, show pop up & animate THE DAY STAMP.

//		btnDoneDaily.onClick.RemoveAllListeners ();
//		btnDoneDaily.onClick.AddListener (SoundUtility.Instance.PlaySFX (SFXData.SfxButton));
//		btnDoneDaily.onClick.AddListener (HidePopupDailyLogin);
	}

	//event
	public void InitPopupDailyEvent ()
	{
		isEventExist = false;

		SetDefault ();
		// SHOW ANIMATION
		// AFTER ANIMATION CLOSE POP, show pop up & animate THE DAY STAMP.

		ShowPopupDailyEvent ();
		GenerateDailyEventPrefab ();
	}

	#endregion

	#region SPAWN BUFF
	void ClearList (List <GameObject> listCreated)
	{		
		count = listCreated.Count;

		for (int i = 0; i < count; i++)
		{
			Destroy (listCreated[i]);
		}

		listCreated.Clear ();
	}

	void GenerateDailyLoginPrefab ()
	{
		ClearList (listCreated);

		listDaily = DailyLoginDataManager.Instance.GetDailyDataNormal ();

		for (int i = 0; i < listDaily.Count; i++)
		{
			GameObject go = Instantiate(prefabDailyLogin) as GameObject;

			DailyLoginItem dailyPrefab = go.GetComponent<DailyLoginItem>();

			go.transform.SetParent (parentDailyLogin[i]);
			SetPrefabTransform (go);

			dailyPrefab.SetItemData (listDaily[i]);

			listCreated.Add (go);
		}

		SetDailyStamp (claimedDailyDayDefault);
		SetPreviewLayerAnim (claimedDailyDayDefault);
	}

	void GenerateDailyEventPrefab ()
	{
		ClearList (listCreated);

		listDaily = DailyLoginDataManager.Instance.GetDailyDataEvent ();

		for (int i = 0; i < listDaily.Count; i++)
		{
			GameObject go = Instantiate(prefabDailyLogin) as GameObject;

			DailyLoginItem dailyPrefab = go.GetComponent<DailyLoginItem>();

			go.transform.SetParent (parentDailyLogin[i]);
			SetPrefabTransform (go);

			dailyPrefab.SetItemData (listDaily[i]);

			listCreated.Add (go);
		}

		SetDailyStamp (claimedDailyDayEvent);
		SetPreviewLayerAnim (claimedDailyDayEvent);
	}

	void SetPrefabTransform (GameObject go)
	{
		go.transform.localPosition = Vector3.zero;
		go.transform.localEulerAngles = Vector3.zero;
		go.transform.localScale = Vector3.one;
	}
	#endregion

	#region BUTTON
	public void BtnDoneDaily ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		Invoke ("HidePopupDailyLogin", 0.25f);
	}

	public void BtnCloseDailyLoginPreview ()
	{
		layerPreview.HidePopupDailyPreview ();

		if (!isEventExist)
		{
			SetAnimationDailyStamp (claimedDailyDayDefault);
		}
		else
		{
			SetAnimationDailyStamp (claimedDailyDayEvent);
		}
//
		isShowingReward = false;
	}

	#endregion

	#region UI
	void SetDefault ()
	{
//		btnDoneDaily.onClick.RemoveAllListeners ();
		titleDailyLogin.SetActive (false);
		titleDailyEvent.SetActive (false);
	}

	public void ShowPopupDailyLogin ()
	{
		titleDailyLogin.SetActive (true);
		titleDailyEvent.SetActive (false);

		popupDailyLogin.SetActive (true);
		EssentialData.popupState = EnumData.PopupState.on;
	}

	public void ShowPopupDailyEvent ()
	{
		titleDailyEvent.SetActive (true);
		titleDailyLogin.SetActive (false);

		popupDailyLogin.SetActive (true);
		EssentialData.popupState = EnumData.PopupState.on;
	}

	public void HidePopupDailyLogin ()
	{
		popupDailyLogin.SetActive (false);
		EssentialData.popupState = EnumData.PopupState.off;

		if (isEventExist)
		{
			Invoke ("InitPopupDailyEvent", 0.25f);
		}
		else
		{
			if (EssentialData.Instance.LoadStatusAllowShowNews())
			{
				if (!EssentialData.isAlreadyShowNews)
				{
					MainMenuController.Instance.newsScript.InitPopupNews ();
				}
			}
		}

		APIGetUserData ();

		//UpperMenuGold.Instance.SetGold (EssentialData.Instance.PlayerData.gold);
	}

	#endregion

	#region UI Animation
	public void SetDailyStamp (int day)
	{
		int index = day - 1;

		for (int i = 0; i < index; i++)
		{
			listCreated[i].GetComponent<DailyLoginItem>().SetStamping();
		}
	}

	public void SetAnimationDailyStamp (int day)
	{
		int index = day - 1;
		listCreated[index].GetComponent<DailyLoginItem>().SetAnimationStamping();
		Invoke ("PlaySoundStamp", 0.75f);
	}

	public void SetPreviewLayerAnim (int day)
	{
		Invoke ("PlaySoundGold", 0.25f);
		Invoke ("PlaySoundGold", 0.26f);

		int index = day - 1;
		DailyRewardData spesificData = new DailyRewardData();
		spesificData = listCreated[index].GetComponent<DailyLoginItem>().GetDailyRewardData();

		layerPreview.SetItemData(spesificData);
		layerPreview.ShowPopupDailyPreview();
		isShowingReward = true;
	}

	void PlaySoundGold ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBuffGold);
	}

	void PlaySoundStamp ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxStamp);
	}

	#endregion

	#region API
	public void APIDailyLoginData() 
	{
		string apiName = "get_daily_event";
		BackEndConnect.Instance.SendRequestToServer (CBDailyLoginData, apiName);
	}

	void CBDailyLoginData(APIResponse response)
	{
		Debug.Log("Call back - CBDailyLoginData is error, called until 4 times.");

		if (!response.isError)
		{
			Debug.Log("daily Data " + response.rawData);
			DailyLoginDataManager.Instance.SaveDailyLoginDataJSON(response.rawData);

			Invoke ("InitPopupDailyNormal", 0.8f);
		}
	}

	// claim
	public void APIDailyClaimData() 
	{
		string apiName = "claim_daily_login";
		string[] fieldName = {APITag.userId,APITag.secretKey};
		string[] input = {EssentialData.Instance.PlayerData.userId, EssentialData.Instance.PlayerData.secretKey};
		int totalInput = 2;

		BackEndConnect.Instance.SendRequestToServer (CBDailyClaimData,apiName,fieldName,input,totalInput);
	}

	private const string TAG_DAILY = "daily_data";
	private const string TAG_DAILY_ID = "daily_login_id";
	private const string TAG_DAILY_DAY = "days";

	void CBDailyClaimData(APIResponse response)
	{
		Debug.Log("Call back - CBDailyClaimData is error, called until 4 times.");
		if (!response.isError)
		{
			Debug.Log("claim daily Data " + response.rawData);

			var message = response.data ["message"].ToString();

			if (message.Equals ("SUCCESS"))
			{
				var dataList = response.data[TAG_DAILY] as List<object>;

				for (int i = 0; i < dataList.Count; i++)
				{	
					var rawData = dataList[i] as Dictionary<string,object>;

					string dailyID = JsonUtility.GetString(rawData, TAG_DAILY_ID);

					Debug.Log("dailyID " + dailyID );

					if (dailyID.Equals("default"))
					{
						claimedDailyDayDefault = JsonUtility.GetInt(rawData, TAG_DAILY_DAY);
//						Debug.Log ("claimedDailyDayDefault " + claimedDailyDayDefault);
					}
					else
					{
						claimedDailyDayEvent = JsonUtility.GetInt(rawData, TAG_DAILY_DAY);
//						Debug.Log ("claimedDailyDayEvent " + claimedDailyDayEvent);

						if (claimedDailyDayEvent > 0)
						{
							isEventExist = true;
						}
					}
				}
					
				APIDailyLoginData(); // PULL THE DAILY LOGIN DATA.
			}
			else if(message.Equals("CLAIMED"))
			{
				//UpperMenuGold.Instance.SetGold (EssentialData.Instance.PlayerData.gold);
				isEventExist = false;
				HidePopupDailyLogin ();
				APIGetUserData ();
			}
		}
	}

	void APIGetUserData()
	{
//		Debug.Log ("get user data called in main menu");
		string apiName = "get_user_data";
		string[] fieldName = {APITag.userId,APITag.secretKey};
		string[] input = {EssentialData.Instance.PlayerData.userId, EssentialData.Instance.PlayerData.secretKey};
		int totalInput = 2;
		BackEndConnect.Instance.SendRequestToServer (CBGetUserData,apiName,fieldName,input,totalInput);
	}

	void CBGetUserData(APIResponse response)
	{
		Debug.Log("Get User Data " + response.rawData);
		if (!response.isError)
		{
			PlayerData player = new PlayerData(response.data);
			EssentialData.Instance.PlayerData = player;

			UpperMenuGold.Instance.SetGold (EssentialData.Instance.PlayerData.gold);
		}
		else
		{
			Debug.Log("Fail get user data " + response.errorMessage);
		}
	}
	#endregion
}
