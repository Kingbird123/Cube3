using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ItemWeaponMelee))]
public class ItemWeaponMeleeEditor : ItemEditor
{

    protected SerializedProperty useButton;
    protected SerializedProperty detectZone;
    protected SerializedProperty activeColor;
    protected SerializedProperty idleColor;

    protected SerializedProperty overrideColor;
    protected SerializedProperty debugColor;


    protected override void GetProperties()
    {
        base.GetProperties();
        useButton = sourceRef.FindProperty("useButton");
        detectZone = sourceRef.FindProperty("detectZone");
        overrideColor = detectZone.FindPropertyRelative("overrideColor");
        debugColor = detectZone.FindPropertyRelative("debugColor");

        activeColor = sourceRef.FindProperty("activeColor");
        idleColor = sourceRef.FindProperty("idleColor");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorGUILayout.PropertyField(useButton);
        EditorGUILayout.PropertyField(detectZone);
        overrideColor.boolValue = true;
        if (!Application.isPlaying)
            debugColor.colorValue = idleColor.colorValue;
        EditorGUILayout.PropertyField(idleColor);
        EditorGUILayout.PropertyField(activeColor);
        
    }

    private void OnSceneGUI()
    {
        var zone = detectZone.GetRootValue<DetectZone>();
        zone.DrawDetectZone(source, sourceRef, source.transform);
    }

}
