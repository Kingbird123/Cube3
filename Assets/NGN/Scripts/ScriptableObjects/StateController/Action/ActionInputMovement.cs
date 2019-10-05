using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    [CreateAssetMenu(menuName = "NGN/Data/Action/InputMovement", order = 1)]
    public class ActionInputMovement : ActionData
    {
        [SerializeField] protected InputProperty horInput;
        [SerializeField] protected InputProperty verInput;
        [SerializeField] protected bool rawInputValues;
        [SerializeField] protected float speed = 5;

        protected override void OnEnter(NGNEntity _owner)
        {
            var inputHor = horInput.GetAxis(rawInputValues);
            var inputVer = verInput.GetAxis(rawInputValues);
            var move = new Vector3(inputHor, 0, inputVer).normalized;
            _owner.transform.Translate( move * speed * Time.deltaTime);
        }

        protected override void OnExit(NGNEntity _owner)
        {
            
        }
    }
}


