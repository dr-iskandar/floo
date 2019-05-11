using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIAchievementController : MonoBehaviour 
{
	private static UIAchievementController instance;

	public static UIAchievementController Instance
	{
		get
		{
			return instance;
		}
	}

	#region VARIABLES
	public GameObject canvasAchievement;
	public Transform parentAchievement;
	public GameObject prefabAchievement;
	public ScrollRect scrollRectAchievement;

	private List <AchievementData> listAchievement = new List<AchievementData> ();
	private List <GameObject> listCreatedObj = new List<GameObject> ();
	private int count;
	#endregion

	void Awake()
	{
		instance = this;
	}

	public void InitAchievement ()
	{
		UIPopupAdditionalUtility.Instance.ShowLoadingPopup ();

		listAchievement = AchievementDataManager.Instance.GetAchievementData();
		GeneratePrefabAchievement ();
		UpperMenuGold.Instance.SetGold (EssentialData.Instance.PlayerData.gold);
	}

	#region BUTTON
	public void BtnClosePanelAchievement ()
	{
//		CloseCanvasAchievement ()
		SoundUtility.Instance.PlaySFX (SFXData.SfxBtnClose);

		MainMenuController.Instance.CanvasDefaultCondition ();
	}

	#endregion

	#region SPAWN

	void ClearList (List <GameObject> listCreated)
	{		
		count = listCreated.Count;

		for (int i = 0; i < count; i++)
		{
			Destroy (listCreated[i]);
		}

		listCreated.Clear ();
	}

	void GeneratePrefabAchievement ()
	{
		ClearList (listCreatedObj);

		for (int i = 0; i < listAchievement.Count; i++)
		{
			GameObject go = Instantiate (prefabAchievement) as GameObject;
			AchievementItem achivePrefab = go.GetComponent <AchievementItem>();

			go.transform.SetParent (parentAchievement);
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
			go.transform.localScale = Vector3.one;

			achivePrefab.SetItemData (listAchievement[i]);

			listCreatedObj.Add (go);
		}

		UIPopupAdditionalUtility.Instance.HideLoadingPopup ();
	}

	#endregion

	#region UI FUNCT
	public void OpenCanvasAchievement ()
	{
		canvasAchievement.SetActive (true);
	}

	public void CloseCanvasAchievement ()
	{
		canvasAchievement.SetActive (false);
	}

	#endregion
}
