using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    [System.Serializable]
    public class NGNStateTransitionHandler
    {
        [SerializeField] protected bool anyState;
        [SerializeField] protected NGNStateHandler prevState;
        [SerializeField] protected NGNTransition transition;
        public NGNTransition Transition { get { return transition; } }
        [SerializeField] protected NGNStateHandler nextState;

        protected NGNEntityStateController controller;

        public void Initialize(NGNEntityStateController _controller)
        {
            controller = _controller;
            transition.Initialize(DoTransition);
        }

        public void DoTransition()
        {
            controller.AddActiveState(nextState);
        }
    }
}


