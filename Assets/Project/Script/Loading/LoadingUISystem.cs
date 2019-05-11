using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingUISystem : MonoBehaviour 
{
	public Text txtPercentage;
	public Text txtNotification;
	public Slider loadingbarSlider;
	public Text txtVersion;

	void Start () 
	{
		loadingbarSlider.value = 0;
	}

	#region UI

	public void UpdateTextLoadingNotification (string text)
	{
		if (!string.IsNullOrEmpty(text))
		{
			txtNotification.text = text;
		}
	}

	public void UpdateLoadingPercentage (float percen)
	{
		string percenString = percen.ToString ("N0") + "%";
		txtPercentage.text = percenString;

		loadingbarSlider.value = percen;	
	}

	public void UpdateTextVersion (string versionString)
	{
		if (!string.IsNullOrEmpty(versionString))
		{
			versionString = LanguageManager.Instance.GetMessage("TXT0001") + " " + versionString;
			txtVersion.text = versionString;
		}
	}
	#endregion

}
