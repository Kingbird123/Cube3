using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : EngineEntity
{
    public enum SpawnSkinOptions { FromData, Override, Child, None }

    public new UnitData Data { get { return (UnitData)data; } }
    public new UnitData CurData { get { return (UnitData)curData; } }

    [SerializeField] protected SpawnSkinOptions skinOptions = SpawnSkinOptions.None;
    public SpawnSkinOptions SkinOption { get { return skinOptions; } }
    [SerializeField] protected GameObject spawnSkinOverride = null;
    [SerializeField] protected GameObject childSkinOverride = null;
    [SerializeField] protected ItemLocationData spawnLocationData = null;
    [SerializeField] protected ChildName[] spawnLocations = null;
    public ChildName[] SpawnLocations { get { return spawnLocations; } set { spawnLocations = value; } }

    protected bool stunned;
    public bool IsStunned { get { return stunned; } }
    private Coroutine stunnedCoroutine;

    protected bool physicsIgnored;
    public bool IsPhysicsIgnored { get { return physicsIgnored; } }
    private Coroutine physicsIgnoredCoroutine;

    protected bool meshesChanged;
    public bool IsMeshesChanged { get { return meshesChanged; } }
    private Renderer[] meshesToChange;
    private Material[] startMeshesMaterials;
    private Coroutine meshChangeCoroutine;

    protected bool invincible;
    public bool IsInvincible { get { return invincible; } set { invincible = value; } }
    private Coroutine invincibleCoroutine;

    protected float curWeight;
    public float CurWeight { get { return curWeight; } }
    protected bool dead;
    public bool IsDead { get { return dead; } }
    protected UnitController controller;
    protected NavMeshAgent agent;

    protected List<UnitBuff> curBuffs = new List<UnitBuff>();

    protected UnitEquip equip;
    protected UnitAnimations anim;
    protected Rigidbody2D rb;
    protected Collider2D col;
    public Collider2D Collider { get { return col; } }

    protected Vector3 spawnPos;
    protected GameObject curSkin;

    protected WaveSpawnerSequence spawnerManager;
    public bool IsInSpawnWave { get { return spawnerManager != null; } }

    protected override void Awake()
    {       
        base.Awake();
        GetSpawnPos();
        SpawnSkin();
    }

    protected override void Start()
    {
        base.Start();
        SetupUnit(true, true);
    }

    protected virtual void Update()
    {
        CheckKillHeight();
    }

    protected override void GetComponents()
    {
        base.GetComponents();
        controller = GetComponent<UnitController>();
        agent = GetComponent<NavMeshAgent>();
        equip = GetComponent<UnitEquip>();
        anim = GetComponent<UnitAnimations>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void GetSpawnPos()
    {
        spawnPos = transform.position;
    }

    public virtual void ChangeSkin(Unit _unit)
    {
        SetDefaultEngineValues();
        ChangeSkin(_unit.CurData);
    }

    public virtual void ChangeSkin(UnitData _unitData, bool _defaultVitals = false)
    {
        SetData(_unitData);
        if (_defaultVitals)
            SetDefaultEngineValues();
        SpawnSkin();
        SpawnUI();
        if (equip)
        {
            equip.GetComponents();
            equip.SpawnItems();
        }
        SetupUnit(false, false);
    }

    public override void SetData(EngineEntityData _data)
    {
        base.SetData(_data);

        //update unit stats
        //weight
        curWeight = CurData.weight;
        if (rb)
            rb.mass = curWeight;

        //update controller stats
        if (controller)
        {
            controller.BaseSpeed = CurData.speed;
            controller.JumpPower = CurData.jumpPower;
            //controller.StartColHeight = curData.skinSize;
            controller.StartColCenter = CurData.skinSize / 2;
        }


        if (agent)
        {
            agent.speed = CurData.speed;
        }
        
    }

    protected virtual void SpawnSkin()
    {
        if (skinOptions != SpawnSkinOptions.None)
        {
            //drop current droppable item
            if (equip)
                equip.DropCurrentItem();
            //destroy current skin
            if (curSkin)
                Destroy(curSkin);

            if (skinOptions == SpawnSkinOptions.FromData)
            {
                if (CurData.setSkin)
                {
                    //spawn new skin
                    curSkin = Instantiate(CurData.skinPrefab, transform.position, transform.rotation);
                    curSkin.transform.SetParent(transform);
                    //set Rotation
                    curSkin.transform.localEulerAngles = CurData.skinRotation;
                    SpawnLocations = CurData.itemSpawnLocations;

                    //set collider size
                    var capsule = col as CapsuleCollider2D;
                    if (capsule)
                    {
                        capsule.size = CurData.skinSize;
                        capsule.offset = new Vector2(0, capsule.size.y / 2);
                    }
                }
            }
            else if (skinOptions == SpawnSkinOptions.Override)
            {
                curSkin = Instantiate(spawnSkinOverride, transform.position, transform.rotation);
                curSkin.transform.SetParent(transform);
            }    
            else if (skinOptions == SpawnSkinOptions.Child)
            {
                curSkin = childSkinOverride;
                SpawnLocations = spawnLocations;
            }
                

        }
            
        if (curSkin)
        {
            //set animator
            var an = curSkin.GetComponent<Animator>();
            if (anim)
                anim.Animator = an;

            GetMaterials();
        }
        else
            Debug.Log("No skin is set up on " + gameObject.name + "! This unit will not be able to use items.");
    }

    protected virtual void SetupUnit(bool _resetVitals, bool _resetLives)
    {
        dead = false;
        //vitals
        if (_resetVitals)
        {
            //SetDefaultEngineValues();
        }

        //buffs
        ActivateAllBuffs(false);
        curBuffs.Clear();
        if (CurData.buffs.Length > 0)
            curBuffs.AddRange(CurData.buffs);
        ActivateAllBuffs(true);

        if (anim)
            anim.PlayIdle();

    }

    public virtual void AddBuff(UnitBuff _buffToAdd)
    {
        _buffToAdd.ActivateBuff(this, true);
        curBuffs.Add(_buffToAdd);
    }

    public virtual void RemoveBuff(UnitBuff _buffToRemove)
    {
        if (!curBuffs.Contains(_buffToRemove))
            return;

        _buffToRemove.ActivateBuff(this, false);
        curBuffs.Remove(_buffToRemove);
    }

    public virtual void ActivateAllBuffs(bool _activate)
    {
        foreach (var buff in curBuffs)
        {
            buff.ActivateBuff(this, _activate);
        }
    }

    protected virtual void CheckKillHeight()
    {
        if (dead)
            return;

        if (transform.position.y < GameManager.instance.GetKillHeight())
        {
            Die();
        }
    }


    //void ChangeMeshes()
    //{
    //    if (!curData.changeMeshMaterialOnHit)
    //        return;

    //    if (meshChangeCoroutine != null)
    //        StopCoroutine(meshChangeCoroutine);
    //    meshChangeCoroutine = StartCoroutine(StartMeshChange());
    //}

    void GetMaterials()
    {
        //if (!curData.changeMeshMaterialOnHit)
            //return;

        meshesToChange = curSkin.GetComponentsInChildren<Renderer>();
        if (meshesToChange.Length > 0)
        {
            startMeshesMaterials = new Material[meshesToChange.Length];
            for (int i = 0; i < meshesToChange.Length; i++)
            {
                startMeshesMaterials[i] = meshesToChange[i].material;
            }
        }
        else
            Debug.Log("No Meshrenderers found on " + curSkin);

    }

    //IEnumerator StartMeshChange()
    //{
    //    ActivateChangeMesh(true);
    //    yield return new WaitForSeconds(curData.changeMeshTime);
    //    ActivateChangeMesh(false);

    //}

    //void ActivateChangeMesh(bool _activate)
    //{
    //    if (_activate)
    //    {
    //        for (int i = 0; i < meshesToChange.Length; i++)
    //        {
    //            if (meshesToChange[i])
    //                meshesToChange[i].material = curData.materialToUse;
    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < meshesToChange.Length; i++)
    //        {
    //            if (meshesToChange[i])
    //                meshesToChange[i].material = startMeshesMaterials[i];
    //        }
    //    }

    //}

    public override void Die(string _reason = default)
    {
        base.Die();
        dead = true;
        ActivateAllBuffs(false);
    }

    public override void Respawn()
    {
        base.Respawn();
        dead = false;
        ResetUnitPosition(spawnPos);
    }

    protected virtual void ResetUnitPosition(Vector3 _pos)
    {
        Debug.Log("resetting " + gameObject.name + " position to " + _pos);
        transform.position = _pos;
        if (rb)
            rb.Sleep();
        if (col)
            col.enabled = true;
    }

    void ResetLevel()
    {
        GameManager.instance.GetSceneTransitionData().ResetCurLevel(true);
    }

    public void SetWaveSpawnerManager(WaveSpawnerSequence _manager)
    {
        spawnerManager = _manager;
    }

}
