using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EngineValueData))]
public class EngineValueDataEditor : Editor
{
    protected SerializedObject sourceRef;
    protected EngineValueData source;
    protected EngineValueData Source { get { return source; } }

    protected virtual void OnEnable()
    {
        source = (EngineValueData)target;
        sourceRef = serializedObject;

        GetProperties();
    }

    public override void OnInspectorGUI()
    {
        SetProperties();
        sourceRef.ApplyModifiedProperties();
    }

    protected virtual void GetProperties()
    {
    }

    protected virtual void SetProperties()
    {
    }

}
