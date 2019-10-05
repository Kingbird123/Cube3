using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;


public static class CameraSwitcherExtensions
{
    static string[] axisNames;

    public static void CameraSwitcherContainerField(this SerializedProperty _cameraSwitcherContainerProperty, string[]_axisNames)
    {
        axisNames = _axisNames;
        var cameraSwitchers = _cameraSwitcherContainerProperty.FindPropertyRelative("cameraSwitchers");
        cameraSwitchers.ArrayFieldButtons("Camera Switch", true, true, true, true, CameraSwitcherField);
    }

    static void CameraSwitcherField(SerializedProperty _property, int _ind)
    {
        var enterCameraManager = _property.FindPropertyRelative("enterCameraManager");
        var enterCameraInd = _property.FindPropertyRelative("enterCameraInd");
        var exitCameraManager = _property.FindPropertyRelative("exitCameraManager");
        var exitCameraInd = _property.FindPropertyRelative("exitCameraInd");
        var ignorePriorityOnExit = _property.FindPropertyRelative("ignorePriorityOnExit");
        var switchButton = _property.FindPropertyRelative("switchButton");
        var switchType = _property.FindPropertyRelative("switchType");
        var activateEvents = _property.FindPropertyRelative("activateEvents");
        var deactivateEvents = _property.FindPropertyRelative("deactivateEvents");

        EditorGUILayout.PropertyField(enterCameraManager);
        var enterManager = enterCameraManager.GetRootValue<VirtualCameraManager>();
        if (enterManager)
        {
            enterCameraInd.intValue = EditorGUILayout.Popup("Enter Camera", enterCameraInd.intValue, enterManager.GetVirtualCameraNames());
            switchButton.InputPropertyField(axisNames);
            EditorGUILayout.PropertyField(switchType);
            if (switchType.enumValueIndex != (int)CameraSwitcher.SwitchType.Single)
            {
                EditorGUILayout.PropertyField(exitCameraManager);
                var man = exitCameraManager.GetRootValue<VirtualCameraManager>();
                if (man)
                {
                    exitCameraInd.intValue = EditorGUILayout.Popup("Exit Camera", exitCameraInd.intValue, man.GetVirtualCameraNames());
                EditorGUILayout.PropertyField(ignorePriorityOnExit);
                    activateEvents.ArrayFieldButtons("Activate Events", true, true, true, true, EngineEventExtensions.EngineEventField);
                    deactivateEvents.ArrayFieldButtons("Deactivate Events", true, true, true, true, EngineEventExtensions.EngineEventField);
                }
                
            }
            
        }          
    }
}
