using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EngineEventOptionValueDelta : EngineEventOption
{
    public EngineValueDelta valueDelta;

    public override void DoEvent(EngineEvent _event)
    {
        base.DoEvent(_event);
        TryGetEntity(ValueDelta);
    }

    void ValueDelta(EngineEntity _engineEntity)
    {
        valueDelta.DoValueDelta(_engineEntity);
    }

    void TryGetEntity(System.Action<EngineEntity> _successCallback)
    {
        var ent = objToUse.GetComponent<EngineEntity>();
        if (ent)
        {
            _successCallback.Invoke(ent);
        }
        else
            Debug.LogError("No " + typeof(EngineEntity) + " found on " + objToUse + ". Make sure you assign the correct object to affect!");
    }

    void Lose()
    {
        GameManager.instance.GameOverLose();
    }

    void Win()
    {
        //GameManager.instance.GetSceneTransitionData.();
    }
}
