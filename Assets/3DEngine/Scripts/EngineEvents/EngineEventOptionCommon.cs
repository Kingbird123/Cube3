using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EngineEventOptionCommon : EngineEventOption
{
    public enum CommonEventType { SetActive, Destroy, Spawn, Die, Respawn, Lose, Win }
    public enum PositionType { SceneObject, Vector3}
    public enum EngineValueType { Delta, Reset }
    public CommonEventType eventType;
    public bool active;
    public float delay;
    public GameObject objToSpawn;
    public PositionType positionType;
    public Vector3 position;
    public Vector3 rotation;
    public SceneObjectProperty positionObj;
    public bool setParent;
    public SceneObjectProperty parentObj;

    public override void DoEvent(EngineEvent _event)
    {
        base.DoEvent(_event);
        EventSwitch(_event);
    }

    void EventSwitch(EngineEvent _event)
    {
        switch (eventType)
        {
            case CommonEventType.SetActive:
                SetActive();
                break;
            case CommonEventType.Destroy:
                Destroy();
                break;
            case CommonEventType.Spawn:
                Spawn(_event);
                break;
            case CommonEventType.Die:
                TryGetUnit(Die);
                break;
            case CommonEventType.Respawn:
                TryGetUnit(Respawn);
                break;
            case CommonEventType.Lose:
                Lose();
                break;
            case CommonEventType.Win:
                Win();
                break;
            default:
                break;
        }
    }

    void SetActive()
    {
        if (objToUse)
            objToUse.SetActive(active);
    }

    void Destroy()
    {
        GameObject.Destroy(objToUse, delay);
    }

    void Spawn(EngineEvent _event)
    {
        var spawn = GameObject.Instantiate(objToSpawn);
        var pos = position;
        var rot = rotation;
        if (positionType == PositionType.SceneObject)
        {
            var obj = positionObj.GetSceneObject(_event.Source, objToUse);
            pos = objToUse.transform.position;
            rot = objToUse.transform.rotation.eulerAngles;       
        }
        spawn.transform.position = pos;
        spawn.transform.rotation = Quaternion.Euler(rot);
        if (setParent)
        {
            var par = parentObj.GetSceneObject();
            spawn.transform.SetParent(par.transform);
        }
    }

    void Die(Unit _unit)
    {
        _unit.Die();
    }

    void Respawn(Unit _unit)
    {
        _unit.Respawn();
    }

    void TryGetUnit(System.Action<Unit> _successCallback)
    {
        var unit = objToUse.GetComponent<Unit>();
        if (unit)
        {
            _successCallback.Invoke(unit);
        }
        else
            Debug.LogError("No " + typeof(Unit) + " found on " + objToUse + ". Make sure you assign the correct object to affect!");
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
