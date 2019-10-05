using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ItemHookShot))]
public class ItemHookShotEditor : Editor
{

    protected SerializedObject sourceRef;
    protected SerializedProperty data;


    public virtual void OnEnable()
    {
        sourceRef = serializedObject;

        GetProperties();
    }

    public override void OnInspectorGUI()
    {

        SetProperties();

        sourceRef.ApplyModifiedProperties();
    }

    public virtual void GetProperties()
    {
        data = sourceRef.FindProperty("data");
    }

    public virtual void SetProperties()
    {
        EditorGUILayout.Space();
        EditorGUILayout.ObjectField(data, typeof(ItemHookShotData));

    }

}
