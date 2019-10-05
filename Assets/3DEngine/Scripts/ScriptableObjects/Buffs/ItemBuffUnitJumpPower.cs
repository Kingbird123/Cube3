using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemUnitJumpPower", menuName = "Data/Buffs/ItemBuffs/ItemUnitJumpPower", order = 1)]
public class ItemBuffUnitJumpPower : ItemBuff
{
    public float jumpPowerDelta;

    public override void ActivateBuff(Item _item, bool _activate)
    {
        var owner = _item.UnitOwner;
        if (!owner)
            return;

        var cont = owner.GetComponent<UnitController>();
        if (_activate)
            cont.JumpPower += jumpPowerDelta;
        else
            cont.JumpPower -= jumpPowerDelta;

    }
}
