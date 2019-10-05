using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using NGN.OdinSerializer;
using System.IO;
using System.Reflection;

namespace NGN
{
    [InitializeOnLoad]
    public class NGNActionSearchEditor : EditorWindow
    {

        public class ActionSearchData
        {
            public object serObject;
            public string path;
        }

        private static ActionSearchData data = new ActionSearchData();
        protected string input;
        protected System.Type[] subclasses;
        protected System.Type[] matches = new System.Type[0];
        static protected List<Object> objCopy = new List<Object>();
        protected const string dataPath = "Assets/NGN/Resources/OdinData/ActionSearchData";

        public static void ShowWindow(object _target, string _propertyPath)
        {
            CarryOverData(_target, _propertyPath);
            GetWindow<NGNActionSearchEditor>("Action Search");
        }

        static void CarryOverData(object _target, string _propertyPath)
        {
            data.serObject = _target;
            data.path = _propertyPath;
            WriteDataToFile();
        }

        private void OnEnable()
        {
            ReadDataFromFile();
            subclasses = typeof(NGNScriptableObject).GetAllSubclasses();
        }

        static void WriteDataToFile()
        {
            byte[] bytes = SerializationUtility.SerializeValue(data, DataFormat.Binary, out objCopy);
            File.WriteAllBytes(dataPath, bytes);
        }

        static void ReadDataFromFile()
        {
            //get data from json
            byte[] bytes = File.ReadAllBytes(dataPath);
            data = SerializationUtility.DeserializeValue<ActionSearchData>(bytes, DataFormat.Binary, objCopy);
        }

        private void OnGUI()
        {
            //search bar
            var style = new GUIStyle(EditorStyles.toolbarSearchField);
            input = EditorGUILayout.TextField("Search", input, style);
            if (input != null && input != "")
                input = Regex.Replace(input, @"\s+", ""); //remove spaces in input 
            //get matches
            matches = FindMatches(subclasses, input);
            //display matches
            for (int i = 0; i < matches.Length; i++)
            {
                if (GUILayout.Button(matches[i].Name))
                {
                    var obj = ScriptableObject.CreateInstance(matches[i]);
                    obj.name = matches[i].Name;
                    SerializedPropertyExtensions.SetNestedObject(data.path, data.serObject, obj);
                }
                    
            }

        }

        private static System.Type[] FindMatches(System.Type[] _types, string _input)
        {
            var matches = new List<System.Type>();
            if (_input != null && _input != "")
            {
                for (int i = 0; i < _types.Length; i++)
                {
                    CultureInfo info = CultureInfo.CurrentUICulture;
                    var name = _types[i].Name;
                    if (info.CompareInfo.IndexOf(name, _input, CompareOptions.OrdinalIgnoreCase) >= 0)
                    {
                        matches.Add(_types[i]);
                    }
                }
            }
            return matches.ToArray();
        }

    }

}

