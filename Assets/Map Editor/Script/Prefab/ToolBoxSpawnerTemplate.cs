using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace FlooMapEditor
{
	public class ToolBoxSpawnerTemplate : ToolboxTileTemplate 
	{
		public float radius;
		public float distance;

		public SpawnerTypeEditor spawnerType;
		public SpawnerMethodEditor spawnMethod;

		public override void SelectToolboxTile()
		{
			//This item will send function to toolbox manager
			ToolboxManager.Instance.SelectToolboxSpawner(this);
		}

		void Awake()
		{
			referenceImage = gameObject.GetComponent<Image>();
		}
	}
}