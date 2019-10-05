using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer (typeof (ItemProperty))]
public class ItemPropertyDrawer : PropertyDrawer
{
    private SerializedProperty itemName;
    private SerializedProperty indexValue;
    private SerializedProperty itemNames;

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

        itemName = property.FindPropertyRelative("itemName");
        indexValue = property.FindPropertyRelative("indexValue");
        itemNames = property.FindPropertyRelative("itemNames");
        var item = fieldInfo.GetActualObjectForSerializedProperty<ItemProperty>(property);
        //display popup
        if (item!= null)
        {
            indexValue.intValue = EditorGUI.Popup(position, indexValue.intValue, item.itemNames);
            itemName.stringValue = item.itemNames[indexValue.intValue];
        }
        
        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}