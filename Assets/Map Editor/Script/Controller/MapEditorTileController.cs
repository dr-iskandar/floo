using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace FlooMapEditor
{
	public class MapEditorTileController : MonoBehaviour, IPointerClickHandler 
	{
		#region IPointerClickHandler implementation

		public void OnPointerClick(PointerEventData eventData)
		{
			if (isEditable)
			{
				//When player is editing tiles
				if (ToolboxManager.Instance.selectedTileType == ToolBoxTileType.Tile)
				{
					ToolboxTileTemplate template = ToolboxManager.Instance.tileTemplate;
					if (string.IsNullOrEmpty(template.assetCode))
					{
						Debug.Log("User want to select this tile");
						MapEditorTileProperty.Instance.ShowProperty(this);
						EditorTileManager.Instance.ShowEditorHighlight(transform.position);
					}
					else
					{
						//User wants to replace this tile with another one

						//Only replace when user pick different template
						if (!template.assetCode.Equals(this.assetCode))
						{
							assetCode = template.assetCode;
							this.gameObject.GetComponent<Renderer>().material = template.GetMaterial();
						}
					}
				}
			}
		}

		#endregion

		public string assetCode = "";

		private bool isEditable = false;

		public void Init(string code, bool isEdit)
		{
			assetCode = code;
			isEditable = isEdit;
		}

		public void SetAssetCode(string code)
		{
			this.assetCode = code;
		}

		public void ApplyRotation(float rotationValue)
		{
			float angle = transform.localEulerAngles.z;
			angle += rotationValue;
			transform.localEulerAngles = new Vector3(0, 0, angle);
		}

		public EditorTileData GenerateData()
		{
			EditorTileData data = new EditorTileData();
			data.mapXPos = transform.localPosition.x;
			data.mapYPos = transform.localPosition.y;
			data.assetCode = assetCode;
			data.angle = transform.localEulerAngles.z;
			return data;
		}
	}
}