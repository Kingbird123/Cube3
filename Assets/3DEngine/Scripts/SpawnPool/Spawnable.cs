using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spawnable
{
    public int poolIndex;
    public string prefabName;
    public string[] poolListNames;
    public SpawnPoolManager poolData;
    public GameObject prefabToSpawn;
    public bool overrideManager;
}
