using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBuff : ScriptableObject
{
    public abstract void ActivateBuff(Unit _unit, bool _activate);
}
