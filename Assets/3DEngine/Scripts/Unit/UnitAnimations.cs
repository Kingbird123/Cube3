using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimations : MonoBehaviour
{
    public enum SyncType { SyncParams, StateSwitch }

    [SerializeField] protected Animator anim;
    public Animator Animator { get { return anim; } set { anim = value; } }
    [SerializeField] protected RuntimeAnimatorController animController;

    [SerializeField] protected bool forceIdle;
    [SerializeField] protected SyncType syncType;
    [SerializeField] protected float crossfadeTime;

    //state fields
    [SerializeField] protected AnimatorParamStateInfo animIdle;
    [SerializeField] protected AnimatorParamStateInfo animHover;

    //param fields
    [SerializeField] protected AnimatorParamStateInfo animGrounded;
    public bool Grounded { set { if (anim && !forceIdle) anim.SetBool(animGrounded.stringValue, value); } }
    [SerializeField] protected AnimatorParamStateInfo animOnPlatform;
    public bool OnPlatform { set { if (anim && !forceIdle) anim.SetBool(animOnPlatform.stringValue, value); } }
    [SerializeField] protected AnimatorParamStateInfo animVelocitySpeed;
    public float VelocitySpeed { set { if (anim && !forceIdle) anim.SetFloat(animVelocitySpeed.stringValue, value); } }
    [SerializeField] protected AnimatorParamStateInfo animDirectionX;
    public float DirectionX { set { if (anim && !forceIdle) anim.SetFloat(animDirectionX.stringValue, value); } }
    [SerializeField] protected AnimatorParamStateInfo animDirectionY;
    public float DirectionY { set { if (anim && !forceIdle) anim.SetFloat(animDirectionY.stringValue, value); } }
    [SerializeField] protected AnimatorParamStateInfo animDirectionZ;
    public float DirectionZ { set { if (anim && !forceIdle) anim.SetFloat(animDirectionZ.stringValue, value); } }

    //health fields
    [SerializeField] protected AnimatorParamStateInfo[] animHurts;
    [SerializeField] protected AnimatorParamStateInfo[] animDeaths;
    [SerializeField] protected AnimatorParamStateInfo animStunned;

    //movement fields
    [SerializeField] protected AnimatorParamStateInfo animJump;
    [SerializeField] protected AnimatorParamStateInfo animRun;

    //attack
    [SerializeField] protected AnimatorParamStateInfo[] animAttacksMelee;
    [SerializeField] protected AnimatorParamStateInfo[] animAttacksRanged;

    public virtual void Start()
    {
        RefreshAnimController();    
    }

    IEnumerator StartFindAnimController()
    {
        int i = 0;
        while (i < 5)
        {
            if (anim == null)
                anim = GetComponentInChildren<Animator>();
            else
                OnAnimFound();
            i++;
            yield return new WaitForEndOfFrame();
        }
    }

    public void RefreshAnimController()
    {
        StartCoroutine(StartFindAnimController());
    }

    protected virtual void OnAnimFound()
    {
        if (forceIdle)
        {
            syncType = SyncType.StateSwitch;
            anim.SetBool(animGrounded.stringValue, true);
            PlayIdle();
        }
    }

    protected virtual void PlayAnim(AnimatorParamStateInfo _anim)
    {
        if (syncType == SyncType.SyncParams)
        {
            StartCoroutine(BoolSwitch(_anim.stringValue));
        }
        else if (syncType == SyncType.StateSwitch)
        {   
            anim.CrossFade(_anim.stringValue, crossfadeTime, _anim.layer);
        }
    }

    void PlayRandomAnim(AnimatorParamStateInfo[] _anims)
    {
        if (_anims.Length < 1)
            return;
        var rand = Random.Range(0, _anims.Length);
        PlayAnim(_anims[rand]);
    }

    public virtual void PlayIdle()
    {
        PlayAnim(animIdle);
    }

    public virtual void PlayHurt()
    {
        PlayRandomAnim(animHurts);
    }

    public virtual void PlayDead()
    {
        PlayRandomAnim(animDeaths);
    }

    public virtual void PlayJump()
    {
        PlayAnim(animJump);
    }

    public virtual void PlayHover()
    {
        PlayAnim(animHover);
    }

    public virtual void PlayStunned()
    {
        PlayAnim(animStunned);
    }

    public virtual void PlayMeleeAttack()
    {
        PlayRandomAnim(animAttacksMelee);
    }

    public virtual void PlayRangedAttack()
    {
        PlayRandomAnim(animAttacksRanged);
    }

    IEnumerator BoolSwitch(string _anim)
    {
        anim.SetBool(_anim, true);
        yield return new WaitForEndOfFrame();
        anim.SetBool(_anim, false);
    }
}
