using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the public functions in the class are triggered by the playercontroller script.
//when the player aims at an object it will trigger the desired hover method
//when the player uses the interaction input, the interact method will trigger
public abstract class Interactable : MonoBehaviour
{
    public enum TriggerType { OnHoverEnter, OnHoverStay, OnHoverExit, OnInteract }
    [SerializeField] protected int triggerMask;
    protected GameObject receiver;

    public virtual void OnHoverEnter(GameObject _receiver = null)
    {
        if (triggerMask == (triggerMask | (1 << (int)TriggerType.OnHoverEnter)))
        {
            receiver = _receiver;
            DoHoverEnter();
        }  
    }

    public virtual void OnHoverStay(GameObject _receiver = null)
    {
        if (triggerMask == (triggerMask | (1 << (int)TriggerType.OnHoverStay)))
        {
            receiver = _receiver;
            DoHoverStay();
        }
    }

    public virtual void OnHoverExit(GameObject _receiver = null)
    {
        if (triggerMask == (triggerMask | (1 << (int)TriggerType.OnHoverExit)))
        {
            receiver = _receiver;
            DoHoverExit();
        }
    }

    public virtual void Interact(GameObject _receiver = null)
    {
        if (triggerMask == (triggerMask | (1 << (int)TriggerType.OnInteract)))
        {
            receiver = _receiver;
            DoInteract();
        }
    }

    protected abstract void DoHoverEnter();
    protected abstract void DoHoverStay();
    protected abstract void DoHoverExit();
    protected abstract void DoInteract();
}
