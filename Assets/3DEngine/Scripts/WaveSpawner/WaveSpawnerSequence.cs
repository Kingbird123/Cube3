using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnerSequence : MonoBehaviour
{
    [System.Serializable]
    public class Spawnable
    {
        [HideInInspector] public string spawnableName;
        public GameObject spawnablePrefab;
    }

    [System.Serializable]
    public class SpawnLocation
    {
        [SerializeField] private string locationName = null;
        [SerializeField] private Transform transLocation = null;
        [SerializeField] private Vector3 position = Vector3.zero;
        [SerializeField] private Vector3 euler = Vector3.zero;

        public string LocationName { get { return locationName; } }
        public Vector3 Position { get { if (transLocation) return transLocation.position; else return position; } }
        public Vector3 Euler { get { if (transLocation) return transLocation.eulerAngles; else return euler; } }
    }

    [SerializeField] private bool viewStats;
    [SerializeField] private Spawnable[] spawnables = null;
    public Spawnable[] Spawnables { get { return spawnables; } }
    [SerializeField] private SpawnLocation[] spawnLocations = null;
    public SpawnLocation[] SpawnLocations { get { return spawnLocations; } }
    [SerializeField] private bool beginOnStart = false;
    [SerializeField] private Wave[] waves = null;

    private Wave curWave;
    protected int curWaveInd = -1;
    public int CurWaveInd { get { return curWaveInd; } }
    public int CurWaveSpawnAmount { get { return curWave.CurSpawnAmount; } }
    public int CurWaveSpawnTotal { get { return curWave.SpawnTotal; } }
    public int CurWaveSpawnTotalTally { get { return curWave.CurSpawnTotal; } }
    public float CurWaveTimer { get { return curWave.WaveTimer; } }
    public float CurWaveTime { get { return curWave.WaveTime; } }

    private void Start()
    {
        if (beginOnStart)
            BeginNextWave();
    }

    public void BeginNextWave()
    {
        if (!(curWaveInd < waves.Length - 1))
            return;

        curWaveInd++;
        curWave = waves[curWaveInd];
        curWave.InitializeWave(this);
    }

    public void RemoveSpawnAmount()
    {
        curWave.RemoveSpawnAmount();
    }

}
