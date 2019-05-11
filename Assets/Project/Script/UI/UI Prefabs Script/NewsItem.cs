using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.UI.Extensions;

public class NewsItem : MonoBehaviour 
{
	public ScrollConflictManager conflictScript;
	public Button btnNews;
	public Image imgNews;
	public Text txtNewsTitle;
	public Text txtNewsDescription;
	public Text txtBehindImage;

	private string stringNewsLink;

	#region SET DATA
	public void SetItemData (NewsData data)
	{
		DataDefault ();
		SetTxtBehindImage ();

		txtNewsTitle.text = data.newsTitle;
		txtNewsDescription.text = data.newsDescription;

		if (!string.IsNullOrEmpty (data.newsLink))
		{
			stringNewsLink = data.newsLink;
			//btnNews.interactable = true;
		}

		if (!string.IsNullOrEmpty (data.newsImagelink))
		{
			StartCoroutine (ImageUtility.CreateSpriteFromUrl (data.newsImagelink, SetImageNews));
		}
	}

	public void SetParentScroll (GameObject go)
	{
		conflictScript.ParentScrollObj = go;
	}

	#endregion

	#region SET UI
	void DataDefault ()
	{
		txtNewsTitle.text = "";
		txtNewsDescription.text = "";
		txtBehindImage.text = "";
		stringNewsLink = "";
		//btnNews.interactable = false;
	}

	void SetTxtBehindImage ()
	{
		txtBehindImage.text = LanguageManager.Instance.GetMessage("TXT0015");
	}

	void SetImageNews (Sprite spriteNews)
	{
		imgNews.sprite = spriteNews;
	}

	#endregion

	#region BUTTON
	public void BtnNewsImage ()
	{
		if (!string.IsNullOrEmpty (stringNewsLink))
		{
			EssentialData.Instance.GoToLink (stringNewsLink);
		}
	}

	#endregion
}
