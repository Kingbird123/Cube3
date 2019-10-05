using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ItemBuildable))]
public class ItemBuildableEditor : Editor
{

    protected SerializedObject sourceRef;
    protected SerializedProperty data;
    protected SerializedProperty rotateButton;
    protected SerializedProperty placeButton;
    protected SerializedProperty toggleIndAdd;
    protected SerializedProperty toggleIndSubract;


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
        rotateButton = sourceRef.FindProperty("rotateButton");
        placeButton = sourceRef.FindProperty("placeButton");
        toggleIndAdd = sourceRef.FindProperty("toggleIndAdd");
        toggleIndSubract = sourceRef.FindProperty("toggleIndSubract");
    }

    public virtual void SetProperties()
    {
        EditorGUILayout.Space();
        EditorGUILayout.ObjectField(data, typeof(ItemBuildableData));
        EditorGUILayout.PropertyField(rotateButton);
        EditorGUILayout.PropertyField(placeButton);
        EditorGUILayout.PropertyField(toggleIndAdd);
        EditorGUILayout.PropertyField(toggleIndSubract);

    }

}
