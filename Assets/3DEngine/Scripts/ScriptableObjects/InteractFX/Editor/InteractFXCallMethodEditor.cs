using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;

[CustomEditor(typeof(InteractFXCallMethod))]
public class InteractFXCallMethodEditor : InteractFXDynamicEditor
{
    private SerializedProperty behaviour;
    private SerializedProperty method;

    string[] componentNames;
    string[] componentTypes;

    protected override void GetProperties()
    {
        base.GetProperties();
        behaviour = sourceRef.FindProperty("behaviour");
        method = sourceRef.FindProperty("method");

        componentNames = ComponentDatabase.GetAllComponentNames(true);
        componentTypes = ComponentDatabase.GetAllComponentAssemblyNames(true);
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorExtensions.LabelFieldCustom("Call Method Settings", FontStyle.Bold);
        behaviour.IndexStringField(componentNames);
        var indexValue = behaviour.FindPropertyRelative("indexValue");
        var stringValue = behaviour.FindPropertyRelative("stringValue");

        method.MethodPropertyField(System.Type.GetType(componentTypes[indexValue.intValue]));
    }

}
