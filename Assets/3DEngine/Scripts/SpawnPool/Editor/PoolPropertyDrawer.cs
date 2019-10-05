using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Pool))]
public class PoolPropertyDrawer : PropertyDrawer
{
    private SerializedProperty sourceRef;

    private SerializedProperty prefabToSpawn;
    private SerializedProperty amountToSpawn;
    private SerializedProperty createMoreIfEmpty;
    private SerializedProperty spawnOldest;

    //need to set field amount manually if you add more fields
    private int fieldAmount;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //set the height of the drawer by the field size and padding
        return (EditorGUIUtility.singleLineHeight * fieldAmount) + (EditorGUIUtility.standardVerticalSpacing * fieldAmount);
    }

    public virtual void GetProperties()
    {
        //get property values
        prefabToSpawn = sourceRef.FindPropertyRelative("prefabToSpawn");
        amountToSpawn = sourceRef.FindPropertyRelative("amountToSpawn");
        createMoreIfEmpty = sourceRef.FindPropertyRelative("createMoreIfEmpty");
        spawnOldest = sourceRef.FindPropertyRelative("spawnOldest");
    }


    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        sourceRef = property;
        GetProperties();

        //divide all field heights by the field amount..then minus the padding
        position.height /= fieldAmount; position.height -= EditorGUIUtility.standardVerticalSpacing;

        // Draw Prefix label...this will push all other content to the right
        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Draw non-indented label instead
        string poolName = "Add Prefab To Spawn";
        if (prefabToSpawn.objectReferenceValue)
        {
            poolName = prefabToSpawn.objectReferenceValue.name;
        }
        EditorGUI.LabelField(position, poolName);

        // Get the start indent level
        var indent = EditorGUI.indentLevel;
        // Set indent amount
        EditorGUI.indentLevel = indent + 1;

        DisplayGUIElements(position, property, label);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    void DisplayGUIElements(Rect position, SerializedProperty property, GUIContent label)
    {
        fieldAmount = 1;

        //prefabToSpawn
        fieldAmount++;
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(position, prefabToSpawn);

        //amountToSpawn
        fieldAmount++;
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(position, amountToSpawn);

        //createMoreIfEmpty
        fieldAmount++;
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(position, createMoreIfEmpty);

        //spawnOldest
        fieldAmount++;
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(position, spawnOldest);

    }

}