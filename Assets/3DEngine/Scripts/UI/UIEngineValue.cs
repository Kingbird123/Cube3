using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

public abstract class UIEngineValue : MonoBehaviour
{
    protected float curValue;
    public float CurValue { get { return curValue; } }
    protected float minValue;
    protected float maxValue;
    protected int id;
    public int Id { get { return id; } }
    protected EngineValue connectedValue;

    public virtual void Initialize(EngineValueData _data)
    {
        id = _data.ID;
        if (_data.valueUIType == EngineValueUIType.Global)
            EngineValueUIManager.AddValue(this);
    }

    public virtual void SetCurValue(object _value)
    {
        if (_value is float)
            SetCurValue((float)_value);
        if (_value is int)
            SetCurValue((int)_value);
    }

    public virtual void SetCurValue(float _value)
    {
        curValue = _value;
    }

    public virtual void SetMinMaxValue(float _min, float _max)
    {
        minValue = _min;
        maxValue = _max;
    }

    public virtual void SyncEngineValue(EngineValue _engineValue)
    {
        SetMinMaxValue(_engineValue.Data.FloatMinValue, _engineValue.Data.FloatMaxValue);
        SetCurValue(_engineValue.Value);
        connectedValue = _engineValue;
        connectedValue.valueChanged += SetCurValue;
        connectedValue.minMaxChanged += SetMinMaxValue;
    }

    protected virtual void OnDisable()
    {
        if (connectedValue != null)
        {
            connectedValue.valueChanged -= SetCurValue;
            connectedValue.minMaxChanged -= SetMinMaxValue;
        }
            
    }
}
