using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class TimedUnspawner : MonoBehaviour
{
    [SerializeField] private float time = 0;
    private CoroutineHandle delayRoutine;
    float timer = 0;

    private void OnEnable()
    {
        timer = 0;
    }

    private void Update()
    {
        RunTimer();
    }

    void RunTimer()
    {
        timer += Time.deltaTime;
        if (timer > time)
        {
            timer = 0;
            SpawnPool.Unspawn(gameObject);
        }
            
    }

}
