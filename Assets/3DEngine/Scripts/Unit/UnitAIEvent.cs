using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnitAIEvent
{

    public string eventName;
    public enum StartType { Instant, WaitForPreviousToFinish }
    public StartType startType;

    public enum EventType { State, Item, Animator, CallMethod, InteractFX }
    public int eventTypeMask;

    //state event
    public enum StateType { ChaseTarget, RemoveChaseTarget, FleeFromTarget, RemoveFleeTarget, LookAtTarget, RemoveLookAtTarget, PauseMovement, ResumeMovement, DefaultState }
    public StateType stateType;

    //use items...unit equip must be attached
    public enum ItemEventType { UseItem, StopUseItem, EquipItem, UnEquipItem, DropItem }
    public ItemEventType itemEventType;
    public int item;

    //anim
    public bool setAnimator;
    public Animator animator;
    public string stateToPlay;
    public float crossfadeTime;
    //method
    public MethodProperty methodToCall;
    //interacts
    public InteractFX[] interacts;

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
    public bool finished;

    //value Amounts
    //public ValueAmountManagerData valueAmountManager;
    public int valueAmountInd;
    public enum ValueOptions { Greater, Less, Equal }
    public ValueOptions valueOption;
    public float comparedValue;

    private Coroutine repeatRoutine;
    private Coroutine waitRoutine;
    private Coroutine eventRoutine;

    private Transform senderTrans;

    private int index;
    private UnitAI curAI;
    private Unit unit;
    private UnitEquip equip;
    private Collider curTarget;

    public void DoEvent(UnitAITrigger _trigger, int _index, Collider _col = null)
    {
        curAI = _trigger.CurAi;
        if (!equip)
            equip = curAI.GetComponent<UnitEquip>();
        index = _index;
        finished = false;

        if (_col)
                curTarget = _col;
            
        if (startType == StartType.Instant)
            StartEvent();
        else if (startType == StartType.WaitForPreviousToFinish && index > 0)
        {
            if (waitRoutine != null)
                curAI.StopCoroutine(waitRoutine);

            waitRoutine = curAI.StartCoroutine(StartWaitForPrevious(_trigger));
        }

    }

    IEnumerator StartWaitForPrevious(UnitAITrigger _trigger)
    {
        var lastEvent = _trigger.events[index - 1];
        while (!lastEvent.finished)
        {
            yield return new WaitForEndOfFrame();
        }
        StartEvent();
    }

    void StartEvent()
    {
        if (eventRoutine != null)
            curAI.StopCoroutine(eventRoutine);
        if (repeatRoutine != null)
            curAI.StopCoroutine(repeatRoutine);

        eventRoutine = curAI.StartCoroutine(StartTriggerEvent());
    }

    IEnumerator StartTriggerEvent()
    {
        bool eventSent = false;
        float delayTimer = 0;
        float totalTimer = 0;

        while (!finished)
        {
            if (finishType == FinishType.Timed)
            {
                totalTimer += Time.deltaTime;
                if (totalTimer > totalTime)
                {
                    totalTimer = totalTime;
                    finished = true;
                }

            }

            delayTimer += Time.deltaTime;
            if (delayTimer > delay && !eventSent)
            {
                delayTimer = delay;
                DoEventSwitch();
                eventSent = true;
                if (finishType == FinishType.OnEventSent)
                    finished = true;
            }

            yield return new WaitForEndOfFrame();
        }

        if (repeat)
        {
            if (repeatRoutine != null)
                curAI.StopCoroutine(repeatRoutine);

            repeatRoutine = curAI.StartCoroutine(StartRepeat());
        }
    }

    IEnumerator StartRepeat()
    {
        float repeatTimer = 0;
        while (true)
        {
            repeatTimer += Time.deltaTime;
            if (repeatTimer > repeatTime)
            {
                repeatTimer = 0;
                DoEventSwitch();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void DoEventSwitch()
    {

        if (eventTypeMask == (eventTypeMask | (1 << 0)))
            DoStateEvent();
        if (eventTypeMask == (eventTypeMask | (1 << 1)))
            DoItemEvent();
        if (eventTypeMask == (eventTypeMask | (1 << 2)))
            DoPlayAnimation();
        if (eventTypeMask == (eventTypeMask | (1 << 3)))
            DoCallMethod();
        if (eventTypeMask == (eventTypeMask | (1 << 4)))
            DoInteractFX();
    }

    void DoStateEvent()
    {

        Transform tar = null;
        if (curTarget)
            tar = curTarget.transform;

        if (stateType == StateType.DefaultState)
            curAI.Controller.DoDefaultMovement(tar);
        else if (stateType == StateType.PauseMovement)
            curAI.Controller.PauseMovement(tar, true);
        else if (stateType == StateType.ResumeMovement)
            curAI.Controller.PauseMovement(tar, false);
        else if (stateType == StateType.ChaseTarget)
            curAI.Controller.ChaseTarget(tar);
        else if (stateType == StateType.RemoveChaseTarget)
            curAI.Controller.RemoveChaseTarget(tar);
        else if (stateType == StateType.FleeFromTarget)
            curAI.Controller.FleeTarget(tar);
        else if (stateType == StateType.RemoveFleeTarget)
            curAI.Controller.RemoveFleeTarget(tar);
        else if (stateType == StateType.LookAtTarget)
            curAI.Controller.LookAtTarget(tar);
        else if (stateType == StateType.RemoveLookAtTarget)
            curAI.Controller.RemoveLookAtTarget(tar);

    }

    void DoItemEvent()
    {
        if (!equip)
            return;

        switch (itemEventType)
        {
            case ItemEventType.UseItem:
                DoUseItem();
                break;
            case ItemEventType.StopUseItem:
                DoStopItem();
                break;
            case ItemEventType.EquipItem:
                DoEquipItem();
                break;
            case ItemEventType.UnEquipItem:
                DoUnequipItem();
                break;
            case ItemEventType.DropItem:
                DoDropItem();
                break;
        }
    }

    void DoUseItem()
    {
        SetItem();
        equip.UseEquippedItem();
    }

    void DoStopItem()
    {
        SetItem();
        equip.StopUseEquippedItem();
    }

    void DoEquipItem()
    {
        SetItem();
        equip.EquipCurItem(true);
    }

    void DoUnequipItem()
    {
        SetItem();
        equip.EquipCurItem(false);
    }

    void DoDropItem()
    {
        SetItem();
        equip.DropCurrentItem();
    }

    void SetItem()
    {
        equip.SetCurItem(item);
    }

    void DoPlayAnimation()
    {
        if (!setAnimator)
            animator = curAI.GetComponentInChildren<Animator>();

        if (animator)
            animator.CrossFade(stateToPlay, crossfadeTime);
    }

    void DoCallMethod()
    {
        methodToCall.InvokeMethod();
    }

    void DoInteractFX()
    {
        foreach (var fx in interacts)
        {
            fx.ActivateFX(curAI.gameObject, curTarget.gameObject);
        }
    }
}