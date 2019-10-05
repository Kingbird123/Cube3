using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

[System.Serializable]
public class User
{
    public string playerName = "PlayerName";
    public int playerSkinInd;
    public int userId;
    public int saveSlotId;
    public int maxHealth = 100;
    public int lives = 3;
    public int points;
    public int levelUnlocked;
    public string levelName;
    public int curCheckPoint;
    public ItemProperty[] inventoryItems;
}
