using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EngineValueUICategory
{
    public int id;
    public List<UIEngineValue> uiValues = new List<UIEngineValue>();

    public void AddUI(UIEngineValue _uiValue)
    {
        if (!uiValues.Contains (_uiValue))
            uiValues.Add(_uiValue);
    }

    public void SetCurValue(float _value)
    {
        for (int i = 0; i < uiValues.Count; i++)
        {
            uiValues[i].SetCurValue(_value);
        }
    }

    public void SetMinMaxValue(float _min, float _max)
    {
        for (int i = 0; i < uiValues.Count; i++)
        {
            uiValues[i].SetMinMaxValue(_min, _max);
        }
    }
}
