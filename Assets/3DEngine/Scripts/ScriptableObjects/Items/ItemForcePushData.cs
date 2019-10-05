using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ForcePush", menuName = "Data/Items/Tools/ForcePush", order = 1)]
public class ItemForcePushData : ItemAimableData
{
    [System.Serializable]
    public class PhysicsProperty
    {
        public IndexStringProperty button;
        public float force;
        public Collider2D forceArea;
        public bool allowHolding;
        public bool consistentForce;
    }
    public PhysicsProperty pushProperty;
    public PhysicsProperty pullProperty;
    public LayerMask mask;
    public int maxObjects = 5;
}
