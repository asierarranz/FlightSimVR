using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SpeedOnly))]
public class SpeedInspector : PropertyDrawer
{

	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		string valueStr;
		//

		valueStr = prop.floatValue.ToString ("0.0 ms-1 ");

		EditorGUI.LabelField(position,label.text,valueStr);


	}
}
