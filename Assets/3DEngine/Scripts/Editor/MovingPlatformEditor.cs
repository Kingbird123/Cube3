using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CanEditMultipleObjects]
[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : Editor
{

    private MovingPlatform source;
    private SerializedObject sourceRef;

    private SerializedProperty travelType;
    private SerializedProperty speed;
    private SerializedProperty arrivalStopTime;
    private SerializedProperty moveType;
    private SerializedProperty travelTime;
    private SerializedProperty points;
    private SerializedProperty loopType;

    private SerializedProperty lineColor;
    private SerializedProperty fontColor;

    private Vector3[] handlePoints = new Vector3[0];

    private GUIStyle header = new GUIStyle();
    private GUIStyle label = new GUIStyle();

    private float firstPosX;
    private float firstPosY;

    private void OnEnable()
    {
        source = (MovingPlatform)target;
        sourceRef = serializedObject;

        travelType = sourceRef.FindProperty("travelType");
        speed = sourceRef.FindProperty("speed");
        arrivalStopTime = sourceRef.FindProperty("arrivalStopTime");
        moveType = sourceRef.FindProperty("moveType");
        loopType = sourceRef.FindProperty("loopType");
        travelTime = sourceRef.FindProperty("travelTime");
        points = sourceRef.FindProperty("points");
        lineColor = sourceRef.FindProperty("lineColor");
        fontColor = sourceRef.FindProperty("fontColor");

        SetGUIStyles();
        SetPlatformStartPos();

    }

    public override void OnInspectorGUI()
    {

        EditorGUILayout.PropertyField(travelType);

        if (travelType.enumValueIndex == 0)
            EditorGUILayout.PropertyField(travelTime);
        else
            EditorGUILayout.PropertyField(speed);

        EditorGUILayout.PropertyField(moveType);
        EditorGUILayout.PropertyField(arrivalStopTime);
        EditorGUILayout.PropertyField(points,true);
        EditorGUILayout.PropertyField(loopType);

        EditorGUILayout.LabelField("Debug", header);
        EditorGUILayout.PropertyField(lineColor);
        if (lineColor.colorValue == Color.clear)
            lineColor.colorValue = Color.cyan;
        EditorGUILayout.PropertyField(fontColor);
        if (fontColor.colorValue == Color.clear)
            fontColor.colorValue = Color.white;


        ForceScaleToOne();
        sourceRef.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        if (Selection.activeGameObject != source.gameObject)
            return;

        DrawPlatformPoints();
        DrawTravelLines();
    }

    void ForceScaleToOne()
    {
        if (source.transform.localScale != Vector3.one)
        {
            Debug.Log("Cannot change scale on a moving platform root! Change collider shape instead");
            source.transform.localScale = Vector3.one;
        }
    }

    void DrawPlatformPoints()
    {
        if (source.points.Count != handlePoints.Length)
        {
            handlePoints = new Vector3[source.points.Count];
        }

        for (int i = 0; i < source.points.Count; i++)
        {
            EditorGUI.BeginChangeCheck();

            //set platform to start position
            SetPlatformStartPos();

            //set Initial positions for other points so they don't all start at zero zero
            if (i > 0 && source.points[i] == Vector3.zero)
            {
                if (i > 1)
                {
                    firstPosX = source.points[i - 1].x + 2;
                    firstPosY = source.points[i - 1].y;
                }
                else
                {
                    firstPosX = source.transform.position.x + 2;
                    firstPosY = source.transform.position.y;
                }
                source.points[i] = new Vector3(firstPosX, firstPosY);
            }
                

            if (handlePoints[i] != source.points[i])
            {
                handlePoints[i] = Handles.PositionHandle(source.points[i], Quaternion.identity);
            }    
            else
                handlePoints[i] = Handles.PositionHandle(handlePoints[i], Quaternion.identity);

            if (EditorGUI.EndChangeCheck())//update script values after dragging
            {
                //need to do this to keep changes to prefabs :/ ... or else the values will revert on play
                Undo.RecordObject(source, "Modified " + source + " properties.");
                source.points[i] = handlePoints[i];
            }
                

        }
    }

    void DrawTravelLines()
    {
        if (source.points.Count < 1)
            return;

        SetHandleValues();

        for (int i = 0; i < source.points.Count; i++)
        {
            //draw lines
            if (i > 1 && i == source.points.Count - 1 && loopType.enumValueIndex == 0)
                Handles.DrawDottedLine(source.points[i], source.points[0], 5);
            else if (i < source.points.Count - 1)
                Handles.DrawDottedLine(source.points[i], source.points[i + 1], 5);

            //draw labels
            label.normal.textColor = fontColor.colorValue;
            Handles.Label(source.points[i], "Point " + i, label);

            //draw discs
            Handles.DrawSolidDisc(source.points[i], Vector3.back, 0.15f);

        }

    }

    void SetHandleValues()
    {
        Handles.color = lineColor.colorValue;
    }

    void SetGUIStyles()
    {
        //header
        header.fontStyle = FontStyle.Bold;
        //label
        label.fontStyle = FontStyle.Bold;
    }

    void SetPlatformStartPos()
    {
        if (source.points.Count > 0)
        {
            if (source.points[0] == Vector3.zero && source.transform.position != Vector3.zero)
                source.points[0] = source.transform.position;
            else if (source.transform.position != source.points[0] && !Application.isPlaying)
            {
                if (Tools.current == Tool.Move)
                {
                    source.points[0] = source.transform.position;
                }
                else
                    source.transform.position = source.points[0];
            }
                
        }
    }
}
