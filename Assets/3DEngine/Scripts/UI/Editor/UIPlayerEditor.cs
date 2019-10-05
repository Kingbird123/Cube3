using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (UIPlayer))]
public class UIPlayerEditor : UIEngineValueEntityEditor
{

    protected SerializedProperty pauseMenu;
    protected SerializedProperty quickMenu;
    protected SerializedProperty defaultAimFX;
    protected SerializedProperty startCursorSettings;
    protected SerializedProperty pauseCursorSettings;

    public override void GetProperties ()
    {
        base.GetProperties();

        pauseMenu = sourceRef.FindProperty ("pauseMenu");
        quickMenu = sourceRef.FindProperty ("quickMenu");
        defaultAimFX = sourceRef.FindProperty ("defaultAimFX");
        startCursorSettings = sourceRef.FindProperty ("startCursorSettings");
        pauseCursorSettings = sourceRef.FindProperty ("pauseCursorSettings");
    }

    public override void SetProperties ()
    {
        base.SetProperties();

        EditorGUILayout.PropertyField (pauseMenu);
        EditorGUILayout.PropertyField (quickMenu);
        EditorGUILayout.PropertyField (defaultAimFX);
        EditorGUILayout.PropertyField (startCursorSettings, true);
        EditorGUILayout.PropertyField (pauseCursorSettings, true);

    }

}