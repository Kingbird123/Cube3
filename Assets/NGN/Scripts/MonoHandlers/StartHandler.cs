using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public class StartHandler : MonoHandler
    {
        protected virtual void Start()
        {
            RunCallbacks();
        }
    }
}


