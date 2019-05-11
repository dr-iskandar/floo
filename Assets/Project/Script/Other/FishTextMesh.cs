using UnityEngine;
using System.Collections;

public class FishTextMesh : MonoBehaviour 
{
	public TextMesh fishName;
	public TextMesh fishLvl;
	public TextMesh shadowName;
	public TextMesh shadowLvl;

	public GameObject expBarGameObj;
	public GameObject expBarMat;

	public PtsController ptsControl;

	private Material mat;
	private float valueExpMax; // CURRENT LEVEL's MAX EXPERIENCE (0/MAX)
	private float expBarValue;
	private float expBarValueSaved = 0;

	//calculate
	private float valueExpOld;
	private float valueExpNew;
	private float changeValue = 0;
	private float timeLapse = 0;
	private int loop;

	void Start ()
	{
		#if UNITY_EDITOR
		timeLapse = Time.deltaTime * 1f;
		#else
		timeLapse = Time.deltaTime;
		#endif

		mat = expBarMat.GetComponent<MeshRenderer>().material;
	}

	#region Update UI

	public void UpdateFishLevel (string levelChange)
	{
		fishLvl.text = levelChange;	
		UpdateShadowTxt ();
	}

	public void SetFishName (string nameString)
	{
		fishName.text = nameString;
		UpdateShadowTxt ();
	}

	public void SetExpBarData (float nowExpMax)
	{
		valueExpMax = nowExpMax;
	}

	public void ResetExpToZero ()
	{
		expBarValueSaved = 1;
		expBarValue = 1;
		AnimateExp (expBarValueSaved, expBarValue);
	}

	/// <summary>
	/// EVERY TIME YOUR FISH GET EXP CALL THIS FUNCT
	/// </summary>
	public void CalculateFishExp (float valueExpCurr)
	{
		if (valueExpCurr >= 0)
		{
			valueExpCurr = valueExpMax - valueExpCurr;

			expBarValue = Mathf.InverseLerp(0, valueExpMax, valueExpCurr);
			// Debug.Log ("expBarValue + valueExpMax + valueExpCurr :" + expBarValue + " " + valueExpMax + " " + valueExpCurr);

			if (expBarValue < 0.0001f)
			{
				expBarValue = 0.0001f;
			}
			
			AnimateExp (expBarValueSaved, expBarValue);
			// Debug.Log ("expBarValueSaved + expBarValue " + expBarValueSaved + " "+ expBarValue);
		}
		else
		{
			Debug.Log (valueExpCurr + " minus value cannot be progress" );
		}
	}

	#endregion

	#region UI Control
	// all text
	public void ShowAllText()
	{
		fishName.gameObject.SetActive (true);
		fishLvl.gameObject.SetActive (true);
		shadowName.gameObject.SetActive (true);
		shadowLvl.gameObject.SetActive (true);
	}

	public void HideAllText()
	{
		fishName.gameObject.SetActive (false);
		fishLvl.gameObject.SetActive (false);
		shadowName.gameObject.SetActive (false);
		shadowLvl.gameObject.SetActive (false);
	}

	// exp Bar
	public void ShowExpBar ()
	{
		expBarGameObj.SetActive (true);
		foreach (Transform child in expBarGameObj.transform)
		{
			child.gameObject.SetActive (true);
		}
	}

	public void HideExpBar ()
	{
		expBarGameObj.SetActive (false);
	}

	void UpdateShadowTxt ()
	{
		shadowName.text = fishName.text;
		shadowLvl.text = fishLvl.text;
	}
	#endregion

	#region ANIMATE EXP BAR

	void AnimateExp (float savedValue, float newValue)
	{
		valueExpOld = savedValue;
		valueExpNew = newValue;

		loop = 15;

		changeValue = newValue - savedValue;
		changeValue = changeValue / loop;

		if (expBarGameObj.activeSelf)
		{
			Invoke("ChangePerLoop", timeLapse);			
		}
	}

	void ChangePerLoop()
	{
		loop--;

		if (loop > 0)
		{
			valueExpOld += changeValue;

			ApplyBarExpChanges (valueExpOld);

			if(this.gameObject.activeSelf)
			{
				Invoke("ChangePerLoop", timeLapse);
			}

			expBarValueSaved = valueExpOld;
//			Debug.Log ("      # " + loop + " (" + valueExpOld  + ")");
		}
		else 
		{
			ApplyBarExpChanges (valueExpNew);

			expBarValueSaved = valueExpNew;
//			Debug.Log ("      $ " + loop + " (" + valueExpNew + ")");
		}
	}

	void ApplyBarExpChanges (float value)
	{
		mat.SetFloat ("_Cutoff", value);
	}

	#endregion
}
