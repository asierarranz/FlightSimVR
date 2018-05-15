using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEditor;

[CustomPropertyDrawer(typeof(TemperatureOnly))]
public class TemperatureInspector : PropertyDrawer {
	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		string valueStr;
		//

		valueStr = prop.floatValue.ToString ("0.0 °C");

		EditorGUI.LabelField(position,label.text,valueStr);


	}
}
