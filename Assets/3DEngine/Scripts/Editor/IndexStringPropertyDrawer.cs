using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System;

[CustomPropertyDrawer (typeof(IndexStringProperty))]
public class IndexStringPropertyDrawer : NestablePropertyDrawer
{
    protected new IndexStringProperty propertyObject { get { return (IndexStringProperty)base.propertyObject; } }
    private SerializedProperty indexValue;
    private SerializedProperty stringValue;
    private SerializedProperty stringValues;

    protected override void Initialize(SerializedProperty prop, Type _type)
    {
        base.Initialize(prop, typeof(IndexStringProperty));
        indexValue = prop.FindPropertyRelative("indexValue");
        stringValue = prop.FindPropertyRelative("stringValue");
        stringValues = prop.FindPropertyRelative("stringValues");
    }

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck();

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        //display popup
        if (propertyObject.stringValues != null)
        {
            indexValue.intValue = EditorGUI.Popup(position, indexValue.intValue, propertyObject.stringValues);
            if (propertyObject.stringValues.Length > 0)
                stringValue.stringValue = propertyObject.stringValues[indexValue.intValue];
        }
            
        if (EditorGUI.EndChangeCheck())
        {
            indexValue.serializedObject.ApplyModifiedProperties();
            stringValue.serializedObject.ApplyModifiedProperties();
            stringValues.serializedObject.ApplyModifiedProperties();
        }
        // Set indent back to what it was
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}