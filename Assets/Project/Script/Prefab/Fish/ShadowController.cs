using UnityEngine;
using System.Collections;

public class ShadowController : MonoBehaviour {

	[System.NonSerialized]public Transform fishTransform;

	void FixedUpdate()
	{
		gameObject.transform.rotation = fishTransform.rotation;
	}

}
