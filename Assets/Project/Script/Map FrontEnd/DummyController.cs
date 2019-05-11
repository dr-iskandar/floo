using UnityEngine;
using System.Collections;

public class DummyController : MonoBehaviour {

	public SkinnedMeshRenderer test;
	public SkinnedMeshRenderer test2;

	void Start()
	{
		test2.sortingOrder = 30;
	}


	void LateUpdate()
	{
		Debug.Log ("layer " + test.sortingOrder);
	}

}
