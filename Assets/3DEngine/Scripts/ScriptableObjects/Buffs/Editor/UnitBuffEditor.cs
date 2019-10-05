using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UnitBuffEditor : Editor
{
    protected SerializedObject sourceRef;
    protected UnitBuff source;
    protected virtual UnitBuff Source { get { return source; } set { source = value; } }

    protected SerializedProperty delay;

    protected virtual void OnEnable()
    {
        source = (UnitBuff)target;
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
