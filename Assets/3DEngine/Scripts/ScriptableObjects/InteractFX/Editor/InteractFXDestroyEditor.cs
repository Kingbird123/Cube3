using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InteractFXDestroy))]
public class InteractFXDestroyEditor : InteractFXDynamicEditor
{
    private SerializedProperty destroyRoot;

    protected override void GetProperties()
    {
        base.GetProperties();
        destroyRoot = sourceRef.FindProperty("destroyRoot");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorExtensions.LabelFieldCustom("Destroy Settings", FontStyle.Bold);
        EditorGUILayout.PropertyField(destroyRoot);
    }

}
