using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PickUpItem", menuName = "Data/Interacts/PickUpItem", order = 1)]
public class InteractFXPickUpItem: InteractFX
{
    [SerializeField] private bool pickupSender = false;
    [SerializeField] private ItemData itemToPickUp = null;
    [SerializeField] private bool setItemSlot = false;
    [SerializeField] private int slot = -1;
    [SerializeField] private bool cancelIfFull = false;
    [SerializeField] private bool destroySenderAfterPickup = true;
    [SerializeField] private float destroyDelay = 0;

    protected override void DoFX(GameObject _sender, GameObject _receiver)
    {
        Item item = null;
        if (pickupSender)
            item = _sender.GetComponent<Item>();
        else if (itemToPickUp)
            item = itemToPickUp.connectedPrefab.GetComponent<Item>();

        var equip = _receiver.GetComponent<UnitEquip>();
        if (equip)
        {
            if (!setItemSlot)
                slot = -1;
            if (equip.IsFull && cancelIfFull)
            {
                Debug.Log(equip + " is full!");
                return;
            }
            else
            {
                equip.PickupItem(item, slot);
                if (destroySenderAfterPickup)
                    Destroy(_sender, destroyDelay);
            }
                
            
        }     
        else
            Debug.Log("No Equip component found on receiver! Adding item failed");

        
    }
   
}
