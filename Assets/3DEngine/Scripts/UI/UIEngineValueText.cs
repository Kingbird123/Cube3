using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEngineValueText : UIEngineValue
{
    [SerializeField] private TextMeshProUGUI text = null;

    public override void SetCurValue(float _value)
    {
        base.SetCurValue(_value);
        text.text = curValue.ToString();
    }
}
