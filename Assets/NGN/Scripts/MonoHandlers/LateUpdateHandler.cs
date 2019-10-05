using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public class LateUpdateHandler : MonoHandler
    {
        protected virtual void LateUpdate()
        {
            RunAllCallBacks();
        }
    }
}


