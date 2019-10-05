using UnityEngine;
using MEC;

namespace NGN
{
    [CreateAssetMenu(menuName = "NGN/Data/StateTrigger/Input", order = 1)]
    public class StateTriggerInput : EntityStateTriggerData
    {
        [SerializeField] protected InputProperty input;
        public override void OnInitialize (EntityStateData _nextState)
        {
            base.OnInitialize(_nextState);
            NGNMonoHandler.SubscribeToUpdate(nextStateCache, OnUpdate);  
        }

        protected virtual void OnUpdate(EntityStateData _nextState)
        {
            if (input.GetInputDown())
            {
                DoOnTrigger(_nextState);
                NGNMonoHandler.UnSubscribeToUpdate(_nextState, OnUpdate);
            }
        }
    }
}


