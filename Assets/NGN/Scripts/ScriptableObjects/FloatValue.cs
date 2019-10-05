using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    [CreateAssetMenu(fileName = "FloatValue", menuName = "Data/Value/FloatValue", order = 1)]
    public class FloatValue : NGNScriptableObject
    {
        [SerializeField] protected float floatValue = 0;
        public float Value { get { return floatValue; } }
        [SerializeField] protected float minValue = 0;
        public float MinValue { get { return minValue; } }
        [SerializeField] protected float maxValue = 0;
        public float MaxValue { get { return maxValue; } }

        protected List<System.Action<float>> valueCallBacks = new List<System.Action<float>>();
        protected List<System.Action<float>> minCallBacks = new List<System.Action<float>>();
        protected List<System.Action<float>> maxCallBacks = new List<System.Action<float>>();

        public virtual void SetValue(float _floatValue)
        {
            floatValue = _floatValue;
            floatValue = Mathf.Clamp(floatValue, minValue, maxValue);
            DoValueCallBacks();
        }

        public virtual void SubtractValue(float _amount)
        {
            SetValue(floatValue - _amount);
        }

        public virtual void AddValue(float _amount)
        {
            SetValue(floatValue + _amount);
        }

        protected virtual void DoValueCallBacks()
        {
            for (int i = 0; i < valueCallBacks.Count; i++)
            {
                valueCallBacks[i].Invoke(Value);
            }
        }

        public virtual void SetMinValue(float _floatValue)
        {
            minValue = _floatValue;
            DoMinCallBacks();
        }

        public virtual void SubtractMinValue(float _amount)
        {
            SetMinValue(minValue - _amount);
        }

        public virtual void AddMinValue(float _amount)
        {
            SetMinValue(minValue + _amount);
        }

        protected virtual void DoMinCallBacks()
        {
            for (int i = 0; i < minCallBacks.Count; i++)
            {
                minCallBacks[i].Invoke(MinValue);
            }
        }

        public virtual void SetMaxValue(float _floatValue)
        {
            maxValue = _floatValue;
            DoMaxCallBacks();
        }

        public virtual void SubtractMaxValue(float _amount)
        {
            SetMaxValue(maxValue - _amount);
        }

        public virtual void AddMaxValue(float _amount)
        {
            SetMaxValue(maxValue + _amount);
        }

        protected virtual void DoMaxCallBacks()
        {
            for (int i = 0; i < maxCallBacks.Count; i++)
            {
                maxCallBacks[i].Invoke(MaxValue);
            }
        }

        public virtual void SubsribeToMinValue(System.Action<float> _valueCallback)
        {
            if (!minCallBacks.Contains(_valueCallback))
                minCallBacks.Add(_valueCallback);
        }

        public virtual void UnSubscribeMinValue(System.Action<float> _valueCallback)
        {
            if (minCallBacks.Contains(_valueCallback))
                minCallBacks.Remove(_valueCallback);
        }

        public virtual void SubsribeToMaxValue(System.Action<float> _valueCallback)
        {
            if (!maxCallBacks.Contains(_valueCallback))
                maxCallBacks.Add(_valueCallback);
        }

        public virtual void UnSubscribeMaxValue(System.Action<float> _valueCallback)
        {
            if (maxCallBacks.Contains(_valueCallback))
                maxCallBacks.Remove(_valueCallback);
        }

        public virtual void SubsribeToValue(System.Action<float> _valueCallback)
        {
            if (!valueCallBacks.Contains(_valueCallback))
                valueCallBacks.Add(_valueCallback);
        }

        public virtual void UnSubscribeToValue(System.Action<float> _valueCallback)
        {
            if (valueCallBacks.Contains(_valueCallback))
                valueCallBacks.Remove(_valueCallback);
        }
    }
}


