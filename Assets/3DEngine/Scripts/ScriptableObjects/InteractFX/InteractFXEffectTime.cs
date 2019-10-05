using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectTime", menuName = "Data/Interacts/EffectTime", order = 1)]
public class InteractFXEffectTime : InteractFX
{

    [SerializeField] private float speedMultiplier = 1;
    [SerializeField] private float smoothTime = 0;
    [SerializeField] private float effectTime = 0;
    [SerializeField] private bool includeSmoothTimeToEffectTime = false;

    protected override void DoFX(GameObject _sender, GameObject _receiver)
    {
        _receiver.GetComponent<MonoBehaviour>().StartCoroutine(StartSpeedChange(effectTime));
    }

    IEnumerator StartSpeedChange(float _time)
    {
        float startSpeed = Time.timeScale;
        float overallTime = effectTime;
        if (includeSmoothTimeToEffectTime)
            overallTime = effectTime - (smoothTime*2);
        //smooth ramp speed
        float timer = 0;
        float perc = 0;
        while (perc < 1)
        {
            timer += Time.deltaTime;
            if (timer > smoothTime)
                timer = smoothTime;
            perc = timer / smoothTime;
            Time.timeScale = Mathf.Lerp(startSpeed, 1 * speedMultiplier, perc);
            //need this for smooth slowmotion
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            GameManager.instance.GetMusicManager().SetPitch(Time.timeScale);
            yield return new WaitForEndOfFrame();
        }

        //set speed and wait
        Time.timeScale = 1 * speedMultiplier;
        GameManager.instance.GetMusicManager().SetPitch(Time.timeScale);
        yield return new WaitForSeconds(overallTime);

        //smooth ramp speed back to normal
        timer = 0;
        perc = 0;
        float curSpeed = Time.timeScale;
        while (perc < 1)
        {
            timer += Time.deltaTime;
            if (timer > smoothTime)
                timer = smoothTime;
            perc = timer / smoothTime;
            Time.timeScale = Mathf.Lerp(curSpeed, 1, perc);
            GameManager.instance.GetMusicManager().SetPitch(Time.timeScale);
            yield return new WaitForEndOfFrame();
        }
        Time.timeScale = 1;
        GameManager.instance.GetMusicManager().SetPitch(Time.timeScale);
        Time.fixedDeltaTime = 0.02f;
    }
}
