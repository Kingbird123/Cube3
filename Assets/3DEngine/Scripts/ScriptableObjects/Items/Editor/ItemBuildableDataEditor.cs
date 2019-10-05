using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemBuildableData))]
public class ItemBuildableDataEditor : ItemAimableDataEditor
{
    protected new ItemBuildableData Source { get { return (ItemBuildableData)source; } }
    public SerializedProperty placeables;
    public SerializedProperty validMaterial;
    public SerializedProperty invalidMaterial;
    public SerializedProperty overlapMask;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetLinkedType<ItemBuildable>();
    }

    protected override void GetProperties()
    {
        base.GetProperties();
        placeables = sourceRef.FindProperty("placeables");
        validMaterial = sourceRef.FindProperty("validMaterial");
        invalidMaterial = sourceRef.FindProperty("invalidMaterial");
        overlapMask = sourceRef.FindProperty("overlapMask");

    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Buildable Properties", FontStyle.Bold);
        EditorExtensions.ArrayFieldCustom(placeables, true, true, PlaceableField);
        EditorGUILayout.PropertyField(validMaterial);
        EditorGUILayout.PropertyField(invalidMaterial);
        EditorGUILayout.PropertyField(overlapMask);
    }

    void PlaceableField(SerializedProperty _property)
    {
        var placeableName = _property.FindPropertyRelative("placeableName");
        var validPlacementMask = _property.FindPropertyRelative("validPlacementMask");
        var snapType = _property.FindPropertyRelative("snapType");
        var transName = _property.FindPropertyRelative("transName");
        var gridSpacing = _property.FindPropertyRelative("gridSpacing");
        var gridOffset = _property.FindPropertyRelative("gridOffset");
        var placeableSize = _property.FindPropertyRelative("placeableSize");
        var previewPrefab = _property.FindPropertyRelative("previewPrefab");
        var placedPrefab = _property.FindPropertyRelative("placedPrefab");

        EditorGUILayout.PropertyField(placeableName);
        EditorGUILayout.PropertyField(validPlacementMask);
        EditorGUILayout.PropertyField(snapType);
        if (snapType.enumValueIndex == 1)
            EditorGUILayout.PropertyField(transName);
        if (snapType.enumValueIndex == 2)
        {
            EditorGUILayout.PropertyField(gridSpacing);
            EditorGUILayout.PropertyField(gridOffset);
        }   
        EditorGUILayout.PropertyField(placeableSize);
        EditorGUILayout.PropertyField(previewPrefab);
        EditorGUILayout.PropertyField(placedPrefab);
    }

}
