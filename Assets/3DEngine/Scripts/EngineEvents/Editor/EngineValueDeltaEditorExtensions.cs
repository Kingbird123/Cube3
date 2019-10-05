using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public static class EngineValueDeltaEditorExtensions
{
    public static void EventOptionValueDeltaField(this SerializedProperty _property, int _index)
    {
        var valueDataManager = _property.FindPropertyRelative("valueDataManager");
        var valueSelection = _property.FindPropertyRelative("valueSelection");
        var engineValueType = _property.FindPropertyRelative("engineValueType");
        var deltaType = _property.FindPropertyRelative("deltaType");
        var valueDelta = _property.FindPropertyRelative("valueDelta");
        var engineValueData = _property.FindPropertyRelative("engineValueData");

        EditorGUILayout.PropertyField(valueDataManager);
        if (valueDataManager.objectReferenceValue)
        {
            valueSelection.EngineValueSelectionField(valueDataManager);

            EditorGUILayout.PropertyField(engineValueType);
            if (engineValueType.enumValueIndex != (int)EngineValueDelta.EngineValueType.Reset)
            {
                EditorGUILayout.PropertyField(deltaType);
                if (deltaType.enumValueIndex == (int)EngineValueDelta.DeltaType.CustomFloat)
                    EditorGUILayout.PropertyField(valueDelta);
                if (deltaType.enumValueIndex == (int)EngineValueDelta.DeltaType.FromData)
                    EditorGUILayout.PropertyField(engineValueData);
            }
                
        }

    }

}
