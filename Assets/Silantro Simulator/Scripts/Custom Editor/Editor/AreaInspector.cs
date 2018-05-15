using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//
[CustomPropertyDrawer(typeof(AreaOnly))]
//
public class AreaInspector : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		string valueStr;
		//

		valueStr = prop.floatValue.ToString ("0.000 m2");

		EditorGUI.LabelField(position,label.text,valueStr);


	}
}
