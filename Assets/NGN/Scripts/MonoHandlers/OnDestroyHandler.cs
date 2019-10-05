using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public class OnDestroyHandler : MonoHandler
    {
        protected virtual void OnDestroy()
        {
            RunCallbacks();
        }
    }
}

