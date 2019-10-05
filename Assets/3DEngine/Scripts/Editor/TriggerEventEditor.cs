using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TriggerEvent))]
public class TriggerEventEditor : Editor
{
    private TriggerEvent source;
    private SerializedObject sourceRef;
    //masks
    private SerializedProperty mask;
    private SerializedProperty triggerTag;
    //button
    private SerializedProperty buttonDownToTrigger;
    private SerializedProperty button;
    //delays
    private SerializedProperty delayTriggerTime;
    private SerializedProperty useRepeatDelay;
    private SerializedProperty repeatDelay;
    //events
    private SerializedProperty useUnityEvents;
    private SerializedProperty triggerEnterEvents;
    private SerializedProperty triggerExitEvents;
    private SerializedProperty triggerStayEvents;
    //interacts
    private SerializedProperty triggerEnterInteracts;
    private SerializedProperty triggerExitInteracts;
    private SerializedProperty triggerStayInteracts;

    private void OnEnable()
    {
        source = (TriggerEvent)target;
        sourceRef = serializedObject;

        buttonDownToTrigger = sourceRef.FindProperty("buttonDownToTrigger");
        button = sourceRef.FindProperty("button");

        mask = sourceRef.FindProperty("mask");
        triggerTag = sourceRef.FindProperty("triggerTag");
        delayTriggerTime = sourceRef.FindProperty("delayTriggerTime");
        useRepeatDelay = sourceRef.FindProperty("useRepeatDelay");
        repeatDelay = sourceRef.FindProperty("repeatDelay");
        //events
        useUnityEvents = sourceRef.FindProperty("useUnityEvents");
        triggerEnterEvents = sourceRef.FindProperty("triggerEnterEvents");
        triggerExitEvents = sourceRef.FindProperty("triggerExitEvents");
        triggerStayEvents = sourceRef.FindProperty("triggerStayEvents");
        //interacts
        triggerEnterInteracts = sourceRef.FindProperty("triggerEnterInteracts");
        triggerExitInteracts = sourceRef.FindProperty("triggerExitInteracts");
        triggerStayInteracts = sourceRef.FindProperty("triggerStayInteracts");
    }

    public override void OnInspectorGUI()
    {
        triggerTag.stringValue = EditorGUILayout.TagField("Trigger2D Tag", triggerTag.stringValue);
        mask.intValue = EditorGUILayout.MaskField("Trigger2D Types", mask.intValue, source.maskOptions);

        EditorGUILayout.PropertyField(delayTriggerTime);
        int i = mask.intValue;

        if (i == 1 | i == 3 | i == 5 | i == -1)
        {
            EditorGUILayout.PropertyField(useUnityEvents);
            
            if (useUnityEvents.boolValue)
                EditorGUILayout.PropertyField(triggerEnterEvents);

                EditorGUILayout.PropertyField(triggerEnterInteracts, true);
        }


        if (i == 2 | i == 3 | i == 6 | i == -1)
        {
            EditorGUILayout.PropertyField(useUnityEvents);
            if (useUnityEvents.boolValue)
                EditorGUILayout.PropertyField(triggerExitEvents);

            EditorGUILayout.PropertyField(triggerExitInteracts, true);

        }


        if (i == 4 | i == 6 | i == 5 | i == -1)
        {
            EditorGUILayout.PropertyField(buttonDownToTrigger);
            if (buttonDownToTrigger.boolValue)
            {
                EditorGUILayout.PropertyField(button);
            }
            EditorGUILayout.PropertyField(useRepeatDelay);
            if (useRepeatDelay.boolValue)
            {
                EditorGUILayout.PropertyField(repeatDelay);
            }
            EditorGUILayout.PropertyField(useUnityEvents);
            if (useUnityEvents.boolValue)
                EditorGUILayout.PropertyField(triggerStayEvents);

            EditorGUILayout.PropertyField(triggerStayInteracts, true);

        }


        sourceRef.ApplyModifiedProperties();
    }
	
}
