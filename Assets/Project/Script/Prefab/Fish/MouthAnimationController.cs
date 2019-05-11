using UnityEngine;
using System.Collections;

public class MouthAnimationController : MonoBehaviour {

	public int fishID;
	public GameObject mouthPosition;

	public void PlayFishMouthAnimation()
	{
		SoundUtility.Instance.PlaySFX (SFXData.SfxEat);
		ResetMouthPosition ();
		switch(fishID) //fish ID Somehow need to be stored somewhere....
		{
		case 1:
			StartCoroutine(PlayNemo ());
			break;
		case 2:
			StartCoroutine(PlayDori ());
			break;
		case 3:
			StartCoroutine(PlayAngler ());
			break;
		case 4: //skeletonfish
			//no animation for number 4
			break;
		case 5:
			//pine follows nemo animation
			StartCoroutine(PlayNemo ());
			break;
		case 6:
			//orca follows nemo animation
			StartCoroutine(PlayNemo ());
			break;
		case 7:
			//rudolf follows nemo animation
			StartCoroutine (PlayNemo ());
			break;
		case 8:
			//snowy follows nemo animation
			StartCoroutine (PlayNemo ());
			break;
		case 9:
			//flower follows nemo animation
			StartCoroutine (PlayNemo ());
			break;
		case 10:
			//dragon follows nemo animation
			StartCoroutine (PlayNemo ());
			break;
		default:
			StartCoroutine(EatAnimation ());
			break;
		} 
	}

	public void ResetMouthPosition()
	{	
		if (mouthPosition != null)
			mouthPosition.transform.localScale = new Vector3 (1,1);
	}

		
	private IEnumerator PlayNemo()
	{
		float xScale = 0.08f;
		float waitTime = 0.01f;

		for (int i = 0; i < 5; i++)
		{
			mouthPosition.transform.localScale += new Vector3 (xScale,xScale);
			yield return new WaitForSeconds (waitTime);
		}
		for (int i = 0; i < 10; i++)
		{
			mouthPosition.transform.localScale -= new Vector3 (xScale/2,xScale/2);
			yield return new WaitForSeconds (waitTime);
		}
	}

	private IEnumerator PlayDori()
	{
		float xScale = 0.08f;
		float waitTime = 0.01f;

		for (int i = 0; i < 5; i++)
		{
			mouthPosition.transform.localScale += new Vector3 (xScale,xScale);
			yield return new WaitForSeconds (waitTime);
		}
		for (int i = 0; i < 10; i++)
		{
			mouthPosition.transform.localScale -= new Vector3 (xScale/2,xScale/2);
			yield return new WaitForSeconds (waitTime);
		}
	}

	private IEnumerator PlayAngler()
	{
		float xPos = 0.3f;
		float waitTime = 0.01f;

		for (int i = 0; i < 5; i++)
		{
			mouthPosition.transform.localPosition -= new Vector3 (xPos,0);
			yield return new WaitForSeconds (waitTime);
		}
		for (int i = 0; i < 10; i++)
		{
			mouthPosition.transform.localPosition += new Vector3 (xPos/2,0);
			yield return new WaitForSeconds (waitTime);
		}
	}

	IEnumerator EatAnimation()
	{
		float xScale = 0.08f;
		float waitTime = 0.01f;

		for (int i = 0; i < 5; i++)
		{
			mouthPosition.transform.localScale += new Vector3 (xScale,xScale);
			yield return new WaitForSeconds (waitTime);
		}
		for (int i = 0; i < 10; i++)
		{
			mouthPosition.transform.localScale -= new Vector3 (xScale/2,xScale/2);
			yield return new WaitForSeconds (waitTime);
		}
	}
}
