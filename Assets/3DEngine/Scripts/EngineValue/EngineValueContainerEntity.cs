using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NGN.OdinSerializer;
[System.Serializable]
public class EngineValueContainerEntity : EngineValueContainer
{
    private EngineEntity owner;
    private List<EngineValueEntity> allUnitSelections = new List<EngineValueEntity>();

    public void InitializeContainer(EngineEntity _owner)
    {
        owner = _owner;
        var data = _owner.Data;
        var dataSels = data.engineValueSelections;
        List<EngineValueSelection> sels = new List<EngineValueSelection>();
        for (int i = 0; i < dataSels.Length; i++)
        {
            var sel = dataSels[i];
            sels.Add(sel.valueSelection);

            var bytes = SerializationUtility.SerializeValue(sel, DataFormat.Binary, out List<Object> serObjs);
            var deSerialized = SerializationUtility.DeserializeValue<EngineValueEntity>(bytes, DataFormat.Binary, serObjs);
            allUnitSelections.Add(deSerialized);
        }

        base.InitializeContainer(data.engineValueManager, sels.ToArray());
        InitializeEngineValueEntityEvents();
        SyncEvents();
    }

    void InitializeEngineValueEntityEvents()
    {
        for (int i = 0; i < allUnitSelections.Count; i++)
        {
            var id = allUnitSelections[i].valueSelection.valueData.ID;
            var val = GetEngineValue(id);
            if (val != null)
            {
                var data = val.Data;
                allUnitSelections[i].InitializeEvents(owner, data);
            }
            
        }
    }

    void SyncEvents()
    {
        if (!owner.Data.engineEventManager)
            return;

        for (int i = 0; i < allValues.Count; i++)
        {
            var id = allValues[i].Data.ID;
            for (int ind = 0; ind < allUnitSelections.Count; ind++)
            {
                if (id == allUnitSelections[ind].valueSelection.valueData.ID)
                {
                    allUnitSelections[ind].SyncEvents(allValues[i]);
                }

            }
        }
    }

    public void CancelEvents()
    {
        if (!owner.Data.engineEventManager)
            return;

        for (int i = 0; i < allValues.Count; i++)
        {
            var id = allValues[i].Data.ID;
            for (int ind = 0; ind < allUnitSelections.Count; ind++)
            {
                if (id == allUnitSelections[ind].valueSelection.valueData.ID)
                {
                    allUnitSelections[ind].CancelEvents(allValues[i]);
                }

            }
        }
    }


    //override entity UI by component
    public void InitializeUI(UIEngineValueEntity _ui)
    {
        SyncUI(_ui);
    }

    void SyncUI(UIEngineValueEntity _ui)
    {
        for (int i = 0; i < allValues.Count; i++)
        {
            //_ui.SyncEngineValue(allValues[i]);
        }
    }
}


