using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProjectMiniJSON;

public class DefaultDataAPI : MonoBehaviour 
{
	private static DefaultDataAPI instance;

	public static DefaultDataAPI Instance {
		get {
			if(instance == null)
			{
				GameObject go = new GameObject("API Default Data");
				instance = go.AddComponent<DefaultDataAPI>();
			}
			
			return instance;
		}
	}

	System.Action<bool> callback;

	//Call this function from anoter class. Set the callback to receive result
	public void SendRequestToServer(System.Action<bool> callback, string apiName)//"apiname = axhLogin.php"
	{
		this.callback = callback;

		//Call API To server
		APIRequest request = new APIRequest();
		request.apiName = apiName;	//api link
		request.callback = RequestCallback;
		StartCoroutine(CoreAPI.Instance.SendRequestToServer(request));
	}

	void RequestCallback(APIResponse response)
	{
		//Return the data from the callback here
	}
}
