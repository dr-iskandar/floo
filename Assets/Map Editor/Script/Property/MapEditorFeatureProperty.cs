using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace FlooMapEditor
{
	public class MapEditorFeatureProperty : MonoBehaviour 
	{
		private static MapEditorFeatureProperty instance;
		
		public static MapEditorFeatureProperty Instance
		{
			get
			{
				return instance;
			}
		}
		
		private MapEditorFeatureController selectedFeature;

		public InputField inputAngle;
		public InputField inputRadius;

		void Awake()
		{
			instance = this;
		}

		void Start()
		{
			
		}
		
		public void HideProperty()
		{
			this.gameObject.SetActive(false);
		}
		
		public void ShowProperty(MapEditorFeatureController feature)
		{
			selectedFeature = feature;
			this.gameObject.SetActive(true);

			inputAngle.text = feature.GetRotation().ToString();
			inputRadius.text = feature.GetRadius().ToString();
		}

		public void AngleChanged()
		{
			Debug.Log("Angle Changed");
			try
			{
				int desiredAngle = int.Parse(inputAngle.text);
				SetAngle(desiredAngle);
			}
			catch(System.Exception e)
			{
				Debug.Log("Fail to update angle " + e.Message);
				inputAngle.text = selectedFeature.GetRotation().ToString();
			}
		}

		public void DeleteMapFeature()
		{
			if (selectedFeature != null)
			{
				Destroy(selectedFeature.gameObject);
			}

			MapEditorHighliterController.Instance.HideHighlight();
			HideProperty();
		}

		public void RadiusChanged()
		{
			Debug.Log("Radius Changed");
			try
			{
				float radius = float.Parse(inputRadius.text);
				selectedFeature.SetRadius(radius);
				selectedFeature.ShowHighlight();
			}
			catch(System.Exception e)
			{
				Debug.Log("Fail to set radius " + e.Message);
				inputRadius.text = selectedFeature.GetRadius().ToString();
			}
		}

		public void SetAngle(int angle)
		{
			while(angle < 0 || angle >= 360)
			{
				if(angle >= 360)
				{
					angle -= 360;
				}
				else if(angle < 0)
				{
					angle += 360;
				}
			}

			selectedFeature.SetRotation(angle);
			inputAngle.text = angle.ToString();
		}
	}
}