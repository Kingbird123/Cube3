using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EngineFloat: EngineValue
{
    public new EngineFloatData Data { get { return (EngineFloatData)data; } }
    public float floatValue;
    public override object Value { get => floatValue; set => floatValue = Mathf.Clamp((float)value, minValue, maxValue); }
    protected float maxValue;
    public override object MaxValue { get => maxValue; set => maxValue = (float)value; }
    protected float minValue;
    public override object MinValue { get => minValue; set => minValue = (float)value; }

    public override void ValueDelta(object _amount)
    {
        base.ValueDelta((float)_amount);
    }

    public override void ValueMaxDelta(object _amount)
    {
        base.ValueMaxDelta((float)_amount);
    }

    public override void ValueMinDelta(object _amount)
    {
        base.ValueMinDelta((float)_amount);
    }

}
