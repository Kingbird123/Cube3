using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PhysicsSettings", menuName = "Data/Interacts/PhysicsSettings", order = 1)]
public class InteractFXPhysicsSettings : InteractFXDynamic
{
    //mask...names are in the editor script
    public enum PhysicsSettings { Mass, Drag, AngularDrag, UseGravity, IsKinematic, Interpolate, CollisionDetection, Constraints, IsTrigger, PhysicMaterial}
    [SerializeField] private int maskInd = 0;

    //rb vars
    [SerializeField] private float mass = 0;
    [SerializeField] private float drag = 0;
    [SerializeField] private float angularDrag = 0;
    [SerializeField] private bool useGravity = false;
    [SerializeField] private bool isKinematic = false;
    [SerializeField] private RigidbodyInterpolation interpolate = RigidbodyInterpolation.None;
    [SerializeField] private CollisionDetectionMode collisionDetection = CollisionDetectionMode.Discrete;
    [SerializeField] private RigidbodyConstraintsProperty constraints = null;

    //collider vars
    [SerializeField] private bool isTrigger = false;
    [SerializeField] private PhysicMaterial physicMaterial = null;

    protected override void AffectObject()
    {
        var rb = affectedGameObject.GetComponent<Rigidbody>();
        if (rb)
        {
            if (maskInd == (maskInd | (1 << 0)))
                rb.mass = mass;
            if (maskInd == (maskInd | (1 << 1)))
                rb.drag = drag;
            if (maskInd == (maskInd | (1 << 2)))
                rb.angularDrag = angularDrag;
            if (maskInd == (maskInd | (1 << 3)))
                rb.useGravity = useGravity;
            if (maskInd == (maskInd | (1 << 4)))
                rb.isKinematic = isKinematic;
            if (maskInd == (maskInd | (1 << 5)))
                rb.interpolation = interpolate;
            if (maskInd == (maskInd | (1 << 6)))
                rb.collisionDetectionMode = collisionDetection;
            if (maskInd == (maskInd | (1 << 7)))
                rb.constraints = constraints.constraints;
        }
        var col = affectedGameObject.GetComponent<Collider>();
        if (col)
        {
            if (maskInd == (maskInd | (1 << 8)))
                col.isTrigger = isTrigger;
            if (maskInd == (maskInd | (1 << 9)))
                col.material = physicMaterial;
        }
    }
}
