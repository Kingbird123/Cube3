using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HookShot", menuName = "Data/Items/Tools/HookShot", order = 1)]
public class ItemHookShotData : ItemAimableData
{
    public enum DragType { StraightAuto, SwingControlled }
    public enum ShootType { HoldButton, Timed }

    public ShootType shootType;
    public InputProperty shootButton;
    public InputProperty cancelButton;
    public GameObject hookPrefab;
    public GameObject lineRenderer;
    public LayerMask damageMask;
    public float damage;
    public float damageRadius = 0.2f;
    public bool retractOnDamage;
    public float fireSpeed;
    public float lifeTime = 3;
    public DragType dragType;
    public float dragSpeed;
    public float controlSpeed = 5;
    public float minDistance = 0.5f;
    public LayerMask hookableSurfaceMask;
    public LayerMask cancelShotMask;
    public LayerMask obstacleCollisionMask;
    public float collisionRadius = 1;
}
