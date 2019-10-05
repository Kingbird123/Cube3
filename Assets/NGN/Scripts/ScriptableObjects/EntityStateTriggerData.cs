using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public abstract class EntityStateTriggerData : NGNScriptableObject
    {
        public delegate void OnTriggerDelegate(EntityStateTriggerData _trigger, EntityStateData _nextState);
        public event OnTriggerDelegate OnTrigger;
        protected EntityStateData nextStateCache;

        protected virtual void DoOnTrigger(EntityStateData _nextState) { nextStateCache = _nextState;  OnTrigger?.Invoke(this, nextStateCache); }
        public virtual void OnInitialize(EntityStateData _nextState) { nextStateCache = _nextState; }
    }
}


