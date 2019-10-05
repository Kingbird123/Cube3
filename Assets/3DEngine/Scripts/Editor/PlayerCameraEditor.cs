using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(PlayerCamera))]
public class PlayerCameraEditor : Editor
{

    private SerializedObject sourceRef;
    private PlayerCamera source;

    //health
    private SerializedProperty autoFindPlayer;
    private SerializedProperty playerTag;
    private SerializedProperty cameraType;
    private SerializedProperty firstPersonOffset;
    private SerializedProperty thirdPersonPivot;
    private SerializedProperty thirdPersonDistance;
    private SerializedProperty scaleDistanceWithPlayer;
    private SerializedProperty clampDistance;
    private SerializedProperty maxDistance;
    private SerializedProperty xRotationLimit;
    private SerializedProperty clampYRotation;
    private SerializedProperty yRotationLimit;
    private SerializedProperty followType;
    private SerializedProperty enableFollowSmoothing;
    private SerializedProperty followSensitivity;
    private SerializedProperty toggleCameraState;
    private SerializedProperty camBumpMask;
    private SerializedProperty bumpDetectPadding;
    private SerializedProperty camBumpSensitivity;
    private SerializedProperty detectPivotCollision;
    private SerializedProperty detectPivotRadius;
    private SerializedProperty pivotBumpType;
    private SerializedProperty pivotBumpOffset;

    private SerializedProperty rayRightPos;
    private SerializedProperty rayLeftPos;
    private SerializedProperty rightHitPoint;
    private SerializedProperty leftHitPoint;

    private SerializedProperty screenPoints;
    private SerializedProperty pivot;

    private void OnEnable()
    {
        sourceRef = serializedObject;
        source = (PlayerCamera)target;

        GetProperties();
    }

    public override void OnInspectorGUI()
    {
        SetProperties();

        sourceRef.ApplyModifiedProperties();
    }

    void GetProperties()
    {
        autoFindPlayer = sourceRef.FindProperty("autoFindPlayer");
        playerTag = sourceRef.FindProperty("playerTag");
        cameraType = sourceRef.FindProperty("cameraType");
        firstPersonOffset = sourceRef.FindProperty("firstPersonOffset");
        thirdPersonPivot = sourceRef.FindProperty("thirdPersonPivot");
        thirdPersonDistance = sourceRef.FindProperty("thirdPersonDistance");
        scaleDistanceWithPlayer = sourceRef.FindProperty("scaleDistanceWithPlayer");
        clampDistance = sourceRef.FindProperty("clampDistance");
        maxDistance = sourceRef.FindProperty("maxDistance");
        xRotationLimit = sourceRef.FindProperty("xRotationLimit");
        clampYRotation = sourceRef.FindProperty("clampYRotation");
        yRotationLimit = sourceRef.FindProperty("yRotationLimit");
        followType = sourceRef.FindProperty("followType");
        enableFollowSmoothing = sourceRef.FindProperty("enableFollowSmoothing");
        followSensitivity = sourceRef.FindProperty("followSensitivity");
        toggleCameraState = sourceRef.FindProperty("toggleCameraState");
        camBumpMask = sourceRef.FindProperty("camBumpMask");
        bumpDetectPadding = sourceRef.FindProperty("bumpDetectPadding");

        detectPivotCollision = sourceRef.FindProperty("detectPivotCollision");
        detectPivotRadius = sourceRef.FindProperty("detectPivotRadius");
        pivotBumpType = sourceRef.FindProperty("pivotBumpType");
        pivotBumpOffset = sourceRef.FindProperty("pivotBumpOffset");

        camBumpSensitivity = sourceRef.FindProperty("camBumpSensitivity");

        rayRightPos = sourceRef.FindProperty("rayRightPos");
        rayLeftPos = sourceRef.FindProperty("rayLeftPos");
        rightHitPoint = sourceRef.FindProperty("rightHitPoint");
        leftHitPoint = sourceRef.FindProperty("leftHitPoint");

        screenPoints = sourceRef.FindProperty("screenPoints");
        pivot = sourceRef.FindProperty("pivot");
    }

    void SetProperties()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(autoFindPlayer);
        if (autoFindPlayer.boolValue)
            playerTag.stringValue = EditorGUILayout.TagField("Player Tag", playerTag.stringValue);
        EditorGUILayout.PropertyField(cameraType);
        EditorGUILayout.PropertyField(xRotationLimit);
        if (cameraType.enumValueIndex == 2)
            EditorGUILayout.PropertyField(toggleCameraState);
        if (cameraType.enumValueIndex == 0 || cameraType.enumValueIndex == 2)
            EditorGUILayout.PropertyField(firstPersonOffset);
        if (cameraType.enumValueIndex == 1 || cameraType.enumValueIndex == 2)
        {
            EditorGUILayout.PropertyField(thirdPersonPivot);
            EditorGUILayout.PropertyField(thirdPersonDistance);
            EditorGUILayout.PropertyField(scaleDistanceWithPlayer);
            if (scaleDistanceWithPlayer.boolValue)
            {
                EditorGUILayout.PropertyField(clampDistance);
                if (clampDistance.boolValue)
                    EditorGUILayout.PropertyField(maxDistance);
            }
            EditorGUILayout.PropertyField(followType);
            if (followType.enumValueIndex == 1)
            {
                EditorGUILayout.PropertyField(clampYRotation);
                if (clampYRotation.boolValue)
                    EditorGUILayout.PropertyField(yRotationLimit);
            }
                
            EditorGUILayout.PropertyField(enableFollowSmoothing);
            if (enableFollowSmoothing.boolValue)
                EditorGUILayout.PropertyField(followSensitivity);
            EditorGUILayout.PropertyField(camBumpMask);
            EditorGUILayout.PropertyField(bumpDetectPadding);

            EditorGUILayout.PropertyField(detectPivotCollision);
            if (detectPivotCollision.boolValue)
            {
                EditorGUILayout.PropertyField(detectPivotRadius);
                EditorGUILayout.PropertyField(pivotBumpType);
                if (pivotBumpType.enumValueIndex == 2)
                    EditorGUILayout.PropertyField(pivotBumpOffset);
            }
            
            EditorGUILayout.PropertyField(camBumpSensitivity);
        }  
        
    }

    private void OnSceneGUI()
    {
        if (!source.pivot)
            return;

        Handles.color = Color.magenta;
        for (int i = 0; i < source.screenPoints.Length; i++)
        {
            
                Handles.DrawLine(source.screenPoints[i], source.pivot.position);
        }

        if (detectPivotCollision.boolValue)
        {
            Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
            EditorExtensions.DrawWireSphere(source.pivot.position, source.pivot.rotation, detectPivotRadius.floatValue, Color.red);
        }
        
    }

}
