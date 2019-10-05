using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(InteractFXEngineFloat))]
public class InteractFXEngineFloatEditor : InteractFXDynamicEditor
{
    protected new InteractFXEngineFloat Source { get { return (InteractFXEngineFloat)source; } }

    protected SerializedProperty valueManager;
    protected SerializedProperty valueSelection;
    protected SerializedProperty valueDelta;

    protected override void GetProperties()
    {
        base.GetProperties();
        valueManager = sourceRef.FindProperty("valueManager");
        valueSelection = sourceRef.FindProperty("valueSelection");
        valueDelta = sourceRef.FindProperty("valueDelta");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Value Select Options", FontStyle.Bold);
        EditorGUILayout.PropertyField(valueManager);
        if (valueManager.objectReferenceValue)
        {
            valueSelection.EngineValueSelectionField(valueManager);
            EditorGUILayout.PropertyField(valueDelta);
        }
        EditorGUILayout.Space();
    }
}