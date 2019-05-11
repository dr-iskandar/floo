using UnityEngine;
using System.Collections;

public class NoRotation : MonoBehaviour {

	public float rotationDegree;

	void LateUpdate()
	{
		gameObject.transform.eulerAngles = new Vector3(0,0,rotationDegree);
	}
}
