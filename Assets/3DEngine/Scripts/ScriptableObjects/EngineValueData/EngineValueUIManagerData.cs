using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EngineValueUIManager", menuName = "Data/Managers/EngineValueUIManager", order = 1)]
public class EngineValueUIManagerData : ScriptableObject
{
    [Header("This manager holds onto all global UI elements in the scene. Do not modify.")]
    public string nothing;
    public static EngineValueUIManagerData instance;
    private List<EngineValueUICategory> uiCategories = new List<EngineValueUICategory>();

    public void Initialize()
    {
        ResetCategories();
        instance = this;
    }

    void ResetCategories()
    {
        uiCategories.Clear();
    }

    public void AddValue(UIEngineValue _uiValue)
    {
        var cat = GetCategory(_uiValue.Id);
        if (cat != null)
            cat.AddUI(_uiValue);
        else
        {
            cat = new EngineValueUICategory { id = _uiValue.Id };
            cat.AddUI(_uiValue);
            uiCategories.Add(cat);
        }   
    }

    public void SetCurValue(int _id, float _value)
    {
        var cat = GetCategory(_id);
        if (cat != null)
            cat.SetCurValue(_value);
    }

    public void SetMinMaxValue(int _id, float _min, float _max)
    {
        var cat = GetCategory(_id);
        if (cat != null)
            cat.SetMinMaxValue(_min, _max);
    }

    EngineValueUICategory GetCategory(int _id)
    {
        for (int i = 0; i < uiCategories.Count; i++)
        {
            if (uiCategories[i].id == _id)
                return uiCategories[i];
        }
        return null;
    }

}
