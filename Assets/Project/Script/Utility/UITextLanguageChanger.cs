using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UITextLanguageChanger : MonoBehaviour
{
	private static UITextLanguageChanger instance;

	public static UITextLanguageChanger Instance
	{
		get
		{
			return instance;
		}
	}

	void Awake()
	{
		instance = this;
	}

	public string sceneName;

	public List <ChangeTextData> listGameObjTextScene = new List<ChangeTextData>();

	[TextArea (2,7)]
	public string noteForThisScene;

	void Start ()
	{
		if (sceneName == SceneManager.GetActiveScene().name)
		{
			StartChangeText ();
		}
	}

	public void StartChangeText ()
	{
		if (listGameObjTextScene.Count > 0)
		{
			for (int i = 0 ; i < listGameObjTextScene.Count; i++)
			{
				if (/*!string.IsNullOrEmpty (listGameObjTextScene[i].messageCodeId) 
					&&*/ listGameObjTextScene[i].gameobjectContainer != null)
				{
					switch (listGameObjTextScene[i].gameObjType)
					{
					case TypeGameObj.Text2D:
						SetMessageText2D (listGameObjTextScene[i]);
						break;

					case TypeGameObj.TextMesh3D:
						SetMessageTextMesh (listGameObjTextScene[i]);
						break;

					case TypeGameObj.Placeholder2D:
						SetMessagePlaceholderInputfield (listGameObjTextScene[i]);
						break;

					default:
						break;
					}
				}
			}
		}
	}


	#region TEXT MODIFIER FUNCT
	Text dataTxt;
	string dataMessg;

	void SetMessageText2D (ChangeTextData dataObj)
	{
		dataTxt = dataObj.gameobjectContainer.GetComponent<Text>();
		dataMessg = dataObj.messageCodeId;

//		Debug.Log (dataObj.gameobjectContainer.name);

		LanguageManager.Instance.SetMessageToText (dataTxt, dataMessg);
	}

	void SetMessageTextMesh (ChangeTextData dataObj)
	{
		TextMesh dataTxtMesh = dataObj.gameobjectContainer.GetComponent<TextMesh>();
		dataMessg = dataObj.messageCodeId;

//		Debug.Log (dataObj.gameobjectContainer.name);

		LanguageManager.Instance.SetMessageToText (dataTxtMesh, dataMessg);
	}

	void SetMessagePlaceholderInputfield (ChangeTextData dataObj)
	{
		dataTxt = dataObj.gameobjectContainer.GetComponent<InputField>().placeholder.GetComponent<Text>();
		dataMessg = dataObj.messageCodeId;

//		Debug.Log (dataObj.gameobjectContainer.name);

		LanguageManager.Instance.SetMessageToText (dataTxt, dataMessg);
	}

	#endregion

}
