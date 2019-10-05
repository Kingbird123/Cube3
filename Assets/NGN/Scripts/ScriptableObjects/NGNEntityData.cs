using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public abstract class NGNEntityData : NGNScriptableObject
    {
        protected NGNEntity owner;

        public virtual void Initialize(NGNEntity _owner)
        {
            owner = _owner;
        }

    }
}


