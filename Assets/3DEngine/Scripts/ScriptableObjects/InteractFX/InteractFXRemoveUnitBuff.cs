using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RemoveUnitBuff", menuName = "Data/Interacts/RemoveUnitBuff", order = 1)]
public class InteractFXRemoveUnitBuff : InteractFX
{
    [SerializeField] private UnitBuff buffToRemove = null;

    protected override void DoFX(GameObject _sender, GameObject _receiver)
    {
        var unit = _receiver.GetComponentInChildren<Unit>();
        if (unit)
        {
            unit.RemoveBuff(buffToRemove);
        }
    }
}
