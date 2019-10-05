using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
namespace NGN
{
    public abstract class NGNEntity : MonoBehaviour
    {
        [SerializeField] protected ValueManager valueManager;

        protected ValueManager instancedValueManager;
        public ValueManager InstancedValueManager { get { return instancedValueManager; } }

        protected virtual void Awake()
        {
            InitiateValueManager();
        }

        protected virtual void InitiateValueManager()
        {
            instancedValueManager = Instantiate(valueManager);
            instancedValueManager.Initialize(this);
        }
       
    }
}


