using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

[CreateAssetMenu(fileName = "EngineEntityData", menuName = "Data/EngineEntity/EntityData", order = 1)]
public class EngineEntityData : ScriptableObject
{
    public enum UIChildOptionsType { None, EntityWorldSpace, EntityRootUI}

    public Sprite avatarIcon;

    //entity
    public EntityDataManager entityDataManager;
    public int entityId;

    //hp
    public EngineEventManager engineEventManager;
    public EngineValueDataManager engineValueManager;
    public EngineValueEntity[] engineValueSelections;

    //Value UI
    public bool spawnUI;
    public UIEngineValueEntity UIToSpawn;
    public UIChildOptionsType childOptions;

    //UI hookups
    public bool syncValuesToUI;
    public LayoutSync.UISyncType entitySyncType;
    public LayoutSyncEntity[] entityLayoutSyncs;

    public string[] GetValueSelectionNames()
    {
        var names = new string[engineValueSelections.Length];
        for (int i = 0; i < engineValueSelections.Length; i++)
        {
            names[i] = engineValueSelections[i].engineValueName;
        }
        return names;
    }

}

