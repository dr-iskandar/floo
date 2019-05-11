using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorSelectItem : MonoBehaviour
{
	#region VARAIBLE
	public Image itemColor;
	public Toggle toggItem;
	public Material matSHV;

	private FishHSVData fishHSVData;
	private HSVData selectedHSVData;
	private System.Action<HSVData> delegateAction;

	#endregion

	#region SET DATA
	public void InitDelegateColor (System.Action<HSVData> actDelegate)
	{
		delegateAction = actDelegate;
	}

	public void InitColorSelection (HSVData data)
	{
		selectedHSVData = data;

		string keywordName = selectedHSVData.fishSkin;

		int codeId = int.Parse(selectedHSVData.colorHSVCodeId);
		fishHSVData =  HSVDataControl.Instance.GetFishHSV (codeId);

		ChangeColorimage();
	}

	public void SetParentToggleGrup (GameObject parent)
	{
		toggItem.isOn = false;
		
		toggItem.group = parent.GetComponent<ToggleGroup>();

		transform.SetParent (parent.transform);
		transform.localPosition = Vector3.zero;
		transform.localEulerAngles = Vector3.zero;
		transform.localScale = Vector3.one;
	}
	#endregion

	#region SET UI
	public void SetImageItem (Sprite sprite)
	{
		itemColor.sprite = sprite;

		Material matPrefab = Instantiate (matSHV) as Material;
		itemColor.material = matPrefab;
	}

	public void ChangeColorimage ()
	{
		float H = fishHSVData.fishHue;
		float S = fishHSVData.fishSaturation;
		float V = fishHSVData.fishValue;

		itemColor.material.SetFloat ("_HueShift", H);
		itemColor.material.SetFloat ("_Sat", S);
		itemColor.material.SetFloat ("_Val",V);
	}

	#endregion

	#region TOOGLE 
	public void ItemSelected ()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxButton);

		if (toggItem.isOn)
		{
			if (!string.IsNullOrEmpty (selectedHSVData.colorHSVCodeId))
			{
				delegateAction (selectedHSVData);
//				Debug.Log ("selected Toogle Color " + selectedHSVData.colorHSVCodeName);
			}
			else
			{
				Debug.Log ("HSV data is not available");
			}
		}
	}
	#endregion
}
