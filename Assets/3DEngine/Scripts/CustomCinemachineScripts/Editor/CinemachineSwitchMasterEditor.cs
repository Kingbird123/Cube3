using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CanEditMultipleObjects]
[CustomEditor(typeof(CinemachineSwitchMaster))]
public class CinemachineSwitchMasterEditor : Editor
{
    private CinemachineSwitchMaster source;
    private SerializedObject sourceRef;

    private SerializedProperty brain;
    private SerializedProperty cinemachineSwitches;
    private SerializedProperty defaultSwitch;


    private void OnEnable()
    {
        source = (CinemachineSwitchMaster)target;
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
        brain = sourceRef.FindProperty("brain");
        cinemachineSwitches = sourceRef.FindProperty("cinemachineSwitches");
        defaultSwitch = sourceRef.FindProperty("defaultSwitch");
    }

    void SetProperties()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(brain);
        EditorExtensions.LabelFieldCustom("Add/Remove Switches", FontStyle.Bold);
        if (cinemachineSwitches.arraySize > 1)
            defaultSwitch.intValue = EditorGUILayout.Popup("Default", defaultSwitch.intValue, source.GetSwitchNames());
        else
            defaultSwitch.intValue = 0;
        cinemachineSwitches.ArrayFieldButtons("Switch", true, true, true, true, CinemachineSwitchEditorExtensions.CinemachineCameraSwitchField);
    }

    
}
