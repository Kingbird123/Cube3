using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InteractFXLoopEvent
{
    public List<InteractFXLoop> interactFXLoops = new List<InteractFXLoop>();
}

[System.Serializable]
public class InteractFXLoop
{
    public string animToPlay;
    public MethodProperty method;
    public InteractFX[] interacts;
    public float delay;
    public bool repeat;
    public float repeatDelay = 1;
    public float totalTime = 1;
    private float delayTimer;
    private float repeatTimer;
    private float timer;

    private bool finished;
    public bool IsFinished { get { return finished; } }
    private GameObject sender;
    private GameObject receiver;

    //gui stuff
    public int currentFieldAmount;

    public void DoLoop(MonoBehaviour _sender, GameObject _receiver)
    {
        sender = _sender.gameObject;
        receiver = _receiver;
        _sender.StartCoroutine(StartLoop());
    }

    public IEnumerator StartLoop()
    {
        finished = false;
        //Delay
        delayTimer = 0;
        while (delayTimer < delay)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer > delay)
                delayTimer = delay;
            yield return new WaitForEndOfFrame();
        }
        //Do first round of FX
        DoFX();
        DoEvents();
        //Start Repeat Loop
        timer = 0;
        repeatTimer = 0;
        while (timer < totalTime)
        {
            timer += Time.deltaTime;
            if (timer > totalTime)
                timer = totalTime;

            if (repeat)
            {
                repeatTimer += Time.deltaTime;
                if (repeatTimer >= repeatDelay)
                {
                    DoFX();
                    DoEvents();
                    repeatTimer = 0;
                }
            }
            
            yield return new WaitForEndOfFrame();
        }
        //finished
        finished = true;
    }

    void DoEvents()
    {
        method.InvokeMethod();
    }

    void DoFX()
    {
        foreach (var interact in interacts)
        {
            interact.ActivateFX(sender, receiver);
        }
    }
}
