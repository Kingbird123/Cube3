using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBuff : ScriptableObject
{
    public abstract void ActivateBuff(Item _item, bool _activate);
}
