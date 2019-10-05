using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EngineEventOptionInteractFX : EngineEventOption
{
    public InteractFX interactFX;

    public override void DoEvent(EngineEvent _event)
    {
        base.DoEvent(_event);
        interactFX.ActivateFX(_event.Source, objToUse);
    }
}
