using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponRanged", menuName = "Data/Items/Weapons/Ranged", order = 1)]
public class ItemWeaponRangedData : ItemAimableData
{
    public enum FireType { Single, Repeated }

    public FireType fireType;
    public int damage;
    public bool instantFirstShot = true;
    public float fireDelay;
    public float fireDistance;
    public float projectileSpeed;
    public bool aimTransformAtTarget;
    public ChildName aimTrans;
    public LayerMask mask;
}
