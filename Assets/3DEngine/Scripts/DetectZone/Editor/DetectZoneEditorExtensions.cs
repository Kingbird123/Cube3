using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public static class DetectZoneEditorExtensions
{

    public static void DetectZoneField(this SerializedProperty _detectZoneProperty, bool _foldoutProperty, bool _indentProperty, int _index = 0)
    {
        //get property values
        var overrideZoneName = _detectZoneProperty.FindPropertyRelative("overrideZoneName");
        var overrideLabel = _detectZoneProperty.FindPropertyRelative("overrideLabel");
        var zoneName = _detectZoneProperty.FindPropertyRelative("zoneName");
        var overrideDetectMask = _detectZoneProperty.FindPropertyRelative("overrideDetectMask");
        var detectMask = _detectZoneProperty.FindPropertyRelative("detectMask");
        var ignoreTriggerColliders = _detectZoneProperty.FindPropertyRelative("ignoreTriggerColliders");
        var overrideDetectType = _detectZoneProperty.FindPropertyRelative("overrideDetectType");
        var detectType = _detectZoneProperty.FindPropertyRelative("detectType");
        var colliderToUse = _detectZoneProperty.FindPropertyRelative("colliderToUse");
        var overridePositionType = _detectZoneProperty.FindPropertyRelative("overridePositionType");
        var positionType = _detectZoneProperty.FindPropertyRelative("positionType");
        var trans = _detectZoneProperty.FindPropertyRelative("trans");
        var worldPos = _detectZoneProperty.FindPropertyRelative("worldPos");
        var offset = _detectZoneProperty.FindPropertyRelative("offset");
        var size = _detectZoneProperty.FindPropertyRelative("size");
        var useTransformAngle = _detectZoneProperty.FindPropertyRelative("useTransformAngle");
        var angle = _detectZoneProperty.FindPropertyRelative("angle");
        var radius = _detectZoneProperty.FindPropertyRelative("radius");
        var height = _detectZoneProperty.FindPropertyRelative("height");
        var overrideColor = _detectZoneProperty.FindPropertyRelative("overrideColor");
        var debugColor = _detectZoneProperty.FindPropertyRelative("debugColor");

        if (_foldoutProperty)
            _detectZoneProperty.isExpanded = EditorGUILayout.Foldout(_detectZoneProperty.isExpanded, _detectZoneProperty.displayName);
        else
            _detectZoneProperty.isExpanded = true;

        if (!_detectZoneProperty.isExpanded)
            return;

        if (_indentProperty)
            EditorGUI.indentLevel++;

        if (!overrideZoneName.boolValue)
        {
            EditorGUILayout.PropertyField(zoneName);
            if (zoneName.stringValue == "")
                zoneName.stringValue = "Detect Zone " + _index;
        }


        if (!overrideDetectMask.boolValue)
            EditorGUILayout.PropertyField(detectMask);

        EditorGUILayout.PropertyField(ignoreTriggerColliders);

        if (!overrideDetectType.boolValue)
            EditorGUILayout.PropertyField(detectType);

        if (detectType.enumValueIndex == 4 || detectType.enumValueIndex == 5)
            EditorGUILayout.PropertyField(colliderToUse);

        if (!overridePositionType.boolValue)
            EditorGUILayout.PropertyField(positionType);

        //Local position type
        if (positionType.enumValueIndex == 1)
            EditorGUILayout.PropertyField(trans);
        //worldPos
        if (positionType.enumValueIndex == 2)
            EditorGUILayout.PropertyField(worldPos);

        //offset
        EditorGUILayout.PropertyField(offset);

        //use transform angle
        if (positionType.enumValueIndex != 2 && detectType.enumValueIndex != 3)
            EditorGUILayout.PropertyField(useTransformAngle);
        else
            useTransformAngle.boolValue = false;

        //size
        if (detectType.enumValueIndex == 1)
            EditorGUILayout.PropertyField(size);
        //angle
        if (!useTransformAngle.boolValue && detectType.enumValueIndex != 3)
            EditorGUILayout.PropertyField(angle);
        //radius
        if (detectType.enumValueIndex == 0 || detectType.enumValueIndex == 2)
            EditorGUILayout.PropertyField(radius);
        //height
        if (detectType.enumValueIndex == 2)
        {
            //clamp height value to radius so we avoid glitches
            EditorGUILayout.PropertyField(height);
            height.floatValue = Mathf.Clamp(height.floatValue, radius.floatValue * 2, Mathf.Infinity);
        }
        //color field
        if (detectType.enumValueIndex != 3 && !overrideColor.boolValue)
            EditorGUILayout.PropertyField(debugColor);

        if (_indentProperty)
            EditorGUI.indentLevel--;
    }

    public static void DrawDetectZone(SerializedProperty _detectZoneProperty, Transform _sourceTrans = null)
    {
        var worldPos = _detectZoneProperty.FindPropertyRelative("worldPos");
        var detectType = _detectZoneProperty.FindPropertyRelative("detectType");
        var offset = _detectZoneProperty.FindPropertyRelative("offset");
        var angle = _detectZoneProperty.FindPropertyRelative("angle");
        var size = _detectZoneProperty.FindPropertyRelative("size");
        var radius = _detectZoneProperty.FindPropertyRelative("radius");
        var height = _detectZoneProperty.FindPropertyRelative("height");
        var positionType = _detectZoneProperty.FindPropertyRelative("positionType");
        var trans = _detectZoneProperty.FindPropertyRelative("trans");
        var transRoot = trans.objectReferenceValue as Transform;
        var useTransformAngle = _detectZoneProperty.FindPropertyRelative("useTransformAngle");
        var debugColor = _detectZoneProperty.FindPropertyRelative("debugColor");


        var pos = offset.vector3Value;
        if (positionType.enumValueIndex == (int)DetectZone.PositionType.World)
        {
            worldPos.vector3Value = Handles.PositionHandle(worldPos.vector3Value, Quaternion.Euler(angle.vector3Value));

            //get final position
            pos = worldPos.vector3Value + offset.vector3Value;
        }
        else if (positionType.enumValueIndex == (int)DetectZone.PositionType.Local && trans.objectReferenceValue)
        {

            pos = transRoot.TransformPoint(offset.vector3Value);
        }
        else if (positionType.enumValueIndex == (int)DetectZone.PositionType.Offset && _sourceTrans)
            pos = _sourceTrans.TransformPoint(offset.vector3Value);

        if (useTransformAngle.boolValue)
        {
            if (positionType.enumValueIndex == (int)DetectZone.PositionType.Local && trans.objectReferenceValue)
                angle.vector3Value = transRoot.eulerAngles;
            else if (positionType.enumValueIndex == (int)DetectZone.PositionType.Offset && _sourceTrans)
                angle.vector3Value = _sourceTrans.eulerAngles;
        }

        var rot = Quaternion.Euler(angle.vector3Value);
        var col = debugColor.colorValue;

        Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
        //draw the objects
        if (detectType.enumValueIndex == (int)DetectZone.DetectAreaType.Box)
            EditorExtensions.DrawWireCube(pos, rot, size.vector3Value, col);
        else if (detectType.enumValueIndex == (int)DetectZone.DetectAreaType.Sphere)
            EditorExtensions.DrawWireSphere(pos, rot, radius.floatValue, col);
        else if (detectType.enumValueIndex == (int)DetectZone.DetectAreaType.Capsule)
            EditorExtensions.DrawWireCapsule(pos, rot, radius.floatValue, height.floatValue, col);

        _detectZoneProperty.serializedObject.ApplyModifiedProperties();
    }

}