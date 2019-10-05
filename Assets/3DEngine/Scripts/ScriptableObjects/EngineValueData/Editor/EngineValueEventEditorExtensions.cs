using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EngineValueEventEditorExtensions
{
    static SerializedProperty engineEventManager;

    public static void EngineValueEventArray(this SerializedProperty _property, SerializedProperty _engineEventManager)
    {
        engineEventManager = _engineEventManager;
        _property.ArrayFieldButtons("Event", true, true, true, true, EngineValueEventField);
        
    }

    public static void EngineValueEventField(SerializedProperty _property, int _ind)
    {
        if (engineEventManager.objectReferenceValue)
        {
            var man = engineEventManager.objectReferenceValue as EngineEventManager;
            if (man)
            {
                var eventName = _property.FindPropertyRelative("eventName");
                var triggerType = _property.FindPropertyRelative("triggerType");
                var compareValue = _property.FindPropertyRelative("compareValue");
                var eventType = _property.FindPropertyRelative("eventType");

                EditorGUILayout.PropertyField(triggerType);
                string comp = "";
                if (triggerType.enumValueIndex == (int)EngineValueEvent.TriggerType.Equal ||
                    triggerType.enumValueIndex == (int)EngineValueEvent.TriggerType.Greater ||
                    triggerType.enumValueIndex == (int)EngineValueEvent.TriggerType.Less)
                {
                    comp = compareValue.floatValue.ToString() + " | ";
                    EditorGUILayout.PropertyField(compareValue);
                }

                var names = man.GetEventNames();
                eventType.intValue = EditorGUILayout.Popup("Engine Event", eventType.intValue, names);



                eventName.stringValue = System.Enum.GetNames(typeof(EngineValueEvent.TriggerType))[triggerType.enumValueIndex] + " | "
                    + comp + names[eventType.intValue];
            }
        }
        
    }
}
