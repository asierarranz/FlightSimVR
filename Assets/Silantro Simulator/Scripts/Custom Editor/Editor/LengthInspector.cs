using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LengthOnly))]
public class LengthInspector : PropertyDrawer {
	//
	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		string valueStr;
		//

		valueStr = prop.floatValue.ToString ("0.00 m");

		EditorGUI.LabelField(position,label.text,valueStr);


	}
}
