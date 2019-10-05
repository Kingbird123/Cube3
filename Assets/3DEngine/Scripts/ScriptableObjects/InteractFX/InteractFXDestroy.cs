using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Destroy", menuName = "Data/Interacts/Destroy", order = 1)]
public class InteractFXDestroy : InteractFXDynamic
{
    [SerializeField] private bool destroyRoot = false;

    protected override void AffectObject()
    {
        KillObject(affectedGameObject);
    }

    void KillObject(GameObject _go)
    {
        if (!_go)
            return;
        if (destroyRoot)
            Destroy(_go.transform.root.gameObject, delay);
        else
            Destroy(_go, delay);
    }


}
