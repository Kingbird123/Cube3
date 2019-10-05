using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemInteractFXData))]
public class ItemInteractFXDataEditor : ItemAimableDataEditor
{
    protected new ItemInteractFXData Source { get { return (ItemInteractFXData)source; } }
    public SerializedProperty interacts;
    public SerializedProperty runInteractsOnOwner;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetLinkedType<ItemInteractFX>();
    }

    protected override void GetProperties()
    {
        base.GetProperties();
        interacts = sourceRef.FindProperty("interacts");
        runInteractsOnOwner = sourceRef.FindProperty("runInteractsOnOwner");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorExtensions.LabelFieldCustom("InteractFX Properties", FontStyle.Bold);
        EditorGUILayout.PropertyField(interacts, true);
        EditorGUILayout.PropertyField(runInteractsOnOwner);
    }

}
