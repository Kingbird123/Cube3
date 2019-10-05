using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileData : ItemData
{
    public enum DestroyType { OnImpact, HitAmount, None }
    public enum OnImpactType { DetectZone, Timer, Velocity }
    public enum CompareType { Greater, Less, Equal }

    public EngineEvent[] launchFX;
    public bool steerable;
    public float horSpeed = 1;
    public float verSpeed = 1;
    public DestroyType destroySelfType;
    public int hitMaxAmount = 1;
    public OnImpactType impactType;
    public float time;
    public CompareType triggerType;
    public float velocity;
    public GameObject[] spawnOnImpact;
    public EngineEvent[] impactFX;
    
}
