using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EngineInt: EngineValue
{
    public new EngineIntData Data { get { return (EngineIntData)data; } }
    protected int intValue;
    public override object Value
    {
        get => intValue;
        set
        {
            var val = (int)value;
            if (val < minValue) intValue = minValue;
            else if (val > maxValue) intValue = maxValue;
            else intValue = val;
        }
    }
    protected int maxValue;
    public override object MaxValue { get => maxValue; set => maxValue = (int)value; }
    protected int minValue;
    public override object MinValue { get => minValue; set => minValue = (int)value; }


    public override void ValueDelta(object _amount)
    {
        base.ValueDelta((int)_amount);
    }

    public override void ValueMaxDelta(object _amount)
    {
        base.ValueMaxDelta((int)_amount);
    }

    public override void ValueMinDelta(object _amount)
    {
        base.ValueMinDelta((int)_amount);
    }
}
