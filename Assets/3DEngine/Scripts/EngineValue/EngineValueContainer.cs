using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EngineValueContainer
{
    public List<EngineFloat> engineFloats = new List<EngineFloat>();
    public List<EngineInt> engineInts = new List<EngineInt>();

    protected List<EngineValue> allValues = new List<EngineValue>();

    public virtual void InitializeContainer(EngineValueDataManager _valueManager, EngineValueSelection[] _selections)
    {
        ClearAllValues();

        for (int i = 0; i < _selections.Length; i++)
        {
            var sel = _selections[i];
            var catInd = sel.category.indexValue;
            var valInd = sel.engineValue.indexValue;
            var cat = _valueManager.engineValueCategories[catInd];
            var valData = cat.engineValueDatas[valInd];
            var engineValue = valData.CreateEngineValue();
            engineValue.InitializeValue(valData);
            allValues.Add(engineValue);
        }
    }

    public void ResetValueToDefault(int _id)
    {
        var val = GetEngineValue(_id);
        val.ResetDefaultValues();
    }

    public void ResetAllDefaultValues()
    {
        for (int i = 0; i < allValues.Count; i++)
        {
            allValues[i].ResetDefaultValues();
        }
    }

    public virtual void AddFloatValue(int _id, float _amount, bool _onlySearchCategory)
    {
        if (_onlySearchCategory)
            ValueDeltaByCategory(_id, _amount);
        else
            ValueDelta(_id, _amount);
    }

    public virtual void SubtractFloatValue(int _id, float _amount, bool _onlySearchCategory)
    {
        if (_onlySearchCategory)
            ValueDeltaByCategory(_id, -_amount);
        else
            ValueDelta(_id, -_amount);

    }

    public virtual void AddIntValue(int _id, int _amount, bool _onlySearchCategory)
    {
        if (_onlySearchCategory)
            ValueDeltaByCategory(_id, _amount);
        else
            ValueDelta(_id, _amount);

    }

    public virtual void SubtractIntValue(int _id, int _amount, bool _onlySearchCategory)
    {
        if (_onlySearchCategory)
            ValueDeltaByCategory(_id, -_amount);
        else
            ValueDelta(_id, -_amount);

    }

    protected virtual void ValueDelta(int _id, float _amount)
    {
        var val = GetEngineValue(_id);
        if (val != null)
            val.ValueDelta(_amount);
    }

    protected virtual void ValueDelta(int _id, int _amount)
    {
        var val = GetEngineValue(_id);
        if (val != null)
            val.ValueDelta(_amount);
    }

    protected virtual void ValueDeltaByCategory(int _id, float _amount)
    {
        var vals = GetEngineCategoryValues(_id);
        for (int i = 0; i < vals.Length; i++)
        {
            vals[i].ValueDelta(_amount);
        }
    }

    public virtual void ValueMaxDelta(int _id, float _amount)
    {
        var val = GetEngineValue(_id);
        if (val != null)
            val.ValueDelta(_amount);
    }

    public EngineValue GetEngineValue(int _id)
    {
        for (int i = 0; i < allValues.Count; i++)
        {
            if (_id == allValues[i].ID)
                return allValues[i];
        }
        Debug.LogError("could not find value with id: " + _id + "!");
        return null;
    }

    public EngineValue[] GetEngineCategoryValues(int _id)
    {
        var vals = new List<EngineValue>();
        for (int i = 0; i < allValues.Count; i++)
        {
            var catString1 = allValues[i].ID.ToString().Substring(0, 4);
            var catString2 = _id.ToString().Substring(0, 4);
            if (catString1 == catString2)
                vals.Add(allValues[i]);
        }
        return vals.ToArray();
    }

    void ClearAllValues()
    {
        engineFloats.Clear();
        engineInts.Clear();
    }
}
