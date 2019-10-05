using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataManager", menuName = "Data/Managers/ItemDataManager", order = 1)]
public class ItemDataManager : ScriptableObject
{
    public List<ItemData> items = new List<ItemData>();
    
    public ItemProperty GetItemProperty(ItemData _itemData)
    {
        return new ItemProperty
        {
            indexValue = GetItemIndex(_itemData),
            itemName = GetItemName(_itemData),
            itemNames = GetItemNames(),
        };
    }

    public int GetItemIndex(ItemData _itemData)
    {
        return items.IndexOf(_itemData);
    }

    public string GetItemName(ItemData _itemData)
    {
        foreach (var item in items)
        {
            if (item == _itemData)
                return item.name;
        }
        Debug.Log(_itemData.name + " not found in " + this);
        return null;
    }

    public ItemData GetItem(int _ind)
    {
        if (items.Count < 1)
        {
            Debug.Log(name + " Inventory is empty!");
            return null;
        }
            
        if (_ind < items.Count)
            return items[_ind];
        return null;
    }

    public string[] GetItemNames()
    {
        var names = new string[items.Count];
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i])
                names[i] = items[i].name;
            else
                names[i] = "[null]";
        }
        return names;
    }
}
