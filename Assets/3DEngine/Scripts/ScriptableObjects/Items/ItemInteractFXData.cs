using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemInteractFX", menuName = "Data/Items/Tools/ItemInteractFX", order = 1)]
public class ItemInteractFXData : ItemAimableData
{
    public InteractFX[] interacts;
    public bool runInteractsOnOwner = true;
}
