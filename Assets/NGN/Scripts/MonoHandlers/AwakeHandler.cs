using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public class AwakeHandler : MonoHandler
    {
        protected virtual void Awake()
        {
            RunCallbacks();
        }
    }
}

