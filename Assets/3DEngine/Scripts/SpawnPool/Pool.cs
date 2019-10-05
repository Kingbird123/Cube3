using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public GameObject prefabToSpawn;
    public int amountToSpawn;
    public bool createMoreIfEmpty;
    public bool spawnOldest;

    private Transform rootFolder;
    private Transform parentFolder;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private List<GameObject> unspawnedObjects = new List<GameObject>();

    public void InitializePool(Transform _rootFolder)
    {
        spawnedObjects.Clear();
        unspawnedObjects.Clear();
        rootFolder = _rootFolder;
        CreateParentFolder();

        //spawn objects and put them into parent folder
        for (int ind = 0; ind < amountToSpawn; ind++)
        {
            CreateObjectForPool();
        }
    }

    void CreateParentFolder()
    {
        //create parent folder
        parentFolder = new GameObject().transform;
        parentFolder.name = prefabToSpawn.name;
        parentFolder.SetParent(rootFolder);
    }

    void CreateObjectForPool()
    {
        //spawn in object
        var prefab = prefabToSpawn;
        var obj = GameObject.Instantiate(prefab, parentFolder);
        //set name to index
        obj.name = prefab.name;
        //unspawn object
        obj.SetActive(false);
        unspawnedObjects.Add(obj);
    }

    public GameObject SpawnObject(Vector3 _pos, Quaternion _rotation, Transform _parent)
    {
        if (unspawnedObjects.Count < 1)
        {
            if (createMoreIfEmpty)
            {
                CreateObjectForPool();
            }
            else
            {
                Debug.LogError(prefabToSpawn.name + " pool is empty! Add more to the pool or allow create more if empty!");
                return null;
            }
            
        }

        var ind = 0;
        if (spawnOldest)
            ind = unspawnedObjects.Count - 1;

        var spawn = unspawnedObjects[ind];
        if (spawn)
        {
            spawn.SetActive(true);
            spawn.transform.position = _pos;
            spawn.transform.rotation = _rotation;
            if (_parent)
                spawn.transform.SetParent(_parent);
            
            unspawnedObjects.RemoveAt(ind);
            spawnedObjects.Add(spawn);

            return spawn;
        }
        else
            return null;
        
    }

    public void UnspawnObject(GameObject _objToUnspawn)
    {
        //search list for object
        var unspawn = spawnedObjects.Find(obj => obj == _objToUnspawn);
        if (unspawn)
        {

            //sleep rigidbody
            var rb = unspawn.GetComponent<Rigidbody>();
            if (rb)
                rb.Sleep();
            var rb2d = unspawn.GetComponent<Rigidbody2D>();
            if (rb2d)
                rb2d.Sleep();

            //deactivate and unspawn
            unspawn.transform.SetParent(parentFolder);
            unspawn.transform.localPosition = Vector3.zero;
            unspawn.transform.rotation = Quaternion.identity;
            unspawn.SetActive(false);

            //switch lists
            spawnedObjects.Remove(unspawn);
            unspawnedObjects.Insert(0, unspawn);
        }
    }

}
