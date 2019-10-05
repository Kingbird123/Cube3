using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDetectInteractFX : DetectZoneTrigger
{
    [SerializeField] protected InteractFX[] interacts;

    protected override void OnEnter(Collider _col)
    {
        base.OnEnter(_col);
        foreach (var fx in interacts)
        {
            fx.ActivateFX(gameObject, _col.gameObject);
        }
    }

}
