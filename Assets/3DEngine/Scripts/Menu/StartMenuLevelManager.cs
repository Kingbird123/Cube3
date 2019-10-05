using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuLevelManager : MonoBehaviour 
{
    [SerializeField] private StartMenuLevel[] levels = null;

    private UserDataManager dataManager;

	// Use this for initialization
	void Start () 
	{
        GetComponents();
        DisplayPlayableLevels();
    }

    void GetComponents()
    {
        //getcomponents
        dataManager = GameManager.instance.GetUserDataManager();
    }

    public void RefreshPlayableLevels()
    {
        StartCoroutine(StartRefreshPlayableLevels());
    }

    IEnumerator StartRefreshPlayableLevels()
    {
        yield return new WaitForEndOfFrame();
        DisplayPlayableLevels();
    }

    void DisplayPlayableLevels()
    {

        foreach (var level in levels)
        {
            if (dataManager.GetCurUser().levelUnlocked >= level.GetLevelNumber())
            {
                level.SetLevelPlayable(true);
            }
            else
            {
                level.SetLevelPlayable(false);
            }

        }
    }
	
}
