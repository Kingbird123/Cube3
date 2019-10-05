using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : EngineEntityData
{
    public GameObject connectedPrefab;
    public System.Type linkedType;
    public bool keepUIActiveIfDropped;
    public bool setOwnerAsParent;
    public float weight;
    public bool droppable;
    public GameObject droppedPrefab;
    public bool quickMenuCompatible;
    public GameObject particleFX;
    public string animState;
    public ItemBuff[] buffs;
    public ItemLocationData locationData;
    public IndexStringProperty defaultSpawnLocation;
}
