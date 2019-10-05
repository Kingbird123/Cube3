using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DetectZone))]
public class DetectZonePropertyDrawer : PropertyDrawer
{
    private SerializedProperty sourceRef;

    private SerializedProperty overrideZoneName;
    private SerializedProperty overrideLabel;
    private SerializedProperty zoneName;
    private SerializedProperty overrideDetectMask;
    private SerializedProperty detectMask;
    private SerializedProperty overrideDetectType;
    private SerializedProperty detectType;
    private SerializedProperty colliderToUse;
    private SerializedProperty overridePositionType;
    private SerializedProperty positionType;
    private SerializedProperty trans;
    private SerializedProperty worldPos;
    private SerializedProperty offset;
    private SerializedProperty size;
    private SerializedProperty useTransformAngle;
    private SerializedProperty angle;
    private SerializedProperty radius;
    private SerializedProperty height;
    private SerializedProperty overrideColor;
    private SerializedProperty debugColor;

    //need to set field amount manually if you add more fields
    private int fieldAmount = 2;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        sourceRef = property;
        GetProperties();
        CalculateFieldAmount();
        //set the height of the drawer by the field size and padding
        return (fieldAmount * EditorGUIUtility.singleLineHeight) + (fieldAmount * EditorGUIUtility.standardVerticalSpacing);
    }

    void CalculateFieldAmount()
    {
        fieldAmount = 0;
        if (!overrideLabel.boolValue)
            fieldAmount++;

        if (!overrideZoneName.boolValue)
            fieldAmount++;

        if (!overrideDetectMask.boolValue)
            fieldAmount++;
        
        //detect type
        if (!overrideDetectType.boolValue)
            fieldAmount++;

        //collider
        if (detectType.enumValueIndex == 4 || detectType.enumValueIndex == 5)
        {
            fieldAmount++;
            return;
        }
            
        //position type
        if (!overridePositionType.boolValue)
            fieldAmount++;

        //offset
        if (positionType.enumValueIndex == 0)
            fieldAmount++;
        //trans + offset
        else if (positionType.enumValueIndex == 1)
            fieldAmount += 2;
        //worldPos + offset
        else if (positionType.enumValueIndex == 2)
            fieldAmount += 2;

        //use transform angle
        if (positionType.enumValueIndex != 2 && detectType.enumValueIndex != 3)
            fieldAmount++;

        //size
        if (detectType.enumValueIndex == 1)
            fieldAmount++;

        //angle
        if (!useTransformAngle.boolValue && detectType.enumValueIndex != 3)
            fieldAmount++;
        //radius
        if (detectType.enumValueIndex == 0 || detectType.enumValueIndex == 2)
            fieldAmount++;
        //height
        if (detectType.enumValueIndex == 2)
            fieldAmount++;
        //color field
        if (detectType.enumValueIndex != 3 && !overrideColor.boolValue)
            fieldAmount++;
    }

    public virtual void GetProperties()
    {
        //get property values
        overrideZoneName = sourceRef.FindPropertyRelative("overrideZoneName");
        overrideLabel = sourceRef.FindPropertyRelative("overrideLabel");
        zoneName = sourceRef.FindPropertyRelative("zoneName");
        overrideDetectMask = sourceRef.FindPropertyRelative("overrideDetectMask");
        detectMask = sourceRef.FindPropertyRelative("detectMask");
        overrideDetectType = sourceRef.FindPropertyRelative("overrideDetectType");
        detectType = sourceRef.FindPropertyRelative("detectType");
        colliderToUse = sourceRef.FindPropertyRelative("colliderToUse");
        overridePositionType = sourceRef.FindPropertyRelative("overridePositionType");
        positionType = sourceRef.FindPropertyRelative("positionType");
        trans = sourceRef.FindPropertyRelative("trans");
        worldPos = sourceRef.FindPropertyRelative("worldPos");
        offset = sourceRef.FindPropertyRelative("offset");
        size = sourceRef.FindPropertyRelative("size");
        useTransformAngle = sourceRef.FindPropertyRelative("useTransformAngle");
        angle = sourceRef.FindPropertyRelative("angle");
        radius = sourceRef.FindPropertyRelative("radius");
        height = sourceRef.FindPropertyRelative("height");
        overrideColor = sourceRef.FindPropertyRelative("overrideColor");
        debugColor = sourceRef.FindPropertyRelative("debugColor");
    }


    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        //divide all field heights by the field amount..then minus the padding
        position.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        // Draw Prefix label...this will push all other content to the right
        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Draw non-indented label instead
        if (!overrideLabel.boolValue)
            EditorGUI.LabelField(position, property.displayName);

        // Get the start indent level
        var indent = EditorGUI.indentLevel;
        // Set indent amount
        if (!overrideLabel.boolValue)
            EditorGUI.indentLevel = indent + 1;

        DisplayGUIElements(position, property, label);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    public virtual void DisplayGUIElements(Rect position, SerializedProperty property, GUIContent label)
    {

        if (!overrideLabel.boolValue)
            //offset position.y by field size
            position.y += position.height;

        if (!overrideZoneName.boolValue)
        {
            EditorGUI.PropertyField(position, zoneName);
        }
        else
            position.y -= position.height;

        if (!overrideDetectMask.boolValue)
        {
            //offset position.y by field size
            position.y += position.height;
            //first field
            EditorGUI.PropertyField(position, detectMask);
        }

        if (!overrideDetectType.boolValue)
        {
            //detect type
            position.y += position.height;
            EditorGUI.PropertyField(position, detectType);
        }

        //collider
        if (detectType.enumValueIndex == 4 || detectType.enumValueIndex == 5)
        {
            //detect type
            position.y += position.height;
            EditorGUI.PropertyField(position, colliderToUse);
            return;
        }

        if (!overridePositionType.boolValue)
        {
            //position type
            position.y += position.height;
            EditorGUI.PropertyField(position, positionType);
        }

        //Local position type
        if (positionType.enumValueIndex == 1)
        {
            position.y += position.height;
            EditorGUI.PropertyField(position, trans);
        }
        //worldPos
        if (positionType.enumValueIndex == 2)
        {
            position.y += position.height;
            EditorGUI.PropertyField(position, worldPos);
        }

        //offset
        position.y += position.height;
        EditorGUI.PropertyField(position, offset);

        //use transform angle
        if (positionType.enumValueIndex != 2 && detectType.enumValueIndex != 3)
        {
            position.y += position.height;
            EditorGUI.PropertyField(position, useTransformAngle);
        }
        else
            useTransformAngle.boolValue = false;

        //size
        if (detectType.enumValueIndex == 1)
        {
            position.y += position.height;
            EditorGUI.PropertyField(position, size);
        }
        //angle
        if (!useTransformAngle.boolValue && detectType.enumValueIndex != 3)
        {
            position.y += position.height;
            EditorGUI.PropertyField(position, angle);
        }
        //radius
        if (detectType.enumValueIndex == 0 || detectType.enumValueIndex == 2)
        {
            position.y += position.height;
            EditorGUI.PropertyField(position, radius);
        }
        //height
        if (detectType.enumValueIndex == 2)
        {
            position.y += position.height;
            //clamp height value to radius so we avoid glitches
            EditorGUI.PropertyField(position, height);
            height.floatValue = Mathf.Clamp(height.floatValue, radius.floatValue * 2, Mathf.Infinity);
        }
        //color field
        if (detectType.enumValueIndex != 3 && !overrideColor.boolValue)
        {
            position.y += position.height;
            EditorGUI.PropertyField(position, debugColor);
        }

    }

}