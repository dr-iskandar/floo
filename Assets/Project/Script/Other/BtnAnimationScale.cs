using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BtnAnimationScale : MonoBehaviour 
{
	private Animator anim;
	private string path;

	void Start ()
	{
		anim = this.GetComponent <Animator>();

//		this.GetComponent<Button> ().transition = Selectable.Transition.None;

		if (anim == null)
		{
			anim = this.gameObject.AddComponent<Animator>();
		}

		path = "Animation/BtnScale";
//		anim.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(path);
		anim.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(path,typeof(RuntimeAnimatorController));
	}

	#region BUTTON
	public void BtnPress ()
	{
		anim.SetTrigger ("Pressed");
	}

	public void BtnRelease ()
	{
		anim.SetTrigger ("Release");
	}

	#endregion
}
