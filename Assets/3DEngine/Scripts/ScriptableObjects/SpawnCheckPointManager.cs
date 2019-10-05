using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnCheckPoint", menuName = "Data/Managers/SpawnCheckPoint", order = 1)]
public class SpawnCheckPointManager : ScriptableObject
{
    [SerializeField] private bool overrideCheckPoint = false;
    [SerializeField] private int checkPoint = 0;
    [SerializeField] private LayerMask detectMask = -1;
    [SerializeField] private DetectZone[] checkPoints = null;
    [SerializeField] private GameObject playerSpawn = null;
    [SerializeField] private bool progressOnly = false;
    [SerializeField] private bool saveProgressToDisc = false;
    [SerializeField] private bool resetProgressOnQuit = false;
    private int curCheckPoint;
    [SerializeField] private int sceneUnlocked = 0;
    [SerializeField] private string nextSceneToPlay = null;
    [SerializeField] private bool freezeGame = false;
    [SerializeField] private bool freezePlayer = false;
    [SerializeField] private float endTime = 1;

    private UserDataManager dataManager;
    private SceneTransitionData sceneTransData;

    // Use this for initialization
    public void Initialize()
    {
        sceneTransData = GameManager.instance.GetSceneTransitionData();
        dataManager = GameManager.instance.GetUserDataManager();

        GameManager.instance.StartCoroutine(StartCheckPointDetection());

        SpawnPlayer();
    }


    public void SpawnPlayer()
    {
        var savedName = default(string);
        if (dataManager)
        {
            savedName = dataManager.GetCurLevelName();
            if (overrideCheckPoint)
                curCheckPoint = checkPoint;
            else if (savedName == sceneTransData.GetCurLevelName())
            {
                curCheckPoint = dataManager.GetCurCheckPoint();
                Debug.Log("Saved level name matches current level...spawning player at checkpoint " + curCheckPoint);
            }
            else
            {
                Debug.Log("Saved level name does not match current level name...Resetting checkpoint to 0");
                curCheckPoint = 0;
            }
        }

        //store spawn in transform var for respawn use
        GameManager.instance.SpawnedPlayer = Instantiate(playerSpawn, checkPoints[curCheckPoint].worldPos,
        Quaternion.identity).GetComponent<Player>();
    }

    public Vector3 GetCurCheckPointPos()
    {
        if (checkPoints.Length > 0)
            return checkPoints[curCheckPoint].worldPos;
        return default(Vector3);
    }

    public void SetCurCheckPoint(int _ind)
    {
        if (progressOnly)
        {
            if (_ind > curCheckPoint)
                curCheckPoint = _ind;
        }
        else
            curCheckPoint = _ind;

        if (saveProgressToDisc)
        {
            dataManager.GetCurUser().curCheckPoint = (curCheckPoint);
            dataManager.SaveLevelName(sceneTransData.GetCurLevelName());
        }
    }

    void ResetAllProgress()
    {
        Debug.Log("Erasing Checkpoint progress for " + sceneTransData.GetCurLevelName());
        dataManager.SaveCheckPoint(0);
        dataManager.SaveLevelName("");
    }

    void OnApplicationQuit()
    {
        if (resetProgressOnQuit)
        {
            ResetAllProgress();
        }
        else
        {
            Debug.Log("Keeping Checkpoint progress for " + sceneTransData.GetCurLevelName());
            dataManager.SaveLevelName(sceneTransData.GetCurLevelName());
            dataManager.SaveCheckPoint(curCheckPoint);
        }
    }

    public IEnumerator StartCheckPointDetection()
    {
        while (Application.isPlaying)
        {
            for (int i = 0; i < checkPoints.Length; i++)
            {
                if (checkPoints[i].Detected())
                {
                    
                    if (i == checkPoints.Length - 1)
                        FinishLevel();
                    else if (curCheckPoint < i)
                        SetCurCheckPoint(i);
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    void FinishLevel()
    {
        if (GameManager.instance.IsWon)
            return;

        //save data
        dataManager.UnlockLevel(sceneUnlocked);
        //using Game manager to "win" the level
        GameManager.instance.LevelWin(nextSceneToPlay, endTime, freezeGame, freezePlayer);
    }
}
