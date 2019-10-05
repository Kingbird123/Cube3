using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Web;
using System;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : EngineEntityDataEditor
{
    protected new ItemData Source { get { return (ItemData)source; } }
    protected SerializedProperty connectedPrefab;
    protected SerializedProperty keepUIActiveIfDropped;
    protected SerializedProperty weight;
    protected SerializedProperty droppable;
    protected SerializedProperty droppedPrefab;
    protected SerializedProperty quickMenuCompatible;
    protected SerializedProperty buffs;
    protected SerializedProperty locationData;
    protected SerializedProperty defaultSpawnLocation;

    protected string[] spawnLocationNames;

    protected override void OnEnable()
    {
        base.OnEnable();
        source = (ItemData)target;
        SetLinkedType<Item>();
    }

    protected virtual void SetLinkedType<T>()
    {
        Source.linkedType = typeof(T);
    }

    protected override void GetProperties()
    {
        base.GetProperties();
        keepUIActiveIfDropped = sourceRef.FindProperty("keepUIActiveIfDropped");
        connectedPrefab = sourceRef.FindProperty("connectedPrefab");
        weight = sourceRef.FindProperty("weight");
        droppable = sourceRef.FindProperty("droppable");
        droppedPrefab = sourceRef.FindProperty("droppedPrefab");
        quickMenuCompatible = sourceRef.FindProperty("quickMenuCompatible");
        buffs = sourceRef.FindProperty("buffs");
        locationData = sourceRef.FindProperty("locationData");
        defaultSpawnLocation = sourceRef.FindProperty("defaultSpawnLocation");
    }

    protected override void SetProperties()
    {
        base.SetProperties();

        EditorExtensions.LabelFieldCustom("Item Properties", FontStyle.Bold);
        EditorGUILayout.PropertyField(connectedPrefab);
        EditorGUILayout.PropertyField(weight);
        EditorGUILayout.PropertyField(droppable);
        if (droppable.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(droppedPrefab);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.PropertyField(quickMenuCompatible);

        EditorGUILayout.PropertyField(locationData);
        if (locationData.objectReferenceValue)
        {
            var locData = locationData.objectReferenceValue as ItemLocationData;
            spawnLocationNames = locData.GetItemLocationNames();
            defaultSpawnLocation.IndexStringPropertyField(locData.GetItemLocationNames());
        }
        else
            EditorExtensions.LabelFieldCustom("Need location data to spawn item on unit", FontStyle.Bold, Color.red);

        EditorGUILayout.PropertyField(buffs, true);
        CheckModelPrefabLink();
    }

    protected override void DisplayUIProperties()
    {
        base.DisplayUIProperties();
        if (spawnUI.boolValue)
            EditorGUILayout.PropertyField(keepUIActiveIfDropped);   
    }

    protected virtual void CheckModelPrefabLink()
    {
        if (!connectedPrefab.objectReferenceValue)
            return;
        var go = connectedPrefab.objectReferenceValue as GameObject;
        if (go)
        {
            var comp = go.GetComponent(Source.linkedType);
            if (comp)
            {
                var itemComp = go.GetComponent<Item>();
                if (itemComp.Data != source)
                {
                    if (GUILayout.Button("Add " + source.name + " data to " + itemComp.name))
                    {
                        itemComp.SetData(source);
                        Undo.RecordObject(go, "Linked data file");
                    }
                }
            }

        }
    }

}
