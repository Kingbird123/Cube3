using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoContainer", menuName = "Data/Items/AmmoData", order = 1)]
public class AmmoData : ItemData
{
    public enum SpreadType { Straight, Angle, Random }
    public enum ProjectileType { Instant, Projectile }

    public int ammoSelection;
    public ProjectileType projectileType;
    public ProjectileData projectile;
    public SpreadType spreadType;
    public int removeAmount;
    public int fireAmount;
    public float angle;
    public float randomAmount;
}
