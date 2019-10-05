using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFiniteData : ItemData
{
    //ui added
    public LayoutSync.UISyncType itemSyncType;
    public UILayoutMaster layoutMaster;
    public LayoutSyncSingle reloadSync;
    public LayoutSyncSingle ammoClipAmountSync;
    public LayoutSyncSingle ammoIndSync;

    //entity ammo type filter
    public IndexStringProperty ammoType;
    public bool preloadAmmo;
    public AmmoData[] ammoDatas;
    public float recoilTime = 0.3f;
    public float delay = 0;
    public bool reloadIfEmpty;
    public float reloadTime = 0.5f;
}
