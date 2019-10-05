using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundFX : MonoBehaviour 
{
    private enum PlayType { Sequence, Random }

    [SerializeField] private AudioSource idleSource = null;
    [SerializeField] private AudioSource vocalSource = null;
    [SerializeField] private AudioClip idleClip = null;
    [SerializeField] private bool stopIdleOnDeath = true;
    [SerializeField] private AudioClip[] attacks = null;
    [SerializeField] private PlayType attackPlayMode = PlayType.Random;
    private int curAttackInd;
    [SerializeField] private AudioClip[] hurts = null;
    [SerializeField] private PlayType hurtPlayMode = PlayType.Random;
    private int curHurtInd;
    [SerializeField] private AudioClip[] deaths = null;
    [SerializeField] private PlayType deathPlayMode = PlayType.Random;
    private int curDeathInd;

    void Start()
    {
        PlayIdleSound();
    }

    void PlayIdleSound()
    {
        idleSource.clip = idleClip;
        idleSource.Play();
        idleSource.loop = true;
    }

    public void PlayAttackSound()
    {
        PlayVocalClipFromArray(attacks, attackPlayMode, curAttackInd, out curAttackInd);
    }

    public void PlayHurtSound()
    {
        PlayVocalClipFromArray(hurts, hurtPlayMode, curHurtInd, out curHurtInd);
    }

    public void PlayDeathSound()
    {
        PlayVocalClipFromArray(deaths, deathPlayMode, curDeathInd, out curDeathInd);

        if (stopIdleOnDeath)
            idleSource.Stop();
    }

    void PlayVocalClipFromArray(AudioClip[] _clips, PlayType _playMode, int _ind, out int _curInd)
    {
        _curInd = 0;

        if (_playMode == PlayType.Random)
            vocalSource.clip = _clips[Random.Range(0, _clips.Length)]; //choose random
        else
        {
            //sequential clip handling
            vocalSource.clip = _clips[_ind];
            _ind++;

            if (_ind > _clips.Length - 1)
                _ind = 0;

            _curInd = _ind;
        }

        //play clip
        vocalSource.Play();
    }

}
