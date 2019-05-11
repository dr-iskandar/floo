using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace FlooMapEditor
{
	public class MapEditorSave : MonoBehaviour 
	{
		public Image panelImage;
		public Text mapName;
		public Text mapWH;
		public Text craetedDate;
		public Text modifiedDate;

		private MapEditorSaveData storedData;

		public void Init(MapEditorSaveData data)
		{
			storedData = data;

			mapName.text = data.mapName;
			mapWH.text = data.width + " x " + data.height;
			craetedDate.text = "Created : " + data.createdDate;
			modifiedDate.text = "Modified : " + data.modifiedDate;
		}

		public void EditMap()
		{
			//Send selected map data to Map Editor system to be loaded and edited
			MapCreatorManager.Instance.EditMap(storedData);
		}
	}

}