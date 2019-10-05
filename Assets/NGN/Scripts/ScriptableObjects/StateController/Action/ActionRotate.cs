using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    [CreateAssetMenu(menuName = "NGN/Data/Action/Rotate", order = 1)]
    public class ActionRotate : ActionData
    {

        [SerializeField] protected float speed = 5;

        protected override void OnEnter(NGNEntity _owner)
        {
            var rot = _owner.transform.localEulerAngles;
            var yRot = rot.y;
            yRot += speed * Time.deltaTime;
            _owner.transform.localEulerAngles = new Vector3(rot.x, yRot, rot.z);
        }

        protected override void OnExit(NGNEntity _owner)
        {    
        }
    }
}


