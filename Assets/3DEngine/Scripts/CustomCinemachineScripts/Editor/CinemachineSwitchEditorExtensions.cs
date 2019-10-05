using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public static class CinemachineSwitchEditorExtensions
{

    private static SerializedProperty sourceRef;

    private static SerializedProperty switchName;
    private static SerializedProperty virtualCameraManager;
    private static SerializedProperty switchPriority;
    private static SerializedProperty cameraSpawnLocation;
    private static SerializedProperty activeCamera;
    static int index;
    public static void CinemachineCameraSwitchField(this SerializedProperty _property, int _ind)
    {
        sourceRef = _property;
        index = _ind;
        GetProperties();
        SetProperties();
    }

    static void GetProperties()
    {
        switchName = sourceRef.FindPropertyRelative("switchName");
        virtualCameraManager = sourceRef.FindPropertyRelative("virtualCameraManager");
        switchPriority = sourceRef.FindPropertyRelative("switchPriority");
        cameraSpawnLocation = sourceRef.FindPropertyRelative("cameraSpawnLocation");
        activeCamera = sourceRef.FindPropertyRelative("activeCamera");
    }

    static void SetProperties()
    {
        EditorGUILayout.PropertyField(virtualCameraManager);
        var manager = virtualCameraManager.GetRootValue<VirtualCameraManager>();
        if (manager)
        {
            switchName.stringValue = index + ": " + manager.name;
            EditorGUILayout.PropertyField(switchPriority);
            EditorGUILayout.PropertyField(cameraSpawnLocation);
            activeCamera.intValue = EditorGUILayout.Popup("Active Camera", activeCamera.intValue, manager.GetVirtualCameraNames());
        }
            
    }

    

    
}
