using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

[System.Serializable]
public class EngineEvent
{
    public string eventName;
    public SceneObjectProperty sceneObject;
    public enum StartType { OnEventCalled, OnPreviousFinished, OnInputAfterPreviousFinished }
    public StartType startType;
    public InputProperty inputButton;

    public enum EventType { Common, Animator, CallMethod, InteractFX, ValueDelta, Event }
    public int eventTypeMask;

    //common
    public EngineEventOptionCommon[] commons;
    //anim
    public EngineEventOptionAnimator[] anims;
    //method
    public EngineEventOptionCallMethod[] methods;
    //interacts
    public EngineEventOptionInteractFX[] interacts;
    //event options
    public EngineEventOptionEvent[] eventOptions;
    //event options
    public EngineEventOptionValueDelta[] valueDeltaOptions;

    public int detectZoneInd;
    private List<Collider2D> cols = new List<Collider2D>();
    private List<Collider2D> enteredCols = new List<Collider2D>();

    //common values
    public enum FinishType { OnEventSent, Timed }
    public float delay;
    public bool repeat;
    public float repeatTime;
    public FinishType finishType;
    public float totalTime;
    public bool active;
    public bool finished;

    private CoroutineHandle repeatRoutine;
    private CoroutineHandle waitRoutine;
    private CoroutineHandle inputRoutine;
    private CoroutineHandle eventRoutine;

    private Transform senderTrans;

    private object events;
    private int eventInd;
    private object source;
    public GameObject Source { get { return source as GameObject; } }
    private GameObject curTarget;
    public GameObject CurTarget { get { return curTarget; } }

    private bool paused;

    public void DoEvent(GameObject _source, object _events, int _eventIndex, GameObject _receiver = null)
    {
        paused = false;
        source = _source;
        events = _events;
        eventInd = _eventIndex;
        active = true;
        finished = false;
        curTarget = sceneObject.GetSceneObject(_source as GameObject, _receiver);

        if (startType == StartType.OnEventCalled)
            StartEvent();
        else if (startType == StartType.OnPreviousFinished && eventInd > 0)
        {
            if (waitRoutine != null)
                Timing.KillCoroutines(waitRoutine);

            waitRoutine = Timing.RunCoroutine(StartWaitForPrevious());
        }
        else if (startType == StartType.OnInputAfterPreviousFinished)
        {
            if (inputRoutine != null)
                Timing.KillCoroutines(inputRoutine);

            inputRoutine = Timing.RunCoroutine(StartWaitForInput());
        }

    }

    public void StopEvent()
    {
        
        paused = false;
        if (waitRoutine != null)
            Timing.KillCoroutines(waitRoutine);
        if (inputRoutine != null)
            Timing.KillCoroutines(inputRoutine);         
        if (eventRoutine != null)
            Timing.KillCoroutines(eventRoutine);
        if (repeatRoutine != null)
            Timing.KillCoroutines(repeatRoutine);
    }

    public void PauseEvent(bool _pause)
    {
        paused = _pause;
    }

    IEnumerator<float> StartWaitForPrevious()
    {
        var eventArray = events as EngineEvent[];
        var lastEvent = eventArray[eventInd - 1];
        while (!lastEvent.finished || paused)
        {
            yield return Timing.WaitForOneFrame;
        }
        StartEvent();
    }

    IEnumerator<float> StartWaitForInput()
    {
        if (eventInd > 0)
        {
            var eventArray = events as EngineEvent[];
            var lastEvent = eventArray[eventInd - 1];
            while (!lastEvent.finished || paused)
            {
                yield return Timing.WaitForOneFrame;
            }
        }
        while (!inputButton.GetInputDown() || paused)
        {
            yield return Timing.WaitForOneFrame;
        }
        StartEvent();
    }

    void StartEvent()
    {
        if (eventRoutine != null)
            Timing.KillCoroutines(eventRoutine);
        if (repeatRoutine != null)
            Timing.KillCoroutines(repeatRoutine);

        eventRoutine = Timing.RunCoroutine(StartTriggerEvent());
    }

    IEnumerator<float> StartTriggerEvent()
    {
        bool eventSent = false;
        float delayTimer = 0;
        float totalTimer = 0;

        while (!finished)
        {
            if (!paused)
            {
                if (finishType == FinishType.Timed)
                {
                    totalTimer += Time.deltaTime;
                    if (totalTimer > totalTime)
                    {
                        totalTimer = totalTime;
                        FinishEvent();
                    }

                }

                delayTimer += Time.deltaTime;
                if (delayTimer > delay && !eventSent)
                {
                    delayTimer = delay;
                    DoEventSwitch();
                    eventSent = true;
                    if (finishType == FinishType.OnEventSent)
                        FinishEvent();
                }
            }
            
            yield return Timing.WaitForOneFrame;
        }

        if (repeat)
        {
            
            if (repeatRoutine != null)
                Timing.KillCoroutines(repeatRoutine);

            repeatRoutine = Timing.RunCoroutine(StartRepeat());
        }
    }

    IEnumerator<float> StartRepeat()
    {
        float repeatTimer = 0;
        while (true)
        {
            if (!paused)
            {
                repeatTimer += Time.deltaTime;
                if (repeatTimer > repeatTime)
                {
                    repeatTimer = 0;
                    DoEventSwitch();
                }
            }
            yield return Timing.WaitForOneFrame;
        }
    }

    void DoEventSwitch()
    {
        if (eventTypeMask == (eventTypeMask | (1 << (int)EventType.Common)))
            DoCommonEvent();
        if (eventTypeMask == (eventTypeMask | (1 << (int)EventType.Animator)))
            DoPlayAnimation();
        if (eventTypeMask == (eventTypeMask | (1 << (int)EventType.CallMethod)))
            DoCallMethod();
        if (eventTypeMask == (eventTypeMask | (1 << (int)EventType.InteractFX)))
            DoInteractFX();
        if (eventTypeMask == (eventTypeMask | (1 << (int)EventType.ValueDelta)))
            DoValueEventOptions();
        if (eventTypeMask == (eventTypeMask | (1 << (int)EventType.Event)))
            DoEventOptions();
        
    }

    void DoCommonEvent()
    {
        for (int i = 0; i < commons.Length; i++)
        {
            commons[i].DoEvent(this);
        }
    }

    void DoPlayAnimation()
    {
        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].DoEvent(this);
        }
    }

    void DoCallMethod()
    {
        for (int i = 0; i < methods.Length; i++)
        {
            methods[i].DoEvent(this);
        }
    }

    void DoInteractFX()
    {
        for (int i = 0; i < interacts.Length; i++)
        {
            interacts[i].DoEvent(this);
        }
    }

    void DoEventOptions()
    {
        for (int i = 0; i < eventOptions.Length; i++)
        {
            eventOptions[i].DoEvent(this);
        }
    }

    void DoValueEventOptions()
    {
        for (int i = 0; i < valueDeltaOptions.Length; i++)
        {
            valueDeltaOptions[i].DoEvent(this);
        }
    }

    void FinishEvent()
    {
        finished = true;
        active = false;
        paused = false;
    }
}
