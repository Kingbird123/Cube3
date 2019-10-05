using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitData))]
public class UnitDataEditor : EngineEntityDataEditor
{
    //skin
    protected SerializedProperty setSkin;
    protected SerializedProperty skinPrefab;
    protected SerializedProperty skinSize;
    protected SerializedProperty skinRotation;

    //spawn locations
    protected SerializedProperty locationData;
    protected SerializedProperty itemSpawnLocations;
    
    //rigidbody
    protected SerializedProperty weight;

    //movement
    protected SerializedProperty speed;
    protected SerializedProperty runSpeed;
    protected SerializedProperty backwardSpeed;
    protected SerializedProperty climbingSpeed;
    protected SerializedProperty jumpPower;

    //buffs
    protected SerializedProperty buffs;


    protected override void GetProperties()
    {
        base.GetProperties();
        //skin
        setSkin = sourceRef.FindProperty("setSkin");
        skinPrefab = sourceRef.FindProperty("skinPrefab");
        skinSize = sourceRef.FindProperty("skinSize");
        skinRotation = sourceRef.FindProperty("skinRotation");

        //spawnlocations
        locationData = sourceRef.FindProperty("locationData");
        itemSpawnLocations = sourceRef.FindProperty("itemSpawnLocations");

        //rigidbody
        weight = sourceRef.FindProperty("weight");
        //movement
        speed = sourceRef.FindProperty("speed");
        runSpeed = sourceRef.FindProperty("runSpeed");
        backwardSpeed = sourceRef.FindProperty("backwardSpeed");
        climbingSpeed = sourceRef.FindProperty("climbingSpeed");
        jumpPower = sourceRef.FindProperty("jumpPower");

        //buffs
        buffs = sourceRef.FindProperty("buffs");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        DisplaySkinProperties();
        DisplaySpawnProperties();
        DisplayRigidbodyProperties();
        DisplayMovementProperties();
        DisplayBuffProperties();
    }

    protected virtual void DisplaySkinProperties()
    {
        EditorGUILayout.LabelField("Skin Properties", boldStyle);
        EditorGUILayout.PropertyField(setSkin);
        if (setSkin.boolValue)
        {
            EditorGUILayout.PropertyField(skinPrefab);
            EditorGUILayout.PropertyField(skinSize);
            EditorGUILayout.PropertyField(skinRotation);
        }  
    }

    protected virtual void DisplaySpawnProperties()
    {
        if (setSkin.boolValue)
        {
            EditorGUILayout.LabelField("Spawn Properties", boldStyle);
            EditorGUILayout.PropertyField(locationData);
            var locData = locationData.GetRootValue<ItemLocationData>();
            if (locData)
            {
                var names = locData.GetItemLocationNames();
                itemSpawnLocations.arraySize = names.Length;
                for (int i = 0; i < names.Length; i++)
                {
                    var ele = itemSpawnLocations.GetArrayElementAtIndex(i);
                    var overrideParent = ele.FindPropertyRelative("overrideParent");
                    var overridePropertyName = ele.FindPropertyRelative("overridePropertyName");
                    var parent = ele.FindPropertyRelative("parent");

                    overrideParent.boolValue = true;
                    parent.objectReferenceValue = skinPrefab.objectReferenceValue;
                    overridePropertyName.stringValue = names[i];
                    EditorGUILayout.PropertyField(ele);

                }
            }
            else
                EditorExtensions.LabelFieldCustom("Need location data to spawn items on unit", FontStyle.Bold, Color.red);
        }
    }

    protected virtual void DisplayRigidbodyProperties()
    {
        EditorGUILayout.LabelField("Rigidbody Properties", boldStyle);
        EditorGUILayout.PropertyField(weight);
    }

    protected virtual void DisplayMovementProperties()
    {
        EditorGUILayout.LabelField("Movement Properties", boldStyle);
        EditorGUILayout.PropertyField(speed);
        EditorGUILayout.PropertyField(runSpeed);
        EditorGUILayout.PropertyField(backwardSpeed);
        EditorGUILayout.PropertyField(climbingSpeed);
        EditorGUILayout.PropertyField(jumpPower);
    }

    protected virtual void DisplayBuffProperties()
    {
        EditorGUILayout.LabelField("Buff Properties", boldStyle);
        EditorGUILayout.PropertyField(buffs, true);
    }
}
