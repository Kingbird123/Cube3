using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ItemWeaponRanged))]
public class ItemWeaponRangedEditor : ItemEditor
{
    protected new EngineEntity Source { get { return (ItemWeaponRanged)source; } }
    protected SerializedProperty fireButton;

    protected override void GetProperties()
    {
        base.GetProperties(); 
        fireButton = sourceRef.FindProperty("fireButton");
    }

    protected override void DisplayDataProperties<T>()
    {
        base.DisplayDataProperties<ItemWeaponRangedData>();
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorExtensions.LabelFieldCustom("Input Options", FontStyle.Bold);
        EditorGUILayout.PropertyField(fireButton);
    }

}
