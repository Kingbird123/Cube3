using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public abstract class NGNTriggerInputData : NGNTriggerData
    {
        public enum InputTriggerType { InputDown, InputUp, InputTrue, InputFalse }
        [SerializeField] protected InputProperty triggerInput;
        [SerializeField] protected InputTriggerType inputTriggerType;

        protected override void OnStartDetection()
        {

        }

        protected virtual void DoInputDetection()
        {

        }

        protected override void OnTrigger()
        {
            OnInputTrigger();
        }
        protected abstract void OnInputTrigger();
    }
}


