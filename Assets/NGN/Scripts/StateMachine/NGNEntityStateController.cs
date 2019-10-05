using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
namespace NGN
{
    public class NGNEntityStateController : NGNEntity
    {
        [SerializeField] protected NGNStateHandler[] states;
        [SerializeField] protected NGNStateTransitionHandler[] transitions;

        protected NGNStateHandler preferredState;
        protected List<NGNStateHandler> currentActiveStates = new List<NGNStateHandler>();

        protected override void Awake()
        {
            base.Awake();
            OnBeginTransitions();
            OnBeginStates();
        }

        protected virtual void OnBeginTransitions()
        {
            for (int i = 0; i < transitions.Length; i++)
            {
                transitions[i].Initialize(this);
            }
        }

        protected virtual void OnBeginStates()
        {
            AddActiveState(states[0]);
        }

        public virtual void AddActiveState(NGNStateHandler _state)
        {
            currentActiveStates.Add(_state);
            preferredState = GetPreferredState();
            BeginPreferredState();
        }

        protected virtual void BeginPreferredState()
        {
            preferredState.Initialize(preferredState, OnStateFinished);
        }

        protected virtual void OnStateFinished(NGNStateHandler _finishedState)
        {
            currentActiveStates.Remove(_finishedState);
            preferredState = GetPreferredState();
            _finishedState.UnSubscribe(_finishedState, OnStateFinished);
            BeginPreferredState();
        }

        protected virtual NGNStateHandler GetPreferredState()
        {
            var curState = currentActiveStates[0];
            for (int i = 0; i < currentActiveStates.Count; i++)
            {
                var state = currentActiveStates[i];
                if (curState.state.Priority < state.state.Priority)
                    curState = state;
            }
            return curState;
        }

        protected virtual void OnDestroy()
        {
        }

    }
}


