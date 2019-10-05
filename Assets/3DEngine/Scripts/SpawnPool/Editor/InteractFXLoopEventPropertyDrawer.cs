using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer(typeof(InteractFXLoopEvent))]
public class InteractFXLoopEventPropertyDrawer : PropertyDrawerCustom
{
    private SerializedProperty interactFXLoops;

    private int reorderableFieldAmount;
    private ReorderableList eventList;
    private float fieldSize;

    private bool initialized;

    protected override void GetProperties(SerializedProperty _property)
    {
        base.GetProperties(_property);
        //get property values
        interactFXLoops = _property.FindPropertyRelative("interactFXLoops");
    }

    protected override void SetFieldAmount()
    {
        if (!initialized)
            Initialize(sourceRef);

        if (eventList.serializedProperty.arraySize > 0)
        {
            fieldAmount = 3;
            for (int i = 0; i < eventList.serializedProperty.arraySize; i++)
            {
                var element = eventList.serializedProperty.GetArrayElementAtIndex(i);
                var currentFieldAmount = element.FindPropertyRelative("currentFieldAmount");
                fieldAmount += currentFieldAmount.intValue;
            }
        }
        else
        {
            fieldAmount = 4;
        }
    }

    protected override void OnGUICustom(Rect position, SerializedProperty property, GUIContent label, bool _prefixLabel = false, int _indentLevel = 0)
    {
        base.OnGUICustom(position, property, GUIContent.none, _prefixLabel, _indentLevel);
    }

    private void Initialize(SerializedProperty property)
    {
        initialized = true;
        fieldSize = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        //set up lists and cache data and whatnot
        eventList = new ReorderableList(property.serializedObject, interactFXLoops, true, true, true, true);
        eventList.drawHeaderCallback = (Rect _position) =>
        {
            EditorGUI.LabelField(_position, "Loops");
        };

    }

    protected override void DisplayGUIElements(Rect position, SerializedProperty property)
    {
        EditorGUI.BeginChangeCheck();

        eventList.elementHeightCallback = (index) =>
        {
            float height = 0;
            var element = eventList.serializedProperty.GetArrayElementAtIndex(index);
            var currentFieldAmount = element.FindPropertyRelative("currentFieldAmount");
            height += currentFieldAmount.intValue;

            return fieldSize * height;
        };

        eventList.drawElementCallback = (Rect pos, int index, bool isActive, bool isFocused) =>
        {
            var element = eventList.serializedProperty.GetArrayElementAtIndex(index);

            var animToPlay = element.FindPropertyRelative("animToPlay");
            var method = element.FindPropertyRelative("method");
            var interacts = element.FindPropertyRelative("interacts");
            var delay = element.FindPropertyRelative("delay");
            var repeat = element.FindPropertyRelative("repeat");
            var repeatDelay = element.FindPropertyRelative("repeatDelay");
            var totalTime = element.FindPropertyRelative("totalTime");
            var currentFieldAmount = element.FindPropertyRelative("currentFieldAmount");

            pos.height = EditorGUIUtility.singleLineHeight;

            currentFieldAmount.intValue = 1;

            //anim
            EditorGUI.PropertyField(pos, animToPlay);

            int methodFieldAmount;
            method.MethodPropertyField(pos, 1, out pos, out methodFieldAmount);
            currentFieldAmount.intValue += methodFieldAmount;

            currentFieldAmount.intValue++;
            //interacts
            pos.y += fieldSize;
            EditorGUI.PropertyField(pos, interacts, true);

            if (interacts.isExpanded)
            {
                currentFieldAmount.intValue++;
                pos.y += fieldSize;
                foreach (var item in interacts)
                {
                    currentFieldAmount.intValue++;
                    pos.y += fieldSize;
                }
                    
            }

            //delay
            currentFieldAmount.intValue++;
            pos.y += fieldSize;
            EditorGUI.PropertyField(pos, delay);

            //repeat
            currentFieldAmount.intValue++;
            pos.y += fieldSize;
            EditorGUI.PropertyField(pos, repeat);

            //repeat delay
            if (repeat.boolValue)
            {
                currentFieldAmount.intValue++;
                pos.y += fieldSize;
                EditorGUI.PropertyField(pos, repeatDelay);
            }

            //totalTime
            currentFieldAmount.intValue++;
            pos.y += fieldSize;
            EditorGUI.PropertyField(pos, totalTime);
        };

        eventList.DoList(position);

        if (EditorGUI.EndChangeCheck())
        {
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}