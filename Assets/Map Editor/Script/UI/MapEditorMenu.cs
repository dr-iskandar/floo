using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace FlooMapEditor
{
	public class MapEditorMenu : MonoBehaviour 
	{
		public Text editedMapName;
		public Text saveNotification;

		public ToolboxManager toolbox;

		void Start()
		{
			HideNotification();
		}

		public void HideMainMenu()
		{
			this.gameObject.SetActive(false);
		}

		public void ShowMainMenu (string mapName) 
		{
			this.gameObject.SetActive(true);
			editedMapName.text = mapName;
			HideNotification();
			toolbox.ShowGridTab();
		}

		public void ShowMessage()
		{
			saveNotification.gameObject.SetActive(true);
			Invoke("HideNotification", 1.5f);
		}

		void HideNotification()
		{
			saveNotification.gameObject.SetActive(false);
		}


	}
}