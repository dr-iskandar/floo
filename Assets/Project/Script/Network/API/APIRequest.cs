using UnityEngine;
using System.Collections;

public class APIRequest
{
	//The name of the API
	public string apiName = "";
	//The data to be sent
	public WWWForm formData = new WWWForm();
	//Custom timeout in seconds
	public int timeout = 30;
	//The callback function of the api call. Return void, Parameter = APIResponse
	public CoreAPI.APICallback callback;
	//The parser needed to parse the api response
	public CoreAPI.APIResponseParser parser;

	public void AddData(string fieldName, string input)
	{
		formData.AddField(fieldName,input);
	}
}
