using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

public class EngineEntity : MonoBehaviour, IPauseListener
{
    [SerializeField] protected EngineEntityData data;
    public EngineEntityData Data { get { return data; } }
    protected EngineEntityData curData;
    public EngineEntityData CurData { get { return curData; } }
    public string CurUnitDataName { get { return CurData.name; } }

    protected int entityID;
    public int EntityID { get { return entityID; } }
    protected EngineValueContainerEntity engineValueContainer;
    public EngineValueContainerEntity EngineValueContainer { get { return engineValueContainer; } }

    [SerializeField] protected SpawnUIOptions spawnUI = SpawnUIOptions.None;
    [SerializeField] protected GameObject UIToSpawn = null;
    [SerializeField] protected bool parentUIToUnit = false;
    protected UIEngineValueEntity syncedUI;
    protected UIEngineValueEntity ui;
    public UIEngineValueEntity UI { get { return ui; } }

    [SerializeField] protected Transform attackTarget;
    public Transform AttackTarget { get { return attackTarget; } }
    public bool IsPaused { get; set; }

    protected virtual void Awake()
    {
        GetComponents();
        LoadDefaultData();
        SetDefaultEngineValues();   
    }

    protected virtual void Start()
    {
        SpawnUI();
    }

    protected virtual void OnDisable()
    {
        engineValueContainer.CancelEvents();
    }

    protected virtual void GetComponents()
    {   
    }

    protected virtual void LoadDefaultData()
    {
        SetData(data);
    }

    public virtual void SetData(EngineEntityData _data)
    {
        curData = _data;
        entityID = _data.entityId;
        engineValueContainer = new EngineValueContainerEntity();
        engineValueContainer.InitializeContainer(this);
    }

    protected virtual void SpawnUI()
    {
        if (ui)
            Destroy(ui.gameObject);
        if (spawnUI != SpawnUIOptions.None)
        {
            if (spawnUI == SpawnUIOptions.FromData)
            {
                var root = transform.root.GetComponentInChildren<EngineEntity>();
                if (curData.spawnUI)
                {
                    ui = GameObject.Instantiate(data.UIToSpawn);
                    if (curData.childOptions == EngineEntityData.UIChildOptionsType.EntityWorldSpace)
                        ui.transform.SetParent(transform);
                    else if (curData.childOptions == EngineEntityData.UIChildOptionsType.EntityRootUI)
                        ui.transform.SetParent(root.ui.transform, false);
                }
                if (curData.syncValuesToUI)
                {
                    if (curData.entitySyncType == LayoutSync.UISyncType.SpawnedUI)
                        syncedUI = ui;
                    else if(curData.entitySyncType == LayoutSync.UISyncType.EntityRootOwnerUI)
                    { 
                        syncedUI = root.UI;
                    }
                    if (syncedUI)
                    {
                        for (int i = 0; i < curData.entityLayoutSyncs.Length; i++)
                        {
                            var syncs = curData.entityLayoutSyncs[i].syncSelections;
                            var layoutMaster = curData.entityLayoutSyncs[i].layoutMaster;
                            for (int ind = 0; ind < syncs.Length; ind++)
                            {
                                var sync = syncs[ind];
                                var valSel = curData.engineValueSelections[sync.valueInd];
                                var masSel = sync.masterInd;
                                var engVal = engineValueContainer.GetEngineValue(valSel.valueSelection.valueData.ID);
                                if (engVal != null)
                                    syncedUI.SyncEngineValue(engVal, layoutMaster, masSel);
                            }
                        }
                    }
                        
                }
            }
            else if (spawnUI == SpawnUIOptions.Override)
            {
                if (UIToSpawn)
                {
                    //spawn ui
                    ui = Instantiate(UIToSpawn).GetComponent<UIEngineValueEntity>();
                    if (parentUIToUnit)
                    {
                        ui.transform.position = transform.position;
                        ui.transform.rotation = transform.rotation;
                        ui.transform.SetParent(transform);
                    }

                }
            }
            //set avatar
            if (ui)
            {
                engineValueContainer.InitializeUI(ui);
                if (curData.avatarIcon)
                    ui.SetAvatarIcon(curData.avatarIcon);
            }

        }
    }

    public virtual void Repaired()
    {
    }

    public virtual void Damaged()
    {
    }

    public virtual void Die(string _reason = default)
    {
    }

    public virtual void Respawn()
    {
    }

    protected virtual void SetDefaultEngineValues()
    {
        engineValueContainer.ResetAllDefaultValues();
    }

    public virtual void ResetValueToDefault(int _id)
    {
        engineValueContainer.ResetValueToDefault(_id);
    }

    public virtual void AddEngineFloatValue(int _id, float _amount)
    {
        engineValueContainer.AddFloatValue(_id, _amount, false);
    }

    public virtual void SubtractEngineFloatValue(int _id, float _amount)
    {
        engineValueContainer.SubtractFloatValue(_id, _amount, false);
    }

    public virtual void AddEngineIntValue(int _id, int _amount)
    {
        engineValueContainer.AddIntValue(_id, _amount, false);
    }

    public virtual void SubtractEngineIntValue(int _id, int _amount)
    {
        engineValueContainer.SubtractIntValue(_id, _amount, false);
    }

    public virtual void AddToMaxValue(int _id, float _amount)
    {
        engineValueContainer.ValueMaxDelta(_id, _amount);
    }

    public EngineValue GetLocalEngineValue(int _id)
    {
        var val = engineValueContainer.GetEngineValue(_id);
        if (val != null)
            return val;

        Debug.Log("could not find local value in " + data.engineValueManager.name);
        return null;
    }
}


