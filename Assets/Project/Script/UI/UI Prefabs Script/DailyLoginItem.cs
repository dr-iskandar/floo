using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DailyLoginItem : MonoBehaviour
{
	public GameObject dailyFish;
	public GameObject goldShell;
	public Text txtGoldValue;
	public GameObject stampObj;
	public GameObject stampObjSpineAnim;

	private DailyRewardData rewardData;

	public void SetItemData (DailyRewardData data)
	{
		rewardData = data;
		SetDefault ();

		if (data.isGoldReward)
		{
			goldShell.SetActive(true);

			int tempValue = int.Parse(data.dailyRewardInfo);
			txtGoldValue.text = "+" + tempValue.ToString("N0");
		}
		else
		{
			dailyFish.SetActive(true);
			txtGoldValue.text = data.dailyRewardInfo;
		}
	}

	public DailyRewardData GetDailyRewardData()
	{
		return rewardData;
	}

	#region UI
	void SetDefault ()
	{
		dailyFish.SetActive (false);
		txtGoldValue.text = "";
		goldShell.SetActive (false);

		stampObj.SetActive (false);
		stampObjSpineAnim.SetActive (false);
	}

	public void SetStamping ()
	{
		stampObj.SetActive (true);
	}

	public void SetAnimationStamping ()
	{
		stampObjSpineAnim.SetActive (true);
	}
	#endregion
}
