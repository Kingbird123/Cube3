using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EngineValueUIManager
{
    public static EngineValueUIManagerData Manager
    {
        get
        {
            if (EngineValueUIManagerData.instance)
                return EngineValueUIManagerData.instance;
            else
            {
                Debug.LogError("There is no Value UI Manager data initialized! Make sure you add one to the GameManager!");
                return null;
            }
        }
    }

    public static void AddValue(UIEngineValue _uiValue)
    {
        Manager.AddValue(_uiValue);
    }

    public static void SetCurValue(int _id, float _value)
    {
        Manager.SetCurValue(_id, _value);
    }

    public static void SetMinMaxValue(int _id, float _min, float _max)
    {
        Manager.SetMinMaxValue(_id, _min, _max);
    }

    
}
