using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;


[CustomEditor(typeof(EngineEventTriggerManager))]
[InitializeOnLoad]//<--Need this to draw all connections
public class EngineEventTriggerManagerEditor : Editor
{
    protected SerializedObject sourceRef;
    protected EngineEventTriggerManager source;

    private SerializedProperty triggerType;
    private SerializedProperty detectZones;
    private SerializedProperty triggers;
    private SerializedProperty preTriggers;

    protected void OnEnable()
    {
        source = (EngineEventTriggerManager)target;
        sourceRef = serializedObject;

        GetProperties();
    }

    protected void GetProperties()
    {
        triggerType = sourceRef.FindProperty("triggerType");
        detectZones = sourceRef.FindProperty("detectZones");
        triggers = sourceRef.FindProperty("triggers");
        preTriggers = sourceRef.FindProperty("preTriggers");
    }

    public override void OnInspectorGUI()
    {
        SetProperties();

        sourceRef.ApplyModifiedProperties();
    }

    protected void SetProperties()
    {
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Trigger Options", FontStyle.Bold);
        EditorGUILayout.PropertyField(triggerType);

        DisplayDetectZones();
        DisplayPreTriggers();
        DisplayTriggers();
    }

    void DisplayDetectZones()
    {
        if (triggerType.enumValueIndex == (int)EngineEventTriggerManager.TriggerType.DetectZones)
        {
            EditorExtensions.LabelFieldCustom("Add/Remove Detect Zones", FontStyle.Bold);
            detectZones.ArrayFieldButtons("Detect Zone", true, true, true, true, DetectZoneField);
        }
    }

    void DetectZoneField(SerializedProperty _property, int _ind)
    {
        _property.DetectZoneField(false, false, _ind);
    }

    void DisplayPreTriggers()
    {
        if (triggerType.enumValueIndex == (int)EngineEventTriggerManager.TriggerType.PreTrigger)
        {
            EditorExtensions.LabelFieldCustom("Add/Remove PreTriggers", FontStyle.Bold);
            preTriggers.ArrayFieldButtons("Pre Trigger", true, true, true, true, PreTriggerField);
        }
    }

    void PreTriggerField(SerializedProperty _property, int _ind)
    {
        var preTriggerName = _property.FindPropertyRelative("preTriggerName");
        var activated = _property.FindPropertyRelative("activated");

        EditorGUILayout.BeginHorizontal();

        var col = Color.green;
        if (!activated.boolValue)
            col = Color.grey;

        EditorExtensions.LabelFieldCustom("◯", FontStyle.Bold, col, 11, 2);

        if (preTriggerName.stringValue == "")
            preTriggerName.stringValue = "PreTrigger " + _ind;
        EditorGUILayout.PropertyField(preTriggerName);


        EditorGUILayout.EndHorizontal();
    }

    void DisplayTriggers()
    {
        EditorExtensions.LabelFieldCustom("Add/Remove Triggers", FontStyle.Bold);
        triggers.ArrayFieldButtons("Trigger", true, true, true, true, TriggerField);
    }

    void TriggerField(SerializedProperty _property, int _ind)
    {
        _property.TriggerEngineEventField();
    }

    protected void OnSceneGUI()
    {
        DrawDetectZones();
    }

    void DrawDetectZones()
    {
        if (triggerType.enumValueIndex == (int)EngineEventTriggerManager.TriggerType.DetectZones)
        {
            for (int i = 0; i < detectZones.arraySize; i++)
            {
                var ele = detectZones.GetArrayElementAtIndex(i);
                var root = ele.GetRootValue<DetectZone>();
                if (root != null)
                    root.DrawDetectZone(source, sourceRef, source.transform);
            }
        }
    }

    //This will load all connections even when object is not selected in the editor
    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    static void DrawConnections(EngineEventTriggerManager _manager, GizmoType gizmoType)
    {
        for (int trigInd = 0; trigInd < _manager.Triggers.Length; trigInd++)
        {
            var receivers = _manager.Triggers[trigInd].Receivers;
            for (int recInd = 0; recInd < receivers.Length; recInd++)
            {
                var receiver = receivers[recInd].Manager;
                if (receiver)
                {
                    var pos = _manager.transform.position;
                    var dest = receiver.transform.position;
                    EditorExtensions.DrawArrowedLine(pos, dest, 2, 0.05f, Color.blue, Color.green);
                }

            }
        }
    }
}
