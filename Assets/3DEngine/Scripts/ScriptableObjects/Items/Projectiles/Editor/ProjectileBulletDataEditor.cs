using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ProjectileBulletData))]
public class ProjectileBulletDataEditor : ProjectileDataEditor
{

    private SerializedProperty enableRicochet;
    private SerializedProperty cornerDetectRadius;
    private SerializedProperty doImpactOnRicochet;
    private SerializedProperty bounceAngleType;

    protected override void SetLinkedType<T>()
    {
        base.SetLinkedType<ProjectileBullet>();
    }

    protected override void GetProperties()
    {
        base.GetProperties();
        enableRicochet = sourceRef.FindProperty("enableRicochet");
        cornerDetectRadius = sourceRef.FindProperty("cornerDetectRadius");
        doImpactOnRicochet = sourceRef.FindProperty("doImpactOnRicochet");
        bounceAngleType = sourceRef.FindProperty("bounceAngleType");

    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorGUILayout.PropertyField(enableRicochet);
        if (enableRicochet.boolValue)
        {
            EditorGUILayout.PropertyField(cornerDetectRadius);
            EditorGUILayout.PropertyField(doImpactOnRicochet);
            EditorGUILayout.PropertyField(bounceAngleType);

        }
    }

}
