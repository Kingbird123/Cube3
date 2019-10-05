using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(EngineEventManager))]
public class EngineEventManagerEditor : Editor
{
    protected EngineEventManager source;
    protected SerializedObject sourceRef;
    //skins
    protected SerializedProperty engineEvents;

    protected virtual void OnEnable()
    {
        sourceRef = serializedObject;
        source = (EngineEventManager)target;
        GetProperties();
    }

    public override void OnInspectorGUI()
    { 
        SetProperties();

        sourceRef.ApplyModifiedProperties();
    }

    protected virtual void GetProperties()
    {
        //enemy
        engineEvents = sourceRef.FindProperty("engineEvents");
    }

    protected virtual void SetProperties()
    {
        EditorGUILayout.Space();
        engineEvents.ArrayFieldButtons("Event Container", true, true, true, true, EngineEventField);
    }

    void EngineEventField(SerializedProperty _property, int _ind)
    {
        var eventArrayName = _property.FindPropertyRelative("eventArrayName");
        var events = _property.FindPropertyRelative("events");

        EditorGUILayout.PropertyField(eventArrayName);
        events.ArrayFieldButtons("Event", true, true, true, true, EngineEventExtensions.EngineEventField);
    }

}
