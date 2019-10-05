using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Draggable", menuName = "Data/Items/Tools/Draggable", order = 1)]
public class ItemDraggableData : ItemAimableData
{
    public InputProperty dragButton;
    public float removeAmmoSpeed = 1;
    public DetectZone dragZone;
    public float dragSensitivity = 10;
    public bool sensitivityByWeight;
    public float minWeight = 0.1f;
    public float maxWeight = 3;
    public float throwForce;
    public bool consistentThrowPower;
    public bool allowDragThroughColliders;
}
