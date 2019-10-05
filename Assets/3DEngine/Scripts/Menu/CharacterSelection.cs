using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour 
{

    private const string CHARKEY = "CharKey";
    private int curInd;

    
    [SerializeField] private CharacterSelectionUI charUI = null;
    [SerializeField] private Transform spawnPos = null;

    private GameObject curSpawnedSkin;
    private PlayerSkinManager skinManager;
    private UserDataManager dataManager;
    

    void Start()
    {
        dataManager = GameManager.instance.GetUserDataManager();
        skinManager = GameManager.instance.GetSkinManager();
        curInd = dataManager.GetCurUser().playerSkinInd;
        SwitchCharacter();

    }

    public void PrevCharacter()
    {
        if (curInd > 0)
            curInd--;
        else
            curInd = skinManager.playerSkins.Count - 1;

        SwitchCharacter();
    }

    public void NextCharacter()
    {
        if (curInd < skinManager.playerSkins.Count - 1)
            curInd++;
        else
            curInd = 0;

        SwitchCharacter();
    }

    void SwitchCharacter()
    {
        if (curSpawnedSkin)
            Destroy(curSpawnedSkin);

        var spawn = skinManager.playerSkins[curInd].skinPrefab;
        curSpawnedSkin = Instantiate(spawn, spawnPos.position, spawnPos.rotation, spawnPos);

        charUI.SetCharName(spawn.name);
    }

    public void ConfirmCharacter()
    {
        dataManager.SetPlayerSkinData(curInd);
    }

}
