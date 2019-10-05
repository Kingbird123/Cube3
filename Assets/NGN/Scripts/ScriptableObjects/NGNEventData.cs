using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public abstract class NGNEventData : NGNScriptableObject
    {
        public abstract void DoEvent(GameObject _sender, GameObject _receiver);
    }
}


