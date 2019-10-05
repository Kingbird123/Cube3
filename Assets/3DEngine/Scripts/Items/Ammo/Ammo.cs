using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Ammo : Item
{
    public new AmmoData Data { get { return (AmmoData)data; } }
    protected EngineValue ammoValue;
    public EngineValue AmmoValue { get { return ammoValue; } }
    protected bool loaded;

    private Transform muzzle;
    private Vector3 pos;
    private Vector3 dir;
    private float speed;
    private float distance;
    private float damage;
    private LayerMask mask;

    protected override void LoadDefaultData()
    {
        base.LoadDefaultData();
        //get ammo value from container
        ammoValue = engineValueContainer.GetEngineValue
            (Data.engineValueSelections[Data.ammoSelection].valueSelection.valueData.ID);
    }

    public virtual void LoadAmmo(bool _load)
    {
        loaded = _load;
        if (!loaded)
        {
            if (ui)
                Destroy(ui.gameObject);
        }
    }

    public virtual void ShootAmmo(Vector3 _pos, Vector3 _dir, Transform _muzzle, float _speed, float _distance, float _damage, LayerMask _mask)
    {
        pos = _pos;
        dir = _dir;
        muzzle = _muzzle;
        speed = _speed;
        distance = _distance;
        damage = _damage;
        mask = _mask;

        if (Data.spreadType != AmmoData.SpreadType.Straight)
        {
            float ang = Data.angle / (Data.fireAmount - 1);
            float curRot = Data.angle / 2;
            if (Data.fireAmount > 1)
            {
                for (int i = 0; i < Data.fireAmount; i++)
                {
                    if (Data.spreadType == AmmoData.SpreadType.Random)
                    {
                        ang = Random.Range(ang - Data.randomAmount, ang + Data.randomAmount);
                        curRot = Random.Range(curRot - Data.randomAmount, curRot + Data.randomAmount);
                    }
                    if (i == 0)
                        muzzle.Rotate(0, 0, -curRot);
                    else
                        muzzle.Rotate(0, 0, ang);

                    dir = muzzle.forward;
                    Shoot();
                }
            }
        }
        else
            Shoot();

        SubtractAmmoValue(Data.removeAmount);
    }

    protected virtual void Shoot()
    {
        if (Data.projectileType == AmmoData.ProjectileType.Instant)
            ShootInstant();
        else
            ShootProjectile();
    }

    Projectile ShootProjectile()
    {
        var spawn = SpawnPool.Spawn(Data.projectile.connectedPrefab, pos, Quaternion.Euler(dir));
        if (spawn)
        {
            var proj = spawn.GetComponent<Projectile>();
            proj.ShootProjectile(speed, (int)damage, dir, mask, transform);
            return proj;
        }
        return null;
    }

    void ShootInstant()
    {
        var hitInfo = new RaycastHit();
        var hit = Physics.Raycast(pos, dir, out hitInfo, distance, mask);
        if (hit)
        {
            var unit = hitInfo.collider.GetComponent<Unit>();
            //if (unit)
            //unit.DamageHp(Data.damage);
        }
    }

    protected virtual void SubtractAmmoValue(float _amount)
    {
        ammoValue.ValueDelta(- _amount);
    }

}
