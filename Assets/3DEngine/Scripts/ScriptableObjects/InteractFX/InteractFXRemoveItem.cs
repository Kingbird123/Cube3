using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RemoveItem", menuName = "Data/Interacts/RemoveItem", order = 1)]
public class InteractFXRemoveItem: InteractFX
{
    [SerializeField] private ItemData itemToRemove = null;

    protected override void DoFX(GameObject _sender, GameObject _receiver)
    {
        _receiver.GetComponent<PlayerEquip>().RemoveItem(itemToRemove);
    }
   
}
