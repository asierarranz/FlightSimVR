using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(WeightOnly))]
public class WeightInspector : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		string valueStr;
		//

		valueStr = prop.floatValue.ToString ("0.00 kg");

		EditorGUI.LabelField(position,label.text,valueStr);


	}
}
