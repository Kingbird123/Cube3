using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneTransitionData", menuName = "Data/Managers/SceneTransitionData", order = 1)]
public class SceneTransitionData : ScriptableObject
{
    [SerializeField] private GameObject loadUIOverlay = null;
    [SerializeField] private GameObject gameOverUIOverlay = null;

    private int curCheckPoint;
    private string curScene;

    #region SCENEMANAGEMENT

    public void LoadLevel(string _name)
    {
        SceneManager.LoadScene(_name);
    }

    public void LoadLevel(int _buildInd)
    {
        SceneManager.LoadScene(_buildInd);
    }

    public void LoadLevelWithLoadingScreen(string _levelName)
    {
            var overlay = Instantiate(loadUIOverlay).GetComponent<SceneTransitionOverlay>();
            overlay.SetNextLevel(_levelName);
    }

    public void ResetCurLevel(bool _useLoadingScreen)
    {
        var sceneName = SceneManager.GetActiveScene().name;
        if (_useLoadingScreen)
            LoadLevelWithLoadingScreen(sceneName);
        else
            SceneManager.LoadScene(sceneName);
    }

    public string GetCurLevelName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public int GetCurLevelInd()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public string GetLevelName(int _buildInd)
    {
        return SceneManager.GetSceneByBuildIndex(_buildInd).name;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadGameOverScene()
    {
        Instantiate(gameOverUIOverlay);
    }

    public void LevelWin()
    {
        
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
    }

    #endregion
}
