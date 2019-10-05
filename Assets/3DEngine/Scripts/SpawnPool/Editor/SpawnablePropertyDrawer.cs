using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System;

[CustomPropertyDrawer(typeof(Spawnable))]
public class SpawnablePropertyDrawer : PropertyDrawer
{
    private SerializedProperty poolIndex;
    private SerializedProperty prefabName;
    private SerializedProperty poolListNames;
    private SerializedProperty prefabToSpawn;
    private SerializedProperty poolData;
    private SerializedProperty overrideManager;

    private int fieldAmount;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        GetProperties(property);
        fieldAmount = 1;
        if (!overrideManager.boolValue)
            fieldAmount++;
        //set the height of the drawer by the field size and padding
        return (EditorGUIUtility.singleLineHeight * fieldAmount) + (EditorGUIUtility.standardVerticalSpacing * fieldAmount);
    }

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        GetProperties(property);

        //divide all field heights by the field amount..then minus the padding
        position.height /= fieldAmount; position.height -= EditorGUIUtility.standardVerticalSpacing;

        DisplayGUIElements(position, property, label);

        EditorGUI.EndProperty();
    }

    void GetProperties(SerializedProperty property)
    {
        poolIndex = property.FindPropertyRelative("poolIndex");
        prefabName = property.FindPropertyRelative("prefabName");
        poolListNames = property.FindPropertyRelative("poolListNames");
        prefabToSpawn = property.FindPropertyRelative("prefabToSpawn");
        poolData = property.FindPropertyRelative("poolData");
        overrideManager = property.FindPropertyRelative("overrideManager");
    }

    void DisplayGUIElements(Rect position, SerializedProperty property, GUIContent label)
    {
        var source = property.GetRootValue<Spawnable>();
        if (source == null)
            return;

        if (!overrideManager.boolValue)
        {
            EditorGUI.PropertyField(position, poolData);
        }

        if (poolData.objectReferenceValue)
        {
            source.poolListNames = source.poolData.GetPoolNames();
        }

        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        poolIndex.intValue = EditorGUI.Popup(position, "Object To Spawn", poolIndex.intValue, source.poolListNames);
        if (source.poolListNames.Length > 0)
        {
            poolIndex.intValue = Mathf.Clamp(poolIndex.intValue, 0, poolListNames.arraySize);
            prefabName.stringValue = source.poolListNames[poolIndex.intValue];
            prefabToSpawn.objectReferenceValue = source.poolData.GetPoolPrefabToSpawn(poolIndex.intValue);
        }

    }
}