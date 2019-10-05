using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBuffEnableJump", menuName = "Data/Buffs/UnitBuffs/PlayerBuffEnableJump", order = 1)]
public class PlayerBuffEnableJump : UnitBuff
{
    public bool enableJump;

    public override void ActivateBuff(Unit _unit, bool _activate)
    {
        var cont = _unit.GetComponent<PlayerController>();
        if (cont)
        {
            cont.JumpEnabled = enableJump == _activate;
        }
            
    }

}
