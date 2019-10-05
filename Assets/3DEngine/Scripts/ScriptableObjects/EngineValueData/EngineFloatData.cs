using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EngineFloat", menuName = "Data/EngineValue/EngineFloat", order = 1)]
public class EngineFloatData : EngineValueData
{
    public float floatValue = 0;
    public float minValue = 0;
    public float maxValue = 0;
    public override object Value { get => floatValue; set => floatValue = (float)value; }
    public override object MaxValue { get => maxValue; set => maxValue = (float)value; }
    public override object MinValue { get => minValue; set => minValue = (float)value; }
}
