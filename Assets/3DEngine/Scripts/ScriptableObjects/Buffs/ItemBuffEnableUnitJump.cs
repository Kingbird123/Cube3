using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemEnableUnitJump", menuName = "Data/Buffs/ItemBuffs/ItemEnableUnitJump", order = 1)]
public class ItemBuffEnableUnitJump : ItemBuff
{
    public bool enableJump;

    public override void ActivateBuff(Item _item, bool _activate)
    {
        var owner = _item.UnitOwner;
        if (!owner)
            return;

        var cont = owner.GetComponent<UnitController>();
        if (cont)
            cont.JumpEnabled = enableJump == _activate;

    }
}
