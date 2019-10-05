using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEngineValueRefill : UIEngineValue
{
    [SerializeField] protected UIEngineValue totalAmountUI;
    [SerializeField] protected UIEngineValue clipAmountUI;
    [SerializeField] protected UIEngineValue clipIndexUI;
    [SerializeField] protected UIEngineValue reloadOverheatUI;

    private void OnEnable()
    {
        ActivateReloadAmount(false);
    }

    public void SetTotalAmount(float _value)
    {
        if (totalAmountUI)
        {
            totalAmountUI.SetCurValue(_value);
        }
    }

    public void SetIndexAmount(float _value)
    {
        if (clipIndexUI)
        {
            clipIndexUI.SetCurValue(_value);
        }

    }

    public override void SetCurValue(float _value)
    {
        base.SetCurValue(_value);
        if (clipAmountUI)
            clipAmountUI.SetCurValue(_value);
        
    }

    public override void SetMinMaxValue(float _min, float _max)
    {
        base.SetMinMaxValue(_min, _max);
        if (clipAmountUI)
            clipAmountUI.SetMinMaxValue(_min, _max);
        
    }

    public void SetReloadAmountValue(float _value)
    {
        if (reloadOverheatUI)
            reloadOverheatUI.SetCurValue(_value);
    }

    public void ActivateReloadAmount(bool _activate)
    {
        if (reloadOverheatUI)
            reloadOverheatUI.gameObject.SetActive(_activate);
    }
}

