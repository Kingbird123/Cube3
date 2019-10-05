using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class LayoutSync
{
    public enum UISyncType { SpawnedUI, EntityRootOwnerUI }
    public UISyncType syncType;
    public UILayoutMaster layoutMaster;
}

[System.Serializable]
public class LayoutSyncEntity : LayoutSync
{
    [System.Serializable]
    public struct LayoutSyncEngineValue
    {
        public int masterInd;
        public int valueInd;
    }
    public EngineValueDataManager valueManager;
    public LayoutSyncEngineValue[] syncSelections;
}

[System.Serializable]
public class LayoutSyncSingle: LayoutSync
{
    public IndexStringProperty syncSelection;
}
