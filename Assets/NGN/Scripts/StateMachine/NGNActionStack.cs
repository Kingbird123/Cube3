using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public abstract class NGNActionStack : Object
    {
        public NGNAction[] actions;
        public bool IsFinished { get; set; }
        public virtual void Initialize()
        {
            for (int i = 0; i < actions.Length; i++)
            {
                IAction prev = null;
                if (i > 0)
                    prev = actions[i - 1];
                actions[i].Initialize(prev);
            }
        }

        protected virtual void DoActions()
        {
            for (int i = 0; i < actions.Length; i++)
            {
                if (actions[i].IsReady)
                    actions[i].RunAction();
            }
        }

        public virtual void Finish()
        {
            IsFinished = true;
        }
    }
}


