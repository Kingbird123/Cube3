using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEngineValueSlider : UIEngineValue
{
    [SerializeField] private Slider slider = null;

    public override void SetCurValue(float _value)
    {
        base.SetCurValue(_value);
        slider.value = curValue;
    }

    public override void SetMinMaxValue(float _min, float _max)
    {
        base.SetMinMaxValue(_min, _max);
        slider.minValue = minValue;
        slider.maxValue = maxValue;
    }
}
