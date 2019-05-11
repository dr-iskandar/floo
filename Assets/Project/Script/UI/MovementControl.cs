using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MovementControl : MonoBehaviour
	, IPointerDownHandler
	, IPointerUpHandler
	, IDragHandler 
{

	private Vector2 position;
	private float limit = 150;
	public GameObject controller;
	public GameObject sphere;

	public Transform fishParentTransform;
	public FishEyeController fishEye;
	bool isUp;

	[System.NonSerialized]public Vector2 worldPoint;
	[System.NonSerialized]public GameObject touchEffect;
	void Awake()
	{
		gameObject.SetActive(false);
		touchEffect = Instantiate((GameObject)Resources.Load("Prefab/Touch Effect",typeof(GameObject)));

	}

	void Start()
	{
		isUp = true;
		isTouched = false;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (GameController.Instance.controllerType == 1)
			OnPointerDownJoypad (eventData);
		else
			OnPointerDownTouch (eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (GameController.Instance.controllerType == 1)
			OnDragJoypad (eventData);
		else
			OnDragTouch (eventData);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (GameController.Instance.controllerType == 1)
			OnPointerUpJoypad (eventData);
		else
			OnPointerUpTouch (eventData);
	}

	public void SetPointerZero()
	{
		if (GameController.Instance.controllerType == 1)
			SetPointerZeroJoypad ();
		else
			SetPointerZeroTouch ();
	}

	void FixedUpdate()
	{
		if (GameController.Instance.controllerType == 1)
			FixedUpdateJoypad ();
		else
			FixedUpdateTouch ();
	}

	#region Joypad Controller

	private void OnPointerDownJoypad(PointerEventData eventData)
	{
		controller.SetActive (true);
		controller.GetComponent<RectTransform> ().position = eventData.position;
		isUp = false;
	}

	private void OnDragJoypad(PointerEventData eventData)
	{
		position = eventData.position - eventData.pressPosition;
		position = Vector2.ClampMagnitude (position,limit);
		sphere.GetComponent<RectTransform> ().localPosition = position;

		//Debug.Log ("Position = " + position.x + ", " + position.y);
		//Debug.Log ("Sent Position = " + position.x /limit + ", " + position.y /limit);
	}

	private void OnPointerUpJoypad(PointerEventData eventData)
	{
		controller.SetActive (false);
		sphere.GetComponent<RectTransform> ().localPosition = Vector2.zero;

		fishEye.SetEyePositionZero ();
		isUp = true;
	}

	private void FixedUpdateJoypad()
	{
		float rotationZ = fishParentTransform.rotation.z;
		float tempRot = 360 - rotationZ;
		Vector3 vector3Temp = new Vector3 (0,0,tempRot);

		fishEye.SetEyeMovement (position.x /50, position.y /50,vector3Temp);

		if (isUp)
			fishEye.SetEyePositionZero ();
	}

	private void SetPointerZeroJoypad()
	{
		try 
		{
			//Debug.Log("Pointer Reset");
			controller.SetActive (false);
			sphere.GetComponent<RectTransform> ().localPosition = Vector2.zero;
			fishEye.SetEyePositionZero ();
			isUp = true;
		}
		catch 
		{

		}
	}
	#endregion

	#region Touch Controller
	private float distance = 4.5f;
	private bool isTouched;
	private bool showTouchEffect;
	private void CastRayToWorld() 
	{
		if (isTouched) {
			Ray ray= Camera.main.ScreenPointToRay(Input.mousePosition);   
			worldPoint = ray.origin + (ray.direction * distance);    
		}
		//Debug.Log( "World point " + worldPoint );
	}

	private void FixedUpdateTouch()
	{
		CastRayToWorld();
		SetShowTouch ();
	}

	private void OnPointerDownTouch(PointerEventData eventData)
	{
		isTouched = true;
		showTouchEffect = false;
	}

	private void OnPointerUpTouch(PointerEventData eventData)
	{
		isTouched = false;
		showTouchEffect = true;
	}

	private void OnDragTouch(PointerEventData eventData)
	{
		showTouchEffect = false;
	}

	private void SetPointerZeroTouch()
	{
		//get player last position then just send that position
		worldPoint = new Vector2 (0,0);
		isTouched = false;
		showTouchEffect = false;

		fishEye.SetEyePositionZero ();
	}

	float x;
	float y;
	private void SetShowTouch()
	{
		if (showTouchEffect) // -> when fish touched that, make it disappear by setting this to false
		{
			//touch effect is shown and stay at the world points
			if (touchEffect.activeSelf == false) 
			{
				touchEffect.SetActive (true);
				//set position here
				touchEffect.transform.position = worldPoint;
			}
		}
		else
		{
			if (touchEffect.activeSelf == true) 
			{
				touchEffect.SetActive (false);
				//ditch position here, preferably to minus coordinates
				touchEffect.transform.position = new Vector2 (-100,-100);
			}
		}
		//string debugLog = string.Format ("Fish Pos = {0} \n World Point = {1}", DisplayGameController.Instance.GetPlayerPosition(), worldPoint);
		//Debug.Log (debugLog);

		//Calculation Begin if showtouch effect is true
		if (showTouchEffect)
		{
			x = Mathf.Abs( DisplayGameController.Instance.GetPlayerPosition ().x - worldPoint.x);
			y = Mathf.Abs( DisplayGameController.Instance.GetPlayerPosition ().y - worldPoint.y);

			//if player reached there, deactivate it
			if ((x + y) < 0.15f) 
			{
				showTouchEffect = false;
			}
		}
	}
	#endregion
}


