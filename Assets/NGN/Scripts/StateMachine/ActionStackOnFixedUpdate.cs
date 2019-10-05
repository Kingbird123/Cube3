using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    [System.Serializable]
    public class ActionStackOnFixedUpdate : NGNActionStack
    {
        public override void Initialize()
        {
            base.Initialize();
            NGNMonoHandler.SubscribeToFixedUpdate(DoActions);
        }
    }
}


