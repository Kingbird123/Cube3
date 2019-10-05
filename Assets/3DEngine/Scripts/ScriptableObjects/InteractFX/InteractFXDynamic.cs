using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractFXDynamic : InteractFX
{
    [SerializeField] private SceneObjectProperty affectedType = null;

    protected GameObject sender;
    protected GameObject receiver;
    protected GameObject affectedGameObject;

    protected override void DoFX(GameObject _sender = null, GameObject _receiver = null)
    {
        sender = _sender;
        receiver = _receiver;
        affectedGameObject = affectedType.GetSceneObject(_sender, _receiver);

        if (affectedGameObject)
            AffectObject();
        else
            Debug.LogError("No object to effect selected");
    }

    protected abstract void AffectObject();
}
