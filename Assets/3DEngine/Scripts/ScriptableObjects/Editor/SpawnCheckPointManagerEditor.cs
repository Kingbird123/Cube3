using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CanEditMultipleObjects]
[CustomEditor(typeof(SpawnCheckPointManager))]
public class SpawnCheckPointManagerEditor : Editor
{

    private SpawnCheckPointManager source;
    private SerializedObject sourceRef;

    private SerializedProperty overrideCheckPoint;
    private SerializedProperty checkPoint;
    private SerializedProperty detectMask;
    private SerializedProperty checkPoints;
    //player stuff
    private SerializedProperty playerSpawn;
    //saving stuff
    private SerializedProperty progressOnly;
    private SerializedProperty saveProgressToDisc;
    private SerializedProperty resetProgressOnQuit;
    //level finish
    private SerializedProperty sceneUnlocked;
    private SerializedProperty nextSceneToPlay;
    private SerializedProperty freezeGame;
    private SerializedProperty freezePlayer;
    private SerializedProperty endTime;


    private void OnEnable()
    {
        source = (SpawnCheckPointManager)target;
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
        SetCheckPointNames();

        sourceRef.ApplyModifiedProperties();
    }

    void GetProperties()
    {
        overrideCheckPoint = sourceRef.FindProperty("overrideCheckPoint");
        checkPoint = sourceRef.FindProperty("checkPoint");
        detectMask = sourceRef.FindProperty("detectMask");
        checkPoints = sourceRef.FindProperty("checkPoints");

        //playerstuff
        playerSpawn = sourceRef.FindProperty("playerSpawn");

        //saving
        progressOnly = sourceRef.FindProperty("progressOnly");
        saveProgressToDisc = sourceRef.FindProperty("saveProgressToDisc");
        resetProgressOnQuit = sourceRef.FindProperty("resetProgressOnQuit");

        //level finish
        sceneUnlocked = sourceRef.FindProperty("sceneUnlocked");
        nextSceneToPlay = sourceRef.FindProperty("nextSceneToPlay");
        freezeGame = sourceRef.FindProperty("freezeGame");
        freezePlayer = sourceRef.FindProperty("freezePlayer");
        endTime = sourceRef.FindProperty("endTime");
    }

    void SetProperties()
    {
        EditorGUILayout.LabelField("------------------------");
        EditorGUILayout.LabelField("CheckPoint Options");
        EditorGUILayout.LabelField("------------------------");
        EditorGUILayout.PropertyField(overrideCheckPoint);
        if (overrideCheckPoint.boolValue)
            EditorGUILayout.PropertyField(checkPoint);
        EditorGUILayout.PropertyField(detectMask);
        checkPoints.arraySize = Mathf.Clamp(checkPoints.arraySize, 2, int.MaxValue);
        checkPoints.ArrayFieldButtons("Checkpoint", true, false, true, true, DisplayCheckPoints);
        
        //playerstuff
        EditorGUILayout.LabelField("------------------------");
        EditorGUILayout.LabelField("Player Options");
        EditorGUILayout.LabelField("------------------------");
        EditorGUILayout.PropertyField(playerSpawn);
        //saving
        EditorGUILayout.LabelField("------------------------");
        EditorGUILayout.LabelField("Data Options");
        EditorGUILayout.LabelField("------------------------");
        EditorGUILayout.PropertyField(progressOnly);
        EditorGUILayout.PropertyField(saveProgressToDisc);
        EditorGUILayout.PropertyField(resetProgressOnQuit);
        //level finish
        EditorGUILayout.LabelField("------------------------");
        EditorGUILayout.LabelField("On Level Finish");
        EditorGUILayout.LabelField("------------------------");
        EditorGUILayout.PropertyField(sceneUnlocked);
        EditorGUILayout.PropertyField(nextSceneToPlay);
        EditorGUILayout.PropertyField(freezeGame);
        EditorGUILayout.PropertyField(freezePlayer);
        EditorGUILayout.PropertyField(endTime);

    }

    void DisplayCheckPoints(SerializedProperty _property, int _index)
    {
        //override detect zone properties
        var overrideLabel = _property.FindPropertyRelative("overrideLabel");
        var overrideZoneName = _property.FindPropertyRelative("overrideZoneName");
        var overrideDetectMask = _property.FindPropertyRelative("overrideDetectMask");
        var pointDetectMask = _property.FindPropertyRelative("detectMask");
        var overridePositionType = _property.FindPropertyRelative("overridePositionType");
        var positionType = _property.FindPropertyRelative("positionType");
        var overrideDetectType = _property.FindPropertyRelative("overrideDetectType");
        var detectType = _property.FindPropertyRelative("detectType");

        overrideZoneName.boolValue = true;
        overrideLabel.boolValue = true;
        overrideDetectMask.boolValue = true;
        pointDetectMask.intValue = detectMask.intValue;
        overridePositionType.boolValue = true;
        positionType.enumValueIndex = (int)DetectZone.PositionType.World;
        overrideDetectType.boolValue = true;
        detectType.enumValueIndex = (int)DetectZone.DetectAreaType.Box;

        //display detect zone field
        _property.DetectZoneField(false, false);
    }

    void SetCheckPointNames()
    {
        for (int i = 0; i < checkPoints.arraySize; i++)
        {
            var prop = checkPoints.GetArrayElementAtIndex(i);
            var pointName = prop.FindPropertyRelative("zoneName");
            pointName.stringValue = "Checkpoint " + i;
        }
    }

    private void OnSceneGUI(SceneView _sceneView)
    {
        DrawCheckPoints();
        _sceneView.Repaint();
    }

    void DrawCheckPoints()
    {
        for (int i = 0; i < checkPoints.arraySize; i++)
        {
            var ele = checkPoints.GetArrayElementAtIndex(i);
            var zoneName = ele.FindPropertyRelative("zoneName");
            var worldPos = ele.FindPropertyRelative("worldPos");
            var offset = ele.FindPropertyRelative("offset");
            var pointName = zoneName.stringValue;

            DetectZoneEditorExtensions.DrawDetectZone(ele);

            //drawlabel
            var style = new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.LowerCenter,
                normal = new GUIStyleState
                {
                    textColor = Color.black,
                }
            };
            Handles.Label(worldPos.vector3Value + offset.vector3Value, pointName, style);
        }

    }

    void PositionCamera(Vector3 _pos)
    {
        var sceneview = SceneView.lastActiveSceneView;
        if (sceneview != null)
        {
            var dir =  (_pos - sceneview.pivot).normalized;
            sceneview.rotation = Quaternion.LookRotation(dir);
            sceneview.pivot = _pos + (-dir * 10);
        }

    }



}
