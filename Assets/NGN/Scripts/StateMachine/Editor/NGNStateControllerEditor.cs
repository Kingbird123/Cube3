using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NGN
{
    [CustomEditor(typeof(NGNEntityStateController))]
    public class NGNStateControllerEditor : Editor
    {
        protected SerializedObject sourceRef;
        protected NGNEntityStateController source;
        protected SerializedProperty iterator;
        protected SerializedProperty valueManager;

        protected virtual void OnEnable()
        {
            source = (NGNEntityStateController)target;
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
            iterator = sourceRef.GetIterator();

        }

        protected virtual void SetProperties()
        {
            
        }

        protected virtual void DisplayEntityData()
        {
            EditorExtensions.LabelFieldCustom("Value Manager", FontStyle.Bold);
        }
    }
}


