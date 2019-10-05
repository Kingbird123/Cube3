using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveSpawner
{
    [SerializeField] private string name;
    [SerializeField] private IndexStringProperty spawnable = null;
    [SerializeField] private IndexStringProperty spawnPos = null;
    [SerializeField] private float delay = 0;
    [SerializeField] private int spawnAmount = 0;
    public int TotalSpawnAmount { get { return spawnAmount; } }
    private int curSpawnAmount;
    [SerializeField] private float spawnDelay = 1;
    [SerializeField] private bool waitForPreviousToFinish = false;

    protected bool finished;
    public bool IsFinished { get { return finished; } }

    public void InitializeSpawner(WaveSpawnerSequence _manager, Wave _wave, WaveSpawner _previousSpawner)
    {

        if (waitForPreviousToFinish && _previousSpawner != null)
            WaitForPrevious(_manager, _wave, _previousSpawner);
        else
            SpawnSequence(_manager, _wave);
    }

    void WaitForPrevious(WaveSpawnerSequence _manager, Wave _wave, WaveSpawner _previousSpawner)
    {
        _manager.StartCoroutine(StartWaitForPrevious(_manager, _wave, _previousSpawner));
    }

    IEnumerator StartWaitForPrevious(WaveSpawnerSequence _manager, Wave _wave, WaveSpawner _previousSpawner)
    {
        while (!_previousSpawner.IsFinished)
            yield return new WaitForEndOfFrame();

        SpawnSequence(_manager, _wave);
    }

    void SpawnSequence(WaveSpawnerSequence _manager, Wave _wave)
    {
        _manager.StartCoroutine(StartSpawnSequence(_manager, _wave));
    }

    IEnumerator StartSpawnSequence(WaveSpawnerSequence _manager, Wave _wave)
    {
        //delay
        float timer = 0;
        while (timer < delay)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        timer = 0;
        while (curSpawnAmount < spawnAmount)
        {
            timer += Time.deltaTime;
            if (timer >= spawnDelay)
            {
                SpawnPrefab(_manager, _wave);
                timer = 0;
            }

            yield return new WaitForEndOfFrame();
        }
        FinishSpawner(_wave);
    }

    void FinishSpawner(Wave _wave)
    {
        finished = true;
        _wave.CheckFinished();
    }

    void SpawnPrefab(WaveSpawnerSequence _manager, Wave _wave)
    {

        var pos = _manager.SpawnLocations[spawnPos.indexValue];
        var spawnable = _manager.Spawnables[this.spawnable.indexValue].spawnablePrefab;
        var spawn = GameObject.Instantiate(spawnable, pos.Position, Quaternion.Euler(pos.Euler));
        var unit = spawn.GetComponent<Unit>();
        if (unit)
            unit.SetWaveSpawnerManager(_manager);
        curSpawnAmount++;
        _wave.AddSpawnAmount();
    }
}
