using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum TypeGameObj
{
	Text2D = 1,
	TextMesh3D = 2,
	Placeholder2D = 3
}

[System.Serializable]
public class ChangeTextData
{
	public GameObject gameobjectContainer;
	public TypeGameObj gameObjType;
	public string messageCodeId;
}
