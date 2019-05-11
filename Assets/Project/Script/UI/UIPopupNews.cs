using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI.Extensions;

public class UIPopupNews : MonoBehaviour
{
	#region VARIABLE
	public ScrollRect scrollRectNews;
	public HorizontalScrollSnap snapScript;

	public GameObject popupNews;

	public Transform parentNewsItem;
	public Transform parentNewsPaging;

	public GameObject prefabNewsItem;
	public GameObject prefabNewsPaging;

	public Text txtDonotShowNews;
	public Toggle togDonotShowNews;

	private bool isLoaded = false;
	private int count;
	private List <NewsData> listNews = new List<NewsData>();
	private List <GameObject> listCreatedNews = new List<GameObject>();
	private List <GameObject> listCreatedPaging = new List<GameObject>();
	#endregion

	void Start ()
	{
		SetTextDontShowNews ();
	}

	public void InitPopupNews ()
	{
		snapScript.enabled = false;
		ShowPopupNews ();
		GenerateNewsPrefab ();

		EssentialData.isAlreadyShowNews = true;
	}

	void OnEnable ()
	{
		DefaultPopupCondition ();
		SetStatusToggleNews ();
	}

	#region SPAWN BUFF
	void ClearList (List <GameObject> listCreated)
	{		
		count = listCreated.Count;

		for (int i = 0; i < count; i++)
		{
			Destroy (listCreated[i]);
		}

		listCreated.Clear ();
	}

	void GenerateNewsPrefab ()
	{
		if (isLoaded)
		{
			return;
		}

		ClearList (listCreatedNews);
		ClearList (listCreatedPaging);

		listNews = NewsDataManager.Instance.GetNewsData();

		for (int i = 0; i < listNews.Count; i++)
		{
			GameObject paging = Instantiate(prefabNewsPaging) as GameObject;
			paging.transform.SetParent (parentNewsPaging);
			SetPrefabTransform (paging);
			paging.GetComponent<Toggle>().group = parentNewsPaging.GetComponent<ToggleGroup>();
			listCreatedPaging.Add (paging);

			GameObject go = Instantiate(prefabNewsItem) as GameObject;
			NewsItem newsPrefab = go.GetComponent<NewsItem>();

			go.transform.SetParent (parentNewsItem);
			SetPrefabTransform (go);

			newsPrefab.SetItemData (listNews[i]);
			newsPrefab.SetParentScroll (scrollRectNews.gameObject);

			listCreatedNews.Add (go);
		}

		isLoaded = true;

		Invoke ("EnableSnap", 0.1f);
	}

	void SetPrefabTransform (GameObject go)
	{
		go.transform.localPosition = Vector3.zero;
		go.transform.localEulerAngles = Vector3.zero;
		go.transform.localScale = Vector3.one;
	}
	#endregion

	#region UI
	public void ShowPopupNews ()
	{
		popupNews.SetActive (true);
		EssentialData.popupState = EnumData.PopupState.on;
	}

	public void HidePopupNews ()
	{
		snapScript.enabled = false;
		popupNews.SetActive (false);
		EssentialData.popupState = EnumData.PopupState.off;
	}

	void EnableSnap ()
	{
		snapScript.enabled = true;
	}

	void DefaultPopupCondition ()
	{
		scrollRectNews.verticalNormalizedPosition = 1.0f;
	}

	void SetTextDontShowNews ()
	{
		txtDonotShowNews.text = LanguageManager.Instance.GetMessage("TXT0016");
	}

	void SetStatusToggleNews ()
	{
		togDonotShowNews.isOn = !EssentialData.Instance.LoadStatusAllowShowNews();
	}

	#endregion

	#region BUTTON - TOOGLE
	public void BtnCloseNews ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);

		Invoke ("HidePopupNews", 0.25f);
	}

	public void SwitchTogShowNews ()
	{
		EssentialData.Instance.SaveStatusAllowShowNews (!togDonotShowNews.isOn);
	}

	#endregion
}
