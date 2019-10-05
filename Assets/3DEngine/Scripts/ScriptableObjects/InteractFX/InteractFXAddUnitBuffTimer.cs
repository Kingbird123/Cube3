using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddUnitBuffTimer", menuName = "Data/Interacts/AddUnitBuffTimer", order = 1)]
public class InteractFXAddUnitBuffTimer : InteractFX
{
    [SerializeField] private UnitBuff buffToAdd = null;
    [SerializeField] private float time = 5;

    protected override void DoFX(GameObject _sender, GameObject _receiver)
    {
        var unit = _receiver.GetComponentInChildren<Unit>();
        if (unit)
        {
            unit.StartCoroutine(StartBuffTimer(unit));
        }
    }

    IEnumerator StartBuffTimer(Unit _unit)
    {
        _unit.AddBuff(buffToAdd);
        yield return new WaitForSeconds(time);
        _unit.RemoveBuff(buffToAdd);
    }
}
