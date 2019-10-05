using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EngineEventOption
{
    public enum AffectedType { EventAssigned, Override }
    public AffectedType affectedObj;
    public GameObject overrideObject;
    protected GameObject objToUse;

    public virtual void DoEvent(EngineEvent _event)
    {
        if (affectedObj == AffectedType.EventAssigned)
            objToUse = _event.CurTarget;  
        else if (affectedObj == AffectedType.Override)
            objToUse = overrideObject;
    }
}
