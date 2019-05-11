using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ProjectMiniJSON;

public class CoreAPI: MonoBehaviour
{
	#region VARIABLES
	private static CoreAPI instance;

	public static CoreAPI Instance {
		get {
			if (instance == null) {
				GameObject go = new GameObject ("CoreAPI");
				instance = go.AddComponent<CoreAPI> ();
			}
			return instance;
		}
	}

	// PUBLIC
	public delegate void APICallback(APIResponse response);
	public delegate APIResponse APIResponseParser(string jsonText);

	// PRIVATE

	private string url;
	private string jsonMessage;
	
	private int requestTimeOut = 30;
	private int defaultTimeOut = 30;

	private double deltaTime;
	
	private bool isSuccess = false;
	
	private WWWForm form;
	#endregion

	private enum APICallbackStatus
	{
		FAILED = 0,
		SUCCESS = 1,
		MAINTENANCE = 102
	}

	private IEnumerator ExecuteAPI ()
	{
		//url = "http://"+NetworkConfig.backendIPAddress+NetworkConfig.backendRoot + url;
		url = "http://"+NetworkConfig.apiIPAddress+":"+NetworkConfig.apiPort+NetworkConfig.apiRoot+url;
		Debug.Log (url);
		yield return null;

		SetBasicForm (form);
		
		WWW www = new WWW (url, form);
		
		DateTime startTime = System.DateTime.Now;
		
		while (!www.isDone)
		{
			deltaTime = (System.DateTime.Now - startTime).TotalSeconds;
			
			if (deltaTime >= requestTimeOut)
			{
				break;
			}
			else
			{
				yield return null;
			}
		}
		
		if (string.IsNullOrEmpty (www.error) && www.isDone)
		{
			jsonMessage = www.text;
			
			isSuccess = true;
		}
		else
		{
			Debug.LogWarning ("Fetch Time : " + deltaTime + " ////  Error : " + www.error);
			
			isSuccess = false;
		}
	}

	private IEnumerator ExecuteVerifyAPI ()
	{
		//url = "http://"+NetworkConfig.backendIPAddress+NetworkConfig.backendRoot + url;
		url = "http://verify.floogame.com:10240/"+url;
//		url = "http://192.168.0.27:10240/"+url;
		Debug.Log (url);
		yield return null;

		SetBasicForm (form);

		WWW www = new WWW (url, form);

		DateTime startTime = System.DateTime.Now;

		while (!www.isDone)
		{
			deltaTime = (System.DateTime.Now - startTime).TotalSeconds;

			if (deltaTime >= requestTimeOut)
			{
				break;
			}
			else
			{
				yield return null;
			}
		}

		if (string.IsNullOrEmpty (www.error) && www.isDone)
		{
			jsonMessage = www.text;

			isSuccess = true;
		}
		else
		{
			Debug.LogWarning ("Fetch Time : " + deltaTime + " ////  Error : " + www.error);

			isSuccess = false;
		}
	}

	public IEnumerator SendVerifyRequestToServer (APIRequest request)
	{
		this.form = request.formData;

		// API URL
		url = request.apiName;

		// Change the timeout
		requestTimeOut = request.timeout;

		bool isDone = false;

		float startTime = Time.time;
		APIResponse response = new APIResponse();

		//Always try to get data
		while(!isDone)
		{
			url = request.apiName;

			IEnumerator ie = ExecuteVerifyAPI ();

			while (ie.MoveNext ())
			{
				yield return ie.Current;
			}

			// CHECK RESPONSE FROM SERVER
			response = new APIResponse ();

			if (!isSuccess)
			{
				SetErrorInfo (response);
				//Not getting response from server, try it again

				if(Time.time - startTime > 15.0f)
				{
					isDone = true;
					break;
				}

			}
			else
			{
				CheckStatus(response,jsonMessage);
				if(!response.isError)
				{
					var jsonData = Json.Deserialize (jsonMessage) as Dictionary<string, object>;
					response.data = jsonData["data"] as Dictionary<string, object>;
					Debug.Log ("Core API Debug = " + jsonMessage);
				}
				isDone = true;
			}
		}

		response.rawData = jsonMessage;

		if (response.isMaintenance)
		{
			Debug.Log("Status Maintenance");
			UIPopupAdditionalUtility.Instance.InitPopMaintenance();
		}
		else
		{
			if (request.callback != null)
			{
				request.callback(response);
			}
			else
			{
				Debug.Log("No callback found. API : " + request.apiName);
			}
		}
		requestTimeOut = defaultTimeOut;
	}

