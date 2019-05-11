using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProjectMiniJSON;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour 
{
	public LoadingUISystem loadingUI;
	public bool isLoadData = true;

	void Start()
	{
		Debug.Log ("start called");
		//Flow Check Version -> Register -> Login -> To Main Menu
		APIAppCheckVersion ();
		loadingUI.UpdateLoadingPercentage(1.0f);
	}

	void APIAppCheckVersion()
	{
		string apiName = "app_check_version/" + GameData.appVersion;
		loadingUI.UpdateTextLoadingNotification(LanguageManager.Instance.GetMessage("LOA0003"));
		BackEndConnect.Instance.SendRequestToServer (CBAppCheckVersion, apiName);
	}
	void CBAppCheckVersion(APIResponse response)
	{
		if (!response.isError)
		{
			int appStatus = BackEndConnect.Instance.ParseInt(response, APITag.appStatus);
			if (appStatus == 1)
			{
				Debug.Log("Allowed to play");
				//Get default parameters
				APIDefaultParameter();

				// if first time player open game == false
//				if (!EssentialData.Instance.LoadStatusFirstOpen ())
//				{
//					// set , first time open TRUE
//					EssentialData.Instance.SaveStatusFirstOpen (true);
//					EssentialData.Instance.SaveStatusAlreadyRating (false);
//					EssentialData.Instance.SaveStatusDeclineRating (false);
//				}

			}
			else if (appStatus == 2)
			{
				Debug.Log("Not allowed to play");
				//Show Notification to DOWNLOAD NEW VERSION HERE

				string messageUpdate = "";

				#if UNITY_ANDROID
				messageUpdate = LanguageManager.Instance.GetMessage("LOA0004");
				#elif UNITY_IPHONE
				messageUpdate = LanguageManager.Instance.GetMessage("LOA0005");
				#else
				messageUpdate = LanguageManager.Instance.GetMessage("LOA0004") + " Unity Editor";
				#endif

				UIPopupAdditionalUtility.Instance.popNoticeScript.InitPopSuddenNotice(messageUpdate,SendToStore);
			}

			loadingUI.UpdateTextVersion (GameData.appVersion);
		}
		else
		{
			APIAppCheckVersion();
		}
	}

	void SendToStore ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
		Invoke ("GoToStore", 0.25f);
	}

	void GoToStore ()
	{
		#if UNITY_ANDROID
		EssentialData.Instance.GoToLink (EssentialData.LINK_STORE_ANDROID);
		#elif UNITY_IPHONE
		EssentialData.Instance.GoToLink (EssentialData.LINK_STORE_IOS);
		#endif
	}

	void APIDefaultParameter()
	{
		loadingUI.UpdateLoadingPercentage(10.0f);
		string apiName = "default_parameter";
		loadingUI.UpdateTextLoadingNotification(LanguageManager.Instance.GetMessage("LOA0006"));
		BackEndConnect.Instance.SendRequestToServer (CBDefaultParameter, apiName);
	}

	void CBDefaultParameter(APIResponse response)
	{
		if (!response.isError)
		{
			Debug.Log("Default Parameter " + response.rawData);
			DefaultParameterManager.Instance.SaveDefaultParameterJSON(response.rawData);
			APIShopData();
		}
	}
		
	void APIShopData() 
	{
		loadingUI.UpdateLoadingPercentage(20.0f);
		string apiName = "get_shop_data";
		//loadingUI.UpdateTextLoadingNotification(LanguageManager.Instance.GetMessage("MSG0008"));
		BackEndConnect.Instance.SendRequestToServer (CBShopData, apiName);
	}

	void CBShopData(APIResponse response)
	{
		if (!response.isError)
		{
			Debug.Log("Shop Data " + response.rawData);
			ShopDataManager.Instance.SaveShopDataJSON(response.rawData);
			APIAchievementData();
		}
	}

	void APIAchievementData()
	{
		loadingUI.UpdateLoadingPercentage(30.0f);
		string apiName = "get_achievement_data";
		//loadingUI.UpdateTextLoadingNotification(LanguageManager.Instance.GetMessage("MSG0008"));
		BackEndConnect.Instance.SendRequestToServer (CBAchievementpData, apiName);
	}

	void CBAchievementpData(APIResponse response)
	{
		if (!response.isError)
		{
			Debug.Log("Achievement Data " + response.rawData);
			AchievementDataManager.Instance.SaveAchievementDataJSON(response.rawData);
			APILeaderboardData();
		}
	}

	void APILeaderboardData()
	{
		loadingUI.UpdateLoadingPercentage(35.0f);
		string apiName = "get_leaderboard";
		//loadingUI.UpdateTextLoadingNotification(LanguageManager.Instance.GetMessage("MSG0008"));
		BackEndConnect.Instance.SendRequestToServer (CBLeaderboardData, apiName);
	}

	void CBLeaderboardData(APIResponse response)
	{
		if (!response.isError)
		{
			Debug.Log("Leaderboard Data " + response.rawData);
			LeaderboardDataManager.Instance.SaveLeaderboardDataJSON(response.rawData);
	//		APINewsData();
				//APILogin();
				StartCoroutine(LoadGameAssets());
		}
	}

	void APINewsData() 
	{
		loadingUI.UpdateLoadingPercentage(40.0f);
		string apiName = "get_all_news";
		BackEndConnect.Instance.SendRequestToServer (CBNewsData, apiName);
	}

	void CBNewsData(APIResponse response)
	{
		if (!response.isError)
		{
			Debug.Log("news Data " + response.rawData);
			NewsDataManager.Instance.SaveNewsDataJSON(response.rawData);
			APIRegister();
		}
	}

	void APIRegister()
	{
		loadingUI.UpdateLoadingPercentage(45.0f);
		string savedUserId = EssentialData.Instance.LoadUserId();
		if (!string.IsNullOrEmpty(savedUserId) && isLoadData)
		{
			EssentialData.Instance.PlayerData.userId = savedUserId;
			APILogin();
		}
		else
		{
			loadingUI.UpdateTextLoadingNotification(LanguageManager.Instance.GetMessage("LOA0007"));
			string apiName = "register";
			string[] fieldName = { APITag.platformId };
			string[] input = { "2" };//TODO Input real platform here
			int totalInput = 1;
			BackEndConnect.Instance.SendRequestToServer(CBRegister, apiName, fieldName, input, totalInput);
		}
	}

	void CBRegister(APIResponse response)
	{
		EssentialData.Instance.PlayerData.userId = BackEndConnect.Instance.ParseString (response, APITag.userId);
		Debug.Log ("User ID = " + EssentialData.Instance.PlayerData.userId);
		EssentialData.Instance.SaveUserId();
		APILogin();
	}

	void APILogin()
	{
		loadingUI.UpdateLoadingPercentage(50.0f);
		loadingUI.UpdateTextLoadingNotification(LanguageManager.Instance.GetMessage("LOA0008"));
		string apiName = "login";
		string[] fieldName = {APITag.userId};
		string[] input = {EssentialData.Instance.PlayerData.userId};
		int totalInput = 1;
		BackEndConnect.Instance.SendRequestToServer (CBLogin,apiName,fieldName,input,totalInput);
	}

	void CBLogin(APIResponse response)
	{
		Debug.Log(response.rawData);
		if (!response.isError)
		{
			PlayerData player = new PlayerData(response.data);
			EssentialData.Instance.PlayerData = player;
			StartCoroutine(LoadGameAssets());
		}
		else
		{
			EssentialData.Instance.PlayerData.userId = "";
			EssentialData.Instance.SaveUserId();
			APIRegister();
		}
	}
		
	IEnumerator LoadGameAssets()
	{
		loadingUI.UpdateTextLoadingNotification(LanguageManager.Instance.GetMessage("LOA0012"));
		loadingUI.UpdateLoadingPercentage(60.0f);
		yield return new WaitForSeconds(0.1f);
		FishObjectPooling.Instance.Init();
		loadingUI.UpdateLoadingPercentage(65.0f);
		yield return new WaitForSeconds(0.1f);
		FoodObjectPooling.Instance.Init();
		loadingUI.UpdateLoadingPercentage(70.0f);
		yield return new WaitForSeconds(0.1f);
		MapFeatureObjectPooling.Instance.Init();
		loadingUI.UpdateLoadingPercentage(90.0f);
		yield return new WaitForSeconds(0.1f);
		PredatorObjectPooling.Instance.Init();
		loadingUI.UpdateLoadingPercentage(93.0f);
		yield return new WaitForSeconds(0.1f);
		loadingUI.UpdateLoadingPercentage(100.0f);
		loadingUI.UpdateTextLoadingNotification(LanguageManager.Instance.GetMessage("LOA0013")); //Please Wait
		SceneManager.LoadScene("MainMenu");
	}

}
