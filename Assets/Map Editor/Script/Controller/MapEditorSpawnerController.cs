using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace FlooMapEditor
{
	public class MapEditorSpawnerController : MonoBehaviour, IPointerClickHandler
	{
		#region IPointerClickHandler implementation

		public void OnPointerClick(PointerEventData eventData)
		{
			if (ToolboxManager.Instance.selectedTileType == ToolBoxTileType.Spawner)
			{
				MapEditorSpawnerProperty.Instance.ShowProperty(this);
				MapEditorHighliterController.Instance.ShowHighlight(transform.localPosition, radius);
			}
		}

		#endregion

		private List<SpawnItemData> spawnItems = new List<SpawnItemData>();
		private int spawnInterval;

		public int SpawnInterval
		{
			get
			{
				return spawnInterval;
			}
			set
			{
				spawnInterval = value;
			}
		}

		private int spawnRate;

		public int SpawnRate
		{
			get
			{
				return spawnRate;
			}
			set
			{
				spawnRate = value;
			}
		}

		private int maxSpawn;

		public int MaxSpawn
		{
			get
			{
				return maxSpawn;
			}
			set
			{
				maxSpawn = value;
			}
		}

		private float radius;
		private int distance;
		private string assetCode;

		public SpawnerMethodEditor spawnMethod;
		public SpawnerTypeEditor spawnType;

		private SpawnerLine spawnerLine;

		public void Init(EditorSpawnerData referenceData)
		{
			assetCode = referenceData.assetCode;
			spawnType = (SpawnerTypeEditor) referenceData.spawnerType;
			spawnMethod = (SpawnerMethodEditor) referenceData.spawnerMethod;

			if (spawnMethod == SpawnerMethodEditor.Line)
			{
				spawnerLine = this.gameObject.GetComponent<SpawnerLine>();
			}

			spawnInterval = referenceData.spawnInterval;
			spawnRate = referenceData.spawnRate;
			maxSpawn = referenceData.maxSpawn;
			spawnItems = referenceData.spawnItem;

			SetPosition(referenceData.spawnStartPos, referenceData.spawnEndPos);
			SetAngle(referenceData.angle);
			SetRadius(referenceData.radius);

			if (spawnMethod == SpawnerMethodEditor.Line)
			{
				SetDistance(distance);
			}
		}

		public void SetRadius(float radius)
		{
			if (spawnMethod == SpawnerMethodEditor.Line)
			{
				//If this was a line spawner, change the radius of the line instead
				spawnerLine.SetRadius(radius);
			}
			else
			{
				transform.localScale = new Vector3(radius * 2, radius*2, radius * 2);
			}

			this.radius = radius;
		}

		public float GetRadius()
		{
			return radius;
		}

		//Set the position from the beggining
		private void SetPosition(Vector2 startPos, Vector2 endPos)
		{
			Vector3 centerTransform = Vector3.zero;
			if (spawnMethod == SpawnerMethodEditor.Line)
			{
				float xDiff = endPos.x - startPos.x;
				float yDiff = endPos.y - startPos.y;
				centerTransform = new Vector3(startPos.x + xDiff / 2, startPos.y + yDiff / 2, 0);
				distance = Mathf.RoundToInt( Vector2.Distance(startPos, endPos) );
			}
			else
			{
				centerTransform = new Vector3(startPos.x, startPos.y, 0);
				distance = 0;
			}

			transform.localPosition = centerTransform;
		}

		public void SetDistance(int distance)
		{
			if (spawnMethod == SpawnerMethodEditor.Line)
			{
				spawnerLine.SetDistance(distance);
			}
		}

		public int GetDistance()
		{
			return Mathf.RoundToInt(distance);
		}

		//Set angles
		public void SetAngle(int angle)
		{
			transform.localEulerAngles = new Vector3(0, 0, angle);
		}

		public int GetAngle()
		{
			return int.Parse(transform.localEulerAngles.z.ToString());
		}

		//Set and Get list of avaiable item
		public void SetListItem(List<SpawnItemData> givenItem)
		{
			spawnItems = givenItem;
		}

		public List<SpawnItemData> GetListItem()
		{
			return spawnItems;
		}

		public EditorSpawnerData GenerateData()
		{
			EditorSpawnerData data = new EditorSpawnerData();
			data.assetCode = assetCode;
			data.spawnerMethod = (int)spawnMethod;
			data.spawnerType = (int)spawnType;
			data.spawnInterval = spawnInterval;
			data.spawnRate = spawnRate;
			data.maxSpawn = maxSpawn;
			data.spawnItem = spawnItems;
			data.angle = GetAngle();
			data.radius = GetRadius();

			if (spawnMethod == SpawnerMethodEditor.Line)
			{
				data.spawnStartPos = spawnerLine.firstPoint.position;
				data.spawnEndPos = spawnerLine.secondPoint.position;
			}
			else
			{
				data.spawnStartPos = new Vector2(transform.localPosition.x, transform.localPosition.y);
				data.spawnEndPos = data.spawnStartPos;
			}

			return data;
		}
	}
}