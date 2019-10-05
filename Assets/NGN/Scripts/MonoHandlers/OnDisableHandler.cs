using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public class OnDisableHandler : MonoHandler
    {
        protected virtual void OnDisable()
        {
            RunCallbacks();
        }
    }
}

