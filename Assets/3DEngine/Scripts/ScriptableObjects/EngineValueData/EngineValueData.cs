using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

public abstract class EngineValueData : ScriptableObject
{
    public EngineValueUIType valueUIType;
    public abstract object MinValue { get; set; }
    public abstract object MaxValue { get; set; }
    public abstract object Value { get; set; }
    [HideInInspector] public int id;
    public int ID { get { return id; } }
    public virtual void SetID(int _id) { id = _id; }

    public float FloatValue { get { return ConvertValueToFloat(Value); } }
    public float FloatMaxValue { get { return ConvertValueToFloat(MaxValue); } }
    public float FloatMinValue { get { return ConvertValueToFloat(MinValue); } }

    public virtual EngineValue CreateEngineValue()
    {
        var val = new EngineValue
        {
            MinValue = MinValue,
            MaxValue = MaxValue,
            Value = Value
        };
        return val;
    }

    protected virtual float ConvertValueToFloat(object _value)
    {
        if (_value is float) return (float)_value;
        if (_value is int) return (int)_value;
        Debug.LogError("Could not convert value!");
        return 0;
    }
}
