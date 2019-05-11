using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace FlooMapEditor
{
	public class MapEditorSpawnerProperty : MonoBehaviour 
	{
		private static MapEditorSpawnerProperty instance;

		public static MapEditorSpawnerProperty Instance
		{
			get
			{
				return instance;
			}
		}
		[Header ("Input Fields")]
		public InputField inputAngle;
		public InputField inputRadius;
		public InputField inputDistance;
		public InputField inputSpawnInterval;
		public InputField inputSpawnRate;
		public InputField inputMaxSpawn;

		[Header ("Buff Options")]
		public GameObject buffOptionsPanel;
		public Toggle toggleInvisiable;
		public Toggle toggleInvulnerable;
		public Toggle toggleSpeed;
		public Toggle toggleEXP;
		public Toggle toggleGold;

		private MapEditorSpawnerController selectedSpawner;

		void Awake()
		{
			instance = this;
		}

		public void ShowProperty(MapEditorSpawnerController spawner)
		{
			selectedSpawner = spawner;
			gameObject.SetActive(true);

			inputAngle.text = selectedSpawner.GetAngle().ToString();
			inputRadius.text = selectedSpawner.GetRadius().ToString();
			if (selectedSpawner.spawnMethod == SpawnerMethodEditor.Point)
			{
				inputDistance.enabled = false;
			}
			else
			{
				inputDistance.enabled = true;
			}
			inputDistance.text = selectedSpawner.GetDistance().ToString();

			inputSpawnInterval.text = selectedSpawner.SpawnInterval.ToString();
			inputSpawnRate.text = selectedSpawner.SpawnRate.ToString();
			inputMaxSpawn.text = selectedSpawner.MaxSpawn.ToString();

			if (selectedSpawner.spawnType == SpawnerTypeEditor.Food)
			{
				buffOptionsPanel.SetActive(false);
			}
			else
			{
				buffOptionsPanel.SetActive(true);
				SetBuffToggle(selectedSpawner.GetListItem());
			}
		}

		public void HideProperty()
		{
			gameObject.SetActive(false);
		}

		public void ApplyBuffChange()
		{
			List<SpawnItemData> selectedPowerUp = new List<SpawnItemData>();
			if (toggleInvisiable.isOn)
			{
				SpawnItemData item = new SpawnItemData();
				item.buffType = (int)BuffTypeEditor.Invisible;
				selectedPowerUp.Add(item);
			}
			if (toggleInvulnerable.isOn)
			{
				SpawnItemData item = new SpawnItemData();
				item.buffType = (int)BuffTypeEditor.StarPower;
				selectedPowerUp.Add(item);
			}
			if (toggleSpeed.isOn)
			{
				SpawnItemData item = new SpawnItemData();
				item.buffType = (int)BuffTypeEditor.Speed;
				selectedPowerUp.Add(item);
			}
			if (toggleEXP.isOn)
			{
				SpawnItemData item = new SpawnItemData();
				item.buffType = (int)BuffTypeEditor.Experience;
				selectedPowerUp.Add(item);
			}
			if (toggleGold.isOn)
			{
				SpawnItemData item = new SpawnItemData();
				item.buffType = (int)BuffTypeEditor.Gold;
				selectedPowerUp.Add(item);
			}

			selectedSpawner.SetListItem(selectedPowerUp);
		}

		public void ApplyPropertiesChange()
		{
			Debug.Log("Angle Changed");
			try
			{
				int desiredAngle = int.Parse(inputAngle.text);
				SetAngle(desiredAngle);

				float radius = float.Parse(inputRadius.text);
				selectedSpawner.SetRadius(radius);

				int distance = int.Parse(inputDistance.text);
				selectedSpawner.SetDistance(distance);

				int spawnInterval = int.Parse(inputSpawnInterval.text);
				selectedSpawner.SpawnInterval = spawnInterval;

				selectedSpawner.SpawnInterval = int.Parse(inputSpawnInterval.text);
				selectedSpawner.SpawnRate = int.Parse(inputSpawnRate.text);
				selectedSpawner.MaxSpawn = int.Parse(inputMaxSpawn.text);

			}
			catch(System.Exception e)
			{
				Debug.Log("Fail to update angle " + e.Message);
				inputAngle.text = selectedSpawner.GetAngle().ToString();
				inputRadius.text = selectedSpawner.GetRadius().ToString();
			}
		}

		public void DeleteSpawner()
		{
			if (selectedSpawner != null)
			{
				Destroy(selectedSpawner.gameObject);
			}

			MapEditorHighliterController.Instance.HideHighlight();
			HideProperty();
		}

		private void SetAngle(int angle)
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

			selectedSpawner.SetAngle(angle);
			inputAngle.text = angle.ToString();
		}

		private void SetBuffToggle(List<SpawnItemData> buffData)
		{
			toggleEXP.isOn = false;
			toggleInvisiable.isOn = false;
			toggleInvulnerable.isOn = false;
			toggleSpeed.isOn = false;
			toggleGold.isOn = false;

			for (int i = 0; i < buffData.Count; i++)
			{
				SpawnItemData item = buffData[i];
				switch ((BuffTypeEditor)item.buffType)
				{
					case BuffTypeEditor.Invisible:
						toggleInvisiable.isOn = true;
						break;
					case BuffTypeEditor.StarPower:
						toggleInvulnerable.isOn = true;
						break;
					case BuffTypeEditor.Speed:
						toggleSpeed.isOn = true;
						break;
					case BuffTypeEditor.Gold:
						toggleGold.isOn = true;
						break;
					case BuffTypeEditor.Experience:
						toggleEXP.isOn = true;
						break;
					default :
						break;
				}
			}
		}
	}
}