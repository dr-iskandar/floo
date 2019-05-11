using UnityEngine;
using System.Collections;

public class FishEyeLashesController : MonoBehaviour {

	public FishEyeController fishEyeControl;
	public SpriteRenderer leftLashes;
	public SpriteRenderer rightLashes;
	public Sprite[] fishEyelashesLeft;
	public Sprite[] fishEyelashesRight;
	public Vector2[] fishEyeLashesPosition;

	public void SetEyeLashes(int fishLevel)
	{
		//setting eyelashes position
		Vector2 rightEyeLashesPos = new Vector2 (fishEyeLashesPosition[fishLevel].x * -1 , fishEyeLashesPosition[fishLevel].y);
		leftLashes.transform.localPosition = fishEyeLashesPosition[fishLevel];
		rightLashes.transform.localPosition = rightEyeLashesPos;

		//setting eye sprites
		leftLashes.sprite = fishEyelashesLeft [fishLevel];
		rightLashes.sprite = fishEyelashesRight [fishLevel];
	}

}
