using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionOverlay : MonoBehaviour
{
    [SerializeField] private bool loadLevelOnSpawn = true;
    [SerializeField] private bool overrideLevelLoad = false;
    [SerializeField] private string overrideLevelName = null;
    [SerializeField] private Slider loadingBar = null;

    [SerializeField] private bool fadeIn = false;
    [SerializeField] private float fadeDelay = 0.3f;
    [SerializeField] private float fadeTime = 3;

    private string nextLevel;

    private float curProgress;
    public float CurLoadingProgress { get { return curProgress; } }

    [SerializeField] private GameObject[] UIObjectsToFade = null;

    // Use this for initialization
    void Start ()
    {
        //UI set
        loadingBar.maxValue = 1;
        loadingBar.minValue = 0;

        loadingBar.gameObject.SetActive(false);

        if (fadeIn)
            StartCoroutine(StartFadeIn());

        if (loadLevelOnSpawn)
            BeginLevelLoading();
    }

    public void BeginLevelLoading ()
    {
        loadingBar.gameObject.SetActive(true);
        StartCoroutine (StartLevelLoad ());
    }

    IEnumerator StartLevelLoad ()
    {
        if (overrideLevelLoad)
            nextLevel = overrideLevelName;

        AsyncOperation loadingScene = SceneManager.LoadSceneAsync (nextLevel, LoadSceneMode.Single);

        while (!loadingScene.isDone)
        {
            curProgress = loadingScene.progress;
            loadingBar.value = Mathf.Lerp(loadingBar.minValue, loadingBar.maxValue, curProgress);
            yield return new WaitForEndOfFrame();
        }
        loadingBar.value = loadingBar.maxValue;
        //reset timescale if game paused
        Time.timeScale = 1;
        yield return new WaitForSeconds (fadeDelay);
        StartCoroutine (StartFadeOut ());
    }

    IEnumerator StartFadeIn()
    {
        float timer = 0;
        float perc = 0;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            if (timer > fadeTime)
                timer = fadeTime;

            perc = timer / fadeTime;
            SetUIAlphas(perc);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator StartFadeOut ()
    {
        float timer = 0;
        float perc = 0;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            if (timer > fadeTime)
                timer = fadeTime;

            perc = timer / fadeTime;
            SetUIAlphas (1 - perc);
            yield return new WaitForEndOfFrame ();
        }

        DestroyLoadingUI ();
    }

    void SetUIAlphas (float _alpha)
    {
        foreach (var obj in UIObjectsToFade)
        {

            Text txt = obj.GetComponent<Text> ();
            Image img = obj.GetComponent<Image> ();
            if (txt)
            {
                Color prevCol = txt.color;
                Color col = new Color (prevCol.r, prevCol.g, prevCol.b, _alpha);
                txt.color = col;
            }
            if (img)
            {
                Color prevCol = img.color;
                Color col = new Color (prevCol.r, prevCol.g, prevCol.b, _alpha);
                img.color = col;
            }
        }
    }

    public void SetNextLevel(string _level)
    {
        nextLevel = _level;
    }

    void DestroyLoadingUI ()
    {
        Destroy (gameObject);
    }
}