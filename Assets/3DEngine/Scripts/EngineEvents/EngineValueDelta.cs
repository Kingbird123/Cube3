using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EngineValueDelta
{
    public enum EngineValueType { Add, Subtract, Reset, Recharge, Overheat }
    public enum DeltaType { FromData, CustomFloat }
    public EngineValueDataManager valueDataManager;
    public EngineValueSelection valueSelection;
    public EngineValueType engineValueType;
    public DeltaType deltaType;
    public float valueDelta;
    public float rechargeSpeed;
    public float overheatTime;
    public EngineValueData engineValueData;

    public void DoValueDelta(EngineEntity _entity)
    {
        int id = valueSelection.valueData.ID;
        var val = _entity.EngineValueContainer.GetEngineValue(id);
        if (engineValueType == EngineValueType.Reset)
            _entity.ResetValueToDefault(id);
        else if (engineValueType == EngineValueType.Recharge)
            val.Recharge(rechargeSpeed);
        else if (engineValueType == EngineValueType.Overheat)
            val.OverHeat(overheatTime);
        else
        {
            var delta = valueDelta;
            if (deltaType == DeltaType.FromData)
            {
                if (engineValueData)
                    delta = engineValueData.FloatValue;
            }

            if (engineValueType == EngineValueType.Add)
            {
                if (valueSelection.valueData.GetType() == typeof(EngineFloatData))
                {
                    _entity.AddEngineFloatValue(id, delta);
                }
                else if (valueSelection.valueData.GetType() == typeof(EngineIntData))
                {
                    _entity.AddEngineIntValue(id, (int)delta);
                }
            }
            else if (engineValueType == EngineValueType.Subtract)
            {
                if (valueSelection.valueData.GetType() == typeof(EngineFloatData))
                {
                    _entity.SubtractEngineFloatValue(id, Mathf.Abs(delta));
                }
                else if (valueSelection.valueData.GetType() == typeof(EngineIntData))
                {
                    _entity.SubtractEngineIntValue(id, (int)Mathf.Abs(delta));

                }
            }
        }

    }
}
