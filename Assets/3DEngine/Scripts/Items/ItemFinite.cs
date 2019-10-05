using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemFinite : Item, IUsable
{
    public new ItemFiniteData Data { get { return (ItemFiniteData)data; } }

    protected List<Ammo> ammos = new List<Ammo>();
    protected int curAmmoInd;
    protected Ammo curAmmo;
    public Ammo LoadedAmmo { get { return curAmmo; } }
    protected bool reloading;
    public bool IsReloading { get { return reloading; } }
    protected float reloadTimer;
    private CoroutineHandle reloadingCoroutine;
    protected bool recoiling;
    public bool IsRecoiling { get { return recoiling; } }
    private float recoilTimer;
    private CoroutineHandle recoilCoroutine;

    public bool IsFireReady { get { if (!curAmmo) return false; return !recoiling && curAmmo.AmmoValue.IsReady; } }

    protected bool inUse;
    public bool IsInUse { get { return inUse; } }
    private CoroutineHandle inUseCoroutine;

    protected MenuManager mm;

    protected UIEngineValue ammoClipAmountChangedUI;
    protected UIEngineValue ammoClipIndexChangedUI;
    protected UIEngineValue ammoReloadUI;

    //delegates
    //clip changed
    public delegate void OnClipAmountChangeDelegate(float _clipAmount);
    public event OnClipAmountChangeDelegate clipAmountChanged;
    void OnClipAmountChanged() { clipAmountChanged?.Invoke(ammos.Count); }

    //clip index changed
    public delegate void OnClipIndexChangeDelegate(float _clipInd);
    public event OnClipIndexChangeDelegate clipIndexChanged;
    void OnClipIndexChanged() { clipIndexChanged?.Invoke(curAmmoInd + 1); }

    //ammo depleted
    public delegate void OnAmmoDepletedDelegate();
    public event OnAmmoDepletedDelegate ammoDepleted;
    void OnAmmoDepleted() { ammoDepleted?.Invoke(); }

    //reload enter
    public delegate void OnReloadEnterDelegate();
    public event OnReloadEnterDelegate reloadEnter;
    void OnReloadEnter() { reloadEnter?.Invoke(); }

    //reloading
    public delegate void OnReloadingDelegate(float _time);
    public event OnReloadingDelegate reloadingTime;
    void OnReloading() { reloadingTime?.Invoke(reloadTimer); }

    //reload enter
    public delegate void OnReloadFinishedDelegate();
    public event OnReloadFinishedDelegate reloadFinished;
    void OnReloadFinished() { reloadEnter?.Invoke(); }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        mm = GameManager.instance.GetMenuManager();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        inUse = false;
        CancelAmmoEvents();
        CancelUIEvents();
    }

    void CancelAmmoEvents()
    {
        if (Data.reloadIfEmpty)
        {
            if (curAmmo)
                curAmmo.AmmoValue.valueEmpty -= Reload;
        }
    }

    void CancelUIEvents()
    {
        if (!Data.syncValuesToUI)
            return;
        if (ammoClipAmountChangedUI)
            clipAmountChanged -= ammoClipAmountChangedUI.SetCurValue;
        if (ammoClipIndexChangedUI)
            clipIndexChanged -= ammoClipAmountChangedUI.SetCurValue;
        if (ammoReloadUI)
            reloadingTime -= ammoReloadUI.SetCurValue;
    }

    protected override void OnOwnerFound()
    {
        base.OnOwnerFound();
        if (recoiling)
            Recoil(false);
        SpawnAmmo();
        SyncItemUI();
    }

    protected virtual void SyncItemUI()
    {
        if (!Data.syncValuesToUI)
            return;
        if (Data.layoutMaster)
        {
            UIEngineValueEntity itemUI = null;
            if (Data.itemSyncType == LayoutSync.UISyncType.SpawnedUI)
                itemUI = ui;
            else if (Data.itemSyncType == LayoutSync.UISyncType.EntityRootOwnerUI)
            {
                var root = transform.root.GetComponentInChildren<EngineEntity>();
                itemUI = root.UI;
            }

            if (itemUI)
            {
                ammoClipAmountChangedUI = itemUI.GetEngineValueUI(Data.layoutMaster,
                Data.ammoClipAmountSync.syncSelection.indexValue);
                clipAmountChanged += ammoClipAmountChangedUI.SetCurValue;
                OnClipAmountChanged();

                ammoClipIndexChangedUI = itemUI.GetEngineValueUI(Data.layoutMaster,
                    Data.ammoIndSync.syncSelection.indexValue);
                clipIndexChanged += ammoClipIndexChangedUI.SetCurValue;
                OnClipIndexChanged();

                ammoReloadUI = itemUI.GetEngineValueUI(Data.layoutMaster,
                    Data.reloadSync.syncSelection.indexValue);
                reloadingTime += ammoReloadUI.SetCurValue;
                ammoReloadUI.SetMinMaxValue(0, Data.reloadTime);
                OnReloading();
            }

        }
    }

    protected virtual void SpawnAmmo()
    {
        if (!Data.preloadAmmo)
            return;

        for (int i = 0; i < Data.ammoDatas.Length; i++)
            AddAmmo(Data.ammoDatas[i]);

        if (ammos.Count > 0)
            SwitchAmmoClip(0);
    }

    public virtual void AddAmmo(AmmoData _ammo)
    {
        var spawn = Instantiate(_ammo.connectedPrefab);
        var ammo = spawn.GetComponent<Ammo>();
        AddAmmo(ammo);
    }

    public virtual void AddAmmo(Ammo _ammo)
    {
        _ammo.PickUp();
        ammos.Add(_ammo);
    }

    protected virtual void Reload()
    {
        CancelAmmoEvents();
        if (curAmmo)
        {
            ammos.Remove(curAmmo);
        }

        //is ammo all gone?
        if (curAmmoInd > ammos.Count - 1)
        {
            curAmmoInd = -1;
            Debug.Log(this + " is depleted! Need more ammo.");
            OnAmmoDepleted();
            OnClipIndexChanged();
            OnClipAmountChanged();
        }
        else
        {
            //if not reload
            OnClipAmountChanged();
            OnReloadEnter();
            reloadingCoroutine = reloadingCoroutine.ReplayCoroutine(StartReload());
        }  
    }

    IEnumerator<float> StartReload()
    {
        reloading = true;
        reloadTimer = 0;
        float perc = 0;
        while (perc != 1)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer > Data.reloadTime)
                reloadTimer = Data.reloadTime;
            perc = reloadTimer / Data.reloadTime;
            OnReloading();
            yield return Timing.WaitForOneFrame;
        }
        SwitchAmmoClip(0);
        OnReloadFinished();
        reloading = false;
    }

    protected virtual void SwitchAmmoClip(int _ind)
    {
        CancelAmmoEvents();
        if (curAmmo)
            curAmmo.LoadAmmo(false);
        for (int i = 0; i < ammos.Count; i++)
        {
            if (_ind == i)
            {
                curAmmoInd = i;
                curAmmo = ammos[i];
                curAmmo.SetOwner(curUnitOwner);
                curAmmo.gameObject.SetActive(true);
                curAmmo.LoadAmmo(true);
            }
            else
            {
                ammos[i].LoadAmmo(false);
                ammos[i].gameObject.SetActive(false);
            }

        }

        if (Data.reloadIfEmpty)
            curAmmo.AmmoValue.valueEmpty += Reload;
        OnClipIndexChanged();
    }

    public virtual void Use()
    {
        inUseCoroutine = inUseCoroutine.ReplayCoroutine(StartUse());
    }

    IEnumerator<float> StartUse()
    {
        inUse = true;
        yield return Timing.WaitForSeconds(Data.delay);
        Use();
    }

    protected virtual void DoItemUse()
    {
        OnUseFinished();
    }

    protected virtual void OnUseFinished()
    {
        Recoil(true);
        inUse = false;
    }

    public virtual void StopUse()
    {
        Stop();
    }

    protected virtual void Stop()
    {
        inUse = false;
    }

    protected virtual void Recoil(bool _reset, System.Action _finishedCallBack = null)
    {
        if (_reset)
            recoilTimer = 0;

        if (recoilCoroutine != null)
            Timing.KillCoroutines(recoilCoroutine);
        recoilCoroutine = Timing.RunCoroutine(StartRecoil(_finishedCallBack));
    }

    IEnumerator<float> StartRecoil(System.Action _finishedCallBack = null)
    {
        recoiling = true;
        while (recoilTimer < Data.recoilTime)
        {
            recoilTimer += Time.deltaTime;
            if (recoilTimer > Data.recoilTime)
                recoilTimer = Data.recoilTime;
            yield return Timing.WaitForOneFrame;
        }
        if (_finishedCallBack != null)
            _finishedCallBack.Invoke();
        recoiling = false;
    }
}
