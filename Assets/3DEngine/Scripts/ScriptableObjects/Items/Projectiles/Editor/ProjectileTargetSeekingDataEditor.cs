using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ProjectileTargetSeekingData))]
public class ProjectileTargetSeekingDataEditor : ProjectileDataEditor
{
    public SerializedProperty keepYFacingUp;

    protected override void SetLinkedType<T>()
    {
        base.SetLinkedType<ProjectileTargetSeeking>();
    }

    protected override void GetProperties()
    {
        base.GetProperties();
        keepYFacingUp = sourceRef.FindProperty("keepYFacingUp");

    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorGUILayout.PropertyField(keepYFacingUp);

    }

}
