using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityDataManager", menuName = "Data/Managers/EntityDataManager", order = 1)]
public class EntityDataManager : ScriptableObject
{
    public EntityInfo[] entities;

    public string[] GetEntityNames()
    {
        var names = new string[entities.Length];
        for (int i = 0; i < entities.Length; i++)
        {
            names[i] = entities[i].entityName;
        }
        return names;
    }
}
