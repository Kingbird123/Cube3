using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;

[CustomEditor(typeof(ProjectileData))]
public class ProjectileDataEditor : ItemDataEditor
{
    protected SerializedProperty launchFX;
    protected SerializedProperty hitMaxAmount;
    protected SerializedProperty steerable;
    protected SerializedProperty horSpeed;
    protected SerializedProperty verSpeed;  
    protected SerializedProperty destroySelfType;
    protected SerializedProperty impactType;
    protected SerializedProperty time;
    protected SerializedProperty triggerType;
    protected SerializedProperty velocity;
    protected SerializedProperty spawnOnImpact;
    protected SerializedProperty impactFX;
    

    protected override void GetProperties()
    {
        base.GetProperties();
        hitMaxAmount = sourceRef.FindProperty("hitMaxAmount");
        steerable = sourceRef.FindProperty("steerable");
        horSpeed = sourceRef.FindProperty("horSpeed");
        verSpeed = sourceRef.FindProperty("verSpeed");
        destroySelfType = sourceRef.FindProperty("destroySelfType");
        impactType = sourceRef.FindProperty("impactType");
        time = sourceRef.FindProperty("time");
        triggerType = sourceRef.FindProperty("triggerType");
        velocity = sourceRef.FindProperty("velocity");
        spawnOnImpact = sourceRef.FindProperty("spawnOnImpact");
        impactFX = sourceRef.FindProperty("impactFX");
        launchFX = sourceRef.FindProperty("launchFX");
    }

    protected override void SetProperties()
    {
        base.SetProperties();

        EditorGUILayout.PropertyField(launchFX, true);
        EditorGUILayout.PropertyField(steerable);
        if (steerable.boolValue)
        {
            EditorGUILayout.PropertyField(horSpeed);
            EditorGUILayout.PropertyField(verSpeed);
        }
        EditorGUILayout.PropertyField(destroySelfType);
        if (destroySelfType.enumValueIndex == 1)
            EditorGUILayout.PropertyField(hitMaxAmount);
        EditorGUILayout.PropertyField(impactType);
        if (impactType.enumValueIndex == 1)
            EditorGUILayout.PropertyField(time);
        else if (impactType.enumValueIndex == 2)
        {
            EditorGUILayout.PropertyField(triggerType);
            EditorGUILayout.PropertyField(velocity);
        }    
        EditorGUILayout.PropertyField(impactFX, true);
        EditorGUILayout.PropertyField(spawnOnImpact, true);
    }

}
