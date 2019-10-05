using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPoolManager", menuName = "Data/Managers/SpawnPoolManager", order = 1)]
public class SpawnPoolManager: ScriptableObject
{
    public static SpawnPoolManager instance;

    [SerializeField] private List<Pool> pools = new List<Pool>();

    private Transform rootFolder;

    public void Initialize()
    {
        instance = this;
        InitializePools();
    }

    void InitializePools()
    {
        //create a master "folder" empty
        rootFolder = new GameObject().transform;
        rootFolder.name = "SpawnPools";

        //create child folders and spawn in our pool objects
        for (int i = 0; i < pools.Count; i++)
        {
            pools[i].InitializePool(rootFolder);
        }
    }


    public GameObject SpawnObject(int _poolIndex, Vector3 _pos, Quaternion _rot, Transform _parent = null)
    {
        if (pools.Count < 1)
            return null;
        return pools[_poolIndex].SpawnObject(_pos, _rot, _parent);
    }

    public GameObject SpawnObject(GameObject _objToSpawn, Vector3 _pos, Quaternion _rot, Transform _parent = null)
    {
        return SpawnObject(_objToSpawn.name, _pos, _rot, _parent);
    }

    public GameObject SpawnObject(string _objToSpawn, Vector3 _pos, Quaternion _rot, Transform _parent = null)
    {
        var pool = FindPool(_objToSpawn);
        if (pool != null)
        {
            return pool.SpawnObject(_pos, _rot, _parent);
        }
        else
            return null;
    }

    public void UnspawnObject(GameObject _objToUnspawn)
    {
        var pool = FindPool(_objToUnspawn.name);
        if (pool != null)
        {
            pool.UnspawnObject(_objToUnspawn);
        }
    }

    Pool FindPool(string _name)
    {
        var pool = pools.Find(obj => obj.prefabToSpawn.name == _name);
        if (pool != null)
            return pool;
        else
        {
            Debug.LogError("No match found for: " + _name + " in the poolmanager!");
            return null;
        }
        
    }

    public GameObject GetPoolPrefabToSpawn(int _ind)
    {
        return pools[_ind].prefabToSpawn;
    }

    public string[] GetPoolNames()
    {
        var names = new string[pools.Count];
        for (int i = 0; i < pools.Count; i++)
        {
            names[i] = pools[i].prefabToSpawn.name;
        }
        return names;
    }
}
