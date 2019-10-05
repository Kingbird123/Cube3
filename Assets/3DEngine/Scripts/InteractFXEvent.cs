using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractFXEvent
{
    public enum StartType { Instant, WaitPreviousToFinish }
    [SerializeField] private InteractFX fxData = null;
    [SerializeField] private StartType startType = StartType.Instant;

    private bool finished;
	public bool IsFinished { get { return finished; } }

    public void DoFXEvent(InteractFXEvent _previousEvent = null, GameObject _sender = null, GameObject _receiver = null)
    {
        if (startType == StartType.Instant)
        {
            DoFX(_sender, _receiver);
        }
        else if (_previousEvent != null)
        {

            Timing.RunCoroutine(StartWaitForPrevious(_previousEvent, _sender, _receiver));
        }
        else
            Debug.LogError("Could start event!");
    }

    IEnumerator<float> StartWaitForPrevious(InteractFXEvent _previousEvent, GameObject _sender = null, GameObject _receiver = null)
    {
        while (!_previousEvent.IsFinished)
        {
            yield return Timing.WaitForOneFrame;
        }
        DoFX(_sender, _receiver);
    }

    void DoFX(GameObject _sender = null, GameObject _receiver = null)
    {
        finished = false;
        fxData.ActivateFX(_sender, _receiver, Finished);
    }

    void Finished()
    {
        finished = true;
    }
}
