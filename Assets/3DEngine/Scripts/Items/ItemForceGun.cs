using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemForceGun : ItemAimable
{
    public new ItemForceGunData Data { get { return (ItemForceGunData)data; } }

    private Rigidbody2D rb;

    protected override void OnOwnerFound()
    {
        base.OnOwnerFound();
        rb = curUnitOwner.GetComponent<Rigidbody2D>();
    }


    void ForcePushUnit()
    {
        if (Data.speedType == ItemForceGunData.SpeedType.Consistent)
            rb.Sleep();

        //var dir = controller.AimDirection;
        //if (Data.directionType == ItemForceGunData.DirectionType.Backward)
            //dir = -controller.AimDirection;

        //var force = new Vector2(dir.x * Data.forcePowerHor, dir.y * Data.forcePowerUp);
        //if (dir.y < 0)
            //force = new Vector2(dir.x * Data.forcePowerHor, dir.y * Data.forcePowerDown);

        //if (Data.forceType == ItemForceGunData.ForceType.AddForce)
            //rb.AddForce(force, Data.forceMode);
        //else if (Data.forceType == ItemForceGunData.ForceType.Velocity)
            //rb.velocity = force;
    }

}
