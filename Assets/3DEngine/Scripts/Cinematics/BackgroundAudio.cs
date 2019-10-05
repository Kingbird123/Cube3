using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{

    [SerializeField] private bool dontDestroy = false;
    [SerializeField] private AudioSource audSource = null;
    [SerializeField] private AudioClip clip = null;
    [SerializeField] private float delayStartTime = 0;
    [SerializeField] private float delayEndTime = 30;
    [SerializeField] private float fadeInTime = 1;
    [SerializeField] private float fadeOutTime = 1;
    private float fadeTimer;

    // Use this for initialization
    void Awake()
    {
        if (dontDestroy)
            DontDestroyOnLoad(this.gameObject);

        audSource.playOnAwake = false;
    }

    void Start()
    {
        StartCoroutine(DelayClip());
    }

    void PlayClip()
    {
        audSource.clip = clip;
        audSource.Play();
    }

    IEnumerator DelayClip()
    {
        yield return new WaitForSeconds(delayStartTime);
        StartCoroutine(Fade(true, fadeInTime));
        yield return new WaitForSeconds(delayEndTime);
        StartCoroutine(Fade(false, fadeOutTime));
    }

    IEnumerator Fade(bool _fadeIn, float _fadeTime)
    {
        if (_fadeIn)
            PlayClip();

        fadeTimer = 0;
        float volume = 0;

        while (fadeTimer < _fadeTime)
        {
            fadeTimer += Time.deltaTime;
            float perc = fadeTimer / _fadeTime;

            if (_fadeIn)
                volume = Mathf.Lerp(0, 1, perc);
            else
                volume = Mathf.Lerp(1, 0, perc);

            audSource.volume = volume;

            yield return new WaitForEndOfFrame();
        }
    }

}
