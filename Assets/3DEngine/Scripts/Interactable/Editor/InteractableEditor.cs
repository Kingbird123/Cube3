using UnityEditor;
using UnityEngine;

public abstract class InteractableEditor : Editor
{
    protected SerializedObject sourceRef;
    protected Interactable source;

    protected SerializedProperty triggerMask;

    protected virtual void OnEnable()
    {
        source = (Interactable)target;
        sourceRef = serializedObject;
        GetProperties();
    }

    public override void OnInspectorGUI()
    {
        SetProperties();
        sourceRef.ApplyModifiedProperties();
    }
    protected virtual void GetProperties()
    {
        triggerMask = sourceRef.FindProperty("triggerMask");
    }
    protected virtual void SetProperties()
    {
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Trigger Options", FontStyle.Bold);
        triggerMask.intValue = EditorGUILayout.MaskField("Trigger Mask", triggerMask.intValue, System.Enum.GetNames(typeof(Interactable.TriggerType)));
    }

}
