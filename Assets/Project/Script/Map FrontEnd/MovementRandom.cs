using UnityEngine;
using System.Collections;

public class MovementRandom : MonoBehaviour {

	private float multiplier = 3;
	private float min = -10;
	private float max = 10;
	private int interval = 20;
	private int localInterval;

	// Update is called once per frame
	void Update () 
	{
		if (localInterval < interval) {
			localInterval++;
		} else {
			localInterval = 0;
			Vector3 direction = new Vector3 (Random.Range(min,max),Random.Range(min,max),0);
			gameObject.GetComponent<Rigidbody>().AddForce (direction * multiplier);
		}
	}
}
