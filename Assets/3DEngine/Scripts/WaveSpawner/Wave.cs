using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public enum TriggerFinishType { AfterSpawnSequence, CurSpawnAmount, Timed }
    public enum CompareType { Less, Equal, Greater }
    public enum OnFinishType { AfterSpawnSequence, CurSpawnAmount }

    [SerializeField] private string waveName;
    [SerializeField] private float delay = 0;
    [SerializeField] private TriggerFinishType finishType = TriggerFinishType.CurSpawnAmount;
    [SerializeField] private float waveTime = 10;
    public float WaveTime { get { return waveTime; } }
    protected float waveTimer;
    public float WaveTimer { get { return waveTimer; } }
    [SerializeField] private CompareType compareType = CompareType.Less;
    [SerializeField] private int compareValue = 1;
    [SerializeField] private WaveSpawner[] spawners = null;

    protected int curSpawnAmount;
    public int CurSpawnAmount { get { return curSpawnAmount; } }
    public int SpawnTotal { get{ int amount = 0; for (int i = 0; i < spawners.Length; i++) { amount += spawners[i].TotalSpawnAmount; } return amount; } }
    protected int curSpawnTotal;
    public int CurSpawnTotal { get { return curSpawnTotal; } }

    protected bool finished;
    public bool IsFinished { get { return finished; } }

    private WaveSpawnerSequence manager;

    public void InitializeWave(WaveSpawnerSequence _manager)
    {
        curSpawnTotal = SpawnTotal;
        manager = _manager;
        manager.StartCoroutine(StartWaveSequence());
    }

    IEnumerator StartWaveSequence()
    {
        //delay
        float timer = 0;
        while (timer < delay)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        ActivateSpawners();

        if (finishType == TriggerFinishType.Timed)
        {
            waveTimer = 0;
            while (waveTimer < waveTime)
            {
                waveTimer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            waveTimer = waveTime;
            FinishWave();
        }

    }

    void ActivateSpawners()
    {
        WaveSpawner prevSpawner = null;
        for (int i = 0; i < spawners.Length; i++)
        {
            if (i > 0)
                prevSpawner = spawners[i - 1];
            spawners[i].InitializeSpawner(manager, this, prevSpawner);
        }
    }

    public void AddSpawnAmount()
    {
        curSpawnAmount++;
        CheckFinished();
    }

    public void RemoveSpawnAmount()
    {
        curSpawnAmount--;
        curSpawnTotal--;
        CheckFinished();
    }

    public void CheckFinished()
    {
        if (finishType == TriggerFinishType.AfterSpawnSequence)
        {
            bool allFinished = false;
            for (int i = 0; i < spawners.Length; i++)
            {
                allFinished = spawners[i].IsFinished;
                if (!allFinished)
                    return;
            }
            finished = allFinished;
        }
        else if (finishType == TriggerFinishType.CurSpawnAmount)
        {
            if (compareType == CompareType.Less)
                finished = curSpawnTotal < compareValue;
            else if (compareType == CompareType.Equal)
                finished = curSpawnTotal == compareValue;
            else if (compareType == CompareType.Greater)
                finished = curSpawnTotal > compareValue;
        }

        if (finished)
        {
            FinishWave();
        }
    }

    void FinishWave()
    {
        finished = true;
        manager.BeginNextWave();
    }
}
