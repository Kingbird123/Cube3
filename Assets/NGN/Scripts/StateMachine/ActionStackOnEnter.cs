using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    [System.Serializable]
    public class ActionStackOnEnter : NGNActionStack
    {
        public override void Initialize()
        {
            base.Initialize();
            DoActions();
        }
    }
}


