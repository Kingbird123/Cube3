using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public abstract class NGNTriggerData : NGNEventDynamicData
    {
        protected List<System.Action<GameObject, GameObject>> triggerActions = new List<System.Action<GameObject, GameObject>>();

        protected override void AffectObject()
        {
            OnStartDetection();
        }

        protected abstract void OnStartDetection();
        protected abstract void OnTrigger();
        public virtual void SubscribeToTrigger(System.Action<GameObject, GameObject> _action)
        {

        }
        public virtual void UnSubscribeToTrigger(System.Action<GameObject, GameObject> _action)
        {

        }

    }
}


