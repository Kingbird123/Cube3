using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;

[CustomEditor(typeof(InteractFXEnableBehaviour))]
public class InteractFXEnableBehaviourEditor : InteractFXDynamicEditor
{
    private SerializedProperty behaviour;
    private SerializedProperty enable;

    string[] componentNames;

    protected override void GetProperties()
    {
        base.GetProperties();
        behaviour = sourceRef.FindProperty("behaviour");
        enable = sourceRef.FindProperty("enable");

        componentNames = ComponentDatabase.GetAllComponentNames();
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorExtensions.LabelFieldCustom("Behaviour Settings", FontStyle.Bold);
        behaviour.IndexStringField(componentNames);
        EditorGUILayout.PropertyField(enable);
    }

}
