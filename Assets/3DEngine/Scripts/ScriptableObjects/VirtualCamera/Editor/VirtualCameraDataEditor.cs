using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;

[CanEditMultipleObjects]
[CustomEditor(typeof(VirtualCameraData))]
public class VirtualCameraDataEditor : Editor
{
    protected SerializedObject sourceRef;
    protected VirtualCameraData source;

    protected SerializedProperty virtualCameraPrefab;
    protected SerializedProperty findTargetOnEnable;
    protected SerializedProperty followAndLookatIdentical;
    protected SerializedProperty setFollowTarget;
    protected SerializedProperty followTarget;
    protected SerializedProperty followOffset;
    protected SerializedProperty setLookAtTarget;
    protected SerializedProperty lookAtTarget;
    protected SerializedProperty lookAtOffset;

    protected virtual void OnEnable()
    {
        source = (VirtualCameraData)target;
        sourceRef = serializedObject;

        GetProperties();
    }

    public override void OnInspectorGUI()
    {
        SetProperties();

        sourceRef.ApplyModifiedProperties();
    }

    protected virtual void GetProperties()
    {
        virtualCameraPrefab = sourceRef.FindProperty("virtualCameraPrefab");
        findTargetOnEnable = sourceRef.FindProperty("findTargetOnEnable");
        followAndLookatIdentical = sourceRef.FindProperty("followAndLookatIdentical");
        setFollowTarget = sourceRef.FindProperty("setFollowTarget");
        followTarget = sourceRef.FindProperty("followTarget");
        followOffset = sourceRef.FindProperty("followOffset");
        setLookAtTarget = sourceRef.FindProperty("setLookAtTarget");
        lookAtTarget = sourceRef.FindProperty("lookAtTarget");
        lookAtOffset = sourceRef.FindProperty("lookAtOffset");      
    }

    protected virtual void SetProperties()
    {
        virtualCameraPrefab.PrefabFieldWithComponent(typeof(CinemachineVirtualCameraBase));
        EditorGUILayout.PropertyField(findTargetOnEnable);
        if (findTargetOnEnable.boolValue)
        {
            EditorGUILayout.PropertyField(setFollowTarget);
            

            if (setFollowTarget.boolValue)
            {
                followTarget.SceneObjectField("Follow");
                EditorGUILayout.PropertyField(followOffset);
            }
                
            EditorGUILayout.PropertyField(setLookAtTarget);
            if (setLookAtTarget.boolValue)
            {
                EditorGUILayout.PropertyField(followAndLookatIdentical);
                if (!followAndLookatIdentical.boolValue)
                {
                    lookAtTarget.SceneObjectField("Target");
                    
                }
                EditorGUILayout.PropertyField(lookAtOffset);

            }
        }
    }
}
