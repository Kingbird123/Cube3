using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NGN
{
    public static class NGNScriptableObjectFieldExtension
    {

        public static void ArraySingleFieldButtons(this SerializedProperty _arrayProperty, System.Action<SerializedProperty, int> _fieldCallback)
        {
            if (_arrayProperty.arraySize == 0)
            {
                if (GUILayout.Button("Initialize " + _arrayProperty.displayName))
                    _arrayProperty.InsertArrayElementAtIndex(0);
                return;
            }
            _arrayProperty.isExpanded = EditorGUILayout.Foldout(_arrayProperty.isExpanded, _arrayProperty.displayName);

            if (_arrayProperty.isExpanded)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < _arrayProperty.arraySize; i++)
                {
                    var prop = _arrayProperty.GetArrayElementAtIndex(i);

                    if (!prop.isExpanded)
                        EditorGUILayout.BeginHorizontal();

                    _fieldCallback.Invoke(prop, i);

                    if (!prop.isExpanded)
                    {
                        var style = new GUIStyle(EditorStyles.miniButton);
                        style.fixedWidth = 20;
                        //down button
                        if (i < _arrayProperty.arraySize - 1)
                        {
                            if (GUILayout.Button("▼", style))
                                _arrayProperty.MoveArrayElement(i, i + 1);
                        }
                        //up button
                        if (i > 0)
                        {
                            if (GUILayout.Button("▲", style))
                                _arrayProperty.MoveArrayElement(i, i - 1);
                        }
                        //add button
                        if (GUILayout.Button("✚", style))
                            _arrayProperty.InsertArrayElementAtIndex(i);
                        //del button
                        if (GUILayout.Button("✖", style))
                            _arrayProperty.DeleteArrayElementAtIndex(i); 
                        EditorGUILayout.EndHorizontal();
                    }


                }
                EditorGUI.indentLevel--;
            }


        }

        public static void ExpandableScriptableObjectField(this SerializedProperty _property, int _index)
        {
            bool arrayChild = _property.name.Contains("data");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUI.indentLevel * 15);
            float buttonSize = 12;
            if (GUILayout.Button("", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
            {
                _property.isExpanded = !_property.isExpanded;
            }
            GUILayout.Space(-EditorGUI.indentLevel * 15);
            if (!arrayChild)
            {
                var labelWidth = GUI.skin.label.CalcSize(new GUIContent(_property.displayName)).x;
                EditorGUILayout.LabelField(_property.displayName, GUILayout.Width(labelWidth));
            }
            EditorGUILayout.ObjectField(_property, GUIContent.none);
            if (!arrayChild)
            {
                //delete button
                var buttonStyle = new GUIStyle(EditorStyles.miniButton);
                if (GUILayout.Button("✖", buttonStyle, GUILayout.Width(20)))
                    _property.objectReferenceValue = null;
            }
            EditorGUILayout.EndHorizontal();
            if (_property.isExpanded)
            {
                CollapseOtherScriptableObjectFields(_property);
                var obj = _property.objectReferenceValue;
                if (obj != null)
                {
                    var ser = new SerializedObject(obj);
                    var it = ser.GetIterator();
                    while (it.NextVisible(true))
                    {
                        ExpandableScriptableObjectChildField(ser, it);
                    }
                    ser.ApplyModifiedProperties();
                    ser.Dispose();
                }
            }

        }

        private static void ExpandableScriptableObjectChildField(SerializedObject _obj, SerializedProperty _property)
        {
            if (_property.name == "m_Script")
                return;

            EditorGUI.indentLevel++;
            var prop = _obj.FindProperty(_property.name);
            if (prop != null)
            {
                if (prop.isArray)
                {
                    System.Action<SerializedProperty, int> action = SerializedPropertyField;
                    if (prop.arraySize > 0)
                    {
                        var p = prop.GetArrayElementAtIndex(0);
                        if (p.IsScriptableObject())
                            action = ExpandableScriptableObjectField;
                    }
                    prop.ArraySingleFieldButtons(action);

                }
                else if (prop.propertyType == SerializedPropertyType.ObjectReference)
                {
                    if (prop.IsScriptableObject())
                        prop.ExpandableScriptableObjectField(0);
                    else
                        EditorGUILayout.ObjectField(prop);
                }
                else
                    EditorGUILayout.PropertyField(prop, true);
            }
            EditorGUI.indentLevel--;
        }

        private static void CollapseOtherScriptableObjectFields(SerializedProperty _prop)
        {
            var obj = _prop.serializedObject;
            var it = obj.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.propertyType == SerializedPropertyType.ObjectReference && it.objectReferenceValue)
                {
                    if (it.propertyPath != _prop.propertyPath)
                        it.isExpanded = false;
                }
            }
        }

        private static void SerializedPropertyField(SerializedProperty _property, int _ind)
        {
            EditorGUILayout.PropertyField(_property, true);
        }

        static bool IsScriptableObject(this SerializedProperty _property)
        {
            if (_property.objectReferenceValue)
            {
                var so = _property.objectReferenceValue as ScriptableObject;
                return so;
            }
            return false;
        }

    }
}


