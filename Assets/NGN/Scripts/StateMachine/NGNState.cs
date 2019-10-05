using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    [System.Serializable]
    public class NGNState
    {
        [SerializeField] protected int priority;
        public int Priority { get { return priority; } }
        [SerializeField] protected Object[] actions;

        protected NGNActionStack[] actionSources;

        public void Initialize()
        {
            actionSources = new NGNActionStack[actions.Length];
            for (int i = 0; i < actions.Length; i++)
            {
                //if (actions.)
            }

            for (int i = 0; i < actions.Length; i++)
            {
                //actions[i].Initialize();
            }
        }

        public void Finish()
        {
            for (int i = 0; i < actions.Length; i++)
            {
                //actions[i].Finish();
            }
        }
    }
}


