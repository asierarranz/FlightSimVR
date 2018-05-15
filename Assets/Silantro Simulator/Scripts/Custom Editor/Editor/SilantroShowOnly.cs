using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//
[CustomPropertyDrawer(typeof(SilantroShowAttribute))]
//
public class SilantroShowOnly : PropertyDrawer {



	string unit = " N";
	//
	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		string valueStr;
		unit = label.tooltip;
	
		switch (prop.propertyType)
		{
		case SerializedPropertyType.Integer:
			valueStr = prop.intValue.ToString ();
			break;
		case SerializedPropertyType.Boolean:
			valueStr = prop.boolValue.ToString ();
			break;
		case SerializedPropertyType.Float:
				valueStr = prop.floatValue.ToString ("0.000") ;
			break;
		case SerializedPropertyType.String:
			valueStr = prop.stringValue;
			break;
		default:
			valueStr = "(not supported)";
			break;
		}
		//

		EditorGUI.LabelField(position,label.text,valueStr + " " + unit);
	}
}
