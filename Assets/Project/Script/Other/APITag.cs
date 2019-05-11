using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class APITag : MonoBehaviour {
	public static string gold 	 = "gold";
	public static string diamond = "diamond";
	public static string mmp 	 = "mmp";
	public static string level 	 = "level";
	public static string exp 	 = "exp";

	public static string playerId 	= "playerId";
	public static string email 		= "email";
	public static string data 		= "data";
	public static string appVersion = "app_version";
	public static string appStatus  = "app_status";
	public static string keyword 	= "keyword";
	public static string fileName 	= "file_name";
	public static string userId		= "user_id";
	public static string platformId = "platform_id";
	public static string secretKey 	= "secret_key";

	public static string heroMeshIndicator 	 = "a";
	public static string horseMeshIndicator  = "b";
	public static string weaponMeshIndicator = "c";
	public static string displayName 		 = "display_name";
	public static string downloadLink 		 = "download_link";
	public static string assetUrlList 		 = "asset_url_list";
	public static string assetKeywordList 	 = "asset_keyword_list";
	public static string currentAssetVersion = "current_asset_version";

	#region SocketTag
	public static string socketURL = "ws://54.254.145.254:10256"; //Dev 1
//	public static string socketURL = "ws://192.168.1.108:10256"; //Nyot PC

	public const string socketTagJoin = "JOIN";
	public const string socketTagUpdate = "UPDATE";
	public const string socketTagUpdateFood = "UPDATE_FOOD";
	public const string socketTagMovement = "MOVEMENT";
	public const string socketTagDead = "DEAD";
	public const string socketTagPredatorDead = "PREDATOR_DEATH";
	public const string socketTagBoost = "BOOST";

	public const string socketTag = "tag";
	public const string socketStatus = "status";
	public const string socketData = "data";
	public const string socketUserData = "user_data";
	public const string socketMapData = "map_data";
	public const string socketOpponents = "opponents";
	public const string socketFoods = "foods";
	public const string socketPredators = "predators";
	public const string socketDetail = "detail";
	public const string socketCoin = "coins";

	//Player DEAD detail
	public const string socketDeadPlayTime = "play_time";
	public const string socketDeadLevel = "level";
	public const string socketDeadPlayerKilled = "player_killed";
	public const string socketDeadGold = "gold";

	public static string[] SocketMapData(string xPosition, string yPosition, string mapFeatureType, string mapFeaturekeyword)
	{
		//Available Tag: JOIN
		string[] result = {xPosition, yPosition, mapFeatureType, mapFeaturekeyword };
		return result;
	}

	public static string[] SocketOpponent(string opponentID,string playerName, string xPosition, string yPosition, string fishAngle,string fishType, string colorCode, string level)
	{
		//Available Tag: UPDATE
		string[] result = {opponentID, playerName, xPosition, yPosition, fishAngle, fishType, colorCode, level};
		return result;
	}
		
	public static string[] SocketFoods(string foodID, string xPosition, string yPosition, string foodType, string colorCode)
	{
		//Available Tag: UPDATE
		string[] result = {foodID, xPosition, yPosition, foodType, colorCode};
		return result;
	}

	public static string[] SocketUserData(string playerID, string playerName, string xPosition, string yPosition, string fishAngle, string colorCode, string fishType, string level, string exp , string topY, string bottomY, string leftX, string rightX, string width, string height)
	{
		//Available Tag: JOIN, UPDATE
		string[] result = {playerID, playerName, xPosition, yPosition, fishAngle, colorCode, fishType, level, exp, topY, bottomY, leftX, rightX, width, height};
		return result;
	}

	public static string[] SocketSendMovement(string playerID, string directionX, string directionY)
	{
		//Available Tag: MOVEMENT
		string[] result = {playerID, directionX, directionY};
		return result;
	}

	#endregion
}
