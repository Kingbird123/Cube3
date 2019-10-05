using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : Item
{
    public new ProjectileData Data { get { return (ProjectileData)data; } }
    [SerializeField] protected DetectZone detectZone;
    private float lifeTimer;
    private float velCheckDelayTimer;
    protected float speed;
    public float Speed { get { return speed; } }
    protected int damage;
    public int Damage { get { return damage; } }
    protected LayerMask mask;
    public LayerMask Mask { get { return mask; } }
    protected Transform target;
    public Transform Target { get { return target; } }
    protected Vector3 targetPos;
    public Vector3 TargetPos { get { return targetPos; } }
    protected float velocitySpeed;
    public float VelocitySpeed { get { return velocitySpeed; } }
    protected Vector3 velocityDirection;
    public Vector3 Direction { get { return velocityDirection; } }
    protected Transform sender;
    public Transform Sender { get { return sender; } }

    private Vector3 lastPos;

    protected RaycastHit rayHit;
    private int curhitAmount;

    protected bool bounced;
    protected bool paused;

    private bool impacted;

    public virtual void FixedUpdate()
    {
        CalculateSpeed();
        Steer();
        DetectCollision();
        DetectTime();
        DetectVelocity();
    }

    //called externally from weapons
    public virtual void ShootProjectile(float _speed, int _damage, Vector3 _direction, LayerMask _mask, Transform _sender = null, Transform _target = null, Vector2 _targetPos = default(Vector2), bool _bounced = false)
    {
        speed = _speed;
        damage = _damage;
        mask = _mask;
        detectZone.detectMask = mask;
        sender = _sender;
        target = _target;
        targetPos = _targetPos;
        velocityDirection = _direction;
        bounced = _bounced;
        transform.rotation = Quaternion.LookRotation(_direction);

        //do launching interactFX
        DoLaunchFX();
    }

    void CalculateSpeed()
    {
        velocityDirection = transform.position - lastPos;
        velocitySpeed = (velocityDirection / Time.deltaTime).magnitude;

        lastPos = transform.position;
    }

    void Steer()
    {
        if (!Data.steerable || impacted)
            return;

        var inputHor = Input.GetAxis("Horizontal");
        var inputVer = Input.GetAxis("Vertical");
        var moveX = inputHor * Data.horSpeed * Time.deltaTime;
        var moveY = inputVer * Data.verSpeed * Time.deltaTime;
        var move = new Vector3(moveX, moveY, 0);
        transform.Translate(move);
    }

    void DetectTime()
    {
        if (Data.impactType != ProjectileData.OnImpactType.Timer)
            return;

        lifeTimer += Time.deltaTime;
        if (lifeTimer > Data.time)
        {
            OnImpact();
        }
    }

    void DetectVelocity()
    {
        if (Data.impactType != ProjectileData.OnImpactType.Velocity)
            return;

        velCheckDelayTimer += Time.deltaTime;
        if (velCheckDelayTimer < 0.5f)
            return;

        if (velocitySpeed == Data.velocity && Data.triggerType == ProjectileData.CompareType.Equal ||
            velocitySpeed > Data.velocity && Data.triggerType == ProjectileData.CompareType.Greater ||
            velocitySpeed < Data.velocity && Data.triggerType == ProjectileData.CompareType.Less)
            OnImpact();

    }

    void DetectCollision()
    {
        if (Data.impactType != ProjectileData.OnImpactType.DetectZone)
            return;

        var hit = detectZone.DetectCollider(transform, out rayHit);

        if (hit)
        {
            if (!impacted)
            {
                OnImpact(hit.gameObject);
                impacted = true;
            }

        }
        else if (impacted)
            impacted = false;
    }

    protected virtual void OnImpact(GameObject _obj = null)
    {
        if (Data.spawnOnImpact.Length > 0)
            DoSpawnOnImpact();

            DoImpactFX(_obj);

        if (Data.destroySelfType == ProjectileData.DestroyType.OnImpact)
            KillProjectile();
        else if (Data.destroySelfType == ProjectileData.DestroyType.HitAmount)
        {
            curhitAmount++;
            if (curhitAmount >= Data.hitMaxAmount)
                KillProjectile();
        }
    }

    void DoSpawnOnImpact()
    {
        foreach (var spawn in Data.spawnOnImpact)
        {
            var spawned = SpawnPool.Spawn(spawn, rayHit.point, Quaternion.identity);
            spawned.transform.rotation = Quaternion.FromToRotation(spawned.transform.up, rayHit.normal);
        }
    }

    void DoLaunchFX()
    {
        for (int i = 0; i < Data.launchFX.Length; i++)
        {
            Data.launchFX[i].DoEvent(gameObject, Data.launchFX, i, this.gameObject);
        }
    }

    void DoImpactFX(GameObject _reciever)
    {
        for (int i = 0; i < Data.impactFX.Length; i++)
        {
            Data.impactFX[i].DoEvent(gameObject, Data.impactFX, i, _reciever);
        }
    }

    public virtual void PauseProjectile(bool _pause)
    {
        paused = _pause;
    }

    public virtual void KillProjectile()
    {
        Destroy(this.gameObject);
    }

}
