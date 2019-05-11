using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AchievementItem : MonoBehaviour 
{
	#region VARIABLE
	public Text achievName;
	public Text achievDesc;
	public Text achievTextProgress;
	public Text txtStaticReward;

	[Header("Reward Value")]
	public GameObject imgGold;
	public Text achievRewardValue;

	[Header("Btn Claim")]
	public GameObject achievClaimGold;
	public Text achievRewardStatus;
	public Button btnClaim;

	[Header("Stars")]
	public GameObject frameStarAchieve;
	public Sprite starEmpty;
	public Sprite starFill;
	public Image[] imgStar;

	[SerializeField]
	private AchievementData achievData;

	#endregion

	#region BUTTON
	public void BtnClaimGold ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		Invoke ("ClaimReward", 0.25f);
	}

	#endregion

	#region SET DATA
	public void SetItemData (AchievementData data)
	{
		DataDefault ();
		SetUITextAchievement ();

		achievData = data;

		int indexStarDone = achievData.clearedStars;

		bool isClaimed 		= achievData.isClaimed[2];

		bool isProgressDone = achievData.isProgressDone;
		bool isGold 		= achievData.isGoldReward;

		achievName.text = achievData.achievementName;
		achievDesc.text = achievData.achievementDescription;
	
		string progressText = "";
		if (achievData.achievementUnlockMethod == 3 || achievData.achievementUnlockMethod == 4)
		{
			long currTime = (achievData.achievementProgressCurr / 1000) / 60;
			long maxTime = (achievData.achievementProgressMax / 1000) / 60;

			progressText = currTime.ToString("N0") + " <b>min</b> / "+ maxTime.ToString("N0") + " <b>min</b>";
		}
		else
		{
			progressText = achievData.achievementProgressCurr.ToString("N0") + " / " + achievData.achievementProgressMax.ToString("N0");
		}
		achievTextProgress.text = progressText;

		if (isGold)
		{
			imgGold.SetActive (true);
			string valueString = achievData.achievementRewardValue.ToString("N0");
			achievRewardValue.text = valueString;
		}
		else // not gold -  show reward image 
		{
			imgGold.SetActive (false);

			achievRewardValue.text = achievData.achievementRewardSkin;
		}

		if (!isClaimed) // NOT YET CLAIMED
		{
			SetImageStar (indexStarDone - 1, starFill);

			if (!isProgressDone)
			{
				ShowStarAchievement ();
			}
			else // if progress done
			{
				ShowBtnClaim ();
			}
		}
		else
		{
			achievTextProgress.text = LanguageManager.Instance.GetMessage("TXT0012"); // COMPLETED

			SetImageStar (indexStarDone, starFill);

			ShowStarAchievement ();
		}
	}

	#endregion

	#region SET UI 
	void DataDefault ()
	{
		achievName.text = "";
		achievDesc.text = "";
		achievTextProgress.text = "";

		achievClaimGold.SetActive (false);

		frameStarAchieve.SetActive (false);

		achievRewardValue.text = "";
		imgGold.SetActive (false);

		SetImageStar (2 , starEmpty);
	}

	public void SetImageStar (int number, Sprite sprite)
	{
		for (int i = 0; i <= number ; i++)
		{
			imgStar[i].sprite = sprite;
		}
	}

	void AnimateImageStar (int number)
	{
		imgStar[number].gameObject.GetComponent <Animator>().SetTrigger ("Scaling");
		SoundUtility.Instance.PlaySFX (SFXData.SfxAchievement);
	}

	/// <summary>
	/// SHOW Btn Claim & HIDE Frame Star.
	/// </summary>
	public void ShowBtnClaim ()
	{
		frameStarAchieve.SetActive (false);
		achievClaimGold.SetActive (true);
	}

	/// <summary>
	/// SHOW Frame Star & HIDE Btn Claim.
	/// </summary>
	public void ShowStarAchievement ()
	{
		frameStarAchieve.SetActive (true);
		achievClaimGold.SetActive (false);
	}

	public void	SetUITextAchievement ()
	{
		LanguageManager.Instance.SetMessageToText(achievRewardStatus, "BTN0011"); //CLAIM

		txtStaticReward.text = LanguageManager.Instance.GetMessage("TXT0011"); // REWARD
	}

	#endregion

	#region CLAIM FUNCTION
	void ClaimReward ()
	{
		if (!string.IsNullOrEmpty (achievData.achievementId))
		{
			UIPopupAdditionalUtility.Instance.ShowLoadingPopup ();

			string apiName = "claim_achievement";
			Debug.Log ("You Claim GOLD " + achievData.achievementName );
			string[] field = { "user_id", "secret_key", "achievement_id" };
			string[] value = { EssentialData.Instance.PlayerData.userId, EssentialData.Instance.PlayerData.secretKey, achievData.achievementId};
			BackEndConnect.Instance.SendRequestToServer (CBClaimReward, apiName, field, value, 3);
		}
	}

	void CBClaimReward(APIResponse response)
	{
		Debug.Log(response.rawData);
		if (!response.isError)
		{
			var message = response.data ["achievement_status"].ToString();
			if (message.Equals ("SUCCESS"))
			{
				var data = response.data["user_data"] as Dictionary<string,object>;
				PlayerData player = new PlayerData(data);
				EssentialData.Instance.PlayerData = player;

				StartCoroutine(AnimationClaimReward());

				if (achievData.isGoldReward)
				{					 
					Debug.Log ("Animation gold here. " + achievData.clearedStars);
				}
				else
				{
					Debug.Log ("You Claim SKIN " + achievData.achievementName );
				}
			}
			else if(message.Equals("FAILED"))
			{
				UIPopupAdditionalUtility.Instance.InitPopNoticeConfirmClose (LanguageManager.Instance.GetMessage("GBL0003"));
				// Something goes wrong with Server.
			}
		}
		else
		{
			Debug.Log ("error " + response.rawData);
			UIPopupAdditionalUtility.Instance.HideLoadingPopup ();
		}
	}

	IEnumerator AnimationClaimReward ()
	{
		UIPopupAdditionalUtility.Instance.HideLoadingPopup ();

		ShowStarAchievement ();
		AnimateImageStar(achievData.clearedStars);

		yield return new WaitForSeconds(1.2f);
		UIAchievementController.Instance.InitAchievement ();
	}
	#endregion
}
