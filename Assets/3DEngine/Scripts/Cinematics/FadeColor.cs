using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeColor : MonoBehaviour 
{
    [SerializeField] private bool fadeToColor = false;
    [SerializeField] private float fadeTime = 2;
    private float fadeTimer;
    [SerializeField] private float fadeDelay = 0;
    [SerializeField] private Image fadeImage = null;

	// Use this for initialization
	void Start () 
	{
        StartCoroutine(FadeDelay());

        if (!fadeToColor)
        {
            Color col = fadeImage.color;
            col.a = 1;
            fadeImage.color = col;
        }

    }

    IEnumerator FadeDelay()
    {
        yield return new WaitForSeconds(fadeDelay);
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        fadeTimer = 0;
        float alpha = 0;
        Color col = fadeImage.color;
        while (fadeTimer < fadeTime)
        {
            fadeTimer += Time.deltaTime;
            float t = fadeTimer / fadeTime;

            if (fadeToColor)
                alpha = Mathf.Lerp(0, 1, t);
            else
                alpha = Mathf.Lerp(1, 0, t);

            col.a = alpha;
            fadeImage.color = col;

            yield return new WaitForEndOfFrame();
        }
    }

}
