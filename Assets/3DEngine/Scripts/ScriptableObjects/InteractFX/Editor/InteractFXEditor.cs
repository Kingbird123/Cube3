using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor (typeof (InteractFX))]
public class InteractFXEditor : Editor
{
    protected SerializedObject sourceRef;
    protected InteractFX source;
    protected virtual InteractFX Source { get { return source; } set { source = value; } }

    protected SerializedProperty delay;

    protected virtual void OnEnable()
    {
        source = (InteractFXDynamic)target;
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
        delay = sourceRef.FindProperty("delay");
    }

    protected virtual void SetProperties()
    {
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Initialize Options", FontStyle.Bold);
        EditorGUILayout.PropertyField(delay);
        EditorGUILayout.Space();
    }
}