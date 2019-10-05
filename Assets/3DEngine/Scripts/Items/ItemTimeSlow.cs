using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTimeSlow : ItemAimable
{
    public new ItemTimeSlowData Data { get { return (ItemTimeSlowData)data; } }

    private Coroutine curRoutine;

    private float startTime;
    private float startPhysicsTime;


    protected override void Start()
    {
        base.Start();
        startTime = Time.timeScale;
        startPhysicsTime = Time.fixedDeltaTime;
    }


    IEnumerator StartEffectTime(bool _slow)
    {
        var targetTimeScale = Data.slowTimeScale;
        var targetPhysicsScale = Data.physicsTimeScale;
        if (!_slow)
        {
            targetTimeScale = startTime;
            targetPhysicsScale = startPhysicsTime;
        }
        var curTime = Time.timeScale;
        var curPhys = Time.fixedDeltaTime;
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer > Data.crossfadeTime)
                timer = Data.crossfadeTime;
            float perc = timer / Data.crossfadeTime;

            Time.timeScale = Mathf.Lerp(curTime, targetTimeScale, perc);
            Time.fixedDeltaTime = Mathf.Lerp(curPhys, targetPhysicsScale, perc);
            yield return new WaitForEndOfFrame();
        }
        
    }

}
