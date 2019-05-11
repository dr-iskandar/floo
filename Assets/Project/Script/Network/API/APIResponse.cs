using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class APIResponse 
{
	public int callStatus;
	
	public bool isConnect = true;
	public bool isError = false;
	public bool isTimeOut = false;
	public bool isMaintenance = false;

	public string errorMessage = "";
	public string rawData;

	public Dictionary<string, object> data;
}

