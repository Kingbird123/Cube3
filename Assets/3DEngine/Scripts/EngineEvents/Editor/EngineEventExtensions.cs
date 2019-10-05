using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class EngineEventExtensions
{
    static SerializedProperty eventName;
    static SerializedProperty affectedObjectOption;
    static SerializedProperty sceneObject;
    static SerializedProperty startType;
    static SerializedProperty inputButton;

    //event type
    static SerializedProperty eventTypeMask;

    //common
    static SerializedProperty commons;
    //anim
    static SerializedProperty anims;

    //method
    static SerializedProperty methods;
    //interacts
    static SerializedProperty interacts;
    static SerializedProperty eventOptions;
    static SerializedProperty valueDeltaOptions;

    //common stuff
    static SerializedProperty delay;
    static SerializedProperty repeat;
    static SerializedProperty repeatTime;
    static SerializedProperty finishType;
    static SerializedProperty totalTime;
    static SerializedProperty finished;
    static SerializedProperty active;

    static SerializedProperty sceneObjectType;
    static SceneObjectProperty sceneObjectSource;
    static GameObject sourceGameObject;

    static void GetSources(SerializedProperty _property)
    {
        sceneObjectSource = sceneObject.GetRootValue<SceneObjectProperty>();
        if (sceneObjectSource != null)
        {
            var rootSource = _property.serializedObject.targetObject as MonoBehaviour;
            if (rootSource)
                sourceGameObject = sceneObjectSource.GetSceneObject(rootSource.gameObject);
            else
                sourceGameObject = null;
        }
    }

    public static void EngineEventField(SerializedProperty _property, int _index)
    {
        eventName = _property.FindPropertyRelative("eventName");
        affectedObjectOption = _property.FindPropertyRelative("affectedObjectOption");
        sceneObject = _property.FindPropertyRelative("sceneObject");
        sceneObjectType = sceneObject.FindPropertyRelative("sceneObjectType");
        startType = _property.FindPropertyRelative("startType");
        inputButton = _property.FindPropertyRelative("inputButton");

        //event type
        eventTypeMask = _property.FindPropertyRelative("eventTypeMask");

        //anim
        anims = _property.FindPropertyRelative("anims");
        commons = _property.FindPropertyRelative("commons");
        //method
        methods = _property.FindPropertyRelative("methods");
        //interacts
        interacts = _property.FindPropertyRelative("interacts");
        eventOptions = _property.FindPropertyRelative("eventOptions");
        valueDeltaOptions = _property.FindPropertyRelative("valueDeltaOptions");

        //common stuff
        delay = _property.FindPropertyRelative("delay");
        repeat = _property.FindPropertyRelative("repeat");
        repeatTime = _property.FindPropertyRelative("repeatTime");
        finishType = _property.FindPropertyRelative("finishType");
        totalTime = _property.FindPropertyRelative("totalTime");
        finished = _property.FindPropertyRelative("finished");
        active = _property.FindPropertyRelative("active");

        //GetSources(_property);

        string label = "Event " + (_index + 1).ToString();
        if (repeat.boolValue)
            label += " (Repeating) ";
        if (active.boolValue)
            label += " (Active) ";
        if (finished.boolValue)
            label += " (Finished) ";
        eventName.stringValue = label;


        EditorGUILayout.PropertyField(sceneObject);

        //start and trigger
        EditorGUILayout.PropertyField(startType);
        if (startType.enumValueIndex == (int)EngineEvent.StartType.OnInputAfterPreviousFinished)
            EditorGUILayout.PropertyField(inputButton);

        //event Type
        eventTypeMask.intValue = EditorGUILayout.MaskField("Event Mask", eventTypeMask.intValue, System.Enum.GetNames(typeof(EngineEvent.EventType)));

        //anim events
        if (eventTypeMask.intValue == (eventTypeMask.intValue | (1 << (int)EngineEvent.EventType.Common)))
        {
            EditorExtensions.LabelFieldCustom("Common Options", FontStyle.Bold);
            commons.ArrayFieldButtons("Common Event", true, true, true, true, EventOptionCommonField);
        }

        //anim events
        if (eventTypeMask.intValue == (eventTypeMask.intValue | (1 << (int)EngineEvent.EventType.Animator)))
        {
            EditorExtensions.LabelFieldCustom("Animator Options", FontStyle.Bold);
            anims.ArrayFieldButtons("Animator State", true, true, true, true, EventOptionAnimatorField);
        }

        //method events
        if (eventTypeMask.intValue == (eventTypeMask.intValue | (1 << (int)EngineEvent.EventType.CallMethod)))
        {
            EditorExtensions.LabelFieldCustom("Call Method Options", FontStyle.Bold);
            methods.ArrayFieldButtons("Method", true, true, true, true, EventOptionCallMethodField);
        }

        //interactFX
        if (eventTypeMask.intValue == (eventTypeMask.intValue | (1 << (int)EngineEvent.EventType.InteractFX)))
        {
            EditorExtensions.LabelFieldCustom("InteractFX Options", FontStyle.Bold);
            interacts.ArrayFieldButtons("InteractFX", true, true, true, true, EventOptionInteractFXField);
        }

        //Events
        if (eventTypeMask.intValue == (eventTypeMask.intValue | (1 << (int)EngineEvent.EventType.ValueDelta)))
        {
            EditorExtensions.LabelFieldCustom("Event Options", FontStyle.Bold);
            valueDeltaOptions.ArrayFieldButtons("ValueOption", true, true, true, true, EventOptionValueDeltaField);
        }

        //Events
        if (eventTypeMask.intValue == (eventTypeMask.intValue | (1 << (int)EngineEvent.EventType.Event)))
        {
            EditorExtensions.LabelFieldCustom("Event Options", FontStyle.Bold);
            eventOptions.ArrayFieldButtons("EventOption", true, true, true, true, EventOptionEventField);
        }

        EditorExtensions.LabelFieldCustom("Time Options", FontStyle.Bold);

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


    public static void EventOptionCommonField(SerializedProperty _property, int _index)
    {
        var affectedObj = _property.FindPropertyRelative("affectedObj");
        var overrideObject = _property.FindPropertyRelative("overrideObject");

        EditorGUILayout.PropertyField(affectedObj);

        if (affectedObj.enumValueIndex == (int)EngineEventOption.AffectedType.Override)
            EditorGUILayout.PropertyField(overrideObject);

        var eventType = _property.FindPropertyRelative("eventType");
        var active = _property.FindPropertyRelative("active");
        var delay = _property.FindPropertyRelative("delay");
        var objToSpawn = _property.FindPropertyRelative("objToSpawn");
        var positionType = _property.FindPropertyRelative("positionType");
        var position = _property.FindPropertyRelative("position");
        var rotation = _property.FindPropertyRelative("rotation");
        var positionObj = _property.FindPropertyRelative("positionObj");
        var setParent = _property.FindPropertyRelative("setParent");
        var parentObj = _property.FindPropertyRelative("parentObj");

        EditorGUILayout.PropertyField(eventType);
        if (eventType.enumValueIndex == (int)EngineEventOptionCommon.CommonEventType.SetActive)
            EditorGUILayout.PropertyField(active);
        else if (eventType.enumValueIndex == (int)EngineEventOptionCommon.CommonEventType.Destroy)
            EditorGUILayout.PropertyField(delay);
        else if (eventType.enumValueIndex == (int)EngineEventOptionCommon.CommonEventType.Spawn)
        {
            EditorGUILayout.PropertyField(positionType);
            if (positionType.enumValueIndex == (int)EngineEventOptionCommon.PositionType.Vector3)
            {
                EditorGUILayout.PropertyField(position);
                EditorGUILayout.PropertyField(rotation);
            }
            else if (positionType.enumValueIndex == (int)EngineEventOptionCommon.PositionType.SceneObject)
                EditorGUILayout.PropertyField(positionObj);

            EditorGUILayout.PropertyField(setParent);
            if (setParent.boolValue)
                EditorGUILayout.PropertyField(parentObj);
        }


    }

    public static void EventOptionAnimatorField(SerializedProperty _property, int _index)
    {
        var affectedObj = _property.FindPropertyRelative("affectedObj");
        var overrideObject = _property.FindPropertyRelative("overrideObject");

        EditorGUILayout.PropertyField(affectedObj);

        if (affectedObj.enumValueIndex == (int)EngineEventOption.AffectedType.Override)
            EditorGUILayout.PropertyField(overrideObject);

        //animator
        var animController = _property.FindPropertyRelative("animController");
        var state = _property.FindPropertyRelative("state");
        var crossfadeTime = _property.FindPropertyRelative("crossfadeTime");

        Animator anim = null;
        GameObject obj = null;
        if (affectedObj.enumValueIndex == (int)EngineEventOption.AffectedType.Override)
        {
            obj = overrideObject.objectReferenceValue as GameObject;
        }
        else if (affectedObj.enumValueIndex == (int)EngineEventOption.AffectedType.EventAssigned)
        {
            if (sceneObjectType.enumValueIndex != (int)SceneObjectProperty.SceneObjectType.ClosestByTag)
            {
                obj = sourceGameObject;
            }
        }
        if (obj)
        {
            anim = obj.GetComponent<Animator>();
            if (anim)
            {
                var names = anim.GetAnimatorStateNames();
                state.IndexStringField(names, (AnimatorController)anim.runtimeAnimatorController);
                EditorGUILayout.PropertyField(crossfadeTime);
            }
            else
                EditorExtensions.LabelFieldCustom("No Animator found on: " + obj.name, FontStyle.Normal, Color.red);
        }
        else
        {
            EditorGUILayout.PropertyField(animController);
            if (animController.objectReferenceValue)
            {
                var cont = animController.objectReferenceValue as RuntimeAnimatorController;
                var animCont = (AnimatorController)cont;
                state.IndexStringField(GetAnimStateNames(animCont), animCont);
                EditorGUILayout.PropertyField(crossfadeTime);
            }

        }

    }

    static string[] GetAnimStateNames(AnimatorController _cont)
    {
        var states = new AnimatorState[_cont.animationClips.Length];
        states = EditorExtensions.GetAnimatorStates(_cont);
        if (states.Length > 0)
        {
            var stateNames = new string[states.Length];
            for (int i = 0; i < states.Length; i++)
            {
                stateNames[i] = states[i].name;
            }
            return stateNames;
        }
        return null;
    }

    public static void EventOptionInteractFXField(SerializedProperty _property, int _index)
    {
        var affectedObj = _property.FindPropertyRelative("affectedObj");
        var overrideObject = _property.FindPropertyRelative("overrideObject");

        EditorGUILayout.PropertyField(affectedObj);

        if (affectedObj.enumValueIndex == (int)EngineEventOption.AffectedType.Override)
            EditorGUILayout.PropertyField(overrideObject);

        var interactFX = _property.FindPropertyRelative("interactFX");
        EditorGUILayout.PropertyField(interactFX);
    }

    public static void EventOptionCallMethodField(SerializedProperty _property, int _index)
    {
        var affectedObj = _property.FindPropertyRelative("affectedObj");
        var overrideObject = _property.FindPropertyRelative("overrideObject");

        EditorGUILayout.PropertyField(affectedObj);

        if (affectedObj.enumValueIndex == (int)EngineEventOption.AffectedType.Override)
            EditorGUILayout.PropertyField(overrideObject);

        var method = _property.FindPropertyRelative("method");
        GameObject obj = null;
        if (affectedObj.enumValueIndex == (int)EngineEventOption.AffectedType.Override)
            obj = overrideObject.objectReferenceValue as GameObject;
        else if (affectedObj.enumValueIndex == (int)EngineEventOption.AffectedType.EventAssigned)
        {
            if (sceneObjectType.enumValueIndex != (int)SceneObjectProperty.SceneObjectType.ClosestByTag ||
                sceneObjectType.enumValueIndex != (int)SceneObjectProperty.SceneObjectType.Receiver)
                obj = sourceGameObject;
        }

        if (obj)
            method.MethodPropertyField(obj);
        else
            method.MethodPropertyField(0);
    }

    public static void EventOptionValueDeltaField(SerializedProperty _property, int _index)
    {
        var affectedObj = _property.FindPropertyRelative("affectedObj");
        var overrideObject = _property.FindPropertyRelative("overrideObject");

        EditorGUILayout.PropertyField(affectedObj);

        if (affectedObj.enumValueIndex == (int)EngineEventOption.AffectedType.Override)
            EditorGUILayout.PropertyField(overrideObject);

        var valueDelta = _property.FindPropertyRelative("valueDelta");
        valueDelta.EventOptionValueDeltaField(0);
    }

    public static void EventOptionEventField(SerializedProperty _property, int _index)
    {
        var receiver = _property.FindPropertyRelative("receiver");

        receiver.TriggerEngineReceiverField(_index);
    }
}
