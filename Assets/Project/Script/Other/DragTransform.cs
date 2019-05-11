using UnityEngine;
using System.Collections;

public class DragTransform : MonoBehaviour {

	public float limit;
	private bool dragging = false;
	private float distance;
	public Player player;
	public float localMultiplier = 100;

	public void MouseDown()
	{
		distance = Vector3.Distance(transform.localPosition, Camera.main.transform.localPosition);
		dragging = true;
	}

	public void MouseUp()
	{
		dragging = false;
		transform.localPosition = new Vector3 (0,0,-1);
	}

	void LateUpdate()
	{
		if (dragging)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 rayPoint = ray.GetPoint(distance);
			rayPoint = rayPoint - transform.parent.position;
			rayPoint = Vector3.ClampMagnitude(rayPoint,limit);
			rayPoint = rayPoint * localMultiplier;
			rayPoint.z = -1;
			transform.localPosition = rayPoint;
			//player.MovePlayer (transform.localPosition * localMultiplier);

			//Debug.Log (string.Format("X = {0} Y = {1}", rayPoint.x, rayPoint.y));
		}
	}
}
