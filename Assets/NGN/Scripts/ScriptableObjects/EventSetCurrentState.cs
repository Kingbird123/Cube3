using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public class EventSetCurrentState : NGNEventDynamicData
    {
        [SerializeField] private EntityStateData state = null;
        NGNEntityStateController controller;

        protected override void AffectObject()
        {
            controller = affectedGameObject.GetComponent<NGNEntityStateController>();
            if (controller)
            {
                //controller.SetCurrentState(state);
            }
        }

    }
}


