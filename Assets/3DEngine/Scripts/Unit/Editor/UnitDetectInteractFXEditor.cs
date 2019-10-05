using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (UnitDetectInteractFX))]
public class UnitDetectInteractFXEditor : DetectZoneTriggerEditor
{
    protected SerializedProperty interacts;

    protected override void GetProperties()
    {
        base.GetProperties();
        interacts = sourceRef.FindProperty("interacts");
    }

    protected override void SetProperties ()
    {
        base.SetProperties();
        EditorGUILayout.PropertyField(interacts, true);

    }

}