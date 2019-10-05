using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaveSpawnerSequence))]
public class WaveSpawnerSequenceEditor : Editor
{
    protected WaveSpawnerSequence source;
    protected SerializedObject sourceRef;

    protected SerializedProperty viewStats;
    protected SerializedProperty spawnables;
    protected SerializedProperty spawnLocations;
    protected SerializedProperty waves;

    public virtual void OnEnable()
    {
        source = (WaveSpawnerSequence)target;
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
        viewStats = sourceRef.FindProperty("viewStats");
        spawnables = sourceRef.FindProperty("spawnables");
        spawnLocations = sourceRef.FindProperty("spawnLocations");
        waves = sourceRef.FindProperty("waves");
    }

    public virtual void SetProperties()
    {
        EditorGUILayout.Space();
        if (Application.isPlaying)
        {
            EditorGUILayout.PropertyField(viewStats);
            if (viewStats.boolValue)
                DisplayStats();
        }
        
        spawnables.ArrayFieldButtons("Spawn", false, false, true, false, DisplaySpawnables);
        spawnLocations.ArrayFieldButtons("Location", true, false, true, true, DisplaySpawnLocations);
        EditorGUILayout.LabelField("--------------");
        waves.ArrayFieldButtons("Wave", false, false, false, false, DisplayWaves);
    }

    void DisplayStats()
    {
        EditorGUILayout.LabelField("--------------");
        EditorGUILayout.LabelField("Wave Index: " + source.CurWaveInd);
        EditorGUILayout.LabelField("Wave Time: " + source.CurWaveTime);
        EditorGUILayout.LabelField("Wave Timer: " + source.CurWaveTimer);
        EditorGUILayout.LabelField("Amount Spawned: " + source.CurWaveSpawnAmount);
        EditorGUILayout.LabelField("Spawn Total: " + source.CurWaveSpawnTotal);
        EditorGUILayout.LabelField("Spawn Tally: " + source.CurWaveSpawnTotalTally);
        EditorGUILayout.LabelField("--------------");
        Repaint();
    }

    void DisplaySpawnables(SerializedProperty _property, int _index)
    {
        var spawnableName = _property.FindPropertyRelative("spawnableName");
        var spawnablePrefab = _property.FindPropertyRelative("spawnablePrefab");

        spawnablePrefab.objectReferenceValue = EditorGUILayout.ObjectField(spawnablePrefab.objectReferenceValue, typeof(GameObject), false);
        if (spawnablePrefab.objectReferenceValue)
            spawnableName.stringValue = spawnablePrefab.objectReferenceValue.name;

    }

    void DisplaySpawnLocations(SerializedProperty _property, int _index)
    {
        var locationName = _property.FindPropertyRelative("locationName");
        var transLocation = _property.FindPropertyRelative("transLocation");
        var position = _property.FindPropertyRelative("position");
        var euler = _property.FindPropertyRelative("euler");

        EditorGUILayout.PropertyField(locationName);
        EditorGUILayout.PropertyField(transLocation);
        if (!transLocation.objectReferenceValue)
        {
            EditorGUILayout.PropertyField(position);
            EditorGUILayout.PropertyField(euler);
        }
    }

    void DisplayWaves(SerializedProperty _property, int _index)
    {
        var waveName = _property.FindPropertyRelative("waveName");
        var delay = _property.FindPropertyRelative("delay");
        var finishType = _property.FindPropertyRelative("finishType");
        var waveTime = _property.FindPropertyRelative("waveTime");
        var compareType = _property.FindPropertyRelative("compareType");
        var compareValue = _property.FindPropertyRelative("compareValue");
        var spawners = _property.FindPropertyRelative("spawners");

        EditorGUILayout.PropertyField(waveName);
        EditorGUILayout.PropertyField(delay);
        EditorGUILayout.PropertyField(finishType);
        if (finishType.enumValueIndex == 2)
        EditorGUILayout.PropertyField(waveTime);
        if (finishType.enumValueIndex == 1)
        {
            EditorGUILayout.PropertyField(compareType);
            EditorGUILayout.PropertyField(compareValue);
        }
        EditorExtensions.ArrayFieldButtons(spawners, "Spawner", true, false, true, true, DisplaySpawners);
        EditorGUILayout.LabelField("--------------");

    }

    void DisplaySpawners(SerializedProperty _property, int _index)
    {
        var name = _property.FindPropertyRelative("name");
        var spawnable = _property.FindPropertyRelative("spawnable");
        var spawnPos = _property.FindPropertyRelative("spawnPos");
        var delay = _property.FindPropertyRelative("delay");
        var spawnAmount = _property.FindPropertyRelative("spawnAmount");
        var spawnDelay = _property.FindPropertyRelative("spawnDelay");
        var waitForPreviousToFinish = _property.FindPropertyRelative("waitForPreviousToFinish");

        EditorExtensions.IndexStringField(spawnable, EditorExtensions.GetDisplayNames(spawnables));
        var spawnName = spawnable.FindPropertyRelative("stringValue");
        name.stringValue = spawnName.stringValue;
        EditorExtensions.IndexStringField(spawnPos, EditorExtensions.GetDisplayNames(spawnLocations));
        EditorGUILayout.PropertyField(delay);
        EditorGUILayout.PropertyField(spawnAmount);
        EditorGUILayout.PropertyField(spawnDelay);
        EditorGUILayout.PropertyField(waitForPreviousToFinish);

    }
}

