using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class LayoutSyncSelectionEditorExtensions
{
    private static SerializedProperty valueSelections;
    private static SerializedProperty layoutMaster;
    private static string[] layoutNames;

    public static void LayoutSyncSelectionArrayField
        (this SerializedProperty _layoutSyncSelectionArray, 
        SerializedProperty _valueSelections, 
        SerializedProperty _layoutMaster)
    {
        layoutMaster = _layoutMaster;
        var master = new SerializedObject(layoutMaster.objectReferenceValue);
        var layouts = master.FindProperty("layouts");
        layoutNames = layouts.GetDisplayNames();
        valueSelections = _valueSelections;
        _layoutSyncSelectionArray.ArrayFieldButtons("Value Sync", true, true, false, false, LayoutSyncSelectionField, true);
        master.Dispose();
    }

    static void LayoutSyncSelectionField(SerializedProperty _property, int _ind)
    {
        var masterInd = _property.FindPropertyRelative("masterInd");
        var valueInd = _property.FindPropertyRelative("valueInd");

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal(GUILayout.Width(250));
        valueInd.intValue = EditorGUILayout.Popup(valueInd.intValue, valueSelections.GetDisplayNames());
        GUILayout.Space(-60);
        masterInd.intValue = EditorGUILayout.Popup(masterInd.intValue, layoutNames);
        EditorGUILayout.EndHorizontal();
    }

    public static void LayoutSyncSingleField(this SerializedProperty _syncProperty, SerializedProperty _layoutMaster = null, SerializedProperty _syncType = null)
    {
        if (_layoutMaster == null || _syncType == null)
            _syncProperty.isExpanded = EditorGUILayout.Foldout(_syncProperty.isExpanded, _syncProperty.displayName);
        else
            _syncProperty.isExpanded = true;
        if (_syncProperty.isExpanded)
        {
            var syncType = _syncProperty.FindPropertyRelative("syncType");
            var layoutMaster = _syncProperty.FindPropertyRelative("layoutMaster");
            var syncSelection = _syncProperty.FindPropertyRelative("syncSelection");

            if (_syncType == null)
                EditorGUILayout.PropertyField(syncType);
            else
                syncType.enumValueIndex = _syncType.enumValueIndex;
            if (_layoutMaster == null)
                EditorGUILayout.PropertyField(layoutMaster);
            else
                layoutMaster.objectReferenceValue = _layoutMaster.objectReferenceValue;

            if (layoutMaster.objectReferenceValue)
            {
                var master = new SerializedObject(layoutMaster.objectReferenceValue);
                var layouts = master.FindProperty("layouts");
                var lNames = layouts.GetDisplayNames();

                syncSelection.IndexStringField(lNames, null, _syncProperty.displayName);
                master.Dispose();
            }
        } 
    }
}
