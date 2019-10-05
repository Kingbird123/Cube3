using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NGN
{
    public static class ObjectSearchFieldEditorExtension
    {
        //need input dictionary if we have multiple fields
        private static readonly Dictionary<SerializedProperty, StringWrapper> inputs = new Dictionary<SerializedProperty, StringWrapper>();

        public static void ScriptableObjectSearchField(this SerializedProperty _objectProperty, SerializedProperty _objToSearch, System.Type _typeFilter)
        {
            var obj = _objectProperty.objectReferenceValue;
            if (!obj)
                ObjectSearchField(_objectProperty, _objToSearch, _typeFilter);
            else
                ObjectPropertyField(_objectProperty);
        }

        public static void ObjectSearchField(this SerializedProperty _objectProperty, SerializedProperty _objToSearch, System.Type _typeFilter)
        {
            var buttonStyle = new GUIStyle(EditorStyles.miniButton);
            inputs.TryGetValue(_objectProperty, out StringWrapper input);
            if (input == null)
            {
                inputs.Add(_objectProperty, new StringWrapper());
                return;
            }
            if (_objectProperty.objectReferenceValue == null)
            {
                //get all nested objects on object
                var props = SearchObjectForObjectProperties(_objToSearch, _typeFilter);

                EditorGUILayout.BeginHorizontal();
                //label
                var labelWidth = GUI.skin.label.CalcSize(new GUIContent(_objectProperty.displayName)).x;
                EditorGUILayout.LabelField(_objectProperty.displayName, GUILayout.Width(labelWidth));

                //get matches before text field so we can cancel it on enter pressed
                var matches = FindMatches(props, input.stringValue);
                //cancel text field on enter
                if (matches.Length == 1)
                {
                    if (NGNEditorExtensions.GetKeyDown(KeyCode.Return))
                        _objectProperty.objectReferenceValue = matches[0];
                }
                //input field
                var style = new GUIStyle(EditorStyles.toolbarSearchField);
                input.stringValue = EditorGUILayout.TextField(input.stringValue, style);
                if (input.stringValue != null && input.stringValue != "")
                {
                    input.stringValue = Regex.Replace(input.stringValue, @"\s+", ""); //remove spaces in input
                    _objectProperty.isExpanded = false;
                }
                //dropdown button
                if (GUILayout.Button("▲/▼", buttonStyle, GUILayout.Width(30)))
                {
                    input.stringValue = "";
                    _objectProperty.isExpanded = !_objectProperty.isExpanded;
                }
                EditorGUILayout.EndHorizontal();
                var dropStyle = new GUIStyle(EditorStyles.miniButton);
                //show all objects in dropdown
                if (_objectProperty.isExpanded)
                {
                    for (int i = 0; i < props.Length; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(labelWidth);
                        if (GUILayout.Button(props[i].name, dropStyle))
                            _objectProperty.objectReferenceValue = props[i];
                        EditorGUILayout.EndHorizontal();
                    }
                }
                else
                {
                    //only show input matches
                    for (int i = 0; i < matches.Length; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(labelWidth);
                        if (GUILayout.Button(matches[i].name, dropStyle))
                            _objectProperty.objectReferenceValue = matches[i];
                        EditorGUILayout.EndHorizontal();
                    }

                }

            }
            else
            {
                ObjectPropertyField(_objectProperty);
            }

        }

        private static void ObjectPropertyField(SerializedProperty _objectProperty)
        {
            _objectProperty.ExpandableScriptableObjectField(0);
        }

        private static Object[] SearchObjectForObjectProperties(SerializedProperty _objToSearch, System.Type _typeFilter)
        {
            var props = new List<Object>();
            if (_objToSearch.objectReferenceValue)
            {
                var ser = new SerializedObject(_objToSearch.objectReferenceValue);
                var it = ser.GetIterator();
                while (it.NextVisible(true))
                {
                    if (it.name != "m_Script")
                    {
                        var names = CheckChildProperty(it, _typeFilter);
                        if (names.Length > 0)
                            props.AddRange(names);
                    }
                }
                ser.Dispose();
            }
            return props.ToArray();
        }

        private static Object[] CheckChildProperty(SerializedProperty _prop, System.Type _typeFilter)
        {
            var objs = new List<Object>();
            if (_prop.isArray)
            {
                for (int i = 0; i < _prop.arraySize; i++)
                {
                    var ele = _prop.GetArrayElementAtIndex(i);
                    var eleObjs = SearchObjectForObjectProperties(ele, _typeFilter);
                    if (eleObjs.Length > 0)
                        objs.AddRange(eleObjs);
                }
            }
            else if (_prop.propertyType == SerializedPropertyType.ObjectReference)
            {
                var obj = _prop.objectReferenceValue;
                if (obj)
                {
                    var type = obj.GetType();
                    if (type == _typeFilter)
                        objs.Add(obj);
                }
            }
            return objs.ToArray();
        }

        private static Object[] FindMatches(Object[] _objs, string _input)
        {
            var matches = new List<Object>();
            if (_input != null && _input != "")
            {
                for (int i = 0; i < _objs.Length; i++)
                {
                    CultureInfo info = CultureInfo.CurrentUICulture;
                    var name = _objs[i].name;
                    if (info.CompareInfo.IndexOf(name, _input, CompareOptions.OrdinalIgnoreCase) >= 0)
                    {
                        matches.Add(_objs[i]);
                    }
                }
            }
            return matches.ToArray();
        }

    }
}


