using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	public GameObject playerController;
	public DragTransform DT;

	void OnMouseDown()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		SpawnController (ray.origin);
		DT.MouseDown();
	}

	void OnMouseUp()
	{
		HideController();
		DT.MouseUp ();
	}

	void SpawnController(Vector3 pos)
	{
		pos.z = 0;
		playerController.transform.position = pos;
		playerController.SetActive (true);
	}

	void HideController()
	{
		playerController.SetActive (false);
	}
}

