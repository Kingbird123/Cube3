using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor (typeof (InteractFXDynamic))]
public class InteractFXDynamicEditor : InteractFXEditor
{
    protected new InteractFXDynamic Source { get { return (InteractFXDynamic)source; }}

    protected SerializedProperty affectedType;

    protected override void GetProperties()
    {
        base.GetProperties();
        affectedType = sourceRef.FindProperty("affectedType");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Affected Object Options", FontStyle.Bold);
        EditorGUILayout.PropertyField(affectedType);
        EditorGUILayout.Space();
    }
}