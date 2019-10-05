using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer (typeof (ChildName))]
public class ChildNamePropertyDrawer : PropertyDrawer
{
    //need to set field amount manually if you add more fields
    private int fieldAmount = 1;
    private float fieldSize = 16;
    private float padding = 2;


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        fieldAmount = 1;
        //set the height of the drawer by the field size and padding
        return (fieldSize * fieldAmount) + (padding * fieldAmount);
    }

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        //EditorGUI.LabelField(position, label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        //get property values
        var overrideParent = property.FindPropertyRelative("overrideParent");
        var overridePropertyName = property.FindPropertyRelative("overridePropertyName");
        var parent = property.FindPropertyRelative("parent");
        var stringValue = property.FindPropertyRelative("stringValue");
        var indexValue = property.FindPropertyRelative("indexValue");
        

        //divide all field heights by the field amount..then minus the padding
        position.height /= fieldAmount; position.height -= padding;

        //get GameObject first
        if (!overrideParent.boolValue)
        {
            fieldAmount++;
            EditorGUI.PropertyField(position, parent);
            //offset position.y by field size
            position.y += fieldSize + padding;
        }
         
        //display popup
        if (parent.objectReferenceValue)
        {
            var go = parent.objectReferenceValue as GameObject;
            //put all child names into array
            Transform[] childs = go.GetComponentsInChildren<Transform>();
            if (childs.Length > 1)
            {
                var childNames = new string[childs.Length];
                for (int i = 1; i < childs.Length; i++)
                {
                    childNames[i] = childs[i].name;
                }
                //display popup
                indexValue.intValue = EditorGUI.Popup(position, indexValue.intValue, childNames);
                if (indexValue.intValue < childNames.Length)
                    stringValue.stringValue = childNames[indexValue.intValue];
            }
            else
                EditorGUI.LabelField(position, "PARENT TRANSFORM NEEDS TO HAVE CHILDREN TRANSFORMS");

        }
        else if (overrideParent.boolValue)
            EditorGUI.LabelField(position, "Must have " + overridePropertyName.stringValue + "!");
        else
            EditorGUI.LabelField(position,"NEED PREFAB FOR REFERENCE");

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}