	private void SetBasicForm (WWWForm form)
	{
		form.AddField ("date_time","2016");//TODO add correspondent platform id
	}
	
	private void SetErrorInfo (APIResponse response)
	{
		response.callStatus = 400;
		
		response.isConnect = false;
		response.isError = true;
		response.isTimeOut = true;
		response.errorMessage = "Can't connect to server!!";
	}
	
	public IEnumerator SendRequestToServer (APIRequest request)
	{
		this.form = request.formData;

		// API URL
		url = request.apiName;

		// Change the timeout
		requestTimeOut = request.timeout;

		bool isDone = false;

		float startTime = Time.time;
		APIResponse response = new APIResponse();

		//Always try to get data
		while(!isDone)
		{
			url = request.apiName;

			IEnumerator ie = ExecuteAPI ();
			
			while (ie.MoveNext ())
			{
				yield return ie.Current;
			}

			// CHECK RESPONSE FROM SERVER
			response = new APIResponse ();

			if (!isSuccess)
			{
				SetErrorInfo (response);
				//Not getting response from server, try it again

				if(Time.time - startTime > 15.0f)
				{
					isDone = true;
					break;
				}

			}
			else
			{
				CheckStatus(response,jsonMessage);
				if(!response.isError)
				{
					var jsonData = Json.Deserialize (jsonMessage) as Dictionary<string, object>;
					if (NetworkConfig.IsUsingEncryption) 
					{
						var encString = jsonData ["data"] as string;
						var decrypted = AES.Decrypt (encString);
						Debug.Log ("decrypted: " + decrypted);
						var data = Json.Deserialize (decrypted) as Dictionary<string, object>;
						response.data = data;
					}
					else
						response.data = jsonData["data"] as Dictionary<string, object>;
					Debug.Log ("Core API Debug = " + jsonMessage);
				}
				isDone = true;
			}
		}

		response.rawData = jsonMessage;

		if (response.isMaintenance)
		{
			Debug.Log("Status Maintenance");
			UIPopupAdditionalUtility.Instance.InitPopMaintenance();
		}
		else
		{
			if (request.callback != null)
			{
				request.callback(response);
			}
			else
			{
				Debug.Log("No callback found. API : " + request.apiName);
			}
		}
		requestTimeOut = defaultTimeOut;
	}

	private void CheckStatus (APIResponse response, string json)
	{
		if(!string.IsNullOrEmpty(json))
		{
			try
			{
				var jsonData = Json.Deserialize (json) as Dictionary<string, object>;

				int status = JsonUtility.GetInt (jsonData, "status");

				if ( status == (int) APICallbackStatus.SUCCESS)
				{
					response.callStatus = status;
					response.isConnect = true;
					response.isError = false;
					response.errorMessage = "";
					response.isMaintenance = false;
				}
				else if(status == (int) APICallbackStatus.MAINTENANCE)
				{
					response.callStatus = status;
					response.isConnect = true;
					response.isError = true;
					response.errorMessage = "SERVER IS UNDER MAINTENANCE";
					response.isMaintenance = true;
				}
				else
				{
					response.callStatus = status;
					response.isConnect = true;
					response.isError = true;
					response.isMaintenance = false;
					response.errorMessage = JsonUtility.GetString (jsonData, "message");

					if (string.IsNullOrEmpty (response.errorMessage))
					{
						response.errorMessage = "Unknown Error";
					}
				}
			}
			catch(System.Exception e)
			{
				response.callStatus = 2;
				response.isConnect = true;
				response.isError = true;
				response.isMaintenance = false;
				response.errorMessage = "Server Error : Invalid Json Format";
				Debug.Log ("Error - " + e);
			}

		}
		else
		{
			response.callStatus = 2;
			response.isConnect = true;
			response.isError = true;
			response.isMaintenance = false;
			response.errorMessage = "Server Error : Empty result returned";
		}
	}
}	
