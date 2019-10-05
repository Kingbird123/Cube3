using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (UIEngineValueEntity))]
public class UIEngineValueEntityEditor : Editor
{
    protected UIEngineValueEntity source;
    protected SerializedObject sourceRef;

    protected SerializedProperty avatarImage;
    protected SerializedProperty UILayouts;

    public virtual void OnEnable ()
    {
        source = (UIEngineValueEntity)target;
        sourceRef = serializedObject;
        GetProperties ();
    }

    public override void OnInspectorGUI ()
    {
        SetProperties ();

        sourceRef.ApplyModifiedProperties ();
    }

    public virtual void GetProperties ()
    {
        avatarImage = sourceRef.FindProperty ("avatarImage");
        UILayouts = sourceRef.FindProperty ("UILayouts");
    }

    public virtual void SetProperties ()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(avatarImage);
        UILayouts.ArrayFieldButtons("UI Layouts", true, true, true, true, EngineValueUIField);

    }

    void EngineValueUIField(SerializedProperty _property, int _ind)
    {
        var layoutMaster = _property.FindPropertyRelative("layoutMaster");
        var engineValues = _property.FindPropertyRelative("engineValues");

        EditorGUILayout.PropertyField(layoutMaster);
        if (layoutMaster.objectReferenceValue)
        {
            var master = new SerializedObject(layoutMaster.objectReferenceValue);
            var layouts = master.FindProperty("layouts");
            engineValues.arraySize = layouts.arraySize;
            for (int i = 0; i < engineValues.arraySize; i++)
            {
                var layEle = layouts.GetArrayElementAtIndex(i);
                var engEle = engineValues.GetArrayElementAtIndex(i);
                var layoutName = layEle.FindPropertyRelative("layoutName").stringValue;
                engEle.objectReferenceValue = EditorGUILayout.ObjectField(layoutName, engEle.objectReferenceValue, typeof(UIEngineValue), true);
            }
            master.Dispose();
        }
    }

}