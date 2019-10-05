using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public static class EngineEventTriggerExtensions
{
    private static SerializedProperty triggerName;
    private static SerializedProperty triggerType;
    private static SerializedProperty detectZoneInd;
    private static SerializedProperty onStayOption;
    private static SerializedProperty repeatDelay;
    private static SerializedProperty input;
    private static SerializedProperty validationMask;
    private static SerializedProperty validationLayer;
    private static SerializedProperty validationTag;
    private static SerializedProperty engineValueDataManager;
    private static SerializedProperty selection;
    private static SerializedProperty valueOption;
    private static SerializedProperty comparedValue;
    private static SerializedProperty activationAmount;
    private static SerializedProperty maxActivations;
    private static SerializedProperty activationType;
    private static SerializedProperty engineEvents;
    private static SerializedProperty receivers;

    private static SerializedProperty triggerProperty;
    private static int index;

    public static void TriggerEngineEventField(this SerializedProperty _triggerProperty, int _index = 0)
    {
        triggerProperty = _triggerProperty;
        index = _index;
        GetProperties();
        SetProperties();
    }

    static void GetProperties()
    {
        triggerName = triggerProperty.FindPropertyRelative("triggerName");
        triggerType = triggerProperty.FindPropertyRelative("triggerType");
        detectZoneInd = triggerProperty.FindPropertyRelative("detectZoneInd");
        onStayOption = triggerProperty.FindPropertyRelative("onStayOption");
        repeatDelay = triggerProperty.FindPropertyRelative("repeatDelay");
        input = triggerProperty.FindPropertyRelative("input");
        validationMask = triggerProperty.FindPropertyRelative("validationMask");
        validationLayer = triggerProperty.FindPropertyRelative("validationLayer");
        validationTag = triggerProperty.FindPropertyRelative("validationTag");
        engineValueDataManager = triggerProperty.FindPropertyRelative("engineValueDataManager");
        selection = triggerProperty.FindPropertyRelative("selection");
        valueOption = triggerProperty.FindPropertyRelative("valueOption");
        comparedValue = triggerProperty.FindPropertyRelative("comparedValue");
        activationAmount = triggerProperty.FindPropertyRelative("activationAmount");
        maxActivations = triggerProperty.FindPropertyRelative("maxActivations");
        activationType = triggerProperty.FindPropertyRelative("activationType");
        engineEvents = triggerProperty.FindPropertyRelative("engineEvents");
        receivers = triggerProperty.FindPropertyRelative("receivers");
    }

    static void SetProperties()
    {
        DisplayTrigger();
        DisplayValidation();
        DisplayActivation();
    }

    static void DisplayTrigger()
    {
        EditorGUILayout.PropertyField(triggerName);
        if (triggerName.stringValue == "")
            triggerName.stringValue = "Trigger " + index;

        //get root manager
        var obj = triggerProperty.serializedObject;
        var root = (EngineEventTriggerManager)obj.targetObject;
        var rootTriggerType = obj.FindProperty("triggerType");

        //only display trigger options if root manager allows detect zones
        if (rootTriggerType.enumValueIndex == (int)EngineEventTriggerManager.TriggerType.DetectZones)
        {
            EditorExtensions.LabelFieldCustom("Trigger Options", FontStyle.Bold);
            EditorGUILayout.PropertyField(triggerType);
            if (triggerType.enumValueIndex != (int)EngineEventTrigger.TriggerType.External)
            {
                if (triggerType.enumValueIndex == (int)EngineEventTrigger.TriggerType.OnStay)
                {
                    EditorGUILayout.PropertyField(onStayOption);
                    if (onStayOption.enumValueIndex == (int)EngineEventTrigger.OnStayOptionType.Repeat)
                        EditorGUILayout.PropertyField(repeatDelay);
                    else if (onStayOption.enumValueIndex == (int)EngineEventTrigger.OnStayOptionType.OnInputDown ||
                        onStayOption.enumValueIndex == (int)EngineEventTrigger.OnStayOptionType.OnInput ||
                        onStayOption.enumValueIndex == (int)EngineEventTrigger.OnStayOptionType.OnInputUp)
                        EditorGUILayout.PropertyField(input);
                }

                //detect zone popup with names from root manager
                detectZoneInd.intValue = EditorGUILayout.Popup("Detect Zone", detectZoneInd.intValue, root.GetDetectZoneNames());
            }
        }
        else if (rootTriggerType.enumValueIndex == (int)EngineEventTriggerManager.TriggerType.Receiver)
            triggerType.enumValueIndex = (int)EngineEventTrigger.TriggerType.External;
    }

    static void DisplayValidation()
    {
        EditorExtensions.LabelFieldCustom("Validation Options", FontStyle.Bold);
        validationMask.intValue = EditorGUILayout.MaskField("Validation Mask", validationMask.intValue, System.Enum.GetNames(typeof(EngineEventTrigger.ValidationType)));
        if (validationMask.intValue == (validationMask.intValue | (1 << (int)EngineEventTrigger.ValidationType.Layer)))
            EditorGUILayout.PropertyField(validationLayer);
        if (validationMask.intValue == (validationMask.intValue | (1 << (int)EngineEventTrigger.ValidationType.Tag)))
            EditorGUILayout.PropertyField(validationTag);
        if (validationMask.intValue == (validationMask.intValue | (1 << (int)EngineEventTrigger.ValidationType.UnitValueAmount)))
        {
            EditorGUILayout.PropertyField(engineValueDataManager);
            if (engineValueDataManager.objectReferenceValue)
            {
                var val = engineValueDataManager.GetRootValue<EngineValueDataManager>();
                if (val)
                {
                    selection.EngineValueSelectionField(engineValueDataManager);
                }

                EditorGUILayout.PropertyField(valueOption);
                EditorGUILayout.PropertyField(comparedValue);
            }
        }

    }

    static void DisplayActivation()
    {
        EditorExtensions.LabelFieldCustom("Activation Options", FontStyle.Bold);
        EditorGUILayout.PropertyField(activationAmount);
        if (activationAmount.enumValueIndex == (int)EngineEventTrigger.ActivationAmountType.Finite)
        {
            EditorGUILayout.PropertyField(maxActivations);
        }
            EditorGUILayout.PropertyField(activationType);
        if (activationType.enumValueIndex == (int)EngineEventTrigger.ActivationType.Solo || activationType.enumValueIndex == (int)EngineEventTrigger.ActivationType.Both)
        {
            EditorExtensions.LabelFieldCustom("Add/Remove Events", FontStyle.Bold);
            engineEvents.ArrayFieldButtons("Event", true, true, true, true, EngineEventExtensions.EngineEventField);
        }   
        if (activationType.enumValueIndex == (int)EngineEventTrigger.ActivationType.Broadcast || activationType.enumValueIndex == (int)EngineEventTrigger.ActivationType.Both)
        {
            EditorExtensions.LabelFieldCustom("Add/Remove Receivers", FontStyle.Bold);
            receivers.ArrayFieldButtons("Receiver", true,true,true,true, EngineEventReceiverExtensions.TriggerEngineReceiverField);
        }
            
    }


}
