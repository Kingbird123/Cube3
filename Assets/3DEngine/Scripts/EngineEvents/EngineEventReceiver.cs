using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EngineEventReceiver
{
    public enum BroadcastType { PreTrigger, Trigger, EventSpecific }
    public enum TriggerBroadcastType { BroadcastAll, Single, Mask }
    public enum PreTriggerBroadcastType { Activate, Deactivate }
    public enum EventOptionType { Activate, Pause, Resume, Stop }
    [SerializeField] protected EngineEventTriggerManager manager;
    public EngineEventTriggerManager Manager { get { return manager; } }
    [SerializeField] protected BroadcastType broadcastType;
    [SerializeField] protected TriggerBroadcastType triggerBroadcastType;
    [SerializeField] protected IndexStringProperty triggerSingle;
    [SerializeField] protected int triggerMask;
    [SerializeField] protected IndexStringProperty preTrigger;
    [SerializeField] protected PreTriggerBroadcastType preTriggerBroadcastType;
    [SerializeField] protected IndexStringProperty eventInd;
    [SerializeField] protected EventOptionType eventOption;


    public void SetManager(EngineEventTriggerManager _manager)
    {
        manager = _manager;
    }

    public void Activate()
    {
        if (broadcastType == BroadcastType.Trigger)
            ActivateTriggers();
        else if (broadcastType == BroadcastType.PreTrigger)
            ActivatePreTrigger();
        else if (broadcastType == BroadcastType.EventSpecific)
            ActivateEvent();
    }

    void ActivateTriggers()
    {
        if (triggerBroadcastType == TriggerBroadcastType.BroadcastAll)
        {
            manager.ActivateAllTriggerEvents();
        }
        else if (triggerBroadcastType == TriggerBroadcastType.Single)
        {
            manager.ActivateTriggerEvents(triggerSingle.indexValue);
        }
        else if (triggerBroadcastType == TriggerBroadcastType.Mask)
        {
            for (int i = 0; i < manager.Triggers.Length; i++)
            {
                if (i.IsInMask(triggerMask))
                {
                    manager.ActivateTriggerEvents(i);
                }
            }

        }
    }

    void ActivatePreTrigger()
    {
        bool activate = preTriggerBroadcastType == PreTriggerBroadcastType.Activate;
        manager.ActivatePreTrigger(preTrigger.indexValue, activate);
    }

    void ActivateEvent()
    {
        var events = manager.Triggers[triggerSingle.indexValue].EngineEvents;
        var chosenEvent = events[eventInd.indexValue];

        if (eventOption == EventOptionType.Activate)
            chosenEvent.DoEvent(manager.gameObject, events, eventInd.indexValue);
        else if (eventOption == EventOptionType.Pause)
            chosenEvent.PauseEvent(true);
        else if (eventOption == EventOptionType.Resume)
            chosenEvent.PauseEvent(false);
        else if (eventOption == EventOptionType.Stop)
            chosenEvent.StopEvent();
    }
}
