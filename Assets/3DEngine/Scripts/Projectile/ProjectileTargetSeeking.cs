using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTargetSeeking : ProjectileBullet
{
    public new ProjectileTargetSeekingData Data { get { return (ProjectileTargetSeekingData)data; } }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        SeekTarget();
    }

    void SeekTarget()
    {
        if (target)
            targetPos = target.position;

        if (!bounced)
            transform.LookAt2D(targetPos, false, Data.keepYFacingUp);
    }

}
