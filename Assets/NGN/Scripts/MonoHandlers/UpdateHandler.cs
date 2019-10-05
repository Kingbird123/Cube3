using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public class UpdateHandler : MonoHandler
    {
        protected virtual void Update()
        {
            RunAllCallBacks();
        }
    }
}


