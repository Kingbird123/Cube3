using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour 
{

    [SerializeField]
    private int mainMenuBuildInd = 0;

    private SceneTransitionData lm;

    void Start()
    {
        lm = GameManager.instance.GetSceneTransitionData();
    }

	public void LoadMainMenu()
    {
        lm.LoadLevel(mainMenuBuildInd);
    }

    public void RestartLevel()
    {
        lm.ResetCurLevel(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
