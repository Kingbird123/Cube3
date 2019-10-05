using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemAimableData))]
public class ItemAimableDataEditor : ItemUseableDataEditor
{
    protected new ItemAimableData Source { get { return (ItemAimableData)source; } }
    private SerializedProperty aimMask;
    private SerializedProperty aimDistance;
    private SerializedProperty aimOffset;
    private SerializedProperty muzzlePos;
    private SerializedProperty muzzlePosInd;
    private SerializedProperty aimFX;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetLinkedType<ItemAimable>();
    }

    protected override void GetProperties()
    {
        base.GetProperties();
        aimDistance = sourceRef.FindProperty("aimDistance");
        aimOffset = sourceRef.FindProperty("aimOffset");
        aimMask = sourceRef.FindProperty("aimMask");
        muzzlePos = sourceRef.FindProperty("muzzlePos");
        muzzlePosInd = sourceRef.FindProperty("muzzlePosInd");
        aimFX = sourceRef.FindProperty("aimFX");

    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorGUILayout.LabelField("Aim Properties", boldStyle);
        EditorGUILayout.PropertyField(aimMask);
        EditorGUILayout.PropertyField(aimDistance);
        EditorGUILayout.PropertyField(aimOffset);
        EditorGUILayout.PropertyField(aimFX);
        EditorExtensions.DisplayAllChildrenPopup("Muzzle Pos", connectedPrefab, muzzlePosInd, muzzlePos);
    }

}
