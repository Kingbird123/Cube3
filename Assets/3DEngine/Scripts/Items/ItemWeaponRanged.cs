using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class ItemWeaponRanged : ItemAimable, IUsable
{
    public new ItemWeaponRangedData Data { get { return (ItemWeaponRangedData)data; } }
    [SerializeField] private InputProperty fireButton = null;
    private CoroutineHandle fireCoroutine;

    private void Update()
    {
        GetInputs();
    }

    void GetInputs()
    {
        if (mm)
        {
            if (mm.IsPaused)
                return;
        }

        if (inputType == UnitEquip.InputType.None)
            return;

        if (fireButton.GetInputDown())
            Use();
    }

    public void Use()
    {
        if (Data.fireType == ItemWeaponRangedData.FireType.Single)
                UseWeapon();
        else
            fireCoroutine = fireCoroutine.ReplayCoroutine(StartFire());
        
    }

    protected override void Stop()
    {
        if (fireCoroutine != null)
            Timing.KillCoroutines(fireCoroutine);
        base.Stop();
    }

    IEnumerator<float> StartFire()
    {
        bool active = true;
        while (active)
        {
            if (inputType == UnitEquip.InputType.User)
                active = fireButton.GetInput();

            UseWeapon();
                
            yield return Timing.WaitForOneFrame;
        }
        Stop();
    }

    void UseWeapon()
    {
        if (!IsFireReady)
            return;

        FireWeapon();
        Recoil(true);
    }

    protected virtual void FireWeapon()
    {
        if (!muzzle)
            return;

        if (!IsFireReady)
            return;

        if (ownerAnim)
            ownerAnim.PlayRangedAttack();

        muzzle.LookAt(unitController.AimPosition);

        //fx
        if (Data.particleFX)
            Instantiate(Data.particleFX, muzzle.position, muzzle.rotation);

        if (curAmmo)
            curAmmo.ShootAmmo(muzzle.position, muzzle.forward, muzzle, Data.projectileSpeed, Data.fireDistance, Data.damage, Data.mask);

    }

    

    //protected override void DrawLine(Vector3 _endPos)
    //{
    //    if (!Data.aimFX)
    //        return;
    //    var projRb = Data.projectile.GetComponent<ProjectileRigidbody>();
    //    if (projRb)
    //    {
    //        DrawArcPrediction();
    //    }
    //    else
    //        base.DrawLine(_endPos);
    //}

    //void DrawArcPrediction()
    //{
    //    int steps = (int)(Data.aimDistance * Data.projectileSpeed);
    //    line.positionCount = steps;
    //    var rb = Data.projectile.GetComponent<Rigidbody>();
    //    var plots = Utils.PhysicsPredictionPoints(muzzle.position, controller.AimDirection * Data.projectileSpeed, rb.drag, steps);
    //    line.SetPositions(plots);
    //}

}
