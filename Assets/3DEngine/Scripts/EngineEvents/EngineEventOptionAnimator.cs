using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EngineEventOptionAnimator : EngineEventOption
{
    public RuntimeAnimatorController animController;
    public AnimatorParamStateInfo state;
    public float crossfadeTime;

    public override void DoEvent(EngineEvent _event)
    {
        base.DoEvent(_event);
        var anim = objToUse.GetComponentInChildren<Animator>();
        if (anim)
        {
            anim.CrossFade(state.stringValue, crossfadeTime, state.layer);
        }
    }
}
