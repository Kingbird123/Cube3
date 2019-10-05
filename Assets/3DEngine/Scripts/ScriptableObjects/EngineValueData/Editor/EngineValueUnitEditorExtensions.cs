using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EngineValueEntityEditorExtensions
{
    static SerializedProperty engineEventManager;
    static SerializedProperty engineValueManager;

    public static void EngineValueUnitField(this SerializedProperty _valueUnitProperty, SerializedProperty _engineValueManager, SerializedProperty _engineEventManager)
    {
        engineEventManager = _engineEventManager;
        engineValueManager = _engineValueManager;
        EngineValueEntityField(_valueUnitProperty, 0);
    }

    public static void EngineValueEntityArrayField(this SerializedProperty _valueUnitArrayProperty, SerializedProperty _engineValueManager, SerializedProperty _engineEventManager)
    {
        engineEventManager = _engineEventManager;
        engineValueManager = _engineValueManager;
        _valueUnitArrayProperty.ArrayFieldButtons("Entity Value", true, true, true, true, EngineValueEntityField);
    }

    static void EngineValueEntityField(SerializedProperty _property, int _ind)
    {
        var engineValueName = _property.FindPropertyRelative("engineValueName");
        var valueSelection = _property.FindPropertyRelative("valueSelection");
        var cat = valueSelection.FindPropertyRelative("category").FindPropertyRelative("stringValue").stringValue;
        var val = valueSelection.FindPropertyRelative("engineValue").FindPropertyRelative("stringValue").stringValue;
        var valueEvents = _property.FindPropertyRelative("valueEvents");

        //value selection
        valueSelection.EngineValueSelectionField(engineValueManager);
        engineValueName.stringValue = cat + " | " + val;

        //value events
        if (engineEventManager.objectReferenceValue)
        {
            EditorExtensions.LabelFieldCustom("Value Events", FontStyle.Bold);
            valueEvents.EngineValueEventArray(engineEventManager);
        }  
    }
}
