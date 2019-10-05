using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractFX : ScriptableObject
{
    [SerializeField] protected float delay;

    public virtual void ActivateFX(GameObject _sender = null, GameObject _receiver = null, System.Action _finishedCallback = null)
    {
        if (delay > 0)
        {
            Timing.RunCoroutine(StartDoFX(_sender, _receiver, _finishedCallback));
        }
        else
            CallFX(_sender, _receiver, _finishedCallback);
    }

    IEnumerator<float> StartDoFX(GameObject _sender = null, GameObject _receiver = null, System.Action _finishedCallback = null)
    {
        float timer = 0;
        while (timer < delay)
        {
            timer += Time.deltaTime;
            if (timer > delay)
                timer = delay;

            yield return Timing.WaitForOneFrame;
        }
        CallFX(_sender, _receiver, _finishedCallback);
    }

    void CallFX(GameObject _sender = null, GameObject _receiver = null, System.Action _finishedCallback = null)
    {
        DoFX(_sender, _receiver);
        if (_finishedCallback != null)
            _finishedCallback.Invoke();
    }

    protected abstract void DoFX(GameObject _sender = null, GameObject _receiver = null);
}
