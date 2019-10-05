using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InteractFXAddForce))]
public class InteractFXAddForceEditor : InteractFXDynamicEditor
{
    private SerializedProperty directionType;
    private SerializedProperty forceDirection;
    private SerializedProperty forceMode;
    private SerializedProperty direction;
    private SerializedProperty disableUnitSpeed;
    private SerializedProperty disableSpeedTime;
    private SerializedProperty force;
    private SerializedProperty consistent;

    protected override void GetProperties()
    {
        base.GetProperties();
        directionType = sourceRef.FindProperty("directionType");
        forceDirection = sourceRef.FindProperty("forceDirection");
        forceMode = sourceRef.FindProperty("forceMode");
        direction = sourceRef.FindProperty("direction");
        disableUnitSpeed = sourceRef.FindProperty("disableUnitSpeed");
        disableSpeedTime = sourceRef.FindProperty("disableSpeedTime");
        force = sourceRef.FindProperty("force");
        consistent = sourceRef.FindProperty("consistent");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorExtensions.LabelFieldCustom("Add Force Settings", FontStyle.Bold);
        EditorGUILayout.PropertyField(directionType);
        if (directionType.enumValueIndex == (int)InteractFXAddForce.DirectionType.Override)
            EditorGUILayout.PropertyField(direction);
        else if (directionType.enumValueIndex == (int)InteractFXAddForce.DirectionType.ClosestPointAngle)
            EditorGUILayout.PropertyField(forceDirection);

        EditorGUILayout.PropertyField(disableUnitSpeed);
        if (disableUnitSpeed.boolValue)
            EditorGUILayout.PropertyField(disableSpeedTime);

        EditorGUILayout.PropertyField(force);
        EditorGUILayout.PropertyField(consistent);

    }

}
