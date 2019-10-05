using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddUnitBuff", menuName = "Data/Interacts/AddUnitBuff", order = 1)]
public class InteractFXAddUnitBuff : InteractFX
{
    [SerializeField] private UnitBuff buffToAdd = null;

    protected override void DoFX(GameObject _sender, GameObject _receiver)
    {
        var unit = _receiver.GetComponentInChildren<Unit>();
        if (unit)
        {
            unit.AddBuff(buffToAdd);
        }
    }
}
