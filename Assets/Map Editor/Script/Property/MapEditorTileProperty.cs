using UnityEngine;
using System.Collections;

namespace FlooMapEditor
{
	public class MapEditorTileProperty : MonoBehaviour 
	{
		private static MapEditorTileProperty instance;

		public static MapEditorTileProperty Instance
		{
			get
			{
				return instance;
			}
		}

		private MapEditorTileController selectedTile;

		void Awake()
		{
			instance = this;
		}

		void Start()
		{
			
		}

		public void HideProperty()
		{
			Debug.Log("Hide Tile Property");
			this.gameObject.SetActive(false);
		}

		public void ShowProperty(MapEditorTileController tile)
		{
			Debug.Log("Show Tile Property");
			selectedTile = tile;
			this.gameObject.SetActive(true);
		}

		public void RotatePlus90()
		{
			selectedTile.ApplyRotation(90.0f);
		}

		public void RotateMinus90()
		{
			selectedTile.ApplyRotation(-90.0f);
		}

	}
}