using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public interface IAction
    {
        bool IsReady { get; set; }
        void Initialize(IAction _prevAction);
        void OnEnter();
    }
}


