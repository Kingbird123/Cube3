using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEnableDash", menuName = "Data/Buffs/UnitBuffs/PlayerEnableDash", order = 1)]
public class PlayerBuffEnableDash : UnitBuff
{
    public float addedJumpPower = 10;

    public override void ActivateBuff(Unit _unit, bool _activate)
    {
        var cont = (PlayerController)_unit.GetComponent<UnitController>();
        if (cont)
        {
            cont.DashEnabled = _activate;
        }
        else
        {
            Debug.Log("No PlayerController found on " + _unit.gameObject);
        }
            
    }

}
