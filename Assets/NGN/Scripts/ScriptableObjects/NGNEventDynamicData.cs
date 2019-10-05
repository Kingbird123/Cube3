using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public abstract class NGNEventDynamicData : NGNEventData
    {
        [SerializeField] private SceneObjectProperty affectedType = null;

        protected GameObject sender;
        protected GameObject receiver;
        protected GameObject affectedGameObject;

        public override void DoEvent(GameObject _sender = null, GameObject _receiver = null)
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
}


