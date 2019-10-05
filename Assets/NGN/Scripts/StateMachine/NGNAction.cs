using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace NGN
{
    [System.Serializable]
    public abstract class NGNAction : IAction
    { 
        public enum BeginType { OnInitialize, OnPreviousReady }
        [SerializeField] protected BeginType beginType;
        [SerializeField] protected GameObject owner;
        [SerializeField] protected float beginDelay;
        public bool IsReady { get; set; }
        protected UpdateHandler updateHandler;
        protected FixedUpdateHandler fixedUpdateHandler;
        protected LateUpdateHandler lateUpdateHandler;
        protected CoroutineHandle waitCoroutine;
        protected IAction prevAction;

        public virtual void Initialize(IAction _prevAction)
        {
            prevAction = _prevAction;
            if (beginType == BeginType.OnInitialize)
            {
                OnBegin();
            }    
            else if (beginType == BeginType.OnPreviousReady)
                waitCoroutine = Timing.RunCoroutine(StartWaitForPrevious());
        }

        IEnumerator<float> StartWaitForPrevious()
        {
            if (prevAction != null)
            {
                while (!prevAction.IsReady)
                    yield return Timing.WaitForOneFrame;
            }
            OnBegin();
        }

        protected virtual void OnBegin()
        {
            if (beginDelay != 0)
                waitCoroutine = Timing.RunCoroutine(StartDelay());
            else
            {
                OnEnter();
            }
        }

        IEnumerator<float> StartDelay()
        {
            yield return Timing.WaitForSeconds(beginDelay);
            OnEnter();
        }

        public virtual void OnEnter()
        {
            IsReady = true;
        }

        public abstract void RunAction();
    }
}


