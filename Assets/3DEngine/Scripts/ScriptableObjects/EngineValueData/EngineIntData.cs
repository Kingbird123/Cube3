using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EngineInt", menuName = "Data/EngineValue/EngineInt", order = 1)]
public class EngineIntData : EngineValueData
{
    public int intValue = 0;
    public int minValue = 0;
    public int maxValue = 0;
    public override object Value { get => intValue; set => intValue = (int)value; }
    public override object MaxValue { get => maxValue; set => maxValue = (int)value; }
    public override object MinValue { get => minValue; set => minValue = (int)value; }

}
