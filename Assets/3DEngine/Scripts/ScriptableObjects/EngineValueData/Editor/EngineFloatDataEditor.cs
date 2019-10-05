using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EngineFloatData))]
public class EngineFloatDataEditor : EngineValueDataEditor
{
    protected new EngineFloatData Source { get { return (EngineFloatData)source; } }

    protected SerializedProperty floatValue;
    protected SerializedProperty minValue;
    protected SerializedProperty maxValue;

    public override void OnInspectorGUI()
    {
        SetProperties();
        sourceRef.ApplyModifiedProperties();
    }

    protected override void GetProperties()
    {
        base.GetProperties();
        floatValue = sourceRef.FindProperty("floatValue");
        minValue = sourceRef.FindProperty("minValue");
        maxValue = sourceRef.FindProperty("maxValue");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        floatValue.FloatFieldClamp(minValue.floatValue, maxValue.floatValue);
        EditorGUILayout.PropertyField(minValue);
        EditorGUILayout.PropertyField(maxValue);
    }

}
