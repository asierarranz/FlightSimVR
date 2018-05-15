using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(SilantroControlSurface))]
public class PropertyWindow : Editor {

	public void ArrayGUI(SerializedProperty list)
	{
		EditorGUILayout.PropertyField (list);
		//
		EditorGUI.indentLevel++;
		for (int i = 0; i < list.arraySize; i++) {
			EditorGUILayout.PropertyField (list.GetArrayElementAtIndex (i), new GUIContent ("Section " + (i + 1).ToString ()));
		}
		EditorGUI.indentLevel--;
	}
	//
	//
	// Diplay the the new window in the inspector 
	public override void OnInspectorGUI()
	{
		//ArrayGUI (serializedObject.FindProperty ("AffectedAerofoilSubdivisions"));
	}
	//
}
