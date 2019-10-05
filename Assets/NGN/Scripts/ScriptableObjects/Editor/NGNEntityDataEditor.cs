using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NGN
{
    [CustomEditor(typeof(NGNScriptableObject))]
    public class NGNScriptableObjectEditor : Editor
    {
        protected SerializedObject sourceRef;
        protected NGNScriptableObject source;
        protected NGNScriptableObject Source { get { return source; } }

        protected virtual void OnEnable()
        {
            source = (NGNScriptableObject)target;
            sourceRef = serializedObject;
            GetProperties();
        }

        public override void OnInspectorGUI()
        {
            SetProperties();
            sourceRef.ApplyModifiedProperties();
        }

        protected virtual void GetProperties()
        {
        }

        protected virtual void SetProperties()
        {
        }


    }
}


