using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public abstract class ActionData : NGNEntityData
    {
        public enum UpdateType { SingleFrame, Update, FixedUpdate, LateUpdate }
        [SerializeField] protected UpdateType updateType;
        protected abstract void OnEnter(NGNEntity _owner);
        protected abstract void OnExit(NGNEntity _owner);

        public virtual void StartAction(NGNEntity _owner)
        {
            SubscribeToMono(_owner);
        }
        private void SubscribeToMono(NGNEntity _owner)
        {
            if (updateType == UpdateType.Update)
                NGNMonoHandler.SubscribeToUpdate(_owner, OnEnter);
            else if (updateType == UpdateType.FixedUpdate)
                NGNMonoHandler.SubscribeToFixedUpdate(_owner, OnEnter);
            else if (updateType == UpdateType.LateUpdate)
                NGNMonoHandler.SubscribeToLateUpdate(_owner, OnEnter);
            else
                OnEnter(_owner);
        }
        public void StopAction(NGNEntity _owner)
        {
            UnSubscribeToMono(_owner);
            OnExit(_owner);
        }
        private void UnSubscribeToMono(NGNEntity _owner)
        {
            if (updateType == UpdateType.Update)
                NGNMonoHandler.UnSubscribeToUpdate(_owner, OnEnter);
            else if (updateType == UpdateType.FixedUpdate)
                NGNMonoHandler.UnSubscribeToFixedUpdate(_owner, OnEnter);
            else if (updateType == UpdateType.LateUpdate)
                NGNMonoHandler.UnSubscribeToLateUpdate(_owner, OnEnter);
        }
    }
}


