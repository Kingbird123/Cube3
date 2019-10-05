using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private bool freezeOnPause = false;
    [SerializeField] private bool enableCameraSwitching = false;
    [SerializeField] private CameraSwitcherContainer cameraSwitch;

    private CinemachineSwitchMaster helper;
    private UIPlayer playerUI;

    private bool paused;
    public bool IsPaused { get { return paused; } }

    public static MenuManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        helper = FindObjectOfType(typeof(CinemachineSwitchMaster)) as CinemachineSwitchMaster;

        //unfreeze the game
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGameOver();
        GetInputs();
        cameraSwitch.DetectCameraSwitch();
    }

    void GetInputs()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (!paused)
            {
                PauseGame(true);
                paused = true;
            }
            else
            {
                PauseGame(false);
                paused = false;
            }
        }
    }

    void CheckGameOver()
    {
        var gm = GameManager.instance;
        if (gm)
        {
            if (gm.IsGameOver)
            {
                if (!playerUI)
                {
                    playerUI = GameManager.instance.SpawnedUI;
                    if (playerUI)
                        playerUI.SetSystemCursor(true, false);
                }
            }
        }
    }

    void PauseGame(bool _pause)
    {
        if (!playerUI)
        {
            playerUI = GameManager.instance.SpawnedUI;
        }

        if (_pause)
        {
            
            
            //activatae ui
            playerUI.PauseMenuSetActive(true);

            if (freezeOnPause)
                Time.timeScale = 0;
        }
            
        else
        {

            playerUI.PauseMenuSetActive(false);

            if (freezeOnPause)
                Time.timeScale = 1;
        }
           
    }

    public bool isPaused()
    {
        return paused;
    }

    public void SetPlayerUI(UIPlayer _playerUI)
    {
        playerUI = _playerUI;
    }
}
