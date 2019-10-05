using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponMelee", menuName = "Data/Items/Weapons/Melee", order = 1)]
public class ItemWeaponMeleeData : ItemAimableData
{
    public enum BounceType { ClosetPoints, XOnly, Override}
    public bool repeatUntilStopped;
    public bool allowSpamming;
    public int damage = 1;
    public float damageDelay = 0.3f;
    public float activeTime = 1;
    public BounceType bounceType;
    public Vector2 direction;
    public float bounceForce = 1;
    public int unitAmount = 1;
}
