using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnableBehaviour", menuName = "Data/Interacts/EnableBehaviour", order = 1)]
public class InteractFXEnableBehaviour : InteractFXDynamic
{
    [SerializeField] private IndexStringProperty behaviour = null;
    [SerializeField] private bool enable = true;

    protected override void AffectObject()
    {
        Behaviour script = (Behaviour)affectedGameObject.GetComponent(behaviour.stringValue);
        if (script)
        {
            script.enabled = enable;
        }
        else
        {
            Debug.Log("Behaviour: " + behaviour + " not be found on " + affectedGameObject.name);
        }
    }

}
