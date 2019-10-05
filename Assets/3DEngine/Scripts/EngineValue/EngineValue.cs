using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;
using MEC;

[System.Serializable]
public class EngineValue
{
    protected EngineValueData data;
    public virtual EngineValueData Data { get { return data; } }
    public virtual object Value { get; set; }
    public virtual object MinValue { get; set; }
    public virtual object MaxValue { get; set; }
    protected object prevValue;
    public float FloatValue { get { return ConvertValueToFloat(Value); } }
    protected float PrevFloatValue { get { return ConvertValueToFloat(prevValue); } }
    public float FloatMaxValue { get { return ConvertValueToFloat(MaxValue); } }
    public float FloatMinValue { get { return ConvertValueToFloat(MinValue); } }

    protected int id;
    public int ID { get { return id; } }

    public bool IsReady { get { return !overheated && !IsEmpty; } }
    public bool IsEmpty { get { return FloatValue <= FloatMinValue; } }
    public bool IsFull { get { return FloatValue >= FloatMaxValue; } }

    protected bool overheated;
    public bool IsOverheated { get { return overheated; } }
    protected CoroutineHandle overheatedCoroutine;
    protected float overheatTimer;

    protected bool recharging;
    public bool IsRecharging { get { return recharging; } }
    protected CoroutineHandle rechargeCoroutine;

    //delegates
    //changed
    public delegate void OnValueChangeDelegate(object _value);
    public event OnValueChangeDelegate valueChanged;
    void OnValueChanged() { valueChanged?.Invoke(Value); }
    //minmax changed
    public delegate void OnMinMaxChangeDelegate(float _min, float _max);
    public event OnMinMaxChangeDelegate minMaxChanged;
    void OnMinMaxChanged() { minMaxChanged?.Invoke(FloatMinValue, FloatMaxValue); }
    //increased
    public delegate void OnValueIncreaseDelegate(object _value);
    public event OnValueIncreaseDelegate valueIncreased;
    void OnValueIncreased() { valueIncreased?.Invoke(Value); }
    //decrease
    public delegate void OnValueDecreaseDelegate(object _value);
    public event OnValueDecreaseDelegate valueDecreased;
    void OnValueDecreased() { valueDecreased?.Invoke(Value); }
    //empty
    public delegate void OnValueEmptyDelegate();
    public event OnValueEmptyDelegate valueEmpty;
    void OnValueEmpty() { valueEmpty?.Invoke(); }
    //full
    public delegate void OnValueFullDelegate();
    public event OnValueFullDelegate valueFull;
    void OnValueFull() { valueFull?.Invoke(); }
    //overheatFinished
    public delegate void OnOverHeatFinishedDelegate();
    public event OnOverHeatFinishedDelegate overheatFinished;
    void OnOverheatFinished() { overheatFinished?.Invoke(); }

    public virtual void InitializeValue(EngineValueData _data)
    {
        id = _data.ID;
        data = _data;
        Value = _data.Value;
        prevValue = Value;
        OnValueChanged();
    }

    public virtual void ValueDelta(object _amount)
    {
        var val = FloatValue + ConvertValueToFloat(_amount);
        Value = val;

        CheckEvents();
    }

    public virtual void ValueMaxDelta(object _amount)
    {
        var val = FloatMaxValue + ConvertValueToFloat(_amount);
        MaxValue = val;

        CheckEvents();
    }

    public virtual void ValueMinDelta(object _amount)
    {
        var val = FloatMaxValue + ConvertValueToFloat(_amount);
        MinValue = val;

        CheckEvents();
    }

    public virtual void ResetDefaultValues()
    {
        Value = Data.Value;
        MinValue = Data.MinValue;
        MaxValue = Data.MaxValue;
        CheckEvents();
    }

    public virtual void Recharge(float _speed)
    {
        if (rechargeCoroutine != null)
            Timing.KillCoroutines(rechargeCoroutine);
        rechargeCoroutine = Timing.RunCoroutine(StartRecharge(_speed));
    }

    IEnumerator<float> StartRecharge(float _speed)
    {
        recharging = true;
        while (FloatValue < FloatMaxValue)
        {
            yield return Timing.WaitForOneFrame;
            if (!overheated)
                ValueDelta(_speed * Time.deltaTime);
        }
        recharging = false;
    }

    public void OverHeat(float _overheatTime)
    {
        if (overheatedCoroutine != null)
            Timing.KillCoroutines(overheatedCoroutine);
        overheatedCoroutine = Timing.RunCoroutine(StartOverheat(_overheatTime));
    }

    IEnumerator<float> StartOverheat(float _overheatTime)
    {
        overheated = true;
        float perc = 0;
        while (perc < 1)
        {
            overheatTimer += Time.deltaTime;
            if (overheatTimer > _overheatTime)
                overheatTimer = _overheatTime;
            perc = overheatTimer / _overheatTime;

            yield return Timing.WaitForOneFrame;
        }
        overheated = false;
        OnOverheatFinished();
    }

    void CheckEvents()
    {
        if (Value != prevValue)
        {
            prevValue = Value;
            OnValueChanged();
        }
        if (FloatValue > PrevFloatValue)
            OnValueIncreased();
        else if (FloatValue < PrevFloatValue)
            OnValueDecreased();
        if (IsEmpty)
            OnValueEmpty();
        if (IsFull)
            OnValueFull();
    }

    float ConvertValueToFloat(object _value)
    {
        if (_value is float) return (float)_value;
        if (_value is int) return (int)_value;
        Debug.LogError("Could not convert value!");
        return 0;
    }
}
