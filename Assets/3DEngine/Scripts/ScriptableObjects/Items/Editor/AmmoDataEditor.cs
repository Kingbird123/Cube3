using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Web;
using System;

[CustomEditor(typeof(AmmoData))]
public class AmmoDataEditor : ItemDataEditor
{
    protected new AmmoData Source { get { return (AmmoData)source; } }
    protected SerializedProperty ammoSelection;
    protected SerializedProperty projectileType;
    protected SerializedProperty projectile;
    protected SerializedProperty spreadType;
    protected SerializedProperty removeAmount;
    protected SerializedProperty fireAmount;
    protected SerializedProperty angle;
    protected SerializedProperty randomAmount;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetLinkedType<Ammo>();
    }

    protected override void GetProperties()
    {
        base.GetProperties();
        ammoSelection = sourceRef.FindProperty("ammoSelection");
        projectileType = sourceRef.FindProperty("projectileType");
        projectile = sourceRef.FindProperty("projectile");
        spreadType = sourceRef.FindProperty("spreadType");
        removeAmount = sourceRef.FindProperty("removeAmount");
        fireAmount = sourceRef.FindProperty("fireAmount");
        angle = sourceRef.FindProperty("angle");
        randomAmount = sourceRef.FindProperty("randomAmount");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorExtensions.LabelFieldCustom("Ammo Properties", FontStyle.Bold);
        ammoSelection.intValue = EditorGUILayout.Popup("Ammo Selection", ammoSelection.intValue, engineValueSelections.GetDisplayNames());
        EditorGUILayout.PropertyField(projectileType);
        if (projectileType.enumValueIndex == (int)AmmoData.ProjectileType.Projectile)
            EditorGUILayout.PropertyField(projectile);
        EditorGUILayout.PropertyField(removeAmount);
        EditorGUILayout.PropertyField(fireAmount);
        EditorGUILayout.PropertyField(spreadType);
        if (spreadType.enumValueIndex == (int)AmmoData.SpreadType.Angle)
            EditorGUILayout.PropertyField(angle);
        else if (spreadType.enumValueIndex == (int)AmmoData.SpreadType.Random)
            EditorGUILayout.PropertyField(randomAmount);

    }

}
