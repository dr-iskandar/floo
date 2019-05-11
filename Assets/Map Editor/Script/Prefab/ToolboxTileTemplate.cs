using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace FlooMapEditor
{
	public enum ToolBoxTileType
	{
		Tile = 1,
		MapFeature = 2,
		Spawner = 3
	}

	public class ToolboxTileTemplate : MonoBehaviour 
	{
		public string assetCode;
		public ToolBoxTileType tileType;

		[HideInInspector]
		public Image referenceImage;

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

		public virtual void SelectToolboxTile()
		{
			//This item will send function to toolbox manager
			ToolboxManager.Instance.SelectToolboxTile(this);
		}

	}
}
