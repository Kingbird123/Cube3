using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor (typeof (InteractFXSequencer))]
public class InteractFXSequencerEditor : DetectZoneTriggerEditor
{
    private SerializedProperty loops;
    private SerializedProperty finishType;
    private SerializedProperty startType;
    private SerializedProperty startDelay;
    private SerializedProperty destroyDelay;

    private ReorderableList sequenceList;

    protected override void GetProperties()
    {
        base.GetProperties();
        loops = sourceRef.FindProperty("loops");
        finishType = sourceRef.FindProperty("finishType");
        startType = sourceRef.FindProperty("startType");
        startDelay = sourceRef.FindProperty("startDelay");
        destroyDelay = sourceRef.FindProperty("destroyDelay");
    }

    protected override void SetProperties()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(startType);
        if (startType.enumValueIndex == 0 || startType.enumValueIndex == 1)
            base.SetProperties();
        if (startType.enumValueIndex == 2)
            EditorGUILayout.PropertyField(startDelay);

        EditorGUILayout.PropertyField(loops);

        EditorGUILayout.PropertyField(finishType);
        if (finishType.enumValueIndex == 2)
            EditorGUILayout.PropertyField(destroyDelay);
    }
}