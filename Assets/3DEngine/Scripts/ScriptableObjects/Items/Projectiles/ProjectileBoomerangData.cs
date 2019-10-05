using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boomerang", menuName = "Data/Items/Projectiles/Boomerang", order = 1)]
public class ProjectileBoomerangData : ProjectileData
{
    public AnimationCurve throwCurve;
    public AnimationCurve returnCurve;
    public float rotateSpeed;
    public bool disableRenderersOnSender = true;
}
