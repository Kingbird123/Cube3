using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    [CreateAssetMenu(menuName = "NGN/Data/StateController/State", order = 1)]
    public class EntityStateData : NGNEntityData
    {
        [SerializeField] protected ActionData[] actions;

        public override void Initialize(NGNEntity _owner)
        {
            base.Initialize(_owner);
            RunActions(_owner);
        }

        public void RunActions(NGNEntity _owner)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].StartAction(_owner);
            }
        }

        public void StopActions(NGNEntity _owner)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].StopAction(_owner);
            }
        }
    }
}


