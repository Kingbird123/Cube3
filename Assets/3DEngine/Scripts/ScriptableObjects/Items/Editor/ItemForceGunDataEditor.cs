using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemForceGunData))]
public class ItemForceGunDataEditor : ItemAimableDataEditor
{
    protected new ItemForceGunData Source { get { return (ItemForceGunData)source; } }
    public SerializedProperty forcePowerUp;
    public SerializedProperty forcePowerDown;
    public SerializedProperty forcePowerHor;
    public SerializedProperty forceType;
    public SerializedProperty forceMode;
    public SerializedProperty speedType;
    public SerializedProperty directionType;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetLinkedType<ItemForceGun>();
    }

    protected override void GetProperties()
    {
        base.GetProperties();
        forcePowerUp = sourceRef.FindProperty("forcePowerUp");
        forcePowerDown = sourceRef.FindProperty("forcePowerDown");
        forcePowerHor = sourceRef.FindProperty("forcePowerHor");
        forceType = sourceRef.FindProperty("forceType");
        forceMode = sourceRef.FindProperty("forceMode");
        speedType = sourceRef.FindProperty("speedType");
        directionType = sourceRef.FindProperty("directionType");


    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorGUILayout.PropertyField(forcePowerUp);
        EditorGUILayout.PropertyField(forcePowerDown);
        EditorGUILayout.PropertyField(forcePowerHor);
        EditorGUILayout.PropertyField(forceType);
        if (forceType.enumValueIndex == 0)
            EditorGUILayout.PropertyField(forceMode);
        EditorGUILayout.PropertyField(speedType);
        EditorGUILayout.PropertyField(directionType);
    }

}
