using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EngineValueEvent
{
    public string eventName;
    public enum TriggerType { Changed, Increased, Decreased, Less, Greater, Equal, Empty, Full }
    public enum EventType { Damaged, Repaired, Die, Respawn, Lose, Win, ValueDelta, Custom }
    public TriggerType triggerType;
    public float compareValue;
    public int eventType;
    public float valueDelta;
    private float lastValue;
    private float minValue;
    private float maxValue;
    private EngineEntity owner;

    public void Initialize(EngineEntity _owner, EngineValueData _data)
    {
        owner = _owner;
        var floatData = _data as EngineFloatData;
        if (floatData)
        {
            maxValue = floatData.maxValue;
            minValue = floatData.minValue;
            lastValue = floatData.floatValue;
            return;
        }
        var intData = _data as EngineIntData;
        if (intData)
        {
            maxValue = intData.maxValue;
            minValue = intData.minValue;
            lastValue = intData.intValue;
            return;
        }
    }

    public void SyncEvent(EngineValue _engineValue)
    {
        _engineValue.valueChanged += CheckEvent;
    }

    public void CancelEvent(EngineValue _engineValue)
    {
        _engineValue.valueChanged -= CheckEvent;
    }

    void CheckEvent(object _value)
    {
        if (_value is float)
            DoTriggerFilter((float)_value);
        if (_value is int)
            DoTriggerFilter((int)_value);
    }

    public void DoTriggerFilter(float _curValue)
    {
        if (triggerType == TriggerType.Changed && _curValue != lastValue ||
            triggerType == TriggerType.Increased && _curValue > lastValue ||
            triggerType == TriggerType.Decreased && _curValue < lastValue ||
            triggerType == TriggerType.Less && _curValue < compareValue ||
            triggerType == TriggerType.Greater && _curValue > compareValue ||
            triggerType == TriggerType.Equal && _curValue == compareValue ||
            triggerType == TriggerType.Empty && _curValue <= minValue ||
            triggerType == TriggerType.Full && _curValue >= maxValue)
            DoEvents();

        lastValue = _curValue;
    }

    void DoEvents()
    {
        owner.Data.engineEventManager.DoEvents(eventType, owner.gameObject, owner.gameObject);
    }

}


