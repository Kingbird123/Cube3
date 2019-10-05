using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EngineEventOptionCallMethod : EngineEventOption
{
    public MethodProperty method;

    public override void DoEvent(EngineEvent _event)
    {
        base.DoEvent(_event);
        if (objToUse != null)
        {
            method.go = objToUse;
            method.InvokeMethod();
        }
        
    }
}
