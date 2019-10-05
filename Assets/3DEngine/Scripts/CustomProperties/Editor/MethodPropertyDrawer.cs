using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;

[CustomPropertyDrawer(typeof(MethodProperty))]
public class MethodPropertyDrawer : PropertyDrawerCustom
{

    protected override void SetFieldAmount()
    {

    }

    protected override void GetProperties(SerializedProperty _property)
    {
        base.GetProperties(_property);
    }

    protected override void OnGUICustom(Rect position, SerializedProperty property, GUIContent label, bool _prefixLabel = false, int _indentLevel = 0)
    {
        base.OnGUICustom(position, property, label, _prefixLabel, _indentLevel);
    }

    protected override void DisplayGUIElements(Rect position, SerializedProperty prop)
    {
        prop.MethodPropertyField(position, 0, out position, out fieldAmount);
    }

}