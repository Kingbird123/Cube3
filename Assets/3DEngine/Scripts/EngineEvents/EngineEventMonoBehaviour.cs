using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineEventMonoBehaviour : MonoBehaviour
{
    public enum BeginType { Awake, OnEnable, Start, OnDisable, OnDestroy }
    [SerializeField] protected int beginMask;
    [SerializeField] protected EngineEvent[] awakeEvents;
    [SerializeField] protected EngineEvent[] startEvents;
    [SerializeField] protected EngineEvent[] onEnableEvents;
    [SerializeField] protected EngineEvent[] onDisableEvents;
    [SerializeField] protected EngineEvent[] onDestroyEvents;

    private void Awake()
    {
        BeginEvents(awakeEvents, BeginType.Awake);
    }

    private void OnEnable()
    {
        BeginEvents(onEnableEvents, BeginType.OnEnable);
    }

    private void Start()
    {
        BeginEvents(startEvents, BeginType.Start);
    }

    private void OnDisable()
    {
        BeginEvents(onDisableEvents, BeginType.OnDisable);
    }

    private void OnDestroy()
    {
        BeginEvents(onDestroyEvents, BeginType.OnDestroy);
    }

    void BeginEvents(EngineEvent[] _events, BeginType _type)
    {
        if (!beginMask.MaskContains((int)_type))
            return;

        for (int i = 0; i < _events.Length; i++)
        {
            //_events[i].DoEvent(,);
        }
    }
}
