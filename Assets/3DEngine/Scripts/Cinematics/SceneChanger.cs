using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour 
{
    [SerializeField] private bool changeOnTrigger = false;
    [SerializeField] private string triggerTag = null;
    [SerializeField] private bool changeOnAnimEnd = false;
    [SerializeField] private Animation anim = null;
    [SerializeField] private float sceneTime = 0;
    [SerializeField] private string nextSceneName = null;
    [SerializeField] private bool loadInBackground = false;
    [SerializeField] private float backgroundStart = 0;
    private AsyncOperation loadingScene;

	// Update is called once per frame
	void Start () 
	{
        if (changeOnTrigger)
            return;


        if (changeOnAnimEnd)
        {
            StartCoroutine(ChangeOnAnimEnd());
        }
        else
            StartCoroutine(ChangeScene());

        if (loadInBackground)
        {
            StartCoroutine(LoadInBackground());
        }

	}

    void OnTriggerEnter(Collider _col)
    {
        if (changeOnTrigger && _col.tag == triggerTag)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(sceneTime);
        if (loadInBackground)
        {
            loadingScene.allowSceneActivation = true; //load background scene
        }
        else
            SceneManager.LoadScene(nextSceneName);

    }

    IEnumerator LoadInBackground()
    {
        //wait to start loading
        yield return new WaitForSeconds(backgroundStart);
        //load scene in background
        loadingScene = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
        //deactivate final loading
        loadingScene.allowSceneActivation = false;
        //will only load up to 90% with scene deactivation
        while (loadingScene.progress < 0.9f)
        {
            Debug.Log("Loading " + nextSceneName + " " + loadingScene.progress * 100 + "%");
            yield return new WaitForSeconds(0.1f); //wait to prevent console spamming
        }

        Debug.Log("Loading " + nextSceneName + " Complete");
    }

    IEnumerator ChangeOnAnimEnd()
    {
        while (anim.isPlaying)
        {
            yield return new WaitForSeconds(0);
        }
        SceneManager.LoadScene(nextSceneName);
    }
}
