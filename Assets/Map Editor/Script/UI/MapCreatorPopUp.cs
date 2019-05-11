using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace FlooMapEditor
{
	public class MapCreatorPopUp : MonoBehaviour 
	{
		public InputField inputName;
		public InputField inputWidth;
		public InputField inputHeight;
		public MapCreatorManager manager;

		public void ClosePopUp()
		{
			inputName.text = "";
			inputWidth.text = "";
			inputHeight.text = "";
			gameObject.SetActive(false);
		}

		public void Show()
		{
			gameObject.SetActive(true);	
		}

		public void ConfirmCreate()
		{
			//Send create info to map creator system
			try
			{
				string name = inputName.text;
				int width = int.Parse(inputWidth.text);
				int height = int.Parse(inputHeight.text);
				
				manager.CreateMap(name, width, height);
				ClosePopUp();
			}
			catch(System.Exception e)
			{
				Debug.Log(e.Message);
			}
		}
	}
}
