using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UILeaderboardController : MonoBehaviour 
{
	private static UILeaderboardController instance;

	public static UILeaderboardController Instance
	{
		get
		{
			return instance;
		}
	}

	#region VARIABLE
	public GameObject canvasLeaderboard;
	public UILeaderPanelController panelLeaderboardScript;
	public GameObject prefabtItemLeaderboard;

	public Text txtPoint;
	public Text txtTime;
	public Text txtEat;

	public Toggle toggPoint;
	public Toggle toggTime;
	public Toggle toggEat;

	public GameObject panelPoint;
	public GameObject panelTime;
	public GameObject panelEat;

	private int count;
	private List <GameObject> listCreatedPoint = new List<GameObject>();
	private List <GameObject> listCreatedTime = new List<GameObject>();
	private List <GameObject> listCreatedEat = new List<GameObject>();

	private List <LeaderboardData> listPoint = new List <LeaderboardData>();
	private List <LeaderboardData> listTimeplay = new List <LeaderboardData>();
	private List <LeaderboardData> listEatFish = new List <LeaderboardData>();

	#endregion

	void Awake()
	{
		instance = this;
	}

	public void InitLeaderboard ()
	{
		SetTabDefault ();
		SwitchTabPanel ();		
		GenerateLeaderList ();
	}

	void GenerateLeaderList ()
	{
		GeneratePrefabLeaderPoint ();
		GeneratePrefabLeaderTime ();
		GeneratePrefabLeaderEat ();
	}

	#region UI FUNCT

	public void SelectTabToogle ()
	{
		SwitchTabPanel ();
	}

	public void OpenCanvasLeaderboard ()
	{
		canvasLeaderboard.SetActive (true);
	}

	public void CloseCanvasLeaderboard ()
	{
		canvasLeaderboard.SetActive (false);
	}

	#endregion

	#region TOGGLE
	void SetTabDefault ()
	{
		panelPoint.SetActive (false);
		panelTime.SetActive (false);
		panelEat.SetActive (false);

//		toggPoint.transform.SetAsLastSibling ();
//		toggEat.transform.SetAsFirstSibling ();
	}

	void OpenTabPoint ()
	{
		panelPoint.SetActive (true);
		panelTime.SetActive (false);
		panelEat.SetActive (false);

//		txtPoint.color = EssentialData.colorBlueLight;
//		txtTime.color = Color.white;
//		txtEat.color = Color.white;
//
//		toggPoint.transform.SetAsLastSibling ();
//		toggEat.transform.SetAsFirstSibling ();
	}

	void OpenTabTimeplay ()
	{
		panelPoint.SetActive (false);
		panelTime.SetActive (true);
		panelEat.SetActive (false);

//		txtPoint.color = Color.white;
//		txtTime.color = EssentialData.colorBlueLight;
//		txtEat.color = Color.white;
//
//		toggTime.transform.SetAsLastSibling ();
//		toggEat.transform.SetAsFirstSibling ();
	}

	void OpenTabEat ()
	{
		panelPoint.SetActive (false);
		panelTime.SetActive (false);
		panelEat.SetActive (true);

//		txtPoint.color = Color.white;
//		txtTime.color = Color.white;
//		txtEat.color = EssentialData.colorBlueLight;
//
//		toggEat.transform.SetAsLastSibling ();
//		toggPoint.transform.SetAsFirstSibling ();
	}

	void SwitchTabPanel ()
	{
		if (toggPoint.isOn)
		{
			OpenTabPoint ();
			panelLeaderboardScript.InitPanelRefresh ();
			SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
		}
		else if (toggTime.isOn)
		{
			OpenTabTimeplay ();
			panelLeaderboardScript.InitPanelRefresh ();
			SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
		}
		else if (toggEat.isOn)
		{
			OpenTabEat ();
			panelLeaderboardScript.InitPanelRefresh ();
			SoundUtility.Instance.PlaySFX (SFXData.SfxButton);
		}
	}

	#endregion

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

	void GeneratePrefabLeaderPoint ()
	{
		ClearList (listCreatedPoint);

		listPoint = LeaderboardDataManager.Instance.GetLeaderboardDataPoint ();

		for (int i = 0; i < listPoint.Count; i++)
		{
			GameObject go = Instantiate (prefabtItemLeaderboard) as GameObject;
			LeaderboardItem leaderboardPrefab = go.GetComponent <LeaderboardItem>();

			go.transform.SetParent (panelLeaderboardScript.InitParentPanelPoint());
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
			go.transform.localScale = Vector3.one;

			leaderboardPrefab.SetItemData (listPoint[i]);

			listCreatedPoint.Add (go);
		}
	}

	void GeneratePrefabLeaderTime ()
	{
		ClearList (listCreatedTime);

		listTimeplay = LeaderboardDataManager.Instance.GetLeaderboardDataTimeplay ();

		for (int i = 0; i < listTimeplay.Count; i++)
		{
			GameObject go = Instantiate (prefabtItemLeaderboard) as GameObject;
			LeaderboardItem leaderboardPrefab = go.GetComponent <LeaderboardItem>();

			go.transform.SetParent (panelLeaderboardScript.InitParentPanelTime());
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
			go.transform.localScale = Vector3.one;

			leaderboardPrefab.SetItemData (listTimeplay[i]);

			listCreatedTime.Add (go);
		}
	}

	void GeneratePrefabLeaderEat ()
	{
		ClearList (listCreatedEat);

		listEatFish = LeaderboardDataManager.Instance.GetLeaderboardDataEat ();

		for (int i = 0; i < listEatFish.Count; i++)
		{
			GameObject go = Instantiate (prefabtItemLeaderboard) as GameObject;
			LeaderboardItem leaderboardPrefab = go.GetComponent <LeaderboardItem>();

			go.transform.SetParent (panelLeaderboardScript.InitParentPanelEat());
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
			go.transform.localScale = Vector3.one;

			leaderboardPrefab.SetItemData (listEatFish[i]);

			listCreatedEat.Add (go);
		}
	}

	#endregion


}
