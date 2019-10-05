using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RemoveItemBuff", menuName = "Data/Interacts/RemoveItemBuff", order = 1)]
public class InteractFXRemoveItemBuff : InteractFX
{
    [SerializeField] private ItemBuff buffToRemove = null;

    protected override void DoFX(GameObject _sender, GameObject _receiver)
    {
        var item = _receiver.GetComponentInChildren<Item>();
        if (item)
        {
            item.RemoveBuff(buffToRemove);
        }
    }
}
