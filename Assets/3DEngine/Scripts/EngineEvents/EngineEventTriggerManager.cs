using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineEventTriggerManager : MonoBehaviour
{
    [System.Serializable]
    public struct PreTrigger
    {
        public string preTriggerName;
        public bool activated;
    }

    public enum TriggerType { DetectZones, Receiver, PreTrigger }
    [SerializeField] protected TriggerType triggerType;
    [SerializeField] protected DetectZone[] detectZones;
    public DetectZone[] DetectZones { get { return detectZones; } }
    [SerializeField] protected PreTrigger[] preTriggers;
    public int PreTriggerAmount { get { return preTriggers.Length; } }
    [SerializeField] protected EngineEventTrigger[] triggers;
    public EngineEventTrigger[] Triggers { get { return triggers; } }

    private void Start()
    {
        ActivateAllTriggers();
    }

    public void ActivateAllTriggers()
    {
        for (int i = 0; i < triggers.Length; i++)
        {
            triggers[i].Activate(this, i);
        }
    }

    public void ActivateTriggerEvents(int _triggerIndex, GameObject _receiver = null)
    {
        var events = triggers[_triggerIndex].EngineEvents;
        for (int i = 0; i < events.Length; i++)
        {
            events[i].DoEvent(gameObject, events, i, _receiver);
        }
    }

    public void ActivateAllTriggerEvents(GameObject _receiver = null)
    {
        for (int i = 0; i < triggers.Length; i++)
        {
            var events = triggers[i].EngineEvents;
            for (int ind = 0; ind < events.Length; ind++)
            {
                events[ind].DoEvent(gameObject, events, ind, _receiver);
            }
        }
    }

    public void ActivatePreTrigger(int _preTriggerInd, bool _activated, GameObject _receiver = null)
    {
        preTriggers[_preTriggerInd].activated = _activated;

        if (AllPreTriggersActivated())
            ActivateAllTriggerEvents(_receiver);
    }

    public bool AllPreTriggersActivated()
    {
        return PreTriggersActivated() >= preTriggers.Length;
    }

    public int PreTriggersActivated()
    {
        int active = 0;
        for (int i = 0; i < preTriggers.Length; i++)
        {
            if (preTriggers[i].activated)
                active++;
        }
        return active;
    }

    public string[] GetPreTriggerNames()
    {
        string[] names = new string[preTriggers.Length];
        for (int i = 0; i < preTriggers.Length; i++)
        {
            names[i] = preTriggers[i].preTriggerName;
        }
        return names;
    }

    public string[] GetTriggerNames()
    {
        string[] names = new string[triggers.Length];
        for (int i = 0; i < triggers.Length; i++)
        {
            names[i] = triggers[i].TriggerName;
        }
        return names;
    }

    public string[] GetDetectZoneNames()
    {
        string[] names = new string[detectZones.Length];
        for (int i = 0; i < detectZones.Length; i++)
        {
            names[i] = detectZones[i].zoneName;
        }
        return names;
    }
}
