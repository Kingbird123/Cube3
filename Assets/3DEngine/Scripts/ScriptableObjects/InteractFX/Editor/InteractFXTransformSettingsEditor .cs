using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor (typeof (InteractFXTransformSettings))]
public class InteractFXTransformSettingsEditor : InteractFXDynamicEditor
{
    protected new InteractFXPhysicsSettings Source { get { return (InteractFXPhysicsSettings)source; } }

    private SerializedProperty maskInd;

    private SerializedProperty setPositionTo;
    private SerializedProperty positionVector;
    private SerializedProperty positionObj;

    private SerializedProperty setRotationTo;
    private SerializedProperty rotationVector;
    private SerializedProperty rotationObj;

    private SerializedProperty setScaleTo;
    private SerializedProperty scaleVector;
    private SerializedProperty scaleObj;

    protected override void GetProperties()
    {
        base.GetProperties();
        maskInd = sourceRef.FindProperty("maskInd");

        setPositionTo = sourceRef.FindProperty("setPositionTo");
        positionVector = sourceRef.FindProperty("positionVector");
        positionObj = sourceRef.FindProperty("positionObj");

        setRotationTo = sourceRef.FindProperty("setRotationTo");
        rotationVector = sourceRef.FindProperty("rotationVector");
        rotationObj = sourceRef.FindProperty("rotationObj");

        setScaleTo = sourceRef.FindProperty("setScaleTo");
        scaleVector = sourceRef.FindProperty("scaleVector");
        scaleObj = sourceRef.FindProperty("scaleObj");


    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorExtensions.LabelFieldCustom("Transform Settings", FontStyle.Bold);

        maskInd.intValue = EditorGUILayout.MaskField("Transform Options", maskInd.intValue, System.Enum.GetNames(typeof(InteractFXTransformSettings.TransformSettings)));

        //position
        if (maskInd.intValue == (maskInd.intValue | (1 << 0)))
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(setPositionTo);
            if (setPositionTo.enumValueIndex == 0)
                EditorGUILayout.PropertyField(positionVector);
            else
                EditorGUILayout.PropertyField(positionObj);
        }
        //rotation
        if (maskInd.intValue == (maskInd.intValue | (1 << 1)))
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(setRotationTo);
            if (setRotationTo.enumValueIndex == 0)
                EditorGUILayout.PropertyField(rotationVector);
            else
                EditorGUILayout.PropertyField(rotationObj);
        }
        //scale
        if (maskInd.intValue == (maskInd.intValue | (1 << 2)))
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(setScaleTo);
            if (setScaleTo.enumValueIndex == 0)
                EditorGUILayout.PropertyField(scaleVector);
            else
                EditorGUILayout.PropertyField(scaleObj);
        }
            


    }
}