using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Data/Units/UnitData", order = 1)]
public class UnitData : EngineEntityData
{
    public enum DeathType { None, Destroy, Respawn }
    public enum NoLivesType { None, GameOverLose, GameOverWin }
    public enum DeathMovement { None, StopIfGrounded, FallThrough }

    //Skin
    public bool setSkin;
    public GameObject skinPrefab;
    public Vector2 skinSize;
    public Vector3 skinRotation;

    //item spawn settings
    public ItemLocationData locationData;
    public ChildName[] itemSpawnLocations;

    //rigidbody
    public float weight = 1;

    //movement
    public float speed = 3;
    public float runSpeed = 6;
    public float backwardSpeed = 2;
    public float climbingSpeed = 2;
    public float jumpPower = 6;

    //buffs
    public UnitBuff[] buffs;
}
