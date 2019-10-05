using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemButtonPressData))]
public class ItemButtonPressDataEditor : ItemUseableDataEditor
{
    protected new ItemButtonPressData Source { get { return (ItemButtonPressData)source; } }
    public SerializedProperty setButton;
    public SerializedProperty button;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetLinkedType<ItemButtonPress>();
    }

    protected override void GetProperties()
    {
        base.GetProperties();
        setButton = sourceRef.FindProperty("setButton");
        button = sourceRef.FindProperty("button");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorGUILayout.LabelField("Button Press Properties",boldStyle);
        EditorGUILayout.PropertyField(setButton);
        if (setButton.boolValue)
            EditorGUILayout.PropertyField(button);

    }

}
