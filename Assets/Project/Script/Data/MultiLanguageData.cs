using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MultiLanguageData
{
	public int languageId;
	public string languageCode;
	public string languageName;

	public MultiLanguageData()
	{
		languageId = 0;
		languageCode = "UNDIFINED";
		languageName = "UNDIFINED";
	}

	public MultiLanguageData(Dictionary<string,object> rawData)
	{
		languageId = JsonUtility.GetInt(rawData, "language_id");
		languageCode = JsonUtility.GetString(rawData, "language_code");
		languageName = JsonUtility.GetString(rawData, "language_name");
	}
}
