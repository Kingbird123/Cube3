using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabSpawner))]
public class PrefabSpawnerEditor : Editor
{

    private SerializedObject sourceRef;
    private PrefabSpawner source;

    private SerializedProperty prefabsToSpawn;


    private void OnEnable()
    {
        source = (PrefabSpawner)target;
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
        prefabsToSpawn = sourceRef.FindProperty("prefabsToSpawn");
    }

    void SetProperties()
    {

        EditorGUILayout.PropertyField(prefabsToSpawn, true);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Spawn Prefabs In Scene"))
        {
            SpawnPrefabs();
        }
        EditorGUILayout.EndHorizontal();
    }

    public void SpawnPrefabs()
    {

        for (int i = 0; i < prefabsToSpawn.arraySize; i++)
        {
            var obj = prefabsToSpawn.GetArrayElementAtIndex(i).objectReferenceValue;
            var spawn = PrefabUtility.InstantiatePrefab(obj);
            source.spawnedPrefab = (GameObject)spawn;
        }
    }


}
