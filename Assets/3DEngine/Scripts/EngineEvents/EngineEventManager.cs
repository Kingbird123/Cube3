using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EngineEventManager", menuName = "Data/Managers/EngineEventManager", order = 1)]
public class EngineEventManager : ScriptableObject
{
    [System.Serializable]
    public class EngineEventArray
    {
        public string eventArrayName;
        public EngineEvent[] events;

        public void DoEvents(GameObject _sender, GameObject _receiver = null)
        {
            for (int i = 0; i < events.Length; i++)
            {
                events[i].DoEvent(_sender, events, i, _receiver);
            }
        }
    }

    public EngineEventArray[] engineEvents;

    public void DoEvents(int _ind, GameObject _sender, GameObject _receiver = null)
    {
        engineEvents[_ind].DoEvents(_sender, _receiver);
    }

    public string[] GetEventNames()
    {
        var names = new string[engineEvents.Length];
        for (int i = 0; i < engineEvents.Length; i++)
            names[i] = engineEvents[i].eventArrayName;

        return names;
    }
}
