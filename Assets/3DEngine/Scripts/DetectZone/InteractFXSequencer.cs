using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractFXSequencer : DetectZoneTrigger
{
    public enum StartType { OnEnter, OnExit, Delay, External }
    public enum FinishType { None, Restart, DestroySelf }
    [SerializeField] private InteractFXLoopEvent loops = null;
    [SerializeField] private FinishType finishType = FinishType.None;
    [SerializeField] private StartType startType = StartType.Delay;
    [SerializeField] private float startDelay = 0;
    [SerializeField] private float destroyDelay = 0;

    private bool initiated;
    private int curIndex;
    private Coroutine loopCoroutine;
    private GameObject receiver;

    protected override void Start()
    {
        base.Start();
        if (startType == StartType.Delay)
        {
            receiver = gameObject;
            StartCoroutine(StartDelay());
        }     
    }

    protected override void OnEnter(Collider _col)
    {
        if (startType != StartType.OnEnter)
            return;

        base.OnEnter(_col);
        receiver = _col.gameObject;
        InitiateSequence();
    }

    protected override void OnExit(Collider _col)
    {
        if (startType != StartType.OnExit)
            return;

        base.OnExit(_col);
        receiver = _col.gameObject;
        InitiateSequence();
    }

    public void InitiateSequence()
    {
        if (initiated)
            return;

        if (loopCoroutine != null)
            StopCoroutine(loopCoroutine);
        if (loops.interactFXLoops.Count > 0)
            loopCoroutine = StartCoroutine(StartLoops());
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startDelay);
        InitiateSequence();
    }

    IEnumerator StartLoops()
    {
        initiated = true;
        curIndex = 0;
        loops.interactFXLoops[curIndex].DoLoop(this, receiver);
        bool looped = false;
        while (!looped)
        {
            if (loops.interactFXLoops[curIndex].IsFinished)
            {
                if (curIndex < loops.interactFXLoops.Count - 1)
                {
                    curIndex++;
                    loops.interactFXLoops[curIndex].DoLoop(this, receiver);
                }
                else
                    looped = true;
            }
            yield return new WaitForEndOfFrame();
        }
        Finish();
    }

    void Finish()
    {
        initiated = false;
        if (finishType == FinishType.Restart)
            InitiateSequence();
        else if (finishType == FinishType.DestroySelf)
            Destroy(gameObject, destroyDelay);
    }
}
