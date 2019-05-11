using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace FlooMapEditor
{
	public class MapEditorSpawnerSpawnDetection : MonoBehaviour, IPointerClickHandler
	{
		#region IPointerClickHandler implementation

		public void OnPointerClick(PointerEventData eventData)
		{
			if(ToolboxManager.Instance.selectedTileType == ToolBoxTileType.Spawner)
			{
				Vector3 pressPosition = eventData.pointerPressRaycast.worldPosition;

				ToolBoxSpawnerTemplate template = (ToolBoxSpawnerTemplate) ToolboxManager.Instance.tileTemplate;
				Object prefab = AssetManager.Instance.GetPrefabByKeyword(template.assetCode);
				GameObject go = Instantiate(prefab) as GameObject;
				go.transform.SetParent(EditorTileManager.Instance.spawnerParent);

				MapEditorSpawnerController spawnControl = go.AddComponent<MapEditorSpawnerController>();
				EditorSpawnerData spawnData = new EditorSpawnerData();
				spawnData.assetCode = template.assetCode;
				spawnData.spawnerMethod = (int) template.spawnMethod;
				spawnData.spawnerType = (int) template.spawnerType;
				spawnData.spawnInterval = 1;
				spawnData.spawnRate = 1;
				spawnData.maxSpawn = 10;
				spawnData.radius = 0.5f;

				if (template.spawnMethod == SpawnerMethodEditor.Point)
				{
					spawnData.spawnStartPos = spawnData.spawnEndPos = new Vector2(pressPosition.x, pressPosition.y);
				}
				else if (template.spawnMethod == SpawnerMethodEditor.Line)
				{
					spawnData.spawnStartPos = new Vector2(pressPosition.x - 0.5f, pressPosition.y);
					spawnData.spawnEndPos = new Vector2(pressPosition.x + 0.5f, pressPosition.y);
				}

				spawnControl.Init(spawnData);

				Debug.Log("Spawner Spawn " + spawnData.assetCode);

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