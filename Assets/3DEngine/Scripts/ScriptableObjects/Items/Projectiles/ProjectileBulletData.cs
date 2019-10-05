using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Data/Items/Projectiles/Bullet", order = 1)]
public class ProjectileBulletData : ProjectileData
{
    public enum BounceAngleType { Reflect, Normal, Opposite }
    public bool enableRicochet;
    public float cornerDetectRadius = 0.3f;
    public bool doImpactOnRicochet;
    public BounceAngleType bounceAngleType;
}
