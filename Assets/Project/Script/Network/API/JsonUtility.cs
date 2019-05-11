using System;
using System.Collections;
using System.Collections.Generic;

public class JsonUtility 
{
	public static string GetString (Dictionary<string, object> value, string key)
	{
		if (value == null
		    || !value.ContainsKey (key)
		    || value[key] == null
		    || string.IsNullOrEmpty (value[key].ToString ()))
		{
			return "";
		}
		else
		{
			return value[key].ToString ();
		}
	}
	
	public static float GetFloat (Dictionary<string, object> value, string key)
	{
		if (value == null
		    || !value.ContainsKey (key)
		    || value[key] == null
		    || string.IsNullOrEmpty (value[key].ToString ()))
		{
			return 0.0f;
		}
		else
		{
			return float.Parse (value[key].ToString ());
		}
	}
	
	public static int GetInt (Dictionary<string, object> value, string key)
	{
		if (value == null
		    || !value.ContainsKey (key)
		    || value[key] == null
		    || string.IsNullOrEmpty (value[key].ToString ()))
		{
			return 0;
		}
		else
		{
			return int.Parse (value[key].ToString ());
		}
	}

	public static long GetLong (Dictionary<string, object> value, string key)
	{
		if (value == null
		    || !value.ContainsKey (key)
		    || value[key] == null
		    || string.IsNullOrEmpty (value[key].ToString ()))
		{
			return 0;
		}
		else
		{
			return Int64.Parse (value[key].ToString ());
		}
	}

	public static bool GetBool (Dictionary<string, object> value, string key)
	{
		if (value == null
		    || !value.ContainsKey (key)
		    || value[key] == null
		    || string.IsNullOrEmpty (value[key].ToString ()))
		{
			return false;
		}
		else
		{
			if(int.Parse (value[key].ToString ()) == 1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
	
	public static DateTime GetDateTime (Dictionary<string, object> value, string key, string format = "yyyy-MM-dd HH:mm:ss")
	{
		string date = GetString (value, key);
		
		DateTime dt = DateTime.Now;

		if (!string.IsNullOrEmpty (date))
		{
			try
			{
				dt = DateTime.ParseExact (date, format, System.Globalization.CultureInfo.InvariantCulture);
			}
			catch(System.Exception ex)
			{
				UnityEngine.Debug.Log(ex.ToString());
			}
		}
		return dt;
	}
}
