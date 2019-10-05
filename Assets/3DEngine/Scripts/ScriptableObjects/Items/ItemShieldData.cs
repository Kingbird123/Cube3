using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield", menuName = "Data/Items/Tools/Shield", order = 1)]
public class ItemShieldData : ItemAimableData
{
    public InputProperty toggleShieldButton;
    public GameObject shieldPrefab;
    public float minDistance = 1;
    public float maxDistance = 5;
    public bool lockToModelPrefabHeight;
    public bool bounceProjectiles;
    public LayerMask bounceMask;
    public bool useIncomingForce;
    public float bounceForce;
    public bool changeDamageMaskAfterBounce;
    public LayerMask damageMask;
    public Vector2 detectSize;
    public Vector2 detectOffset;
    public bool grabObjects;
    public InputProperty grabButton;
    public bool deactivateShieldOnReload;
    public bool disableUnitMovementWhenShieldActive;
}
