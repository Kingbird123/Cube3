using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CallMethod", menuName = "Data/Interacts/CallMethod", order = 1)]
public class InteractFXCallMethod : InteractFXDynamic
{
    [SerializeField] private IndexStringProperty behaviour = null;
    [SerializeField] private MethodProperty method = null;

    protected override void AffectObject()
    {
        var script = (Behaviour)affectedGameObject.GetComponent(behaviour.stringValue);
        if (script)
        {
            method.go = affectedGameObject;
            method.InvokeMethod();
        }
    }

}
