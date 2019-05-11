using UnityEngine;
using System.Collections;

public class WaterRippleController : MonoBehaviour 
{
	public MeshRenderer renderer;

	// Use this for initialization
	void Start () 
	{
		renderer.sortingOrder = 30;
	}
	

}
