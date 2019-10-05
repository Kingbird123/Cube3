
// ORIGINAL CREATORS:
// Developed by Tom Kail at Inkle
// Released under the MIT Licence as held at https://opensource.org/licenses/MIT
//https://gist.github.com/tomkail/ba4136e6aa990f4dc94e0d39ec6a058c#file-extendedscriptableobjectdrawer-cs

// Must be placed within a folder named "Editor"
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Globalization;

namespace NGN
{
    /// <summary>
    /// Extends how ScriptableObject object references are displayed in the inspector
    /// Shows you all values under the object reference
    /// Also provides a button to create a new ScriptableObject if property is null.
    /// </summary>
    [CustomPropertyDrawer(typeof(NGNScriptableObject), true)]
    public class NGNScriptableObjectPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = EditorGUIUtility.singleLineHeight;
            if (property.isExpanded)
            {
                var data = property.objectReferenceValue as ScriptableObject;
                if (data == null) return EditorGUIUtility.singleLineHeight;
                SerializedObject serializedObject = new SerializedObject(data);
                SerializedProperty prop = serializedObject.GetIterator();
                if (prop.NextVisible(true))
                {
                    do
                    {
                        if (prop.name == "m_Script") continue;
                        var subProp = serializedObject.FindProperty(prop.name);
                        float height = EditorGUI.GetPropertyHeight(subProp, null, true) + EditorGUIUtility.standardVerticalSpacing;
                        totalHeight += height;
                    }
                    while (prop.NextVisible(false));
                }
                // Add a tiny bit of height if open for the background
                totalHeight += EditorGUIUtility.standardVerticalSpacing;
            }
            return totalHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (property.objectReferenceValue != null)
            {
                property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), property.isExpanded, property.displayName, true);
                EditorGUI.PropertyField(new Rect(EditorGUIUtility.labelWidth + 14, position.y, position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), property, GUIContent.none, true);
                if (GUI.changed) property.serializedObject.ApplyModifiedProperties();
                if (property.objectReferenceValue == null) EditorGUIUtility.ExitGUI();

                if (property.isExpanded)
                {
                    // Draw a background that shows us clearly which fields are part of the ScriptableObject
                    GUI.Box(new Rect(0, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing - 1, Screen.width, position.height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing), "");

                    EditorGUI.indentLevel++;
                    var data = (ScriptableObject)property.objectReferenceValue;
                    SerializedObject serializedObject = new SerializedObject(data);

                    // Iterate over all the values and draw them
                    SerializedProperty prop = serializedObject.GetIterator();
                    position.y = EditorGUIUtility.singleLineHeight;
                    position.EndHorizontal();
                    if (prop.NextVisible(true))
                    {
                        do
                        {
                            // Don't bother drawing the class file
                            if (prop.name == "m_Script") continue;
                            float height = EditorGUI.GetPropertyHeight(prop, new GUIContent(prop.displayName), true);
                            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, height), prop, true);
                            position.EndHorizontal();
                        }
                        while (prop.NextVisible(false));
                    }
                    if (GUI.changed)
                        serializedObject.ApplyModifiedProperties();

                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                DisplaySearchField(position, property, label);
            }
            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }

        protected virtual void DisplaySearchField(Rect position, SerializedProperty property, GUIContent label)
        {
            //buttonLabel
            var labelWidth = GUI.skin.label.CalcSize(new GUIContent(property.displayName)).x + 15;
            var labelRect = new Rect(position.x, position.y, labelWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelRect, property.displayName);
            //button
            var buttonRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            if (GUI.Button(buttonRect, "Add Action"))
                NGNActionSearchEditor.ShowWindow(property.serializedObject.targetObject, property.propertyPath);         
        }

        
    }
}


