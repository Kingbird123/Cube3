using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickMenuUI : UIEngineValueTally
{
    [SerializeField] private Sprite emptySlotSprite = null;

    public void UpdateQuickMenuItems(ItemData[] _items)
    {
        if (curSlots.Count != _items.Length)
            SetCurValue(_items.Length);

        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i])
                curSlots[i].SetIcon(_items[i].avatarIcon);
            else
                curSlots[i].SetIcon(emptySlotSprite);
        }
    }

    public void EquipToSlot(ItemData _item, int _ind)
    {
        if (!_item)
            curSlots[_ind].SetIcon(emptySlotSprite);
        else
            curSlots[_ind].SetIcon(_item.avatarIcon);
    }
}
