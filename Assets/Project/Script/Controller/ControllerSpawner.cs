using UnityEngine;
using System.Collections;

public class ControllerSpawner : MonoBehaviour {
	public GameObject controller;
	public Vector3 rayPoint;
	private float distance;
	private static ControllerSpawner instance;
	public static ControllerSpawner Instance {
		get {
			if (instance == null) {
				GameObject go = new GameObject ("ControllerSpawner");
				instance = go.AddComponent<ControllerSpawner> ();
			}
			return instance;
		}
	}

	void LateUpdate()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		distance = Vector3.Distance(transform.position, Camera.main.transform.position);
		Vector3 rayPoint = ray.GetPoint(distance);
		Debug.Log ("raypoint = "+rayPoint);
	}

	void OnMouseDown()
	{
		ShowControl ();
	}

	void OnMouseUp()
	{
		transform.localPosition = new Vector3 (0,0,-0.6f);
		HideControl ();
	}

	void ShowControl()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 rayPoint = ray.GetPoint(distance);
		controller.transform.position = rayPoint;
	}

	void HideControl()
	{
		controller.transform.position = new Vector3 (100,100,100);
	}
}
