using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : UnitAnimations
{
    //synced params
    [SerializeField] protected AnimatorParamStateInfo animInputHorizontal;
    public float InputHorizontal { set { if (anim && !forceIdle) anim.SetFloat(animInputHorizontal.stringValue, Mathf.Abs(value)); } }
    [SerializeField] protected AnimatorParamStateInfo animInputVertical;
    public float InputVertical { set { if (anim && !forceIdle) anim.SetFloat(animInputVertical.stringValue, value); } }
    [SerializeField] protected AnimatorParamStateInfo animCrouch;
    public bool Crouching { set { if (anim && !forceIdle) anim.SetBool(animCrouch.stringValue, value); } }
    [SerializeField] protected AnimatorParamStateInfo animClimbing;
    public bool Climbing { set { if (anim && !forceIdle) anim.SetBool(animClimbing.stringValue, value); } }
    [SerializeField] protected AnimatorParamStateInfo animRunning;
    public bool Running { set { if (anim && !forceIdle) anim.SetBool(animRunning.stringValue, value); } }
    [SerializeField] protected AnimatorParamStateInfo animDashing;
    public bool Dashing { set { if (anim && !forceIdle) anim.SetBool(animDashing.stringValue, value); } }
    [SerializeField] protected AnimatorParamStateInfo animRolling;
    public bool Rolling { set { if (anim && !forceIdle) anim.SetBool(animRolling.stringValue, value); } }

    //hybrid params
    [SerializeField] protected AnimatorParamStateInfo animDoubleJump;

    public void PlayDoubleJump()
    {
        PlayAnim(animDoubleJump);
    }
}
