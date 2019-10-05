using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileBullet : Projectile
{
    public new ProjectileBulletData Data { get { return (ProjectileBulletData)data; } }

    private Vector3 dir;

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        MoveForward();
    }

    void MoveForward()
    {
        if (!paused)
            transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }

    public override void ShootProjectile(float _speed, int _damage, Vector3 _direction, LayerMask _mask, Transform _sender = null, Transform _target = null, Vector2 _targetPos = default, bool _bounced = false)
    {
        base.ShootProjectile(_speed, _damage, _direction, _mask, _sender, _target, _targetPos, _bounced);
        dir = _direction.normalized;
    }

    protected override void OnImpact(GameObject _obj = null)
    {
        if (!Data.enableRicochet)
            base.OnImpact(_obj);
        else
        {
            if (Data.doImpactOnRicochet)
                base.OnImpact(_obj);

            //check if potential ricochet point is too close to another object. ie corner.
            var norm = rayHit.normal;
            var reflectDir = Vector3.Reflect(velocityDirection, rayHit.normal).normalized;
            var ray = new Ray(rayHit.point, reflectDir);
            var info = new RaycastHit();
            if (Physics.Raycast(ray, Data.cornerDetectRadius, mask))
            {
                //change the normal so a corner becomes "flat"
                norm = (info.point - rayHit.point).normalized;
            }

            if (Data.bounceAngleType == ProjectileBulletData.BounceAngleType.Opposite)
                dir = -dir;
            else if (Data.bounceAngleType == ProjectileBulletData.BounceAngleType.Normal)
                dir = norm;
            else if (Data.bounceAngleType == ProjectileBulletData.BounceAngleType.Reflect)
                dir = Vector3.Reflect(velocityDirection, norm).normalized;
        }
        
    }


}
