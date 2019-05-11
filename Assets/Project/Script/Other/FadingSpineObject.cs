using UnityEngine;
using Spine.Unity;
using System.Collections;

public class FadingSpineObject : MonoBehaviour 
{
	public SkeletonGraphic spineAnimation;
	private float lop = 0.05f;
	private Color coltemp;
	private bool animPlay = false;

	public void ShowNosAnimation ()
	{
		if (!animPlay)
		{
			spineAnimation.gameObject.SetActive (true);

			coltemp = Color.white;
			coltemp.a = 1;
			spineAnimation.color = coltemp;

			animPlay = true;
//			Debug.Log ("FADOIN " + animPlay);
		}
	}

	public void HideNosAnimation ()
	{
		if (animPlay)
		{
			spineAnimation.gameObject.SetActive (true);

			if (spineAnimation.gameObject.activeSelf)
			{
				StartCoroutine(loopFadeOut());
			}
			else
			{
				Debug.Log ("no aplha");
				CloseNosAnimation ();
			}
			animPlay = false;
//			Debug.Log ("FADOUT " + animPlay);
		}
	}

	public void CloseNosAnimation ()
	{
		spineAnimation.gameObject.SetActive (false);

		animPlay = false;
	}

	IEnumerator loopFadeOut ()
	{
		coltemp = spineAnimation.color;
		for (int i = 0; i < 20; i++)
		{
//			Debug.Log ("Faw alpha "+ coltemp.a);
			coltemp.a -= lop;

			spineAnimation.color = coltemp;
			yield return new WaitForSeconds (0.05f);
		}
	}

}
