using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LeaderboardItem : MonoBehaviour
{
	public Text leaderNickname;
	public Text leaderDesc;
	public Text txtRankTop;
	public Text txtRankNormal;

	public GameObject IconEat;
	public GameObject IconTimer;
	public GameObject leaderRankTop;
	public GameObject leaderRankNormal;

	public Sprite[] spriteRankTop;
	public Image imgRankTop;
	public Image imgPlayerFish;
	public Material fishMat;

	[SerializeField]
	private LeaderboardData leaderData;

	#region SET DATA
	public void SetItemData (LeaderboardData data)
	{
		DataDefault();

		leaderData = data;

		leaderNickname.text = leaderData.leaderboardPlayerName;

		SetLeaderboardPlayerRank (leaderData.rankingNumber);

		SetLeaderBoardType (leaderData.leaderboardType);

		SetLeaderboardPlayerFish (leaderData.spriteFish);

		SetLeaderboardColorFish (leaderData.colorCode);
	}

	#endregion

	#region UI
	void DataDefault ()
	{
		leaderNickname.text = "";
		leaderDesc.text = "";

		txtRankTop.text = "";
		leaderRankTop.SetActive (false);

		txtRankNormal.text = "";
		leaderRankNormal.SetActive (false);

		IconEat.SetActive (false);
		IconTimer.SetActive (false);
	}

	void SetLeaderboardRankTop (int rankTop)
	{
		imgRankTop.sprite = spriteRankTop[rankTop-1];
	}

	void SetLeaderboardPlayerRank (int rank)
	{
		if (rank > 0 && rank < 4)
		{
			SetLeaderboardRankTop (rank);

			txtRankTop.text = rank.ToString();
			leaderRankTop.SetActive (true);
		}
		else if (rank > 3)
		{
			txtRankNormal.text = rank.ToString();
			leaderRankNormal.SetActive (true);
		}
	}

	void SetLeaderBoardType (int type)
	{
		if (type == 1) // point
		{
			leaderDesc.text = leaderData.totalPoint.ToString("N0");
		}
		else if (type == 2) // time
		{
			IconTimer.SetActive (true);
			leaderDesc.text = SetTimerFromMillisecond (leaderData.totalTimePlay);
		} 
		else if (type == 3) // eat
		{
			IconEat.SetActive (true);
			leaderDesc.text = leaderData.totalEat.ToString("N0");
		} 
	}

	string SetTimerFromMillisecond (long playTime)
	{
		long totalSecond = playTime / 1000;
		long totalMinute = totalSecond / 60;
		long playHour = totalMinute / 60;

		long playSecond = totalSecond % 60;
		long playMinute = totalMinute % 60;
		string displayedPlayTime = playHour.ToString().PadLeft(2,'0') + ":" + playMinute.ToString().PadLeft(2,'0') + ":" + playSecond.ToString().PadLeft(2,'0');

		return displayedPlayTime;
	}

	void SetLeaderboardPlayerFish (Sprite spriteFish)
	{
		imgPlayerFish.sprite = spriteFish;

		Material matPrefab = Instantiate (fishMat) as Material;
		imgPlayerFish.material = matPrefab;
	}

	void SetLeaderboardColorFish (int codeId)
	{
		if (codeId > 0)
		{
			FishHSVData fishHSVData =  HSVDataControl.Instance.GetFishHSV (codeId);

			float H = fishHSVData.fishHue;
			float S = fishHSVData.fishSaturation;
			float V = fishHSVData.fishValue;

			imgPlayerFish.material.SetFloat ("_HueShift", H);
			imgPlayerFish.material.SetFloat ("_Sat", S);
			imgPlayerFish.material.SetFloat ("_Val",V);
		}
	}

	#endregion
}
