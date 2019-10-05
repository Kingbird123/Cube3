using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ItemWeaponPush))]
public class ItemWeaponPushEditor : ItemEditor
{
    protected new ItemWeaponPush Source { get { return (ItemWeaponPush)source; } }
    protected SerializedProperty fireButton;

    protected override void GetProperties()
    {
        base.GetProperties();
        fireButton = sourceRef.FindProperty("fireButton");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorGUILayout.PropertyField(fireButton);
    }

    protected override void DisplayDataProperties<T>()
    {
        base.DisplayDataProperties<ItemWeaponPushData>();
    }
}
