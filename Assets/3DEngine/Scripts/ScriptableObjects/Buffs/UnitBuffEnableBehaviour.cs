using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitBuffEnableBehaviour", menuName = "Data/Buffs/UnitBuffs/UnitBuffEnableBehaviour", order = 1)]
public class UnitBuffEnableBehaviour : UnitBuff
{
    public IndexStringProperty behaviour = null;
    public bool enable = true;

    public override void ActivateBuff(Unit _unit, bool _activate)
    {
        Behaviour script = (Behaviour)_unit.GetComponent(behaviour.stringValue);
        if (script)
        {
            script.enabled = enable == _activate;
        }
        else
        {
            Debug.Log("Behaviour: " + behaviour + " not be found on " + _unit.gameObject.name);
        }
            
    }

}
