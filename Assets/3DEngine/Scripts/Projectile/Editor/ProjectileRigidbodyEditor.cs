using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ProjectileRigidbody))]
public class ProjectileRigidbodyEditor : ProjectileEditor
{
    protected override void GetProperties()
    {
        base.GetProperties();
    }

    protected override void SetProperties()
    {
        base.SetProperties();
    }

    protected override void DisplayDataProperties()
    {
        EditorGUILayout.ObjectField(data, typeof(ProjectileRigidbodyData));
    }

}
