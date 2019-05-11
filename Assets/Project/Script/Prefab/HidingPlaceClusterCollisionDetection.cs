using UnityEngine;
using System.Collections;

public class HidingPlaceClusterCollisionDetection : MonoBehaviour 
{
	public HidingPlaceClusterManager manager;

	void OnTriggerEnter(Collider other)
	{
		FishController collidingFish = other.GetComponent<FishController> ();
		if (collidingFish != null) 
		{
			collidingFish.TriggerTextFadeOut();
		}

		//Tell fish animation controller that it has entered a hiding place collider
		if (other.tag.Equals(EssentialData.TAG_PLAYER)) 
		{
			manager.TriggerEntered();
		}
	}

	void OnTriggerExit(Collider other)
	{
		FishController collidingFish = other.GetComponent<FishController> ();

		if (collidingFish != null) 
		{
			collidingFish.TriggerTextFadeIn();
		}

		//Tell fish animation controller that it has exit a hiding place collider
		if (other.tag.Equals(EssentialData.TAG_PLAYER)) 
		{
			manager.TriggerExit();
		}
	}
}
