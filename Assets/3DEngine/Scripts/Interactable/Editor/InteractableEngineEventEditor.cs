using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(InteractableEngineEvent))]
public class InteractableEngineEventEditor : InteractableEditor
{
    protected SerializedProperty hoverEnterEvents;
    protected SerializedProperty hoverStayEvents;
    protected SerializedProperty hoverExitEvents;
    protected SerializedProperty interactEvents;

    protected override void GetProperties()
    {
        base.GetProperties();
        hoverEnterEvents = sourceRef.FindProperty("hoverEnterEvents");
        hoverStayEvents = sourceRef.FindProperty("hoverStayEvents");
        hoverExitEvents = sourceRef.FindProperty("hoverExitEvents");
        interactEvents = sourceRef.FindProperty("interactEvents");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        DisplayEvents();
    }

    void DisplayEvents()
    {
        if (triggerMask.intValue == (triggerMask.intValue | (1 << (int)Interactable.TriggerType.OnHoverEnter)))
        {
            EditorExtensions.LabelFieldCustom("On Hover Enter:", FontStyle.Bold);
            hoverEnterEvents.ArrayFieldButtons("Enter Event", true, true, true, true, EngineEventExtensions.EngineEventField);
        }
        if (triggerMask.intValue == (triggerMask.intValue | (1 << (int)Interactable.TriggerType.OnHoverStay)))
        {
            EditorExtensions.LabelFieldCustom("On Hover Stay:", FontStyle.Bold);
            hoverStayEvents.ArrayFieldButtons("Stay Event", true, true, true, true, EngineEventExtensions.EngineEventField);
        }
        if (triggerMask.intValue == (triggerMask.intValue | (1 << (int)Interactable.TriggerType.OnHoverExit)))
        {
            EditorExtensions.LabelFieldCustom("On Hover Exit:", FontStyle.Bold);
            hoverExitEvents.ArrayFieldButtons("Exit Event", true, true, true, true, EngineEventExtensions.EngineEventField);
        }
        if (triggerMask.intValue == (triggerMask.intValue | (1 << (int)Interactable.TriggerType.OnInteract)))
        {
            EditorExtensions.LabelFieldCustom("On Interact:", FontStyle.Bold);
            interactEvents.ArrayFieldButtons("Interact Event", true, true, true, true, EngineEventExtensions.EngineEventField);
        }

    }

}
