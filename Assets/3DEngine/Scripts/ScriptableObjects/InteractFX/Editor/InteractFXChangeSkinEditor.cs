using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InteractFXChangeSkin))]
public class InteractFXChangeSkinEditor : Editor
{
    protected SerializedObject sourceRef;

    private SerializedProperty needPrerequisiteSkin;
    private SerializedProperty prerequisiteSkin;
    private SerializedProperty particleFX;
    private SerializedProperty copyLocalUnitValues;
    private SerializedProperty delay;
    private SerializedProperty skinData;
    private SerializedProperty freezeReciever;
    private SerializedProperty doAnimations;
    private SerializedProperty crossFadeTime;
    private SerializedProperty senderAnimToPlay;
    private SerializedProperty receiverAnimToPlay;
    private SerializedProperty spawnObjectAfterChange;
    private SerializedProperty spawnObjects;
    private SerializedProperty destroySenderAfterChange;
    private SerializedProperty destroyRoot;
    private SerializedProperty destroyDelay;

    void OnEnable()
    {
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
        needPrerequisiteSkin = sourceRef.FindProperty("needPrerequisiteSkin");
        prerequisiteSkin = sourceRef.FindProperty("prerequisiteSkin");
        copyLocalUnitValues = sourceRef.FindProperty("copyLocalUnitValues");
        delay = sourceRef.FindProperty("delay");
        skinData = sourceRef.FindProperty("skinData");
        freezeReciever = sourceRef.FindProperty("freezeReciever");
        doAnimations = sourceRef.FindProperty("doAnimations");
        crossFadeTime = sourceRef.FindProperty("crossFadeTime");
        senderAnimToPlay = sourceRef.FindProperty("senderAnimToPlay");
        receiverAnimToPlay = sourceRef.FindProperty("receiverAnimToPlay");
        spawnObjectAfterChange = sourceRef.FindProperty("spawnObjectAfterChange");
        spawnObjects = sourceRef.FindProperty("spawnObjects");
        destroySenderAfterChange = sourceRef.FindProperty("destroySenderAfterChange");
        destroyRoot = sourceRef.FindProperty("destroyRoot");
        destroyDelay = sourceRef.FindProperty("destroyDelay");
    }

    void SetProperties()
    {
        EditorGUILayout.PropertyField(needPrerequisiteSkin);
        if (needPrerequisiteSkin.boolValue)
            EditorGUILayout.PropertyField(prerequisiteSkin);
        EditorGUILayout.PropertyField(skinData);
        EditorGUILayout.PropertyField(copyLocalUnitValues);
        EditorGUILayout.PropertyField(freezeReciever);
        EditorGUILayout.PropertyField(doAnimations);
        if (doAnimations.boolValue)
        {
            EditorGUILayout.PropertyField(senderAnimToPlay);
            EditorGUILayout.PropertyField(receiverAnimToPlay);
        }
        EditorGUILayout.PropertyField(crossFadeTime);
        EditorGUILayout.PropertyField(delay);
        EditorGUILayout.PropertyField(spawnObjectAfterChange);
        if (spawnObjectAfterChange.boolValue)
            EditorGUILayout.PropertyField(spawnObjects, true);
        EditorGUILayout.PropertyField(destroySenderAfterChange);
        if (destroySenderAfterChange.boolValue)
        {
            EditorGUILayout.PropertyField(destroyRoot);
            EditorGUILayout.PropertyField(destroyDelay);
        }
            
    }

}
