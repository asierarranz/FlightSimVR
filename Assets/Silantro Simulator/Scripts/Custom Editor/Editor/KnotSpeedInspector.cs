using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(KnotSpeedOnly))]
public class KnotSpeedInspector : PropertyDrawer {
	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		string valueStr;
		//

		valueStr = prop.floatValue.ToString ("0.00 Knots");

		EditorGUI.LabelField(position,label.text,valueStr);


	}
}
