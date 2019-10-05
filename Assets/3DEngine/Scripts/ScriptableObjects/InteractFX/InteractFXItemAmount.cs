using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemAmount", menuName = "Data/Interacts/ItemAmount", order = 1)]
public class InteractFXItemAmount : InteractFX
{
    [SerializeField] private bool needPrerequisiteItem = false;
    [SerializeField] private ItemData prerequisiteItem = null;
    [SerializeField] private float amountDelta = 0;

    private UnitEquip equip;

    protected override void DoFX(GameObject _sender = null, GameObject _receiver = null)
    {
        equip = _receiver.GetComponent<UnitEquip>();
        if (needPrerequisiteItem)
        {
            if (equip.CurItem.Data != prerequisiteItem)
            {
                Debug.Log("Invalid skin prerequisite!");
                return;
            }
                
        }
        if (equip.CurUseable != null)
        {
            var finite = equip.CurUseable as ItemFinite;
            if (finite)
                finite.LoadedAmmo.AmmoValue.ValueDelta(amountDelta);
        }
        
    }
}
