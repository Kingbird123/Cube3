using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using System;
using System.Linq;

[CustomEditor(typeof(SpawnPoolManager))]
public class SpawnPoolManagerEditor : Editor
{
    private SerializedObject sourceRef;
    private SpawnPoolManager source;

    private SerializedProperty pools;

    private void OnEnable()
    {
        source = (SpawnPoolManager)target;
        sourceRef = serializedObject;

        GetProperties();
    }

    public override void OnInspectorGUI()
    {
        SetProperties();
        sourceRef.ApplyModifiedProperties();
    }

    void GetProperties()
    { 
        pools = sourceRef.FindProperty("pools");
    }

    void SetProperties()
    {
        pools.ArrayFieldButtons("Pool Object", true, true, true, true, DisplayPoolList);   
    }

    void DisplayPoolList(SerializedProperty _property, int _ind)
    {
        var prefabToSpawn = _property.FindPropertyRelative("prefabToSpawn");
        var amountToSpawn = _property.FindPropertyRelative("amountToSpawn");
        var createMoreIfEmpty = _property.FindPropertyRelative("createMoreIfEmpty");
        var spawnOldest = _property.FindPropertyRelative("spawnOldest");

        EditorGUILayout.PropertyField(prefabToSpawn);
        EditorGUILayout.PropertyField(amountToSpawn);
        EditorGUILayout.PropertyField(createMoreIfEmpty);
        EditorGUILayout.PropertyField(spawnOldest);
    }
}
