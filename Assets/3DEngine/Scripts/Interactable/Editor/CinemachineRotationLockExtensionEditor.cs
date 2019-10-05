using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(CinemachineRotationLockExtension))]
public class CinemachineRotationLockExtensionEditor : Editor
{
    protected CinemachineRotationLockExtension source;
    protected SerializedObject sourceRef;
    //skins
    protected SerializedProperty lockTo;
    protected SerializedProperty lockAxisMask;
    protected SerializedProperty rotation;

    protected virtual void OnEnable()
    {
        sourceRef = serializedObject;
        source = (CinemachineRotationLockExtension)target;
        GetProperties();
    }

    public override void OnInspectorGUI()
    { 
        SetProperties();

        sourceRef.ApplyModifiedProperties();
    }

    protected virtual void GetProperties()
    {
        //enemy
        lockTo = sourceRef.FindProperty("lockTo");
        lockAxisMask = sourceRef.FindProperty("lockAxisMask");
        rotation = sourceRef.FindProperty("rotation");
    }

    protected virtual void SetProperties()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(lockTo);
        lockAxisMask.intValue = EditorGUILayout.MaskField("Lock Axis", lockAxisMask.intValue, System.Enum.GetNames(typeof(CinemachineRotationLockExtension.LockAxisType)));
        if (lockTo.enumValueIndex == (int)CinemachineRotationLockExtension.LockToType.Custom)
            EditorGUILayout.PropertyField(rotation);
    }

   
}
