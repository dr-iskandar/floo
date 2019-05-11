using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace FlooMapEditor
{
	public class MapEditorFeatureController : MonoBehaviour, IPointerClickHandler
	{
		#region IPointerClickHandler implementation

		public void OnPointerClick(PointerEventData eventData)
		{
			//When player is editing tiles
			if (ToolboxManager.Instance.selectedTileType == ToolBoxTileType.MapFeature)
			{
				Debug.Log("User want to select this map feature");

				MapEditorFeatureProperty.Instance.ShowProperty(this);

				ShowHighlight();
			}
		}

		#endregion

		public string assetCode = "";
		private MapFeatureType mapType;

		public void Init(EditorMapFeatureData data)
		{
			assetCode = data.assetCode;
			mapType = (MapFeatureType) data.mapFeatureType;

			SetPosition(data.mapXPos,data.mapYPos);
			SetRotation(data.angle);
			SetRadius(data.radius);

			HidingPlaceClusterManager cluster = gameObject.GetComponent<HidingPlaceClusterManager>();
			if (cluster != null)
			{
				cluster.Init(false);
			}
		}

		public void SetAssetCode(string code)
		{
			this.assetCode = code;
		}

		public Vector3 GetPosition()
		{
			return transform.localPosition;
		}

		public void SetPosition(float xPos, float yPos)
		{
			transform.localPosition = new Vector3(xPos, yPos, 0);
		}

		public int GetRotation()
		{
			return int.Parse(transform.localEulerAngles.z.ToString());
		}

		public void SetRotation(float angle)
		{
			transform.localEulerAngles = new Vector3(0, 0, angle);

			HidingPlaceClusterManager cluster = gameObject.GetComponent<HidingPlaceClusterManager>();
			if (cluster != null)
			{
				cluster.StabilizeHidingPlaces();
			}
		}

		public float GetRadius()
		{
			return transform.localScale.x/2;
		}

		public void SetRadius(float radius)
		{
			transform.localScale = new Vector3(radius*2, radius*2, 1.0f);
		}

		public void ShowHighlight()
		{
			MapEditorHighliterController.Instance.ShowHighlight(this.transform.localPosition, GetRadius());
		}

		public EditorMapFeatureData GenerateData()
		{
			EditorMapFeatureData data = new EditorMapFeatureData();
			data.mapXPos = transform.localPosition.x;
			data.mapYPos = transform.localPosition.y;
			data.assetCode = assetCode;
			data.angle = transform.localEulerAngles.z;
			data.radius = transform.localScale.x / 2;
			data.mapFeatureType = (int)mapType;
			return data;
		}
	}
}