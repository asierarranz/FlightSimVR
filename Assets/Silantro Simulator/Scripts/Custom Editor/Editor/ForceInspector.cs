﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//
[CustomPropertyDrawer(typeof(ForceOnly))]
//
public class ForceInspector : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		string valueStr;
		//
	
		valueStr = prop.floatValue.ToString ("0.00 N");

		EditorGUI.LabelField(position,label.text,valueStr);


	}
}