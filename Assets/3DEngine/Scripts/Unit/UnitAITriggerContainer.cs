using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnitAITriggerContainer
{
    public List<UnitAITrigger> unitAITriggers = new List<UnitAITrigger>();

    private UnitAI curAI;

    public void ActivateAllEvents(UnitAI _sender, bool _activate)
    {
        if (unitAITriggers.Count < 1)
            return;

        curAI = _sender;

        for (int i = 0; i < unitAITriggers.Count; i++)
        {
            ActivateEvent(i, _activate);
        }
    }

    void ActivateEvent(int _ind, bool _activate)
    {
        unitAITriggers[_ind].ActivateTriggerDetection(curAI, _activate);
    }
}