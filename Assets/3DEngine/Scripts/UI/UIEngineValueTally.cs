using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEngineValueTally : UIEngineValue
{
    [SerializeField] protected Transform slotParent;
    [SerializeField] protected UISlot slotPrefab;

    protected List<UISlot> curSlots = new List<UISlot>();

    public override void SetCurValue(float _value)
    {
        base.SetCurValue(_value);
        while (curValue > curSlots.Count)
        {
            var slot = Instantiate(slotPrefab, slotParent);
            curSlots.Add(slot);
        }
        while (curValue < curSlots.Count)
        {
            Destroy(curSlots[curSlots.Count - 1].gameObject);
            curSlots.RemoveAt(curSlots.Count - 1);
        }
    }

    public virtual void SetActiveSlot(int _ind)
    {
        for (int i = 0; i < curSlots.Count; i++)
        {
            curSlots[i].SetSelected(i == _ind);
        }
    }
}
