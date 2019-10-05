using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CanEditMultipleObjects]
[CustomEditor(typeof(PatrolData))]
public class PatrolDataEditor : Editor
{

    private PatrolData source;
    private SerializedObject sourceRef;

    private SerializedProperty patrolPoints;


    private void OnEnable()
    {
        source = (PatrolData)target;
        sourceRef = serializedObject;

        SceneView.duringSceneGui += OnSceneGUI;

        GetProperties();
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    public override void OnInspectorGUI()
    {
        SetProperties();

        sourceRef.ApplyModifiedProperties();
    }

    void GetProperties()
    {
        patrolPoints = sourceRef.FindProperty("patrolPoints");
    }

    void SetProperties()
    {
        EditorGUILayout.PropertyField(patrolPoints, true);
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        DrawPoints();
        SceneView.RepaintAll();
    }

    void DrawPoints()
    {
        if (patrolPoints == null)
            return;
        if (patrolPoints.arraySize < 1)
            return;

        //label style
        var style = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.LowerCenter,
            normal = new GUIStyleState
            {
                textColor = Color.black,
            }
        };

        for (int i = 0; i < patrolPoints.arraySize; i++)
        {
            var ele = patrolPoints.GetArrayElementAtIndex(i);
            var position = ele.FindPropertyRelative("position");
            var euler = ele.FindPropertyRelative("euler");

            //position handles
            EditorGUI.BeginChangeCheck();
            position.vector3Value = Handles.PositionHandle(position.vector3Value, Quaternion.Euler(euler.vector3Value));

            if (EditorGUI.EndChangeCheck())
                sourceRef.ApplyModifiedProperties();

            Handles.Label(position.vector3Value, "point " + i, style);
        }

    }

}
