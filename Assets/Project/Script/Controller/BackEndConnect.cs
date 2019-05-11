using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ProjectMiniJSON;

public class BackEndConnect : MonoBehaviour {
	//WebSocket Variable
	public WebSocket backendWebSocket;
	public bool socketReady = false;
	//Instance Method
	private static BackEndConnect instance;
	private List<string> importantTag;
	public static BackEndConnect Instance {
		get {
			if (instance == null) {
				GameObject go = new GameObject ("BackEndAPI");
				instance = go.AddComponent<BackEndConnect> ();
				instance.Init();
			}
		return instance;
		}
	}

	public void Init()
	{
		//Add importnant tag that can not be missed or thrown away
		importantTag = new List<string>();
		importantTag.Add(APITag.socketTagUpdateFood);
		importantTag.Add(APITag.socketTagDead);
	}

	//When Destoyed, terminate established connection
	void OnDestroy()
	{
		if (socketReady)
			backendWebSocket.Close();
	}

	//default template
	void RequestCallback(APIResponse response)
	{
		callback (response);
		//Return the data from the callback here
	}

	//private static DefaultDataAPI instance;
	System.Action<APIResponse> callback;

	//Call this function from anoter class. Set the callback to receive result
	public void SendRequestToServer(System.Action<APIResponse> callback, string apiName)
	{	
		this.callback = callback;
		//Call API To server
		APIRequest request = new APIRequest();
		request.apiName = apiName;
		request.callback = RequestCallback;
		StartCoroutine(CoreAPI.Instance.SendRequestToServer(request));
	}

	public void SendRequestToServer(System.Action<APIResponse> callback, string apiName, string[] fieldName, string[] input, int totalField)
	{
		this.callback = callback;
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		//Call API To server
		APIRequest request = new APIRequest();
		request.apiName = apiName;
		request.callback = RequestCallback;
		for (int i = 0; i < totalField; i++) 
		{
			j.AddField (fieldName [i], input [i]);
		}
		string data = j.Print ();
		if (NetworkConfig.IsUsingEncryption)
			data = AES.Encrypt (data);
		request.AddData (APITag.data, data);
		StartCoroutine(CoreAPI.Instance.SendRequestToServer(request));
	}

	public void SendVerifyRequestToServer(System.Action<APIResponse> callback, string receiptData, string receiptSignature, string userId, string productId, string secretKey)
	{
		this.callback = callback;

		APIRequest request = new APIRequest();
		request.apiName = "verify_android";
		request.callback = RequestCallback;
	
		request.AddData ("receiptData", AES.Encrypt(receiptData, NetworkConfig.IsUsingEncryption));
		request.AddData ("receiptSignature", AES.Encrypt(receiptSignature, NetworkConfig.IsUsingEncryption));
		request.AddData ("userId",AES.Encrypt( userId, NetworkConfig.IsUsingEncryption));
		request.AddData ("productId", AES.Encrypt(productId, NetworkConfig.IsUsingEncryption));
		request.AddData ("secretKey", AES.Encrypt(secretKey, NetworkConfig.IsUsingEncryption));
		StartCoroutine(CoreAPI.Instance.SendVerifyRequestToServer(request));
	}
		
	///<summary> Parse response data to become a list of string with the given api tag </summary>
	///<param name="response"> API response to be parsed</param>
	///<param name="tags"> API Tag to parse the API response with</param>
	///<returns> Returns a list of string after the parse </returns>
	public List<string> ParseList(APIResponse response, string tags)
	{
		var assetList = response.data [tags] as List<object>;
		List<string> newList = new List<string>();
		for (int i = 0; i < assetList.Count; i++) 
		{
			newList.Add (assetList[i].ToString());
		}
		return newList;
	}
		
	///<summary> Parse response data to become a dictionary of <string,string> with the given api tag </summary>
	///<param name="response"> API response to be parsed</param>
	///<param name="mainTag"> Main Tag to parse the API response, the header tag before the dictionary tags</param>
	///<param name="keyTag"> Key Tag to parse the API response, key of the dictionary</param>
	///<param name="valueTag"> Value Tag to parse the API response, value of the dictionary</param>
	///<returns> Returns a dictionary of <string,string> after the parse </returns>
	public Dictionary<string,string> ParseDictionary(APIResponse response, string mainTag ,string keyTag, string valueTag)
	{
		var asset = response.data [mainTag] as List<object>;
		Dictionary<string,string> dict = new Dictionary<string, string> ();
		for (int i = 0; i < asset.Count;i++)
		{
			var assetData = asset[i] as Dictionary<string,object>;
			string keyword = JsonUtility.GetString (assetData,keyTag);
			string fileName = JsonUtility.GetString (assetData,valueTag);
			dict[keyword] = fileName;
		}
		return dict;
	}

