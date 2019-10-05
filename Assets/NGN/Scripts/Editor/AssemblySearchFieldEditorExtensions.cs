using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NGN
{
    public static class AssemblySearchFieldEditorExtensions
    {
        //need input dictionary if we have multiple fields
        static StringWrapper input = new StringWrapper();
        private static Type[] subClassTypes;

        public static void CreateScriptableObjectSearchField(this SerializedProperty _objectProperty, Type _baseClass)
        {
            if (subClassTypes == null)
                subClassTypes = GetAllSubclasses(_baseClass);
            TypeSearchField(_objectProperty, subClassTypes, LoadObject);
        }

        static void LoadObject(SerializedProperty _property, Type _type)
        {
            var obj = ScriptableObject.CreateInstance(_type);
            _property.objectReferenceValue = obj;
            _property.isExpanded = false;
        }

        public static void TypeSearchField(this SerializedProperty _objectProperty, Type[] _dataBase, Action<SerializedProperty,Type> _onSelection) 
        {
            var buttonStyle = new GUIStyle(EditorStyles.miniButton);
            if (_objectProperty.objectReferenceValue == null)
            {
                EditorGUILayout.BeginHorizontal();
                //label
                var labelWidth = GUI.skin.label.CalcSize(new GUIContent(_objectProperty.displayName)).x;
                EditorGUILayout.LabelField(_objectProperty.displayName, GUILayout.Width(labelWidth));

                //get matches before text field so we can cancel it on enter pressed
                var matches = FindMatches(_dataBase, input.stringValue);
                if (_objectProperty.isExpanded)
                    matches = _dataBase;
                //cancel text field on enter
                if (matches.Length == 1)
                {
                    if (NGNEditorExtensions.GetKeyDown(KeyCode.Return))
                        _onSelection.Invoke(_objectProperty,matches[0]);
                }
                //input field
                var style = new GUIStyle(EditorStyles.toolbarSearchField);
                input.stringValue = EditorGUILayout.TextField(input.stringValue, style);
                if (input.stringValue != null && input.stringValue != "")
                {
                    
                    input.stringValue = Regex.Replace(input.stringValue, @"\s+", ""); //remove spaces in input
                    _objectProperty.isExpanded = true;
                }
                //dropdown button
                if (GUILayout.Button("▲/▼", buttonStyle, GUILayout.Width(30)))
                {
                    input.stringValue = "";
                    _objectProperty.isExpanded = !_objectProperty.isExpanded;
                }
                EditorGUILayout.EndHorizontal();
                var dropStyle = new GUIStyle(EditorStyles.miniButton);
                //only show input matches
                if (_objectProperty.isExpanded)
                {
                    for (int i = 0; i < matches.Length; i++)
                    {
                        
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(labelWidth);
                        if (GUILayout.Button(matches[i].Name, dropStyle))
                            _onSelection.Invoke(_objectProperty, matches[i]);
                        EditorGUILayout.EndHorizontal();
                    }
                }
                

            }

        }

        private static Type[] FindMatches(Type[] _searchBank, string _input)
        {
            var matches = new List<Type>();
            if (_input != null && _input != "")
            {
                for (int i = 0; i < _searchBank.Length; i++)
                {
                    CultureInfo info = CultureInfo.CurrentUICulture;
                    var name = _searchBank[i].Name;
                    if (info.CompareInfo.IndexOf(name, _input, CompareOptions.OrdinalIgnoreCase) >= 0)
                    {
                        matches.Add(_searchBank[i]);
                    }
                }
            }
            return matches.ToArray();
        }

        public static Type[] GetAllSubclasses(this Type _baseClass)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var allTypes = new List<Type>();
            for (int i = 0; i < assemblies.Length; i++)
            {
                var assembly = assemblies[i];
                var types = assembly.GetTypes();
                for (int ind = 0; ind < types.Length; ind++)
                {
                    var type = types[ind];
                    if (type.IsSubclassOf(_baseClass))
                        allTypes.Add(type);
                }
            }
            return allTypes.ToArray();
        }
    }
}


