using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public static class EngineEventReceiverExtensions
{
    private static SerializedProperty manager;
    private static SerializedProperty broadcastType;
    private static SerializedProperty triggerBroadcastType;
    private static SerializedProperty triggerSingle;
    private static SerializedProperty triggerMask;
    private static SerializedProperty preTrigger;
    private static SerializedProperty preTriggerBroadcastType;
    private static SerializedProperty eventInd;
    private static SerializedProperty eventOption;

    private static SerializedProperty receiverProperty;
    private static int index;

    public static void TriggerEngineReceiverField(this SerializedProperty _receiverProperty, int _index = 0)
    {
        receiverProperty = _receiverProperty;
        index = _index;
        GetProperties();
        SetProperties();
    }

    static void GetProperties()
    {
        manager = receiverProperty.FindPropertyRelative("manager");
        broadcastType = receiverProperty.FindPropertyRelative("broadcastType");
        triggerBroadcastType = receiverProperty.FindPropertyRelative("triggerBroadcastType");
        triggerSingle = receiverProperty.FindPropertyRelative("triggerSingle");
        triggerMask = receiverProperty.FindPropertyRelative("triggerMask");
        preTrigger = receiverProperty.FindPropertyRelative("preTrigger");
        preTriggerBroadcastType = receiverProperty.FindPropertyRelative("preTriggerBroadcastType");
        eventInd = receiverProperty.FindPropertyRelative("eventInd");
        eventOption = receiverProperty.FindPropertyRelative("eventOption");
    }

    static void SetProperties()
    {
        DisplayReceiver();
    }

    static void DisplayReceiver()
    {
        EditorGUILayout.PropertyField(manager);
        if (manager.objectReferenceValue)
        {
            var man = manager.objectReferenceValue as EngineEventTriggerManager;
            if (man)
            {
                //var root = receiverProperty.serializedObject.targetObject;
                //if (root)
                //{
                    //if (root == man)
                    //{
                        //Debug.LogError("Cannot add same manager as a receiver! Resetting to prevent loop errors");
                        //manager.objectReferenceValue = null;
                    //}
                //}

                EditorExtensions.LabelFieldCustom("Broadcast Options", FontStyle.Bold);
                EditorGUILayout.PropertyField(broadcastType);

                if (broadcastType.enumValueIndex == (int)EngineEventReceiver.BroadcastType.Trigger)
                {
                    EditorGUILayout.PropertyField(triggerBroadcastType);
                    if (triggerBroadcastType.enumValueIndex == (int)EngineEventReceiver.TriggerBroadcastType.Single)
                        triggerSingle.IndexStringField(man.GetTriggerNames());
                    if (triggerBroadcastType.enumValueIndex == (int)EngineEventReceiver.TriggerBroadcastType.Mask)
                        triggerMask.intValue = EditorGUILayout.MaskField("Trigger Mask", triggerMask.intValue, man.GetTriggerNames());
                }
                else if (broadcastType.enumValueIndex == (int)EngineEventReceiver.BroadcastType.PreTrigger)
                {
                    preTrigger.IndexStringField(man.GetPreTriggerNames());
                    EditorGUILayout.PropertyField(preTriggerBroadcastType);
                }
                else if (broadcastType.enumValueIndex == (int)EngineEventReceiver.BroadcastType.EventSpecific)
                {
                    triggerSingle.IndexStringField(man.GetTriggerNames());
                    var trigInd = triggerSingle.FindPropertyRelative("indexValue").intValue;
                    eventInd.IndexStringField(man.Triggers[trigInd].GetEventNames());
                    EditorGUILayout.PropertyField(eventOption);
                }

            }
           
        }

    }

}
