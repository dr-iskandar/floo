using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LanguageData
{
	public string messageCode;
	public string messageValue;
	public int messageSize;

	public LanguageData()
	{
		messageCode = "UNDIFINED";
		messageValue = "";
		messageSize = 100;
	}

	public LanguageData(Dictionary<string,object> rawData)
	{
		messageCode = JsonUtility.GetString(rawData, "message_code");
		messageValue = JsonUtility.GetString(rawData, "message_value");
		messageSize = JsonUtility.GetInt(rawData, "message_size");
	}
}
