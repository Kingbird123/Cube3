using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public class FixedUpdateHandler : MonoHandler
    {
        protected virtual void FixedUpdate()
        {
            RunAllCallBacks();
        }
    }
}


