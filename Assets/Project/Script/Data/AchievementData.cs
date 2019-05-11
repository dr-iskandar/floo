using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AchievementData
{
	public string achievementName;
	public string achievementId;
	public string achievementDescription;

	public long achievementProgressCurr;
	public long achievementProgressMax;

	public int achievementRewardValue;
	public string achievementRewardSkin;

	public bool isProgressDone;
	public bool isGoldReward;

	public int clearedStars;
	public int achievementUnlockMethod;


	public List<string> allAchievementId;
	public List<string> allAchievementName;
	public List<string> allAchievementDescription;
	public List<long> allAchievementProgressMax;
	public List<object> allAchievementReward;
	public List<bool> isClaimed;

	public AchievementData ()
	{
		achievementName = "";
		achievementId = "";
		achievementDescription = "" ;

		achievementProgressCurr = 0;
		achievementProgressMax = 0;

		achievementRewardValue = 0;
		achievementRewardSkin = "";

		achievementUnlockMethod = 0;

		isProgressDone = false;
		isGoldReward = false;

		clearedStars = 0;

		allAchievementId = new List<string>();
		allAchievementName = new List<string>();
		allAchievementDescription = new List<string>();
		allAchievementProgressMax = new List<long>();
		allAchievementReward = new List<object>();
		isClaimed = new List<bool>();
	}
}