using UnityEngine;
using System.Collections;
//using Facebook.Unity;
using System.Collections.Generic;
//using Facebook.MiniJSON;
using System;

namespace Utilities {

	public class SocialManager : MonoBehaviour {
		private static SocialManager instance;

		public static SocialManager Instance {
			get {
				if (instance == null) {
					GameObject go = new GameObject("Social Manager");
					instance = go.AddComponent<SocialManager> ();
				} 

				return instance;
			}
		}

		public string userId = "";
		private string accessKey = "";
		private long expiredTimeStamp = 0;
		private Uri shareLink = new Uri("http://floo.acelegends.com/");
		private Uri imageLink = new Uri("http://floo.acelegends.com/wp-content/uploads/2016/11/Feature-Graphic_1200x630.jpg");
		private string title = "Floo: Fish Aquatic Adventure";
		public bool IsLoggedIn = false;
		public bool IsFBInitialized = false;
		private Action delegateSpecific;

		#region INIT
		public void InitFB() 
		{
			/*IsFBInitialized = FB.IsInitialized;

			if (!FB.IsInitialized)
			{
				// Initialize the Facebook SDK
				FB.Init(this.OnInitComplete, this.OnHideUnity);
			}
			else
			{
				Debug.Log ("already Init fb");
				// Already initialized, signal an app activation App Event
				FB.ActivateApp();
				IsLoggedInFB ();
			}*/

		}

		private void OnInitComplete()
		{
			/*if (FB.IsInitialized) 
			{
				Debug.Log ("success FB");
				// Signal an app activation App Event
				//FB.ActivateApp();
				IsLoggedInFB ();
			}
			else 
			{
				Debug.Log("Failed to Initialize the Facebook SDK");
			}*/
		}

		private void OnHideUnity(bool isGameShown)
		{

		}

		#endregion

		#region LOGIN
		private List<string> perms = new List<string>(){"public_profile", "email", "user_friends"};

		public void LoginFB() 
		{
			UIPopupAdditionalUtility.Instance.ShowLoadingPopup ();
			//FB.LogInWithReadPermissions (perms, this.HandleLoginResult);
		}

		public void LoginFB(Action delegateLoginSuccess) 
		{
			delegateSpecific = delegateLoginSuccess;

			UIPopupAdditionalUtility.Instance.ShowLoadingPopup ();
			//FB.LogInWithReadPermissions (perms, this.HandleLoginResult);
		}

		#endregion

		#region LOGOUT - SHARE - ISLOGGINED
		public void LogoutFB() 
		{
			//FB.LogOut();

			//IsLoggedInFB ();
		}

		public void ShareFB (string description)
		{
			/*FB.ShareLink(
				shareLink,
				title,
				description,
				imageLink,
				callback: this.HandleShareResult);*/
		}

		/*public bool IsLoggedInFB ()
		{
			//IsLoggedIn = FB.IsLoggedIn;

			//return IsLoggedIn;
		}*/

		#endregion

		/*protected void HandleShareResult(IResult result)
		{
			if (result == null)
			{
				return;
			}

			// Some platforms return the empty string instead of null.
			if (!string.IsNullOrEmpty(result.Error))
			{
				Debug.Log ("share Error" + result.Error);
			}
			else if (result.Cancelled)
			{
				Debug.Log ("share canceled");
			}
			else if (!string.IsNullOrEmpty(result.RawResult))
			{
				Debug.Log ("share " + result.RawResult + IsLoggedIn +" "+ userId); 

				string apiName = "share_facebook";
				string[] field = { "user_id", "secret_key", "facebook_id"};
				string[] value = { EssentialData.Instance.PlayerData.userId, EssentialData.Instance.PlayerData.secretKey, userId};
				BackEndConnect.Instance.SendRequestToServer (CBShareFB, apiName, field, value, 3);
			}
			else
			{
				Debug.Log ("share sucess " + IsLoggedIn +" "+ userId);
			}

		}*/

		void CBShareFB(APIResponse response)
		{
			Debug.Log(response.rawData);
			if (!response.isError)
			{
				var data = response.data["user_data"] as Dictionary<string,object>;
				PlayerData player = new PlayerData(data);
				EssentialData.Instance.PlayerData = player;
			}
		}

		/*protected void HandleLoginResult(IResult result)
		{
			if (result == null)
			{
				Debug.Log ("login null");
				return;
			}

			// Some platforms return the empty string instead of null.
			if (!string.IsNullOrEmpty(result.Error))
			{
				Debug.Log ("login Error");
			}
		else if (result.Cancelled)
			{
				Debug.Log ("login canceled");
			}
			else if (!string.IsNullOrEmpty(result.RawResult))
			{
				var jsonData = Json.Deserialize (result.RawResult) as Dictionary<string, System.Object>;
				userId = jsonData ["user_id"].ToString ();
				accessKey = jsonData ["access_token"].ToString ();
				long.TryParse (jsonData ["expiration_timestamp"].ToString (), out expiredTimeStamp);

				if (delegateSpecific != null)
				{
					delegateSpecific ();
				}

				IsLoggedInFB ();

				RegisterFacebookAccount ();

				Debug.Log ("Login success " + IsLoggedIn +" "+ userId);
			}
			else
			{
				Debug.Log ("login Others");
				//empty
			}

			UIPopupAdditionalUtility.Instance.HideLoadingPopup ();
		}*/

		#region API REGISTER FACEBOOK ID
		public void RegisterFacebookAccount ()
		{
			/*if (IsLoggedInFB ())
			{
				APIRegisterFacebook (userId);
			}*/
		}

		void APIRegisterFacebook(string facebookId)
		{
			string apiName = "register_facebook";
			string[] fieldName = {APITag.userId, APITag.secretKey, "facebook_id"};
			string[] input = {EssentialData.Instance.PlayerData.userId, EssentialData.Instance.PlayerData.secretKey, facebookId};
			int totalInput = 3;
			BackEndConnect.Instance.SendRequestToServer (CBRegisterFacebook,apiName,fieldName,input,totalInput);
		}

		void CBRegisterFacebook(APIResponse response)
		{
			Debug.Log("Get User Data " + response.rawData);
			if (!response.isError)
			{
				var msg = response.data ["message"] as string;
				var data = response.data ["user_data"] as Dictionary<string,object>;
				PlayerData player = new PlayerData (data);
				EssentialData.Instance.PlayerData = player;
				EssentialData.Instance.SaveUserId();

				MainMenuController.Instance.InitializeData();
				MainMenuController.Instance.dailyScript.InitGetDailyLogin();

				Debug.Log ("REGISTER FACEBOOK SUCCESS");
			}
			else
			{
				Debug.Log("Fail register facebook " + response.errorMessage);
			}
		}
		#endregion
	}
}

