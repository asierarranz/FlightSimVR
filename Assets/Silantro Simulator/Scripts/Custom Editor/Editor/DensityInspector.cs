using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//
[CustomPropertyDrawer(typeof(DensityOnly))]
//
public class DensityInspector : PropertyDrawer {
	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		string valueStr;
		//

			valueStr = prop.floatValue.ToString ("0.000 Kg/m3");

		EditorGUI.LabelField(position,label.text,valueStr);


	}
}
