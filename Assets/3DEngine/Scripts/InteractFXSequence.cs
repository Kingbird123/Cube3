using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractFXSequence
{
    [SerializeField] private float delay = 0;
    [SerializeField] private List<InteractFXEvent> fxEvents = new List<InteractFXEvent>();
    
    public void ActivateFXSequence(GameObject _sender = null, GameObject _receiver = null)
    {
        if (delay > 0)
        {

            Timing.RunCoroutine(StartFXSequence(_sender, _receiver));
        }
        else
            DoFXList(_sender, _receiver);
    }

    IEnumerator<float> StartFXSequence(GameObject _sender = null, GameObject _receiver = null)
    {
        float timer = 0;
        while (timer < delay)
        {
            timer += Time.deltaTime;
            if (timer > delay)
                timer = delay;

            yield return Timing.WaitForOneFrame;
        }
        DoFXList(_sender, _receiver);
    }

    void DoFXList(GameObject _sender = null, GameObject _receiver = null)
    {
        for (int i = 0; i < fxEvents.Count; i++)
        {
            InteractFXEvent prev = null;
            if (i > 0)
                prev = fxEvents[i - 1];
            fxEvents[i].DoFXEvent(prev, _sender, _receiver);
        }
    }
}
