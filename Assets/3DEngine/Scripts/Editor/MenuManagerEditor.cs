using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CanEditMultipleObjects]
[CustomEditor(typeof(MenuManager))]
public class MenuManagerEditor : Editor
{

    private MenuManager source;
    private SerializedObject sourceRef;

    private SerializedProperty freezeOnPause;
    private SerializedProperty enableCameraSwitching;
    private SerializedProperty cameraSwitch;

    string[] axisNames;
    private void OnEnable()
    {
        source = (MenuManager)target;
        sourceRef = serializedObject;
        axisNames = EditorExtensions.GetInputAxisNames();
        GetProperties();
    }

    public override void OnInspectorGUI()
    {
        SetProperties();

        sourceRef.ApplyModifiedProperties();
    }

    void GetProperties()
    {
        freezeOnPause = sourceRef.FindProperty("freezeOnPause");
        enableCameraSwitching = sourceRef.FindProperty("enableCameraSwitching");
        cameraSwitch = sourceRef.FindProperty("cameraSwitch");
    }

    void SetProperties()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(freezeOnPause);
        EditorGUILayout.PropertyField(enableCameraSwitching);
        if (enableCameraSwitching.boolValue)
        {
            cameraSwitch.CameraSwitcherContainerField(axisNames);
        }
        
    }

    
}
