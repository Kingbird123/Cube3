using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitBuffInvincibility", menuName = "Data/Buffs/UnitBuffs/UnitBuffInvincibility", order = 1)]
public class UnitBuffInvincibility : UnitBuff
{
    public bool enableInvincibility;

    public override void ActivateBuff(Unit _unit, bool _activate)
    {
        _unit.IsInvincible = _activate == enableInvincibility;    
    }

}
