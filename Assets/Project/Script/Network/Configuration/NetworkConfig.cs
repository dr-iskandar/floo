using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkConfig
{
	public enum EnvironmentType
	{
		Local = 1,
		Staging = 2,
		Development = 3,
		Production = 4
	}

	public static EnvironmentType runEnvironment = EnvironmentType.Production;

	private static bool isInitialize = false;
	
	private static Dictionary<string,string> environmentConfig;
	
	private const bool preventProduction = false;

	private static bool isUsingEncryption = true;

	public static void InitConfig()
	{
		if(!isInitialize)
		{
			//Can not access production if 
			#if UNITY_EDITOR
			if(preventProduction && runEnvironment > EnvironmentType.Development)
			{
				runEnvironment = EnvironmentType.Development;
			}
			#endif
			
			if(Debug.isDebugBuild)
			{
				if(preventProduction && runEnvironment > EnvironmentType.Development)
				{
					runEnvironment = EnvironmentType.Development;
				}
			}
			
			environmentConfig = new Dictionary<string, string>();
			string socketAddr = "";
			string port = "";
			string backendAddr = "";
			string root = "";
			switch(runEnvironment)
			{
			case EnvironmentType.Staging:
				socketAddr = "ws://floo-dev-alb-72152033.ap-southeast-1.elb.amazonaws.com:10256";
				port = "8888";
				backendAddr = "54.251.180.139";
				root = "/fl_api/";
				isUsingEncryption = false;
				break;
			case EnvironmentType.Development:
				socketAddr = "ws://dev.floogame.com:10256";
				port = "8888";
				backendAddr = "dev.floogame.com";
				root = "/fl_api/";
				isUsingEncryption = true;
				break;
			case EnvironmentType.Production:
				//socketAddr = "ws://game.floogame.com:10256";
				//port = "8888";
				//backendAddr = "api.floogame.com";
				socketAddr = "ws://103.196.116.229:8080";
				port = "8080";
				backendAddr = "103.196.116.229";
				root = "/fl_api_new/";
				isUsingEncryption = true;
				break;
			default:	//The Default is local environment
				socketAddr = "ws://192.168.0.20:10256";
				port = "8080";
				backendAddr = "192.168.0.20";
				root = "/fl_api/";
				isUsingEncryption = true;
				break;
				
			}
			
			environmentConfig["socketAddress"] = socketAddr;
			environmentConfig["socketPort"] = port;
			environmentConfig["backendAddress"] = backendAddr;
			environmentConfig["backendRoot"] = root;
			
			Debug.Log("Game Connected to " + runEnvironment + " Server");
			
			isInitialize = true;
		}
	}

	public static string socketIPAddress 
	{
		get{
			InitConfig();
			return environmentConfig["socketAddress"];
		}
	}
	public static string apiPort
	{
		get{
			InitConfig();
			return environmentConfig["socketPort"];
		}
	}
	public static string apiIPAddress
	{
		get{
			InitConfig();
			return environmentConfig["backendAddress"];
		}
	}
	
	public static string apiRoot
	{
		get{
			InitConfig();
			return environmentConfig["backendRoot"];
		}
	}

	public static bool IsUsingEncryption
	{
		get{
			InitConfig();
			return isUsingEncryption;
		}
	}
}
