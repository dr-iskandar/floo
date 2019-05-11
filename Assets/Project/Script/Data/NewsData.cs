using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NewsData
{
	public string newsID;
	public string newsTitle;
	public string newsDescription;
	public string newsImagelink;
	public string newsLink;

	private const string TAG_ITEM_ID = "news_id";
	private const string TAG_ITEM_TITLE = "title";
	private const string TAG_ITEM_DESCRIPTION = "desc";
	private const string TAG_ITEM_IMAGE = "image_link";
	private const string TAG_ITEM_LINK = "post_link";

	public NewsData ()
	{

	}

	public NewsData (Dictionary<string,object> rawData)
	{
		newsID 			= JsonUtility.GetString(rawData, TAG_ITEM_ID);
		newsTitle 		= JsonUtility.GetString(rawData, TAG_ITEM_TITLE);
		newsDescription = JsonUtility.GetString(rawData, TAG_ITEM_DESCRIPTION);
		newsImagelink	= JsonUtility.GetString(rawData, TAG_ITEM_IMAGE);
		newsLink 		= JsonUtility.GetString(rawData, TAG_ITEM_LINK);
	}
}
