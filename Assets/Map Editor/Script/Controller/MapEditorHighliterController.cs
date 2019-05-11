using UnityEngine;
using System.Collections;

namespace FlooMapEditor
{
	public class MapEditorHighliterController : MonoBehaviour 
	{
		private static MapEditorHighliterController instance;

		public static MapEditorHighliterController Instance {
			get {
				return instance;
			}
		}

		public Transform topLine;
		public Transform bottomLine;
		public Transform leftLine;
		public Transform rightLine;

		void Awake()
		{
			instance = this;
		}

		void Start()
		{

		}

		public void HideHighlight()
		{
			this.gameObject.SetActive(false);
		}

		public void ShowHighlight(Vector3 centerPosition, float radius)
		{
			this.gameObject.SetActive(true);

			transform.localPosition = new Vector3(centerPosition.x,centerPosition.y,transform.localPosition.z);

			//Set top line
			topLine.localPosition = new Vector3(0,radius,0);
			topLine.localScale = new Vector3(radius*2,0.05f,1);

			//Set bottom line
			bottomLine.localPosition = new Vector3(0,-radius,0);
			bottomLine.localScale = new Vector3(radius*2,0.05f,1);
			
			//Set left line
			leftLine.localPosition = new Vector3(-radius,0,0);
			leftLine.localScale = new Vector3(0.05f,radius*2,1);
			
			//Set right line
			rightLine.localPosition = new Vector3(radius,0,0);
			rightLine.localScale = new Vector3(0.05f,radius*2,1);
			

		}

	}
}