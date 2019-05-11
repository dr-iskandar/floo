using UnityEngine;
using System.Collections;

public class NetworkUtility 
{
	public static int GetPlatformId()
	{
		int platFormId = 1;
		#if UNITY_ANDROID
		platFormId = 2;
		#elif UNITY_IOS
		platFormId = 2;
		#else
		platFormId = 1;
		#endif

		return platFormId;
	}
}
