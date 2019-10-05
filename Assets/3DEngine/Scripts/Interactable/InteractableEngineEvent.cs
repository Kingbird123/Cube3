using UnityEngine;

public class InteractableEngineEvent : Interactable
{

    [SerializeField] protected EngineEvent[] hoverEnterEvents;
    [SerializeField] protected EngineEvent[] hoverStayEvents;
    [SerializeField] protected EngineEvent[] hoverExitEvents;

    [SerializeField] protected EngineEvent[] interactEvents;

    protected override void DoHoverEnter()
    {
        for (int i = 0; i < hoverEnterEvents.Length; i++)
        {
            hoverEnterEvents[i].DoEvent(gameObject, hoverEnterEvents, i, receiver);
        }
    }

    protected override void DoHoverStay()
    {
        for (int i = 0; i < hoverStayEvents.Length; i++)
        {
            hoverStayEvents[i].DoEvent(gameObject, hoverStayEvents, i, receiver);
        }
    }

    protected override void DoHoverExit()
    {
        for (int i = 0; i < hoverExitEvents.Length; i++)
        {
            hoverExitEvents[i].DoEvent(gameObject, hoverExitEvents, i, receiver);
        }
    }

    protected override void DoInteract()
    {
        for (int i = 0; i < interactEvents.Length; i++)
        {
            interactEvents[i].DoEvent(gameObject, interactEvents, i, receiver);
        }
    }
}