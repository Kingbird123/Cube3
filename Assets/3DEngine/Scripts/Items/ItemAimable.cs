using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemAimable : ItemFinite
{
    public new ItemAimableData Data { get { return (ItemAimableData)data; } }
    protected Transform muzzle;
    public Transform Muzzle { get { return muzzle; } }
    protected PlayerController playerController;
    protected UnitController unitController;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDisable()
    {
        if (UIPlayer.instance && Data.aimFX)
            UIPlayer.instance.RemoveAimFXHandler(Data.aimFX);
        base.OnDisable();
    }

    protected virtual void FixedUpdate()
    {
    }

    protected virtual void LateUpdate()
    {
    }

    protected override void OnOwnerFound()
    {
        base.OnOwnerFound();
        if (dropped)
            return;
        unitController = curUnitOwner.GetComponent<UnitController>();
        playerController = unitController as PlayerController;
        muzzle = transform.FindDeepChild(Data.muzzlePos);
        muzzle.forward = unitController.transform.forward;

        if (UIPlayer.instance && Data.aimFX)
            UIPlayer.instance.AddAimFXHandler(Data.aimFX);
    }

}
