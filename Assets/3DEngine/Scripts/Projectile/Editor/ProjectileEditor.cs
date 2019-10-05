using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Projectile), true)]
public class ProjectileEditor : Editor
{
    protected SerializedObject sourceRef;
    protected Projectile source;
    protected SerializedProperty data;
    protected SerializedProperty detectZone;
    protected DetectZone zone;

    protected virtual void OnEnable()
    {
        sourceRef = serializedObject;
        source = (Projectile)target;

        GetProperties();
    }

    public override void OnInspectorGUI()
    {

        SetProperties();

        sourceRef.ApplyModifiedProperties();
    }

    protected virtual void GetProperties()
    {
        data = sourceRef.FindProperty("data");
        detectZone = sourceRef.FindProperty("detectZone");
    }

    protected virtual void SetProperties()
    {
        EditorGUILayout.Space();
        DisplayDataProperties();
        SetZoneProperties();
        EditorGUILayout.PropertyField(detectZone);
        
    }

    protected virtual void DisplayDataProperties()
    {
        EditorGUILayout.ObjectField(data, typeof(ProjectileData));
    }

    void SetZoneProperties()
    {
        zone = detectZone.GetRootValue<DetectZone>();
        zone.overrideZoneName = true;
        zone.overrideDetectMask = true;
        zone.overridePositionType = true;
        zone.positionType = DetectZone.PositionType.Offset;
    }

    protected virtual void OnSceneGUI()
    {
        if (zone == null)
            return;

        zone.DrawDetectZone(source, sourceRef, source.transform);
    }

}
