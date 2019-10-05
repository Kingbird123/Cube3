using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EngineFloat", menuName = "Data/Interacts/EngineFloat", order = 1)]
public class InteractFXEngineFloat : InteractFXDynamic
{
    public enum SelectionType { Category, EngineValue }
    [SerializeField] private EngineValueDataManager valueManager;
    [SerializeField] private SelectionType selectionType;
    [SerializeField] private EngineValueSelection valueSelection;
    [SerializeField] private float valueDelta = 0;

    protected override void AffectObject()
    {
        var ent = affectedGameObject.GetComponent<EngineEntity>();
        if (ent)
        {
            int id = valueSelection.valueData.ID;
            if (valueDelta < 0)
                ent.SubtractEngineFloatValue(id, Mathf.Abs(valueDelta));
            else
                ent.AddEngineFloatValue(id, valueDelta);
        }
        
    }
}
