using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float torque;
	public float multiplier;

	public void MovePlayer(Vector3 direction)
	{
		//Debug.Log ("Player moved to " + direction);
		GetComponent<Rigidbody2D>().AddForce (direction * multiplier);
	}

	public void RotatePlayer (float currRotation)
	{
		transform.rotation = Quaternion.Euler (new Vector3 (transform.rotation.x, transform.rotation.y, currRotation));
	}


}
