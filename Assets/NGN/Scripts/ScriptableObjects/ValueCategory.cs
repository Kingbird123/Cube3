using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    [CreateAssetMenu(fileName = "ValueCategory", menuName = "Data/Value/ValueCategory", order = 1)]
    public class ValueCategory : NGNScriptableObject
    {
        [SerializeField] protected FloatValue[] floatValues;
        protected FloatValue[] instancedFloatValues;
        public FloatValue[] InstancedFloatValues { get { return instancedFloatValues; } }

        public virtual void Initialize()
        {
            instancedFloatValues = floatValues.GetInstanceArrayCopy();
        }

        public FloatValue GetFloatValue(FloatValue _value)
        {
            for (int i = 0; i < instancedFloatValues.Length; i++)
            {
                if (_value.GUID == instancedFloatValues[i].GUID)
                    return instancedFloatValues[i];
            }
            Debug.LogError("Could not find " + _value + " in " + this);
            return null;
        }
    }
}


