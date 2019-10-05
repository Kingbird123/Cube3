using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public enum UserMode { None, CurUser, Override }
    [SerializeField] private bool setData = false;
    [SerializeField] private UserMode userMode = UserMode.None;
    [SerializeField] private UserDataManager userDataManager;
    [SerializeField] private IndexStringProperty user = null;

    private int curPoints;
    public int CurPoints { get { return curPoints; } }

    public new UIPlayer UI { get { return (UIPlayer)ui; } }
    private UserDataManager dataManager;
    private SpawnCheckPointManager spawnManager;
    private PlayerSkinManager skinManager;

    private PlayerSoundFX soundFX;
    private User curUser;

    protected override void GetComponents()
    {
        base.GetComponents();
        //get components
        var gm = GameManager.instance;
        if (gm)
        {
            dataManager = gm.GetUserDataManager();
            spawnManager = gm.GetSpawnManager();
            skinManager = gm.GetSkinManager();
            gm.SpawnedPlayer = this;
        }
    }

    protected override void LoadDefaultData()
    {
        if (setData)
        {
            curData = Data;
        }
        else
        {
            if (userMode == UserMode.CurUser)
                curUser = dataManager.GetCurUser();
            else if (userMode == UserMode.Override)
                curUser = dataManager.GetUser(user.indexValue);
            
            curData = skinManager.playerSkins[curUser.playerSkinInd];
        }
        SetData(curData);

    }

    protected override void SpawnSkin()
    {
        base.SpawnSkin();
        //get sound
        if (curSkin)
            soundFX = curSkin.GetComponent<PlayerSoundFX>();
    }

    public override void SetData(EngineEntityData _data)
    {
        base.SetData(_data);

        //update data
        if (skinManager)
        {
            if (skinManager.playerSkins.IndexOf(CurData) != -1)
                dataManager.SetPlayerSkinData(skinManager.playerSkins.IndexOf(CurData));
            else
                Debug.Log("Make sure you add " + _data + " to the skin manager: " + skinManager + "!");
        }
    }

    protected override void SpawnUI()
    {
        base.SpawnUI();
        if (ui)
            GameManager.instance.SpawnedUI = (UIPlayer)ui;
    }

    public void AddPoints(int _amount)
    {
        curPoints += _amount;
        //if (UI)
            //UI.SetPointsValue(curPoints);
    }

    public override void Die(string _reason)
    {
        base.Die();

        //play death sound
        if (soundFX)
            soundFX.PlayDeathSound();
    }

    protected override void ResetUnitPosition(Vector3 _pos)
    {
        var pos = spawnManager.GetCurCheckPointPos();
        if (pos != default(Vector3))
            base.ResetUnitPosition(pos);
        else
            Debug.LogError("Failed to respawn player!");
    }

}
