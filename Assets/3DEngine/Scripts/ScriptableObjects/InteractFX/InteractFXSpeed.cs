using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectSpeed", menuName = "Data/Interacts/EffectSpeed", order = 1)]
public class InteractFXSpeed: InteractFX
{

    [SerializeField] private float speedMultiplier = 1;
    [SerializeField] private float effectTime = 0;
    [SerializeField] private float smoothTime = 0;
    [SerializeField] private bool includeSmoothTimeToEffectTime = false;

    protected override void DoFX(GameObject _sender, GameObject _receiver)
    {
        UnitController cont = _receiver.GetComponent<UnitController>();
        cont.StartCoroutine(StartSpeedChange(effectTime, cont));
    }

    IEnumerator StartSpeedChange(float _time, UnitController _controller)
    {
        _controller.IsSpeedEffected = true;

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
            _controller.SpeedMultiplier = (Mathf.Lerp(1, 1 * speedMultiplier, perc));

            yield return new WaitForEndOfFrame();
        }
        //set speed and wait
        _controller.SpeedMultiplier = (1 * speedMultiplier);
        yield return new WaitForSeconds(overallTime);
        
        //smooth ramp speed back to normal
        timer = 0;
        perc = 0;
        while (perc < 1)
        {
            timer += Time.deltaTime;
            if (timer > smoothTime)
                timer = smoothTime;
            perc = timer / smoothTime;
            _controller.SpeedMultiplier = (Mathf.Lerp(1 * speedMultiplier, 1, perc));

            yield return new WaitForEndOfFrame();
        }
        _controller.SpeedMultiplier = 1;
        _controller.IsSpeedEffected = false;
    }
}
