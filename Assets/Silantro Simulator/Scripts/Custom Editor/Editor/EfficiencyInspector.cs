﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(EfficiencyOnly))]
public class EfficiencyInspector : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		string valueStr;
		//

		valueStr = prop.floatValue.ToString ("0.00 Percent");

		EditorGUI.LabelField(position,label.text,valueStr);


	}
}
