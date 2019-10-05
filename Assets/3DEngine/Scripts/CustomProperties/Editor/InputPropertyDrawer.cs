using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer (typeof (InputProperty))]
public class InputPropertyDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        //put all input managers axis into an array
        var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
        var obj = new SerializedObject(inputManager);
        var axisArray = obj.FindProperty("m_Axes");
        var inputAxisNames = new string[axisArray.arraySize];
        for (int i = 0; i < inputAxisNames.Length; i++)
        {
            inputAxisNames[i] = axisArray.GetArrayElementAtIndex(i).FindPropertyRelative("m_Name").stringValue;
        }

        var stringValue = property.FindPropertyRelative("stringValue");
        var indexValue = property.FindPropertyRelative("indexValue");

        //display popup
        indexValue.intValue = Mathf.Clamp(indexValue.intValue, 0, inputAxisNames.Length - 1);
        indexValue.intValue = EditorGUI.Popup(position, indexValue.intValue, inputAxisNames);
        SerializedProperty elementHor = axisArray.GetArrayElementAtIndex(indexValue.intValue);
        stringValue.stringValue = elementHor.FindPropertyRelative("m_Name").stringValue;
        obj.Dispose();

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}