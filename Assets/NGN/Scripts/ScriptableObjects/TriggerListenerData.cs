using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public class TriggerListenerData : NGNEventData
    {
        [SerializeField] protected NGNTriggerData trigger;

        public override void DoEvent(GameObject _sender, GameObject _receiver)
        {
            trigger.DoEvent(_sender, _receiver);
        }
    }
}


