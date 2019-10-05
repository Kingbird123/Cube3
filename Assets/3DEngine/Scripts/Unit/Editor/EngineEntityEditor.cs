using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EngineEntity))]
public class EngineEntityEditor : Editor
{
    protected EngineEntity source;
    protected EngineEntity Source { get { return source; } }
    protected SerializedObject sourceRef;
    //skins
    protected SerializedProperty data;

    protected SerializedProperty spawnUI;
    protected SerializedProperty UIToSpawn;
    protected SerializedProperty parentUIToUnit;

    protected SerializedProperty attackTarget;

    public virtual void OnEnable()
    {
        sourceRef = serializedObject;
        source = (EngineEntity)target;
        GetProperties();
    }

    public override void OnInspectorGUI()
    {

        SetProperties();

        sourceRef.ApplyModifiedProperties();
    }

    protected virtual void GetProperties()
    {
        //data
        data = sourceRef.FindProperty("data");
        //ui
        spawnUI = sourceRef.FindProperty("spawnUI");
        UIToSpawn = sourceRef.FindProperty("UIToSpawn");
        parentUIToUnit = sourceRef.FindProperty("parentUIToUnit");

        attackTarget = sourceRef.FindProperty("attackTarget");
    }

    protected virtual void SetProperties()
    {
        EditorGUILayout.Space();
        DisplayDataProperties<EngineEntityData>();
        DisplayUIProperties();
        DisplayAttackTargetProperties();
    }

    protected virtual void DisplayDataProperties<T>()
    {
        EditorExtensions.LabelFieldCustom("Data Options", FontStyle.Bold);
        data.objectReferenceValue = EditorGUILayout.ObjectField("Data", data.objectReferenceValue, typeof(T), false);
    }

    protected virtual void DisplayUIProperties()
    {
        EditorGUILayout.PropertyField(spawnUI);
        if (spawnUI.enumValueIndex == 1)
        {
            EditorGUILayout.PropertyField(UIToSpawn);
            EditorGUILayout.PropertyField(parentUIToUnit);
        }

    }

    protected virtual void DisplayAttackTargetProperties()
    {
        EditorExtensions.LabelFieldCustom("Attack Target Options", FontStyle.Bold);
        EditorGUILayout.PropertyField(attackTarget);
    }
}
