using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (DetectZoneTrigger))]
public class DetectZoneTriggerEditor : Editor
{
    protected DetectZoneTrigger source;
    protected SerializedObject sourceRef;

    protected SerializedProperty detectZone;
    protected GUIStyle boldStyle;

    public virtual void OnEnable ()
    {
        source = (DetectZoneTrigger)target;
        sourceRef = serializedObject;
        SetupGUIStyle();
        GetProperties ();
    }

    void SetupGUIStyle()
    {
        boldStyle = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
        };
    }

    public override void OnInspectorGUI ()
    {
        SetProperties ();

        sourceRef.ApplyModifiedProperties ();
    }

    protected virtual void GetProperties ()
    {
        detectZone = sourceRef.FindProperty ("detectZone");
    }

    protected virtual void SetProperties ()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField (detectZone);

    }

    protected virtual void OnSceneGUI()
    {
        var detect = detectZone.GetRootValue<DetectZone>();
        if (detect != null)
            detect.DrawDetectZone(source, sourceRef, source.transform);
    }

}