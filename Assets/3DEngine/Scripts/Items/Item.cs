using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public abstract class Item : EngineEntity
{
    public new ItemData Data { get { return (ItemData)data; } }
    protected bool dropped;
    public bool IsDropped { get { return dropped; } }
    protected Unit curUnitOwner;
    public Unit UnitOwner { get { return curUnitOwner; } }
    protected Unit lastUnitOwner;
    public Unit LastUnitOwner { get { return lastUnitOwner; } }
    protected UnitAnimations ownerAnim;
    protected UnitEquip ownerEquip;

    protected List<ItemBuff> curBuffs = new List<ItemBuff>();
    public List<ItemBuff> CurBuffs { get { return curBuffs; } }

    protected UnitEquip.InputType inputType;

    protected bool spawnedOnce;
    public bool SpawnedOnce { get { return spawnedOnce; } }

    protected override void Start()
    {
    }

    protected virtual void OnEnable()
    {
        if (!curUnitOwner)
            Timing.RunCoroutine(StartWaitForOwner());
        else
            SpawnUI();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (ui)
            Destroy(ui.gameObject);
        if (curUnitOwner)
            ActivateAllBuffs(false);
    }

    public virtual void SetOwner(Unit _unit)
    {
        curUnitOwner = _unit;

        if (curUnitOwner)
            lastUnitOwner = curUnitOwner;
    }

    IEnumerator<float> StartWaitForOwner()
    {
        while (!curUnitOwner && !dropped)
        {
            yield return Timing.WaitForOneFrame;
        }
        OnOwnerFound();
        if (!dropped)
            ActivateAllBuffs(true);
    }

    protected virtual void OnOwnerFound()
    {
        ownerEquip = curUnitOwner.GetComponent<UnitEquip>();
        ownerAnim = curUnitOwner.GetComponent<UnitAnimations>();
        inputType = ownerEquip.InputOption;

        var locInd = Data.defaultSpawnLocation.indexValue;
        var spawnLocName = curUnitOwner.SpawnLocations[locInd].stringValue;
        var spawnLoc = curUnitOwner.transform.FindDeepChild(spawnLocName);
        //set position of item
        transform.SetParent(spawnLoc);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        //spawn UI here instead of start
        SpawnUI();

    }

    protected override void LoadDefaultData()
    {
        if (!spawnedOnce)
            base.LoadDefaultData();
        if (dropped)
            return;
        spawnedOnce = true;
        curBuffs = new List<ItemBuff>();
        curBuffs.AddRange(Data.buffs);
    }

    public virtual void CopyValues(Item _item)
    {
        spawnedOnce = _item.SpawnedOnce;
        curBuffs.Clear();
        curBuffs.AddRange(_item.CurBuffs);
        if (_item.LastUnitOwner)
            lastUnitOwner = _item.LastUnitOwner;
    }

    public virtual void PickUp()
    {
        dropped = false;
    }

    public virtual void Drop()
    {
        dropped = true;
    }

    public virtual void AddBuff(ItemBuff _buffToAdd)
    {
        _buffToAdd.ActivateBuff(this, true);
        curBuffs.Add(_buffToAdd);
    }

    public virtual void RemoveBuff(ItemBuff _buffToRemove)
    {
        if (!curBuffs.Contains(_buffToRemove))
            return;

        _buffToRemove.ActivateBuff(this, false);
        curBuffs.Remove(_buffToRemove);
    }

    public virtual void ActivateAllBuffs(bool _activate)
    {
        if (!(curBuffs.Count > 0))
            return;

        foreach (var buff in curBuffs)
        {
            buff.ActivateBuff(this, _activate);
        }
    }

    public virtual void KillItem()
    {
        if (curUnitOwner)
        {
            var equip = curUnitOwner.GetComponent<UnitEquip>();
            equip.RemoveItem(Data);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
