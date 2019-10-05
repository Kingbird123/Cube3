using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRigidbody : Projectile
{
    public new ProjectileRigidbodyData Data { get { return (ProjectileRigidbodyData)data; } }

    protected Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected override void OnImpact(GameObject _obj = null)
    {
        base.OnImpact();
    }

    public override void ShootProjectile(float _speed, int _damage, Vector3 _direction, LayerMask _mask, Transform _sender = null, Transform _target = null, Vector2 _targetPos = default(Vector2), bool _bounced = false)
    {
        base.ShootProjectile(_speed, _damage, _direction, _mask, _sender, _target);
        LaunchProjectile();
    }

    public override void PauseProjectile(bool _pause)
    {
        base.PauseProjectile(_pause);
        rb.Sleep();
    }

    void LaunchProjectile()
    {
        rb.Sleep();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }
}
