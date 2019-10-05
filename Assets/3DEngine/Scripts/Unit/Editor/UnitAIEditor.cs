using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitAI))]
public class UnitAIEditor : Editor
{
    protected UnitAI source;
    protected SerializedObject sourceRef;

    protected SerializedProperty detectZones;
    protected SerializedProperty unitAIEvents;
    protected SerializedProperty unitAITriggers;

    protected GUIStyle boldStyle;

    protected UnitEquip equipSource;

    public virtual void OnEnable()
    {
        source = (UnitAI)target;
        sourceRef = serializedObject;
        SetupGUIStyle();
        GetProperties();
    }

    void SetupGUIStyle()
    {
        boldStyle = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
        };
    }

    public override void OnInspectorGUI()
    {
        SetProperties();

        sourceRef.ApplyModifiedProperties();
    }

    public virtual void GetProperties()
    {
        detectZones = sourceRef.FindProperty("detectZones");
        unitAIEvents = sourceRef.FindProperty("unitAIEvents");
        unitAITriggers = unitAIEvents.FindPropertyRelative("unitAITriggers");

        equipSource = source.GetComponent<UnitEquip>();
    }

    public virtual void SetProperties()
    {
        EditorGUILayout.Space();
        detectZones.ArrayFieldButtons("Detect Zone", true, true, true, true, DisplayDetectZone);
        unitAITriggers.ArrayFieldButtons("AI Event Trigger", true, true, true, true, DisplayAITriggers);

    }

    void DisplayDetectZone(SerializedProperty _property, int _index)
    {
        var overrideLabel = _property.FindPropertyRelative("overrideLabel");
        overrideLabel.boolValue = true;
        EditorGUILayout.PropertyField(_property);
    }

    void DisplayAITriggers(SerializedProperty _property, int _index)
    {
        //trigger
        var triggerName = _property.FindPropertyRelative("triggerName");
        var triggerType = _property.FindPropertyRelative("triggerType");
        var eventLoadType = _property.FindPropertyRelative("eventLoadType");
        var useType = _property.FindPropertyRelative("useType");
        var amount = _property.FindPropertyRelative("amount");
        var detectZoneInd = _property.FindPropertyRelative("detectZoneInd");
        var events = _property.FindPropertyRelative("events");


        //valueamount
        var engineValueManager = _property.FindPropertyRelative("engineValueManager");
        var valueSelection = _property.FindPropertyRelative("valueSelection");
        var valueOption = _property.FindPropertyRelative("valueOption");
        var comparedValue = _property.FindPropertyRelative("comparedValue");

        triggerName.stringValue = triggerType.enumNames[triggerType.enumValueIndex] + " | ";
        var detectNames = source.GetDetectZoneNames();
        if (triggerType.enumValueIndex != 3)
        {
            if (detectNames.Length > 0)
                triggerName.stringValue += detectNames[detectZoneInd.intValue];
        }
            
        EditorGUILayout.PropertyField(triggerType);
        EditorGUILayout.PropertyField(eventLoadType);
        EditorGUILayout.PropertyField(useType);

        if (useType.enumValueIndex == 1)
        {
            EditorGUILayout.PropertyField(amount);
        }
        if (amount.intValue < 1)
            amount.intValue = 1;

        //detect zone
        if (triggerType.enumValueIndex != 3)
        {
            detectZoneInd.intValue = EditorGUILayout.Popup("Detect Zone To Use", detectZoneInd.intValue, detectNames);
        }
        else
        {
            //value Amount
            EditorGUILayout.PropertyField(engineValueManager);
            var man = engineValueManager.GetRootValue<EngineValueDataManager>();
            if (man)
            {

                valueSelection.EngineValueSelectionField(engineValueManager);
                EditorGUILayout.PropertyField(valueOption);
                EditorGUILayout.PropertyField(comparedValue);

                triggerName.stringValue += " | " + System.Enum.GetNames(typeof(UnitAITrigger.ValueOptions))[valueOption.enumValueIndex]
                    + " | " + comparedValue.floatValue;
            }
        }
        events.ArrayFieldButtons("Event", true, true, true, true, DisplayAIEvents);

    }

    void DisplayAIEvents(SerializedProperty _property, int _index)
    {

        var eventName = _property.FindPropertyRelative("eventName");
        var startType = _property.FindPropertyRelative("startType");

        //event type
        var eventTypeMask = _property.FindPropertyRelative("eventTypeMask");
        //state type
        var stateType = _property.FindPropertyRelative("stateType");
        //items
        var itemEventType = _property.FindPropertyRelative("itemEventType");
        var item = _property.FindPropertyRelative("item");
        //anim
        var setAnimator = _property.FindPropertyRelative("setAnimator");
        var animator = _property.FindPropertyRelative("animator");
        var stateToPlay = _property.FindPropertyRelative("stateToPlay");
        var crossfadeTime = _property.FindPropertyRelative("crossfadeTime");
        //method
        var methodToCall = _property.FindPropertyRelative("methodToCall");
        //interacts
        var interacts = _property.FindPropertyRelative("interacts");
        //common stuff
        var delay = _property.FindPropertyRelative("delay");
        var repeat = _property.FindPropertyRelative("repeat");
        var repeatTime = _property.FindPropertyRelative("repeatTime");
        var finishType = _property.FindPropertyRelative("finishType");
        var totalTime = _property.FindPropertyRelative("totalTime");
        var finished = _property.FindPropertyRelative("finished");


        string label = "Event " + (_index + 1).ToString();
        if (repeat.boolValue)
            label += " (Repeating) ";
        eventName.stringValue = label;

        //start and trigger
        EditorGUILayout.PropertyField(startType);


        //event Type
        eventTypeMask.intValue = EditorGUILayout.MaskField("Event Mask", eventTypeMask.intValue, System.Enum.GetNames(typeof(UnitAIEvent.EventType)));

        //state events
        if (eventTypeMask.intValue == (eventTypeMask.intValue | (1 << 0)))
        {
            EditorGUILayout.LabelField("Unit State", boldStyle);
            EditorGUILayout.PropertyField(stateType);
        }

        //item events
        if (eventTypeMask.intValue == (eventTypeMask.intValue | (1 << 1)))
        {
            EditorGUILayout.LabelField("Item Options", boldStyle);
            EditorGUILayout.PropertyField(itemEventType);

            var names = equipSource.GetItemNames();
            item.intValue = EditorGUILayout.Popup("Item", item.intValue, names);
        }

        //anim events
        if (eventTypeMask.intValue == (eventTypeMask.intValue | (1 << 2)))
        {
            EditorGUILayout.LabelField("Animator", boldStyle);
            EditorGUILayout.PropertyField(setAnimator);

            if (setAnimator.boolValue)
            {
                EditorGUILayout.PropertyField(animator);
            }
            else if (animator.objectReferenceValue)
                animator.objectReferenceValue = null;

            EditorGUILayout.PropertyField(stateToPlay);
            EditorGUILayout.PropertyField(crossfadeTime);
        }

        //method events
        if (eventTypeMask.intValue == (eventTypeMask.intValue | (1 << 3)))
        {
            EditorGUILayout.LabelField("Call Method", boldStyle);
            methodToCall.MethodPropertyField();
        }

        if (eventTypeMask.intValue == (eventTypeMask.intValue | (1 << 4)))
        {

            EditorGUILayout.LabelField("Interact FX", boldStyle);
            EditorGUILayout.PropertyField(interacts, true);

        }

        EditorGUILayout.LabelField("Time Options", boldStyle);

        //delay
        EditorGUILayout.PropertyField(delay);

        //repeat
        EditorGUILayout.PropertyField(repeat);

        //repeat delay
        if (repeat.boolValue)
        {
            EditorGUILayout.PropertyField(repeatTime);
        }

        //finishtype
        EditorGUILayout.PropertyField(finishType);

        //totalTime
        if (finishType.enumValueIndex == 1)
        {
            EditorGUILayout.PropertyField(totalTime);
        }
    }

    public virtual void OnSceneGUI()
    {
        DetectZone[] detect = detectZones.GetRootValue<DetectZone[]>();
        if (detect != null)
        {
            foreach (var zone in detect)
            {
                if (zone.debugColor == default(Color))
                    zone.debugColor = Color.cyan;
                zone.DrawDetectZone(source, sourceRef, source.transform);
            }

        }

    }

}