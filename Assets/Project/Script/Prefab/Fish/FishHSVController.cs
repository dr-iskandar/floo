using UnityEngine;
using System.Collections;
using Spine;
using Spine.Unity;

public class FishHSVController : MonoBehaviour {

	[System.NonSerialized]public MeshRenderersFish fish;
	[System.NonSerialized]public FishEyeController fishEye;
	[System.NonSerialized]public GameObject blinkEffect;

	public void SetFishHSV(FishHSVData data)
	{
		fish.SetFishHSV (data.fishHue, data.fishSaturation, data.fishValue);
		fishEye.SetEyesHSV (data.fishEyeHue, data.fishEyeSaturation, data.fishEyeValue);
	}

//	public void SetSilverEffect()
//	{
//		//blinkEffect.SetActive (true);
//	}
//
//	public void SetGoldEffect()
//	{
//		//blinkEffect.SetActive (true);
//		//blinkEffect.GetComponent<SkeletonAnimator> ().skeleton.SetSkin ("gold_shine");
//		//blinkEffect.GetComponent<SkeletonAnimator> ().skeleton.SetToSetupPose();
//	}
}
