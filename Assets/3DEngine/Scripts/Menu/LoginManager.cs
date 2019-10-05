using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private GameObject loginPanelPrefab = null;
    [SerializeField] private Transform loginPanelParent = null;
    [SerializeField] private int saveSlots = 3;
    [SerializeField] private string nextSceneToPlay = null;

    private UserDataManager dataManager;
    private SceneTransitionData sceneTrans;

    private List<LoginPanel> loadedPanels = new List<LoginPanel>();

    // Use this for initialization
    void Start()
    {
        dataManager = GameManager.instance.GetUserDataManager();
        sceneTrans = GameManager.instance.GetSceneTransitionData();
        RefreshPlayerList();
    }

    public void RefreshPlayerList()
    {
        foreach (var panel in loadedPanels)
        {
            Destroy(panel.gameObject);
        }
        loadedPanels.Clear();

        for (int i = 0; i < saveSlots; i++)
        {
            var obj = Instantiate(loginPanelPrefab, loginPanelParent);
            LoginPanel panel = obj.GetComponent<LoginPanel>();
            if (panel)
            {
                panel.SetLoginManager(this);
                panel.SetSlotInd(i);
                panel.LoadNewPlayerWindow(true);

                loadedPanels.Add(panel);
            }
        }

        DisplayPlayers();
    }

    void DisplayPlayers()
    {
            
        if (dataManager.Users.Count > 0)
        {
            foreach (var panel in loadedPanels)
            {
                foreach (var user in dataManager.Users)
                {
                    if (user.saveSlotId == panel.GetSlotInd())
                    {
                        panel.LoadNewPlayerWindow(false);
                        panel.SetTextData(user.playerName, user.points,
                            user.lives, user.levelUnlocked);
                    }
                }
            }

        }
        else
        {
            LoadAllNewPlayerWindows();
        }
    }

    void LoadAllNewPlayerWindows()
    {
        foreach (var panel in loadedPanels)
        {

                panel.LoadNewPlayerWindow(true);
            
        }
    }

    public void PlayNextScene()
    {
        sceneTrans.LoadLevelWithLoadingScreen(nextSceneToPlay);
    }

    public string GetNextSceneToPlay()
    {
        return nextSceneToPlay;
    }

    public GameObject GetLoginPanelParent()
    {
        return loginPanelParent.gameObject;
    }

}
