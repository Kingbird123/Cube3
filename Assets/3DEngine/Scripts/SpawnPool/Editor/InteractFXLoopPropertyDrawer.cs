using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(InteractFXLoop))]
public class InteractFXLoopPropertyDrawer : PropertyDrawerCustom
{
    private SerializedProperty animToPlay;
    private SerializedProperty method;
    private SerializedProperty interacts;
    private SerializedProperty delay;
    private SerializedProperty repeat;
    private SerializedProperty repeatDelay;
    private SerializedProperty totalTime;
    private SerializedProperty currentFieldAmount;

    private int methodFieldAmount;

    protected override void GetProperties(SerializedProperty _property)
    {
        base.GetProperties(_property);
        //get property values
        animToPlay = sourceRef.FindPropertyRelative("animToPlay");
        method = sourceRef.FindPropertyRelative("method");
        interacts = sourceRef.FindPropertyRelative("interacts");
        delay = sourceRef.FindPropertyRelative("delay");
        repeat = sourceRef.FindPropertyRelative("repeat");
        repeatDelay = sourceRef.FindPropertyRelative("repeatDelay");
        totalTime = sourceRef.FindPropertyRelative("totalTime");
        currentFieldAmount = sourceRef.FindPropertyRelative("currentFieldAmount");
    }

    protected override void SetFieldAmount()
    {
        fieldAmount = 4 + methodFieldAmount;
        if (interacts.isExpanded)
        {
            fieldAmount++;
            foreach (var item in interacts)
                fieldAmount++;
        }
        if (repeat.boolValue)
            fieldAmount++;

        currentFieldAmount.intValue = fieldAmount;
    }

    protected override void OnGUICustom(Rect position, SerializedProperty property, GUIContent label, bool _prefixLabel = false, int _indentLevel = 0)
    {
        base.OnGUICustom(position, property, label, _prefixLabel, _indentLevel);
    }

    protected override void DisplayGUIElements(Rect position, SerializedProperty prop)
    {
        if (animToPlay == null)
            GetProperties(prop);

        //anim
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(position, animToPlay);

        //method
        method.MethodPropertyField(position, 1, out position, out methodFieldAmount);

        //interacts
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(position, interacts, true);

        if (interacts.isExpanded)
        {
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            foreach (var item in interacts)
                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        //delay
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(position, delay);

        //repeat
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(position, repeat);

        //repeat delay
        if (repeat.boolValue)
        {
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(position, repeatDelay);
        }

        //totalTime
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(position, totalTime);

        
    }

}