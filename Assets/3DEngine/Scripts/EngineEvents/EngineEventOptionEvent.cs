using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EngineEventOptionEvent : EngineEventOption
{
    [SerializeField] protected EngineEventReceiver receiver;

    public override void DoEvent(EngineEvent _event)
    {
        base.DoEvent(_event);

            //receiver.SetManager(manager);
            receiver.Activate();
    }
}
