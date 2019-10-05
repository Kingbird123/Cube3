using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(ItemWeaponMeleeData))]
public class ItemWeaponMeleeDataEditor : ItemAimableDataEditor
{
    protected new ItemWeaponMeleeData Source { get { return (ItemWeaponMeleeData)source; } }

    private SerializedProperty repeatUntilStopped;
    private SerializedProperty allowSpamming;
    private SerializedProperty damage;
    private SerializedProperty damageDelay;
    private SerializedProperty activeTime;
    private SerializedProperty bounceType;
    private SerializedProperty direction;
    private SerializedProperty bounceForce;
    private SerializedProperty unitAmount;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetLinkedType<ItemWeaponMelee>();
    }

    protected override void GetProperties()
    {
        base.GetProperties();
        repeatUntilStopped = sourceRef.FindProperty("repeatUntilStopped");
        allowSpamming = sourceRef.FindProperty("allowSpamming");
        damage = sourceRef.FindProperty("damage");
        damageDelay = sourceRef.FindProperty("damageDelay");
        activeTime = sourceRef.FindProperty("activeTime");
        bounceType = sourceRef.FindProperty("bounceType");
        direction = sourceRef.FindProperty("direction");
        bounceForce = sourceRef.FindProperty("bounceForce");
        unitAmount = sourceRef.FindProperty("unitAmount");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorGUILayout.LabelField("Melee Weapon Properties", boldStyle);
        EditorGUILayout.PropertyField(repeatUntilStopped);
        EditorGUILayout.PropertyField(allowSpamming);
        EditorGUILayout.PropertyField(damage);
        EditorGUILayout.PropertyField(damageDelay);
        EditorGUILayout.PropertyField(activeTime);
        EditorGUILayout.PropertyField(bounceType);
        if (bounceType.enumValueIndex == 2)
            EditorGUILayout.PropertyField(direction);
        EditorGUILayout.PropertyField(bounceForce);
        EditorGUILayout.PropertyField(unitAmount);
    }

}
