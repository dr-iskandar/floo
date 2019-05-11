using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainmenuFishColorManager : MonoBehaviour 
{
	public static MainmenuFishColorManager Instance;

	#region VARIABLE
	public Image fishImageMin;
	public Image fishImageMax;
	public Material fishImageMaterial;

	private FishController controlFish;

	private HSVData selectedHSVData;	// code and name color
	private FishHSVData fishHSVData;	// hsv value 
	private List <HSVData> listColor = new List <HSVData>();

	private GameObject createPrefab;
	//Local HSV Container, no HSV by default
	private float H = 0;
	private float S = 1;
	private float V = 1;
	private int codeId;

	#endregion

	void Awake ()
	{
		Instance = this;
		SetMaterialImage ();
	}

	public void Start () 
	{		
		listColor = DefaultParameterManager.Instance.GetListHSVColorData();
	}

	#region SETDATA
	void ClearPrefab ()
	{
		if (createPrefab != null)
		{
			Destroy (createPrefab);
		}		
	}

	public void CreateMenuFish (string keyword)
	{
		ClearPrefab ();

		Object prefab = AssetManager.Instance.GetPrefabByKeyword(keyword);

		GameObject go = Instantiate(prefab) as GameObject;
		FishController control = go.GetComponent<FishController>();
		if (control == null)
		{
			control = go.AddComponent<FishController>();
		}

		go.name = prefab.name + " menu";
		go.transform.SetParent(this.transform);

		go.transform.localPosition = new Vector3 (0, 1.5f);
		go.transform.localEulerAngles =new Vector3 (0, 0, 90);
		go.transform.localScale = Vector3.one;

		createPrefab = go;
		controlFish = control;

		SetCleanFish ();
	}

	void SetCleanFish ()
	{
		controlFish.fishIndicator.gameObject.SetActive (false);
	}

	#endregion

	#region Change FISH COLOR/HSV

	public void StoreFishHSVData (HSVData data)
	{
		selectedHSVData = data;

		if (!string.IsNullOrEmpty(selectedHSVData.colorHSVCodeId))
		{
			codeId = int.Parse(selectedHSVData.colorHSVCodeId);
			fishHSVData =  HSVDataControl.Instance.GetFishHSV (codeId);
		}
	}

	public void ExecuteFishColorChange ()
	{
		if (!string.IsNullOrEmpty(selectedHSVData.colorHSVCodeId))
		{
			ChangeFishColorShop ();
			ChangeFishColorMenu();

			UIPopupAdditionalUtility.Instance.HideLoadingPopup ();
		}
	}

	public void ResetFishColorChange ()
	{
		Debug.Log ("still hardcod-ed, default");

		int resetCodeId = 0;
		ChangeFishColorPreview (resetCodeId);
		ChangeFishColorMenu (resetCodeId);

		selectedHSVData = new HSVData ();

		UIPopupAdditionalUtility.Instance.HideLoadingPopup ();
	}

	// MENU
	public void ChangeFishColorMenu ()
	{	
		StartCoroutine ("HSVChange");
	}

	public void ChangeFishColorMenu (int id)
	{	
		codeId = id;
		StartCoroutine ("HSVChange");
	}

	IEnumerator HSVChange()
	{
		yield return new WaitForSeconds (0.01f);

		controlFish.SetMainMenuFishHSV (codeId);

		UIPopupAdditionalUtility.Instance.HideLoadingPopup ();
	}
	// SHOP PREVIEW
	public void ChangeFishColorShop ()
	{	
		SetFishHSV (fishImageMax, fishHSVData);
		SetFishHSV (fishImageMin, fishHSVData);
	}

	public void ChangeFishColorPreview (int codeId)
	{	
		FishHSVData fishHSVData =  HSVDataControl.Instance.GetFishHSV (codeId);

		SetFishHSV (fishImageMax, fishHSVData);
		SetFishHSV (fishImageMin, fishHSVData);
	}

	public void ResetFishColorPreview ()
	{
		ResetFishSprite (fishImageMax);
		ResetFishSprite (fishImageMin);
	}
	#endregion

	#region Set HSV
	void ResetFishSprite (Image imageUI)
	{
		imageUI.material.SetFloat ("_HueShift", 0);
		imageUI.material.SetFloat ("_Sat", 1);
		imageUI.material.SetFloat ("_Val", 1);
	}

	public void SetFishHSV (Image imageUI, FishHSVData data)
	{
		H = data.fishHue;
		S = data.fishSaturation;
		V = data.fishValue;

		imageUI.material.SetFloat ("_HueShift", H);
		imageUI.material.SetFloat ("_Sat", S);
		imageUI.material.SetFloat ("_Val",V);
	}

	// mesh
	public void SetFishHSV (MeshRenderer mesh, FishHSVData data)
	{
		H = data.fishHue;
		S = data.fishSaturation;
		V = data.fishValue;

		for (int i = 0; i < mesh.materials.Length; i++)
		{
			mesh.materials[i].SetFloat ("_HueShift", H);
			mesh.materials[i].SetFloat ("_Sat", S);
			mesh.materials[i].SetFloat ("_Val",V);
		}
	}

	#endregion

	#region UI
	public void SetMaterialImage ()
	{
		Material matPrefab = Instantiate (fishImageMaterial) as Material;
		fishImageMax.material = matPrefab;
		fishImageMin.material = matPrefab;

	}
	#endregion
}
