using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Torch", menuName = "Data/Items/Tools/Torch", order = 1)]
public class ItemTorchData : ItemAimableData
{
    public float lightMaxRange = 3;
    public float lightDrainSpeed = 1;
    public float hpDrainSpeed = 0.2f;
}
