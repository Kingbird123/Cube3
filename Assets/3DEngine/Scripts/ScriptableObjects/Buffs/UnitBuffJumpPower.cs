using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitJumpPower", menuName = "Data/Buffs/UnitBuffs/UnitJumpPower", order = 1)]
public class UnitBuffJumpPower : UnitBuff
{
    public float addedJumpPower = 10;

    public override void ActivateBuff(Unit _unit, bool _activate)
    {
        var cont = _unit.GetComponent<UnitController>();
        if (cont)
        {
            if (_activate)
                cont.JumpPower += addedJumpPower;
            else
                cont.JumpPower -= addedJumpPower;
        }
            
    }

}
