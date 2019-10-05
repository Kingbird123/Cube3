using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public abstract class NGNTransition : ITransition
    {
        public bool IsTriggered { get; set; }
        protected NGNStateTransitionHandler handler;

        protected List<System.Action> onTransitionCallbacks = new List<System.Action>();

        public virtual void Initialize()
        {
            OnInitialize();
        }

        public virtual void Initialize(System.Action _onTransitionCallback)
        {
            Subscribe(_onTransitionCallback);
            OnInitialize();
        }

        protected abstract void OnInitialize();

        protected virtual void OnTransition()
        {
            DoTransitionActions();
        }

        public virtual void Subscribe(System.Action _onTransitionCallback)
        {
            if (!onTransitionCallbacks.Contains(_onTransitionCallback))
                onTransitionCallbacks.Add(_onTransitionCallback);
        }

        public virtual void UnSubscribe(System.Action _onTransitionCallback)
        {
            if (onTransitionCallbacks.Contains(_onTransitionCallback))
                onTransitionCallbacks.Remove(_onTransitionCallback);
        }

        protected virtual void DoTransitionActions()
        {
            for (int i = 0; i < onTransitionCallbacks.Count; i++)
            {
                onTransitionCallbacks[i].Invoke();
            }
        }
    }
}


