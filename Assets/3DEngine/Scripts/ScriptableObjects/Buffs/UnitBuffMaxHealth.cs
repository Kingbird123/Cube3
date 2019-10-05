using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitMaxHealth", menuName = "Data/Buffs/UnitBuffs/UnitMaxHealth", order = 1)]
public class UnitBuffMaxHealth : UnitBuff
{
    public int addedHp = 10;

    public override void ActivateBuff(Unit _unit, bool _activate)
    {
            
    }

}
