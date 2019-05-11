using UnityEngine;
using System.Collections;
using Spine;
using Spine.Unity;

public class BuffController : MonoBehaviour {

	[System.NonSerialized]public SkeletonAnimation BuffAnimation;
	[System.NonSerialized]public MeshRenderersFish fishRenderers;
	[System.NonSerialized]public Transform buffParentTransform;
	[System.NonSerialized]public GameObject buffAnimationObject;
	[System.NonSerialized]public GameObject nosBuffGameObject;

	//Private Namespace
	private string buffGold = "gold";
	private string buffExp = "expup";
	private string buffInvisible = "invi";
	private string buffInvulnerable = "invul";
	private string buffSpeed = "speed";
	private bool isPlayer = false;

	public void SetBuffGold()
	{
		string childName = GetChildName();
		if (childName == buffGold) {
			//do nothing
		} else {
			SetNormal();
			InstantiateChild (6);
		}		
	}

	public void SetBuffExp()
	{
		string childName = GetChildName();
		if (childName == buffExp) {
			//do nothing
		} else {
			SetNormal();
			InstantiateChild (2);
		}
	}

	public void SetBuffInvisible(bool isPlayerLocal)
	{
		string childName = GetChildName();
		if (childName == buffInvisible) {
			//do nothing
		} else {
			SetNormal();
			isPlayer = isPlayerLocal;
			InstantiateChild (3);
		}
	}

	public void SetBuffStarPower()
	{
		string childName = GetChildName();
		if (childName == buffInvulnerable) {
			//do nothing
		} else {
			SetNormal();
			InstantiateChild (5);
		}
	}

	public void SetBuffInvulnerable()
	{
		string childName = GetChildName();
		if (childName == buffInvulnerable) {
			//do nothing
		} else {
			SetNormal();
			InstantiateChild (1);
		}
	}

	public void SetBuffSpeed()
	{
		string childName = GetChildName();
		if (childName == buffSpeed) {
			//do nothing
		} else {
			SetNormal();
			InstantiateChild (4);
		}
	}

	public void SetNoBuff()
	{
		BuffAnimation.name = "No Buff";
		SetNormalInactive ();
	}

	public void ActivateNosEffect()
	{
		nosBuffGameObject.SetActive (true);
	}

	public void DeactivateNosEffect()
	{
		nosBuffGameObject.SetActive (false);
	}

	public void IncrementLayer()
	{
		BuffAnimation.GetComponent<MeshRenderer> ().sortingOrder += 1;
	}

	private string GetChildName()
	{
		return buffAnimationObject.name;
	}

	private void SetNormalInactive()
	{
		MakeVisible ();
		InvulStop ();
		buffAnimationObject.SetActive (false);
	}

	private void SetNormal()
	{
		MakeVisible();
		InvulStop ();
		buffAnimationObject.SetActive (true);
	}

	private void InstantiateChild(int id)
	{
		string name;
		switch (id) 
		{
		case 6:
			name = buffGold;
			SoundUtility.Instance.PlaySFX (SFXData.SfxBuffGold);
			break;
		case 2:
			name = buffExp;
			SoundUtility.Instance.PlaySFX (SFXData.SfxBuffExp);
			break;
		case 3:
			name = buffInvisible;
			SoundUtility.Instance.PlaySFX (SFXData.SfxBuffInvi);
			MakeInvisible ();
			break;
		case 1: //spawn invul
			name = buffInvulnerable;
			//no sound because first spawn buff
			break;
		case 4:
			name = buffSpeed;
			SoundUtility.Instance.PlaySFX (SFXData.SfxBuffSpeed);
			break;
		case 5: //star power
			name = buffInvulnerable;
			SoundUtility.Instance.PlaySFX (SFXData.SfxBuffInvul);
			InvulStart();
			break;
		default:
			name = buffGold;
			Debug.LogError ("Error - Instantiate Child Error");
			break;
		}
		//Debug.Log ("IC - " + name);

		BuffAnimation.name = name;
		BuffAnimation.skeleton.SetSkin (name);
		BuffAnimation.skeleton.SetToSetupPose ();
		BuffAnimation.state.SetAnimation (0,"buff_effect_all",true);

		//BuffAnimation.transform.localPosition = Vector3.zero;
		BuffAnimation.transform.localScale = new Vector3(1f,1f,1f);
		BuffAnimation.transform.localRotation = Quaternion.Euler (Vector3.zero);
	}

	void MakeInvisible()
	{
		fishRenderers.DisableAll ();
		buffAnimationObject.SetActive (isPlayer);
	}

	void MakeVisible()
	{
		fishRenderers.EnableAll ();
	}

	void InvulStart()
	{
		fishRenderers.InvulnerableEffectStart ();
	}

	void InvulStop()
	{
		fishRenderers.InvulnerableEffectStop ();
	}
}
