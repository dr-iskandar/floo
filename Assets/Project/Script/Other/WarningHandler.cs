using UnityEngine;
using System.Collections;

public class WarningHandler : MonoBehaviour {
	private static WarningHandler instance;
	public static WarningHandler Instance
	{
		get
		{
			return instance;
		}
	}

	[System.NonSerialized] public GameObject warningSign;
	[System.NonSerialized] public Vector2 predatorPosition;
	[System.NonSerialized] public Vector2 playerPosition;
	[System.NonSerialized] public float xTotal;
	[System.NonSerialized] public float yTotal;

	public bool isSharkAround;

	private Vector2 maxValue;
	private Vector2 minValue;
	private Vector2 totalValue;
	private float xFinal;
	private float yFinal;

	private Vector2 localVector;

	void Awake()
	{
		instance = this;
		warningSign = Instantiate((GameObject)Resources.Load("Prefab/Predator/Predator Warning",typeof(GameObject)));
	}

	void FixedUpdate()
	{
		if (isSharkAround)
		{
			//if not active then activate it
			if (!warningSign.activeInHierarchy)
				warningSign.SetActive (true);

			//camera adjust
			yTotal = (float) (Camera.main.orthographicSize * 2.0);
			xTotal = (float) (yTotal * Screen.width / Screen.height);
			totalValue = new Vector2 (xTotal, yTotal);

			//Debug.Log("total value = " + totalValue);

			//readjust position
			SetPlayerPosition();

			maxValue = playerPosition + (totalValue / 2);
			minValue = playerPosition - (totalValue / 2);

			float xClampAddition = 0.06f;
			float yClampAddition = 0.06f;

			xFinal = Mathf.Clamp (predatorPosition.x,minValue.x + xClampAddition,maxValue.x - xClampAddition);
			yFinal = Mathf.Clamp (predatorPosition.y,minValue.y + yClampAddition, maxValue.y - yClampAddition);

			bool sameX = CompareValue (localVector.x,xFinal);
			bool sameY = CompareValue (localVector.y,yFinal);
			if (sameX && sameY) 
			{
				//if same then do nothing
			}
			else 
			{
				localVector = new Vector2 (xFinal,yFinal);
				warningSign.transform.position =  new Vector2 (xFinal,yFinal);
			}
		}
		else
		{
			//if shown then hid the sign
			if (warningSign.activeInHierarchy) 
			{
				warningSign.SetActive (false);
				warningSign.transform.position = new Vector2 (-100f, -100f);
			}
		}
	}


	public void SetSharkPosition(float xPos, float yPos)
	{
		predatorPosition = new Vector2(xPos,yPos);
		//Debug.Log ("Shark Pos x =" + xPos + " y = " + yPos);
	}


	private float diff = 0.04f;
	private bool CompareValue(float valueOne, float valueTwo)
	{
		bool result;
		float token = Mathf.Abs (valueOne - valueTwo);
		if (token <= diff)
			result = true;
		else
			result = false;
		return result;
	}

	private void SetPlayerPosition()
	{
		try
		{
			playerPosition = DisplayGameController.Instance.GetPlayerPosition();
			//Debug.Log("Player Position = " + playerPosition);
		}
		catch
		{
			
		}
	}

	RectTransform rtui;

	public void ToCanvas ()
	{
	//	Vector2 canvasPosition;
	//	bool isHit = RectTransformUtility.ScreenPointToLocalPointInRectangle (rtui,localVector,Camera.main,canvasPosition);
	}

	private Vector2 WorldToCanvasPosition() 
	{
		//Vector position (percentage from 0 to 1) considering camera size.
		//For example (0,0) is lower left, middle is (0.5,0.5)
		//		Vector2 temp = camera.WorldToViewportPoint(position);

		RectTransform canvas = UIGameController.Instance.CanvasOverlay.GetComponent<RectTransform>();

		Vector3 newV3 = new Vector3 (localVector.x, localVector.y, 0);

		Vector2 temp = Camera.main.WorldToViewportPoint(newV3);

		//Calculate position considering our percentage, using our canvas size
		//So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)
		temp.x *= canvas.sizeDelta.x;
		temp.y *= canvas.sizeDelta.y;

		//The result is ready, but, this result is correct if canvas recttransform pivot is 0,0 - left lower corner.
		//But in reality its middle (0.5,0.5) by default, so we remove the amount considering cavnas rectransform pivot.
		//We could multiply with constant 0.5, but we will actually read the value, so if custom rect transform is passed(with custom pivot) , 
		//returned value will still be correct.

		temp.x -= canvas.sizeDelta.x * canvas.pivot.x;
		temp.y -= canvas.sizeDelta.y * canvas.pivot.y;

		return temp;
	}

	public void ButtONTest ()
	{
//		WorldToCanvasPosition ();
		Debug.Log ("WorldToCanvasPosition " + WorldToCanvasPosition ());
	}

}
