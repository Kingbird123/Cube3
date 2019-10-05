using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Unit))]
public class UnitEditor : EngineEntityEditor
{
    protected SerializedProperty skinOptions;
    protected SerializedProperty childSkinOverride;
    protected SerializedProperty spawnSkinOverride;
    protected SerializedProperty spawnLocationData;
    protected SerializedProperty spawnLocations;

    private UnitEquip equip;

    protected override void GetProperties()
    {
        base.GetProperties();
        //skins
        data = sourceRef.FindProperty("data");
        childSkinOverride = sourceRef.FindProperty("childSkinOverride");
        skinOptions = sourceRef.FindProperty("skinOptions");
        spawnSkinOverride = sourceRef.FindProperty("spawnSkinOverride");
        spawnLocationData = sourceRef.FindProperty("spawnLocationData");
        spawnLocations = sourceRef.FindProperty("spawnLocations");


        equip = source.GetComponent<UnitEquip>();
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        DisplaySpawnProperties();

    }

    protected override void DisplayDataProperties<T>()
    {
        //skins
        base.DisplayDataProperties<UnitData>();
        if (data.objectReferenceValue)
        {
            EditorExtensions.LabelFieldCustom("Skin Options", FontStyle.Bold);
            EditorGUILayout.PropertyField(skinOptions);
            if (skinOptions.enumValueIndex == (int)Unit.SpawnSkinOptions.Override)
                spawnSkinOverride.objectReferenceValue = EditorGUILayout.ObjectField("Prefab Override", spawnSkinOverride.objectReferenceValue, typeof(GameObject), false);
            else if (skinOptions.enumValueIndex == (int)Unit.SpawnSkinOptions.Child)
            {
                EditorGUILayout.PropertyField(childSkinOverride);
                var child = childSkinOverride.GetRootValue<GameObject>();
                if (child)
                {
                    if (!source.transform.FindDeepChild(child.name))
                    {
                        Debug.Log("You must assign a child of " + source + " to " + childSkinOverride.displayName);
                        childSkinOverride.objectReferenceValue = null;
                    }
                }
            }
            
        }
        else
            EditorExtensions.LabelFieldCustom("You must assign a unit data file to " + data.displayName, FontStyle.Bold, Color.red);
    }

    protected virtual void DisplaySpawnProperties()
    {
        if (skinOptions.enumValueIndex != 3)
        {
            if (skinOptions.enumValueIndex == 1 || skinOptions.enumValueIndex == 2)
            {
                EditorGUILayout.PropertyField(spawnLocationData);
                var locData = spawnLocationData.GetRootValue<ItemLocationData>();
                if (locData)
                {
                    var names = locData.GetItemLocationNames();
                    spawnLocations.arraySize = names.Length;
                    for (int i = 0; i < names.Length; i++)
                    {
                        var ele = spawnLocations.GetArrayElementAtIndex(i);
                        var overrideParent = ele.FindPropertyRelative("overrideParent");
                        var overridePropertyName = ele.FindPropertyRelative("overridePropertyName");
                        var parent = ele.FindPropertyRelative("parent");

                        overrideParent.boolValue = true;
                        if (skinOptions.enumValueIndex == 1)
                            parent.objectReferenceValue = spawnSkinOverride.objectReferenceValue;
                        if (skinOptions.enumValueIndex == 2)
                            parent.objectReferenceValue = childSkinOverride.objectReferenceValue;
                        overridePropertyName.stringValue = names[i];
                        EditorGUILayout.PropertyField(ele);

                    }
                }
                else
                    EditorExtensions.LabelFieldCustom("Need location data to spawn items on unit", FontStyle.Bold, Color.red);
            }
            
        }
        else if (equip)
            EditorExtensions.LabelFieldCustom("You must assign a skin to equip items!", FontStyle.Bold, Color.red);
    }

}