	///<summary> Parse response data to obtain a list of dictionary keys. Can also be used to obtain separate string set </summary>
	///<param name="response"> API response to be parsed</param>
	///<param name="mainTag"> Main Tag to parse the API response, the header tag before the dictionary tags</param>
	///<param name="keyTag"> Key Tag to parse the API response, key of the dictionary</param>
	///<returns> Returns a list of keys for the dictionary keywords</returns>
	public List<string> ParseDictionaryKey(APIResponse response, string mainTag ,string keyTag)
	{
		var asset = response.data [mainTag] as List<object>;
		List<string> keys = new List<string>();
		for (int i = 0; i < asset.Count;i++)
		{
			var assetData = asset[i] as Dictionary<string,object>;
			string keyword = JsonUtility.GetString (assetData, keyTag);
			keys.Add (keyword);
		}
		return keys;
	}

	///<summary> Parse response data to become an int </summary>
	public int ParseInt (APIResponse response, string tags)
	{
		return int.Parse(response.data[tags].ToString());
	}

	public string ParseString (APIResponse response, string tags)
	{
		return response.data[tags].ToString();
	}

	public float ParseFloat (APIResponse response, string tags)
	{
		return float.Parse(response.data[tags].ToString());
	}

	///<summary> debug the given object to a structured debug.log</summary>
	public void DebugObject(List<string> obj)
	{
		int count = obj.Count;
		string debug = string.Format ("The List has {0} content", count);
		for (int i = 0; i < count; i++)
		{
			debug += string.Format ("\n{0}. {1}", i+1, obj[i]);
		}
		Debug.Log (debug);
	}

	///<summary> debug the given dictionary to a structured debug.log with the given list of keys</summary>
	public void DebugObject(Dictionary<string,string> dict, List<string> keys)
	{
		int count = keys.Count;
		string debug = string.Format ("The Dictionary has {0} content", count);
		for (int i = 0; i < count; i++)
		{
			debug += string.Format ("\n{0}. key = {1} \n    value = {2}", i+1, keys[i], dict[keys[i]]);
		}
		Debug.Log (debug);
	}
		
	#region Socket Web Connect
	//parse strings to be one string format to be sent via web socket
	public string CompileString(string[] content)
	{
		string result = "";
		for (int i = 0; i < content.Length; i++)
		{
			result += content [i];
			result += ",";
		}
		result = result.Remove (result.Length - 1);
		return result;
	}
		
	public void SendSocket(string SocketTag, string data)
	{
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField ("tag",SocketTag);
		j.AddField ("data", data);
		backendWebSocket.SendString(j.ToString());
 		//backendWebSocket.SendString("Hi there");
	}

	//set the websocket to new websocket when called
	public IEnumerator EstablishConnection()
	{
		Debug.Log ("Trying to connect...");
		socketReady = false;
		backendWebSocket = new WebSocket(new Uri(NetworkConfig.socketIPAddress));
		yield return StartCoroutine(backendWebSocket.Connect());
		Debug.Log ("Socket Connected!");
		socketReady = true;
	}

	public void ClearBuffer()
	{
		backendWebSocket.ClearQueue();
	}

	bool ContainImportantMessage(string receivedMessage)
	{
		for (int i = 0; i < importantTag.Count; i++)
		{
			if (receivedMessage.Contains(importantTag[i]))
			{
				//Debug.Log("Get message " + importantTag[i]);
				return true;
			}
		}
		return false;
	}

	public string ReceiveMessage()
	{
		string receivedMessage = null;
		while(true)
		{
			string temporaryMessage = backendWebSocket.RecvString();
			//Debug.Log("Received message " + temporaryMessage);
			if (temporaryMessage == null )
			{
				break;
			}
			receivedMessage = temporaryMessage;

			if (ContainImportantMessage(receivedMessage))
			{
				break;
			}

			if(backendWebSocket.QueueSize < 5)
			{
				break;
			}
		}

		return receivedMessage;
	}

	#endregion
}
