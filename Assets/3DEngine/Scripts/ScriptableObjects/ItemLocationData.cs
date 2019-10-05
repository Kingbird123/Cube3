using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemLocation", menuName = "Data/ItemLocation", order = 1)]
public class ItemLocationData : ScriptableObject
{
    [System.Serializable]
    public class ItemLocationProperty
    {
        public string itemName;
    }

    public ItemLocationProperty[] itemLocationProperties;


    public string[] GetItemLocationNames()
    {
        var names = new string[itemLocationProperties.Length];
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = itemLocationProperties[i].itemName;
        }
        return names;
    }
}
