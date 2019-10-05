using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    [CreateAssetMenu(fileName = "ValueManager", menuName = "NGN/Data/Manager/ValueManager", order = 1)]
    public class ValueManager : NGNScriptableObject
    {
        [SerializeField] protected ValueCategory[] valueCategories;
        protected ValueCategory[] instancedValueCategories;

        public virtual void Initialize(NGNEntity _owner)
        {
            instancedValueCategories = valueCategories.GetInstanceArrayCopy();
            InitializeValues();
        }

        protected virtual void InitializeValues()
        {
            for (int i = 0; i < instancedValueCategories.Length; i++)
            {
                instancedValueCategories[i].Initialize();
            }
        }

        public FloatValue GetFloatValue(FloatValue _value)
        {
            for (int i = 0; i < instancedValueCategories.Length; i++)
            {
                var cat = instancedValueCategories[i];
                var catVals = cat.InstancedFloatValues;
                for (int ind = 0; ind < catVals.Length; ind++)
                {
                    var val = catVals[ind];
                    if (val)
                    {
                        if (_value.GUID == val.GUID)
                            return val;
                    }
                    
                }
            }
            Debug.LogError("Could not find " + _value + " in " + this);
            return null;
        }
    }
}


