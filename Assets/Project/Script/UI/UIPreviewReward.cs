using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPreviewReward : MonoBehaviour 
{
	
	public GameObject popupDailyPreview;
	public GameObject dailyFish;
	public GameObject goldShell;
	public Text txtGoldValue;
	public Text txtInfo;

	public void SetItemData (DailyRewardData data)
	{
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

	#region UI
	void SetDefault ()
	{
		dailyFish.SetActive (false);
		goldShell.SetActive (false);
		txtGoldValue.text = "";
		txtInfo.text = "";
		SetTextInfo ();
	}

	void SetTextInfo ()
	{
		txtInfo.text = LanguageManager.Instance.GetMessage("TXT0017");
	}

	public void ShowPopupDailyPreview ()
	{
		popupDailyPreview.SetActive (true);
		EssentialData.popupState = EnumData.PopupState.on;
	}

	public void HidePopupDailyPreview ()
	{
		popupDailyPreview.SetActive (false);
		EssentialData.popupState = EnumData.PopupState.off;
	}

	#endregion
}
