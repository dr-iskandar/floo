using UnityEngine;
using System.Collections;

public class HidingPlaceClusterManager : MonoBehaviour 
{
	public Transform[] hidingPlaces;
	public MeshRenderers[] renderers;
	public Collider selfCollider;
	public Collider[] detectionCollider;
	public SpriteRenderer[] spriteRend;

	private int totalEntered = 0;

	//Initialize the cluster. If using the detection collider, the self collider will be disabled and vise versa
	public void Init(bool isUsingDetectionCollider = true)
	{
		if (isUsingDetectionCollider)
		{
			selfCollider.enabled = false;
			for (int i = 0; i < detectionCollider.Length; i++)
			{
				detectionCollider[i].enabled = true;
			}
		}
		else
		{
			selfCollider.enabled = true;
			for (int i = 0; i < detectionCollider.Length; i++)
			{
				detectionCollider[i].enabled = false;
			}
		}
	}

	//Set the rotation of all hiding places to zero in global position
	public void StabilizeHidingPlaces()
	{
		for (int i = 0; i < hidingPlaces.Length; i++)
		{
			hidingPlaces[i].eulerAngles = Vector3.zero;
		}
	}

	//Will be called by the detection collider
	public void TriggerEntered()
	{
		totalEntered += 1;
		if (totalEntered == 1)
		{
			//Fade Out All mesh renderers. Player fish just enter one detection
			if (renderers != null)
			{
				for (int i = 0; i < renderers.Length; i++)
				{
					//Play Fade Out transition here
					renderers[i].TriggerEnter();
				}
			}
		}
	}

	//Will be called by the detection collider
	public void TriggerExit()
	{
		totalEntered -= 1;
		if (totalEntered <= 0)
		{
			//Fade In All mesh renderes, Player fish already left
			if (renderers != null)
			{
				for (int i = 0; i < renderers.Length; i++)
				{
					//Play Fade in animation here
					renderers[i].TriggerExit();
				}
			}
		}
	}
}
