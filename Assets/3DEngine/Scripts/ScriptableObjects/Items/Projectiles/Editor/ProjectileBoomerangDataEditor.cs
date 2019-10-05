using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ProjectileBoomerangData))]
public class ProjectileBoomerangDataEditor : ProjectileDataEditor
{

    //health
    public SerializedProperty throwCurve;
    public SerializedProperty returnCurve;
    public SerializedProperty rotateSpeed;
    public SerializedProperty disableRenderersOnSender;

    protected override void SetLinkedType<T>()
    {
        base.SetLinkedType<ProjectileBoomerang>();
    }

    protected override void GetProperties()
    {
        base.GetProperties();
        throwCurve = sourceRef.FindProperty("throwCurve");
        returnCurve = sourceRef.FindProperty("returnCurve");
        rotateSpeed = sourceRef.FindProperty("rotateSpeed");
        disableRenderersOnSender = sourceRef.FindProperty("disableRenderersOnSender");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorGUILayout.PropertyField(throwCurve);
        EditorGUILayout.PropertyField(returnCurve);
        EditorGUILayout.PropertyField(rotateSpeed);
        EditorGUILayout.PropertyField(disableRenderersOnSender);
    }

}
