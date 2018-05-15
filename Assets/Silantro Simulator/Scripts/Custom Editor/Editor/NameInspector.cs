using UnityEditor;
using UnityEngine;
//
[CustomPropertyDrawer(typeof(NameOnly))]
public class NameInspector : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		string valueStr;
		//

		valueStr = prop.stringValue;

		EditorGUI.LabelField(position,label.text,valueStr);


	}
}
