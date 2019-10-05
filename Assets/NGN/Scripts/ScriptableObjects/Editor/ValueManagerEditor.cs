using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NGN
{
    [CustomEditor(typeof(ValueManager))]
    public class ValueManagerEditor : NGNScriptableObjectEditor
    {
        protected new ValueManager Source { get { return (ValueManager)source; } }

        protected SerializedProperty valueCategories;

        protected override void GetProperties()
        {
            base.GetProperties();
            valueCategories = sourceRef.FindProperty("valueCategories");
        }

        protected override void SetProperties()
        {
            base.SetProperties();
            DisplayValueCategories();
        }

        protected virtual void DisplayValueCategories()
        {
            valueCategories.ArraySingleFieldButtons(NGNScriptableObjectFieldExtension.ExpandableScriptableObjectField);
        }


    }
}


