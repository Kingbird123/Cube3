using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnPool
{
    public static SpawnPoolManager Manager
    {
        get
        {
            if (SpawnPoolManager.instance)
                return SpawnPoolManager.instance;
            else
            {
                Debug.LogError("There is no Spawn Pool Manager data initialized! Make sure you add one to the GameManager!");
                return null;
            }
        }
    }

    public static GameObject Spawn2D(int _poolIndex, Vector2 _pos, Quaternion _rot, Transform _parent = null)
    {
        return Manager.SpawnObject(_poolIndex, _pos, _rot, _parent);
    }

    public static GameObject Spawn2D(string _objectToSpawn, Vector2 _pos, Quaternion _rot, Transform _parent = null)
    {
        return Manager.SpawnObject(_objectToSpawn, _pos, _rot, _parent);
    }

    public static GameObject Spawn2D(GameObject _objectToSpawn, Vector2 _pos, Quaternion _rot, Transform _parent = null)
    {
        return Manager.SpawnObject(_objectToSpawn, _pos, _rot, _parent);
    }

    public static GameObject Spawn(int _poolIndex, Vector3 _pos, Quaternion _rot, Transform _parent = null)
    {
        return Manager.SpawnObject(_poolIndex, _pos, _rot, _parent);
    }

    public static GameObject Spawn(string _objectToSpawn, Vector3 _pos, Quaternion _rot, Transform _parent = null)
    {
        return Manager.SpawnObject(_objectToSpawn, _pos, _rot, _parent);
    }

    public static GameObject Spawn(GameObject _objectToSpawn, Vector3 _pos, Quaternion _rot, Transform _parent = null)
    {
        return Manager.SpawnObject(_objectToSpawn, _pos, _rot, _parent);
    }

    public static void Unspawn(GameObject _objToUnspawn)
    {
        Manager.UnspawnObject(_objToUnspawn);
    } 
}
