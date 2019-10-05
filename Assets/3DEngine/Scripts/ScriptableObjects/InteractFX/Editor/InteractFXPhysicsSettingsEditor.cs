using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor (typeof (InteractFXPhysicsSettings))]
public class InteractFXPhysicsSettingsEditor : InteractFXDynamicEditor
{
    protected new InteractFXPhysicsSettings Source { get { return (InteractFXPhysicsSettings)source; } }

    private SerializedProperty maskInd;
    private SerializedProperty mass;
    private SerializedProperty drag;
    private SerializedProperty angularDrag;
    private SerializedProperty useGravity;
    private SerializedProperty isKinematic;
    private SerializedProperty interpolate;
    private SerializedProperty collisionDetection;
    private SerializedProperty constraints;
    private SerializedProperty isTrigger;
    private SerializedProperty physicMaterial;

    protected override void GetProperties()
    {
        base.GetProperties();
        maskInd = sourceRef.FindProperty("maskInd");
        mass = sourceRef.FindProperty("mass");
        drag = sourceRef.FindProperty("drag");
        angularDrag = sourceRef.FindProperty("angularDrag");
        useGravity = sourceRef.FindProperty("useGravity");
        isKinematic = sourceRef.FindProperty("isKinematic");
        interpolate = sourceRef.FindProperty("interpolate");
        collisionDetection = sourceRef.FindProperty("collisionDetection");
        constraints = sourceRef.FindProperty("constraints");
        isTrigger = sourceRef.FindProperty("isTrigger");
        physicMaterial = sourceRef.FindProperty("physicMaterial");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorExtensions.LabelFieldCustom("Physics Settings", FontStyle.Bold);

        maskInd.intValue = EditorGUILayout.MaskField("Physics Options", maskInd.intValue, System.Enum.GetNames(typeof(InteractFXPhysicsSettings.PhysicsSettings)));

        if (maskInd.intValue == (maskInd.intValue | (1 << 0)))
            EditorGUILayout.PropertyField(mass);
        if (maskInd.intValue == (maskInd.intValue | (1 << 1)))
            EditorGUILayout.PropertyField(drag);
        if (maskInd.intValue == (maskInd.intValue | (1 << 2)))
            EditorGUILayout.PropertyField(angularDrag);
        if (maskInd.intValue == (maskInd.intValue | (1 << 3)))
            EditorGUILayout.PropertyField(useGravity);
        if (maskInd.intValue == (maskInd.intValue | (1 << 4)))
            EditorGUILayout.PropertyField(isKinematic);
        if (maskInd.intValue == (maskInd.intValue | (1 << 5)))
            EditorGUILayout.PropertyField(interpolate);
        if (maskInd.intValue == (maskInd.intValue | (1 << 6)))
            EditorGUILayout.PropertyField(collisionDetection);
        if (maskInd.intValue == (maskInd.intValue | (1 << 7)))
            EditorGUILayout.PropertyField(constraints);
        if (maskInd.intValue == (maskInd.intValue | (1 << 8)))
            EditorGUILayout.PropertyField(isTrigger);
        if (maskInd.intValue == (maskInd.intValue | (1 << 9)))
            EditorGUILayout.PropertyField(physicMaterial);


    }
}