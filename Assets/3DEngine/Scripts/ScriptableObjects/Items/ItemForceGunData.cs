using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ForceGun", menuName = "Data/Items/Tools/ForceGun", order = 1)]
public class ItemForceGunData : ItemAimableData
{
    public enum ForceType { AddForce, Velocity }
    public enum DirectionType { Forward, Backward }
    public enum SpeedType { Additive, Consistent }
    public float forcePowerUp = 15;
    public float forcePowerDown = 15;
    public float forcePowerHor = 15;
    public ForceType forceType;
    public ForceMode2D forceMode;
    public SpeedType speedType;
    public DirectionType directionType;
}
