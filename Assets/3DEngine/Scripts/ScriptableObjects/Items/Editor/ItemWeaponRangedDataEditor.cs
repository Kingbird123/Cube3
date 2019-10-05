using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(ItemWeaponRangedData))]
public class ItemWeaponRangedDataEditor : ItemAimableDataEditor
{
    protected new ItemWeaponRangedData Source { get { return (ItemWeaponRangedData)source; } }

    private SerializedProperty fireType;
    private SerializedProperty damage;
    private SerializedProperty instantFirstShot;
    private SerializedProperty fireDelay;
    private SerializedProperty fireDistance;
    private SerializedProperty projectileSpeed;
    public SerializedProperty aimTransformAtTarget;
    public SerializedProperty aimTrans;
    private SerializedProperty mask;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetLinkedType<ItemWeaponRanged>();
    }

    protected override void GetProperties()
    {
        base.GetProperties();
        ammoType = sourceRef.FindProperty("ammoType");
        preloadAmmo = sourceRef.FindProperty("preloadAmmo");
        ammoDatas = sourceRef.FindProperty("ammoDatas");
        fireType = sourceRef.FindProperty("fireType");
        fireDelay = sourceRef.FindProperty("fireDelay");
        instantFirstShot = sourceRef.FindProperty("instantFirstShot");
        damage = sourceRef.FindProperty("damage");
        fireDistance = sourceRef.FindProperty("fireDistance");
        projectileSpeed = sourceRef.FindProperty("projectileSpeed");
        aimTransformAtTarget = sourceRef.FindProperty("aimTransformAtTarget");
        aimTrans = sourceRef.FindProperty("aimTrans");
        mask = sourceRef.FindProperty("mask");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorGUILayout.LabelField("Weapon Properties", boldStyle);
        EditorGUILayout.PropertyField(fireType);
        EditorGUILayout.PropertyField(damage);
        if (fireType.enumValueIndex == (int)ItemWeaponRangedData.FireType.Repeated)
        {
            EditorGUILayout.PropertyField(instantFirstShot);
            EditorGUILayout.PropertyField(fireDelay);
        }
        EditorGUILayout.PropertyField(fireDistance);
        EditorGUILayout.PropertyField(projectileSpeed);
        EditorGUILayout.PropertyField(aimTransformAtTarget);
        if (aimTransformAtTarget.boolValue)
            EditorGUILayout.PropertyField(aimTrans);
        EditorGUILayout.PropertyField(mask);
    }

}
