using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public class LateStartHandler : MonoHandler
    {
        protected virtual void Start()
        {
            StartCoroutine(StartLateStart());
        }

        IEnumerator StartLateStart()
        {
            yield return new WaitForEndOfFrame();
            LateStart();
        }

        protected virtual void LateStart()
        {
            RunCallbacks();
        }
    }
}


