using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace FlooMapEditor
{
	public class MapEditorFeatureSpawnDetection : MonoBehaviour, IPointerClickHandler
	{
		#region IPointerClickHandler implementation

		public void OnPointerClick(PointerEventData eventData)
		{
			if(ToolboxManager.Instance.selectedTileType == ToolBoxTileType.MapFeature)
			{
				Vector3 pressPosition = eventData.pointerPressRaycast.worldPosition;

				ToolboxMapFeatureTemplate template = (ToolboxMapFeatureTemplate) ToolboxManager.Instance.tileTemplate;
				Object prefab = AssetManager.Instance.GetPrefabByKeyword(template.assetCode);
				GameObject go = Instantiate(prefab) as GameObject;
				go.transform.SetParent(EditorTileManager.Instance.mapFeatureParent);

				MapEditorFeatureController featureController = go.AddComponent<MapEditorFeatureController>();
				EditorMapFeatureData data = new EditorMapFeatureData();
				data.assetCode = template.assetCode;
				data.tileOptionType = (int)TileOptionType.MapFeature;
				data.mapFeatureType = (int)template.mapFeatureType;
				data.mapXPos = pressPosition.x;
				data.mapYPos = pressPosition.y;
				data.radius = template.radius;
				featureController.Init(data);

				Debug.Log("Spawn A Map Feature " + data.assetCode);


			}
		}

		#endregion


		public void SetMapData(EditorMapData mapData)
		{
			transform.localPosition = new Vector3(mapData.width/2.0f, mapData.height/2.0f,0);
			transform.localScale = new Vector3(mapData.width,mapData.height,1);
		}

		public void Activate()
		{
			gameObject.SetActive(true);
		}

		public void Deactive()
		{
			gameObject.SetActive(false);
		}
	}
}