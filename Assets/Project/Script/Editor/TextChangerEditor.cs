using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ChangeTextData))]
public class TextChangerEditor : PropertyDrawer 
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
	{
		label = EditorGUI.BeginProperty (position, label, property);

		position = EditorGUI.PrefixLabel (position, label);

		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = -3;

		var typeRect = new Rect (position.x, position.y, 14 , position.height);
		var gameObjRect = new Rect (position.x+62, position.y, 60, position.height);
		var messgRect = new Rect (position.x+163, position.y, position.width-163, position.height);

		EditorGUI.PropertyField (typeRect, property.FindPropertyRelative ("gameObjType"), GUIContent.none);
		EditorGUI.PropertyField (gameObjRect, property.FindPropertyRelative ("gameobjectContainer"), GUIContent.none);
		EditorGUI.PropertyField (messgRect, property.FindPropertyRelative ("messageCodeId"), GUIContent.none);

		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty ();
	}
}
