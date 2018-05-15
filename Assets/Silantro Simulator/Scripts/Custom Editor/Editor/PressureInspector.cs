using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEditor;

[CustomPropertyDrawer(typeof(PressureOnly))]
public class PressureInspector : PropertyDrawer {
	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		string valueStr;
		//

		valueStr = prop.floatValue.ToString ("0.00 Kpa");

		EditorGUI.LabelField(position,label.text,valueStr);


	}
}
