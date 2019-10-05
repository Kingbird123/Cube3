using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public class OnEnableHandler : MonoHandler
    {
        protected virtual void OnEnable()
        {
            RunCallbacks();
        }
    }
}

