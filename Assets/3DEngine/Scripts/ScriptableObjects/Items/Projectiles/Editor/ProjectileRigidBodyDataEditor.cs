using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ProjectileRigidbodyData))]
public class ProjectileRigidbodyDataEditor : ProjectileDataEditor
{


    protected override void SetLinkedType<T>()
    {
        base.SetLinkedType<ProjectileRigidbody>();
    }

    protected override void GetProperties()
    {
        base.GetProperties();

    }

    protected override void SetProperties()
    {
        base.SetProperties();
    }

}
