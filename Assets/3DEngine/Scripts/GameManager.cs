using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] private UserDataManager userData = null;
    [SerializeField] private PlayerSkinManager skinManager = null;
    [SerializeField] private SpawnCheckPointManager spawnManager = null;
    [SerializeField] private bool initializeSpawnManagerOnStart = true;
    [SerializeField] private SceneTransitionData sceneTransData = null;
    [SerializeField] private EngineValueUIManagerData valueUIManagerData = null;
    [SerializeField] private SpawnPoolManager poolManagerData = null;
    [SerializeField] private float killHeight = -20;
    [SerializeField] private MenuManager menuManager = null;
    [SerializeField] private MusicManager musicManager = null;

    private Player spawnedPlayer;
    public Player SpawnedPlayer { get { return spawnedPlayer; } set { spawnedPlayer = value; } }
    public UIPlayer SpawnedUI { get; set; }
    private bool won;
    public bool IsWon { get { return won; } }
    private bool gameOver;
    public bool IsGameOver { get { return gameOver; } }
	// Use this for initialization
	void Awake ()
    {
        instance = this;
        InitializePool();
        InitializeUIManager();
	}

    void Start()
    {
        gameOver = false;
        BeginMusic();
        if (userData)
            userData.InitializeData();
        if (spawnManager)
        {
            if (initializeSpawnManagerOnStart)
                spawnManager.Initialize();
        }
            
    }
	
    void InitializePool()
    {
        if (poolManagerData)
            poolManagerData.Initialize();
    }

    void InitializeUIManager()
    {
        if (valueUIManagerData)
            valueUIManagerData.Initialize();
    }

    void BeginMusic()
    {
        if (musicManager)
            //play music
            musicManager.PlayBackgroundMusic();
    }

    public void LevelWin(string _nextScene, float _endTime, bool _freezeGame, bool _freezePlayer)
    {
        StartCoroutine(StartLevelWin(_nextScene, _endTime, _freezeGame, _freezePlayer));
    }

    IEnumerator StartLevelWin(string _nextScene, float _endTime, bool _freezeGame, bool _freezePlayer)
    {
        won = true;

        //do UI effects

        //freeze game
        float curScale = Time.timeScale;
        if (_freezeGame)
        {
            Time.timeScale = 0;
        }
        
        //freezePlayer
        if (_freezePlayer)
        {
            var cont = spawnedPlayer.GetComponent<UnitController>();
            cont.DisableMovement(true);
            cont.DisableAiming(true);
        }

        //play win music
        if (musicManager)
            musicManager.PlayGameOverWinMusic();

        //wait to load next level
        yield return new WaitForSecondsRealtime(_endTime);

        Time.timeScale = curScale;

        //load level
        sceneTransData.LoadLevelWithLoadingScreen(_nextScene);

    }

    public void GameOverLose()
    {
        gameOver = true;
        userData.SaveCheckPoint(0);
        if (musicManager)
            musicManager.PlayGameOverLoseMusic();
        sceneTransData.LoadGameOverScene();
    }

    public UserDataManager GetUserDataManager()
    {
        return userData;
    }

    public PlayerSkinManager GetSkinManager()
    {
        return skinManager;
    }

    public SpawnCheckPointManager GetSpawnManager()
    {
        return spawnManager;
    }

    public SceneTransitionData GetSceneTransitionData()
    {
        return sceneTransData;
    }

    public MenuManager GetMenuManager()
    {
        return menuManager;
    }

    public MusicManager GetMusicManager()
    {
        return musicManager;
    }

    public float GetKillHeight()
    {
        return killHeight;
    }

}
