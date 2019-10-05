using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NGN
{
    [CustomEditor(typeof(NGNEntity))]
    public class NGNEntityEditor : Editor
    {
        protected SerializedObject sourceRef;
        protected NGNEntity source;

        protected SerializedProperty valueManager;

        protected virtual void OnEnable()
        {
            source = (NGNEntity)target;
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
            valueManager = sourceRef.FindProperty("valueManager");
        }

        protected virtual void SetProperties()
        {
            DisplayEntityData();
        }

        protected virtual void DisplayEntityData()
        {
            EditorExtensions.LabelFieldCustom("Value Manager", FontStyle.Bold);
            valueManager.ExpandableScriptableObjectField(0);
        }
    }
}


