using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemMaxAmount", menuName = "Data/Buffs/ItemBuffs/ItemMaxAmount", order = 1)]
public class ItemBuffMaxAmount : ItemBuff
{
    public float addToMaxValue = 1;
    public bool addToCurValue = true;

    public override void ActivateBuff(Item _item, bool _activate)
    {
        var useable = (ItemFinite)_item;
        if (useable)
        {
            var ammo = useable.LoadedAmmo.AmmoValue;
            if (ammo != null)
            {
                if (_activate)
                    ammo.ValueMaxDelta(addToMaxValue);
                else
                    ammo.ValueMaxDelta(-addToMaxValue);

                if (addToCurValue)
                {
                    if (_activate)
                        ammo.ValueDelta(addToMaxValue);
                    else
                        ammo.ValueDelta(-addToMaxValue);
                }
            }
            
        }
        
            

    }

}
