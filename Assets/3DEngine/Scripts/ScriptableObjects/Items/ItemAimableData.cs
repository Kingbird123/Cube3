using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAimableData : ItemFiniteData
{
    public LayerMask aimMask;
    public float aimDistance = 10;
    public Vector3 aimOffset;
    public string muzzlePos;
    public int muzzlePosInd;
    public AimReticalFX aimFX;
}
