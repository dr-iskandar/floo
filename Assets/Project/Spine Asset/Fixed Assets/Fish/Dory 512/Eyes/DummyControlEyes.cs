using UnityEngine;
using System.Collections;

public class DummyControlEyes : MonoBehaviour {

	public FishEyeController fishEyes;
	public FishLevelController fishLevel;

	public void StartEyechange()
	{
		StartCoroutine (StartChanges());
	}

	IEnumerator StartChanges()
	{
		for (int i = 1; i < 11; i++)
		{
			Debug.Log ("Level" + i);
			fishLevel.SkinLevel (i);
			fishEyes.SetEyeLevel (i);
			yield return new WaitForSeconds (3);
		}

	}
}
