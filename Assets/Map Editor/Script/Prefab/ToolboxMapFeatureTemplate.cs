using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace FlooMapEditor
{
	public class ToolboxMapFeatureTemplate : ToolboxTileTemplate 
	{
		public float radius;
		public MapFeatureType mapFeatureType;

		void Awake()
		{
			referenceImage = this.gameObject.GetComponent<Image>();
		}
		
		void Start()
		{
			
		}
		
		public void SetImageSelect()
		{
			referenceImage.color = Color.green;
		}
		
		public void SetImageUnselect()
		{
			referenceImage.color = Color.white;
		}
		
		public Material GetMaterial()
		{
			return referenceImage.material;
		}
		
		public void SelectToolboxMapFeature()
		{
			//This item will send function to toolbox manager
			ToolboxManager.Instance.SelectToolboxMapFeature(this);
		}
	}
}