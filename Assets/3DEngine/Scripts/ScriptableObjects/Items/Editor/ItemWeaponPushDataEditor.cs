using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Web;
using System;

[CustomEditor(typeof(ItemWeaponPushData))]
public class ItemWeaponPushDataEditor : ItemDataEditor
{
    protected new ItemWeaponPushData Source { get { return (ItemWeaponPushData)source; } }
    protected SerializedProperty affectedMask;
    protected SerializedProperty obstacleMask;
    protected SerializedProperty force;
    protected SerializedProperty upwardForce;
    protected SerializedProperty radius;
    protected SerializedProperty setAngle;
    protected SerializedProperty angle;
    protected SerializedProperty fallOffCurve;


    protected override void OnEnable()
    {
        base.OnEnable();
        SetLinkedType<ItemWeaponPush>();
    }

    protected override void GetProperties()
    {
        base.GetProperties();
        affectedMask = sourceRef.FindProperty("affectedMask");
        obstacleMask = sourceRef.FindProperty("obstacleMask");
        force = sourceRef.FindProperty("force");
        upwardForce = sourceRef.FindProperty("upwardForce");
        radius = sourceRef.FindProperty("radius");
        setAngle = sourceRef.FindProperty("setAngle");
        angle = sourceRef.FindProperty("angle");
        fallOffCurve = sourceRef.FindProperty("fallOffCurve");

    }

    protected override void SetProperties()
    {
        base.SetProperties();

        DisplayPushProperties();
    }

    protected virtual void DisplayPushProperties()
    {
        EditorGUILayout.PropertyField(affectedMask);
        EditorGUILayout.PropertyField(obstacleMask);
        EditorGUILayout.PropertyField(force);
        EditorGUILayout.PropertyField(upwardForce);
        EditorGUILayout.PropertyField(radius);
        EditorGUILayout.PropertyField(setAngle);
        if (setAngle.boolValue)
            angle.FloatFieldClamp(0, 360);
        EditorGUILayout.PropertyField(fallOffCurve);
    }

}
