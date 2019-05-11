using UnityEngine;
using System.Collections;

public class Clicked : MonoBehaviour {

	public TextMesh hitText;

	void OnMouseDown()
	{
		hitText.text = "Clicked";
	}

	void OnMouseUp()
	{
		hitText.text = "Not Clicked";
	}

}
