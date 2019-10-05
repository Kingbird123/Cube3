using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Items/Weapons/Push", order = 1)]
public class ItemWeaponPushData : ItemAimableData
{
    public LayerMask affectedMask;
    public LayerMask obstacleMask;
    public float force;
    public float upwardForce;
    public bool XZOnly;
    public float radius;
    public bool setAngle;
    public float angle;
    public AnimationCurve fallOffCurve = AnimationCurve.Linear(0, 1, 1, 0);
}
