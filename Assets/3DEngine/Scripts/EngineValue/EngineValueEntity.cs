using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EngineValueEntity
{
    public string engineValueName;
    public EngineValueSelection valueSelection;
    public EngineValueEvent[] valueEvents;

    public void InitializeEvents(EngineEntity _owner, EngineValueData _data)
    {
        for (int i = 0; i < valueEvents.Length; i++)
        {
            valueEvents[i].Initialize(_owner, _data);
        }
    }

    public void SyncEvents(EngineValue _engineValue)
    {
        for (int i = 0; i < valueEvents.Length; i++)
        {
            valueEvents[i].SyncEvent(_engineValue);
        }
    }

    public void CancelEvents(EngineValue _engineValue)
    {
        for (int i = 0; i < valueEvents.Length; i++)
        {
            valueEvents[i].CancelEvent(_engineValue);
        }
    }
}
