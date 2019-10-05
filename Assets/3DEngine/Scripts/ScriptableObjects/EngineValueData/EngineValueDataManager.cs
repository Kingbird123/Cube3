using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EngineValueManager", menuName = "Data/Managers/EngineValueManager", order = 1)]
public class EngineValueDataManager : ScriptableObject
{
    public EngineValueCategory[] engineValueCategories;

    //should be called in editor
    public void RefreshIDs()
    {
        for (int i = 0; i < engineValueCategories.Length; i++)
        {
            var cat = engineValueCategories[i];
            for (int ind = 0; ind < cat.engineValueDatas.Length; ind++)
            {
                var data = cat.engineValueDatas[ind];
                //create a 5 digit serial number id for the value. eg. 100010001
                var catSerial = CreateSerial(5, i);
                var valSerial = CreateSerial(5, ind);
                if (data)
                    data.SetID(int.Parse(catSerial + valSerial));
            }

        }
        
    }

    string CreateSerial(int _digits, int _value)
    {
        int digits = _digits;
        var val = 1;
        if (_value != 0)
            val = (int)Mathf.Floor(Mathf.Log10(_value) + 1);
        for (int i = 0; i < val; i++)
            digits--;
        string serial = "1";
        for (int i = 0; i < digits - 1; i++)
            serial += "0";
        return serial += _value.ToString();
    }

    public string[] GetCategoryNames()
    {
        string[] names = new string[engineValueCategories.Length];
        for (int i = 0; i < engineValueCategories.Length; i++)
            names[i] = engineValueCategories[i].categoryName;
        return names;
    }

    public string[] GetEngineValueDataNames(int _categoryInd)
    {
        return engineValueCategories[_categoryInd].GetEngineValueDataNames();
    }

}
