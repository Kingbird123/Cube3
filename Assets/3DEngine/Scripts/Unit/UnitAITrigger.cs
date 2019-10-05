using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnitAITrigger
{
    public string triggerName;

    public enum TriggerType { OnEnter, OnExit, OnStay, ValueAmount }
    public TriggerType triggerType;

    public enum EventLoadType { Sequence, RandomSingle }
    public EventLoadType eventLoadType;

    public enum UseType { Continuous, Limited }
    public UseType useType;
    public int amount;
    private int amountInd;

    public int detectZoneInd;

    //value Amounts
    public EngineValueDataManager engineValueManager;
    public EngineValueSelection valueSelection;
    public enum ValueOptions { Greater, Less, Equal }
    public ValueOptions valueOption;
    public float comparedValue;
    private Coroutine valueRoutine;

    //events
    public UnitAIEvent[] events;

    private Transform senderTrans;
    private bool triggerActive;

    public UnitAI CurAi { get; set; }
    private Unit unit;
    private UnitEquip equip;
    private Collider curTarget;

    //gui stuff
    public int currentFieldAmount;

    public void ActivateTriggerDetection(UnitAI _sender, bool _activate)
    {
        CurAi = _sender;
        equip = CurAi.GetComponent<UnitEquip>();
        unit = CurAi.GetComponent<Unit>();
        senderTrans = _sender.transform;

        switch (triggerType)
        {
            case TriggerType.OnEnter:
                DoEnterDetection(_activate);
                break;
            case TriggerType.OnExit:
                DoExitDetection(_activate);
                break;
            case TriggerType.OnStay:
                DoStayDetection(_activate);
                break;
            case TriggerType.ValueAmount:
                DoValueAmountDetection(_activate);
                break;
        }
    }

    void DoEnterDetection(bool _activate)
    {
        if (_activate)
            CurAi.detectZones[detectZoneInd].AddEnterTrigger(CurAi, TriggerEvents);
        else
            CurAi.detectZones[detectZoneInd].RemoveEnterTrigger(CurAi, TriggerEvents);
    }

    void DoExitDetection(bool _activate)
    {
        if (_activate)
            CurAi.detectZones[detectZoneInd].AddExitTrigger(CurAi, TriggerEvents);
        else
            CurAi.detectZones[detectZoneInd].RemoveExitTrigger(CurAi, TriggerEvents);
    }

    void DoStayDetection(bool _activate)
    {
        if (_activate)
            CurAi.detectZones[detectZoneInd].AddStayTrigger(CurAi, TriggerEvents);
        else
            CurAi.detectZones[detectZoneInd].RemoveStayTrigger(CurAi, TriggerEvents);
    }

    void DoValueAmountDetection(bool _activate)
    {
        if (_activate)
        {
            if (valueRoutine != null)
                CurAi.StopCoroutine(valueRoutine);
            valueRoutine = CurAi.StartCoroutine(StartValueAmountDetection());
        }

    }

    IEnumerator StartValueAmountDetection()
    {
        bool valueTriggered = false;
        var valueLocal = unit.GetLocalEngineValue(valueSelection.valueData.ID);
        while (!valueTriggered)
        {
            var value = valueLocal.Value;

            valueTriggered = valueOption == ValueOptions.Greater && (float)value > comparedValue ||
                             valueOption == ValueOptions.Equal && (float)value == comparedValue ||
                             valueOption == ValueOptions.Less && (float)value < comparedValue;

            yield return new WaitForEndOfFrame();
        }
        TriggerEvents();
    }

    void TriggerEvents(Collider _col = null)
    {
        if (_col)
        {
            //cancel event if target is the cur ai unit...ie self.
            if (_col.transform != CurAi.transform)
                curTarget = _col;
            else
                return;
        }

        if (eventLoadType == EventLoadType.Sequence)
        {
            for (int i = 0; i < events.Length; i++)
                events[i].DoEvent(this, i, _col);
        }
        else if (eventLoadType == EventLoadType.RandomSingle)
        {
            int rand = Random.Range(0,events.Length);
            events[rand].DoEvent(this, rand, _col);
        }
        

        if (useType == UseType.Limited)
        {
            amountInd++;
            Debug.Log(amountInd);
            if (amountInd >= amount)
                ActivateTriggerDetection(CurAi, false);
        }
    }

}