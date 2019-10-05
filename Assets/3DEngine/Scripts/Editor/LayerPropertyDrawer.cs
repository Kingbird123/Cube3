﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer (typeof (LayerProperty))]
public class LayerPropertyDrawer : PropertyDrawer
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

        var indexValue = property.FindPropertyRelative("indexValue");
        var stringValue = property.FindPropertyRelative("stringValue");

        //display popup
        indexValue.intValue = EditorGUI.LayerField(position, indexValue.intValue);
        stringValue.stringValue = LayerMask.LayerToName(indexValue.intValue);
        int mask = LayerMask.GetMask(stringValue.stringValue);
        var layerProperty = fieldInfo.GetActualObjectForSerializedProperty<LayerProperty>(property);
        layerProperty.maskValue = mask;

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}