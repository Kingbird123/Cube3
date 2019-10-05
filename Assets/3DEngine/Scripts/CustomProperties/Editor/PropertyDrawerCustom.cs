using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;

public abstract class PropertyDrawerCustom : PropertyDrawer
{
    protected SerializedProperty sourceRef;

    //need to set field amount manually if you add more fields
    protected int fieldAmount;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        GetProperties(property);
        SetFieldAmount();
        //set the height of the drawer by the field size and padding
        return (EditorGUIUtility.singleLineHeight * fieldAmount) + (EditorGUIUtility.standardVerticalSpacing * fieldAmount);
    }

    protected abstract void SetFieldAmount();

    protected virtual void GetProperties(SerializedProperty _property)
    {
        sourceRef = _property;
    }

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        OnGUICustom(position, property, label);
    }

    protected virtual void OnGUICustom(Rect position, SerializedProperty property, GUIContent label, bool _prefixLabel = false, int _indentLevel = 0)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        //divide all field heights by the field amount..then minus the padding
        position.height /= fieldAmount; position.height -= EditorGUIUtility.standardVerticalSpacing;

        // Draw Prefix label...this will push all other content to the right
        if (_prefixLabel)
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Draw non-indented label instead
        EditorGUI.LabelField(position, label);

        // Set indent amount
        EditorGUI.indentLevel += _indentLevel;

        DisplayGUIElements(position, property);

        // Set indent back to what it was
        EditorGUI.indentLevel -= _indentLevel;

        EditorGUI.EndProperty();
    }

    protected abstract void DisplayGUIElements(Rect position, SerializedProperty prop);

}