using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemAmount", menuName = "Data/Interacts/PickUpAmmo", order = 1)]
public class InteractFXPickUpAmmo : InteractFXDynamic
{
    [SerializeField] private bool setAmmo = false;
    [SerializeField] private EngineValueData ammoToPickUp = null;

    private UnitEquip equip;

    protected override void AffectObject()
    {
        equip = affectedGameObject.GetComponent<UnitEquip>();
        if (!equip)
            return;

        var ammo = ammoToPickUp;
        var finite = equip.CurItem as ItemFinite;
        if (finite)
        {
            if (!setAmmo)
                ammo = finite.LoadedAmmo.AmmoValue.Data;
            finite.LoadedAmmo.AmmoValue.ValueDelta(ammo.Value);
        }
            
    }
}
