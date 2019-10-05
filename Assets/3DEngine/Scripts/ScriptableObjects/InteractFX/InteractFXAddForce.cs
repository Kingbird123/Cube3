using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddForce", menuName = "Data/Interacts/AddForce", order = 1)]
public class InteractFXAddForce : InteractFXDynamic
{
    public enum DirectionType { ClosestPointAngle, XZOnly, Override }
    public enum ForceDirection { Away, Towards }

    [SerializeField] private DirectionType directionType = DirectionType.Override;
    [SerializeField] private ForceDirection forceDirection = ForceDirection.Away;
    [SerializeField] private ForceMode forceMode = ForceMode.Impulse;
    [SerializeField] private Vector3 direction = Vector3.zero;
    [SerializeField] private bool disableUnitSpeed = false;
    [SerializeField] private float disableSpeedTime = 1;

    [SerializeField] private float force = 1;
    [SerializeField] private bool consistent = false;

    UnitController unit;

    protected override void AffectObject()
    {
        unit = affectedGameObject.GetComponent<UnitController>();
        var rb = affectedGameObject.GetComponent<Rigidbody>();
        var col = affectedGameObject.GetComponent<Collider>();
        var center = sender.transform.position;

        //gather direction information
        var dir = direction;
        if (directionType == DirectionType.ClosestPointAngle)
        {
            dir = (col.bounds.ClosestPoint(center) - center).normalized;
            if (forceDirection == ForceDirection.Towards)
                dir = (center - col.bounds.ClosestPoint(center)).normalized;
        }
        else if (directionType == DirectionType.XZOnly)
        {
            dir = col.transform.position - center;
            dir = new Vector3(dir.x, direction.y, dir.z).normalized;
        }

        if (unit && disableUnitSpeed)
            unit.DisableSpeedSmooth(disableSpeedTime);
        if (rb)
        {
            if (consistent)
                rb.Sleep();
            rb.AddForce(dir * force, forceMode);
        }
        else if (affectedGameObject)
            Debug.LogError("need a Rigidbody on: " + affectedGameObject.name + " for " + this + " to function!");

    }
}
