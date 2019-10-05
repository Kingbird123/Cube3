using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PrefabSpawner", menuName = "Data/Utilities/PrefabSpawner", order = 1)]
public class PrefabSpawner : ScriptableObject
{
    [SerializeField] private GameObject[] prefabsToSpawn;

    public GameObject spawnedPrefab;

    //the prefabs are spawned through the editor script

}
