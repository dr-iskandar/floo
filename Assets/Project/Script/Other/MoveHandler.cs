using UnityEngine;
using System.Collections;

public class MoveHandler : MonoBehaviour {


	void Start () {
	
	}

	void OnTriggerenter(Collision collisionInfo)
	{
		print("No longer in contact with " + collisionInfo.transform.name);
	}

	void OnCollisionExit(Collision collisionInfo) {
		print("No longer in contact with " + collisionInfo.transform.name);
	}

	// Update is called once per frame
	void LateUpdate () 
	{
		
	}
}
