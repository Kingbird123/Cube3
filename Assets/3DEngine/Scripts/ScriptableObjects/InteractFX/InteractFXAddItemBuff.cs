using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddItemBuff", menuName = "Data/Interacts/AddItemBuff", order = 1)]
public class InteractFXAddItemBuff : InteractFX
{
    [SerializeField] private ItemBuff buffToAdd = null;

    protected override void DoFX(GameObject _sender, GameObject _receiver)
    {
        var item = _receiver.GetComponentInChildren<Item>();
        if (item)
        {
            item.AddBuff(buffToAdd);
        }
    }
}
