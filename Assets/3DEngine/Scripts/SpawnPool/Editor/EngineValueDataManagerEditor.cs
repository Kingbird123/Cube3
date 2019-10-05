using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using System;
using System.Linq;

[CustomEditor(typeof(EngineValueDataManager))]
public class EngineValueDataManagerEditor : Editor
{
    private SerializedObject sourceRef;
    private EngineValueDataManager source;

    private SerializedProperty engineValueCategories;

    private void OnEnable()
    {
        source = (EngineValueDataManager)target;
        sourceRef = serializedObject;

        GetProperties();
    }

    public override void OnInspectorGUI()
    {
        SetProperties();
        sourceRef.ApplyModifiedProperties();
        source.RefreshIDs();
    }

    void GetProperties()
    {
        engineValueCategories = sourceRef.FindProperty("engineValueCategories");
    }

    void SetProperties()
    {
        engineValueCategories.ArrayFieldButtons("Category", true, true, true, true, DisplayEngineValueCategories);   
    }

    void DisplayEngineValueCategories(SerializedProperty _property, int _ind)
    {
        var categoryName = _property.FindPropertyRelative("categoryName");
        var valueType = _property.FindPropertyRelative("valueType");
        var engineValueDatas = _property.FindPropertyRelative("engineValueDatas");

        EditorGUILayout.PropertyField(categoryName);
        EditorGUILayout.PropertyField(valueType);
        engineValueDatas.ArrayFieldButtons("Value", true, false, false, false, DisplayEngineValue);
    }

    void DisplayEngineValue(SerializedProperty _property, int _ind)
    {
        EditorGUILayout.PropertyField(_property);
    }
}
