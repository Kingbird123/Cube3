using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    [CreateAssetMenu(menuName = "NGN/Data/Action/TriggerInputEvents", order = 1)]
    public class TriggerInputEventsData : NGNTriggerInputData
    {
        [SerializeField] protected NGNEventData[] events;

        protected override void OnInputTrigger()
        {
            DoEvents();
        }

        protected virtual void DoEvents()
        {
            for (int i = 0; i < events.Length; i++)
            {
                events[i].DoEvent(sender, affectedGameObject);
            }
        }
    }
}


