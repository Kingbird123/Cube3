using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;


public static class MethodPropertyDrawerExtension
{
    private static SerializedProperty objectType;
    private static SerializedProperty behaviour;
    private static SerializedProperty go;
    private static SerializedProperty goInstanceValue;
    private static SerializedProperty behaviourInd;
    private static SerializedProperty behaviourToUse;
    private static SerializedProperty methodToCall;
    private static SerializedProperty methodInd;
    private static SerializedProperty parameterValues;

    private static GameObject goToUse;
    private static System.Type[] behaviourTypes;

    static string[] componentNames;
    static string[] componentTypes;

    static void GetProperties(SerializedProperty _property)
    {
        //get property values
        objectType = _property.FindPropertyRelative("objectType");
        behaviour = _property.FindPropertyRelative("behaviour");
        go = _property.FindPropertyRelative("go");
        goInstanceValue = _property.FindPropertyRelative("goInstanceValue");
        behaviourInd = _property.FindPropertyRelative("behaviourInd");
        behaviourToUse = _property.FindPropertyRelative("behaviourToUse");
        methodToCall = _property.FindPropertyRelative("methodToCall");
        methodInd = _property.FindPropertyRelative("methodInd");
        parameterValues = _property.FindPropertyRelative("parameterValues");
    }

    public static void MethodPropertyField(this SerializedProperty _methodProperty, GameObject _go)
    {
        GetProperties(_methodProperty);
        MethodPropertyWithoutGameObjectField(_methodProperty, _go);
    }

    public static void MethodPropertyField(this SerializedProperty _methodProperty, int _index = 0)
    {
        GetProperties(_methodProperty);

        EditorGUILayout.PropertyField(objectType);
        if (objectType.enumValueIndex == (int)MethodProperty.ObjectType.Assigned)
        {
            //game object to use
            var goLabel = "Add Game Object To Begin";
            if (go.objectReferenceValue)
                goLabel = "Game Object";
            go.objectReferenceValue = EditorGUILayout.ObjectField(goLabel, go.objectReferenceValue, typeof(GameObject), true);

            if (go.objectReferenceValue)
            {
                var tempGo = go.objectReferenceValue as GameObject;
                MethodPropertyWithoutGameObjectField(_methodProperty, tempGo);
            }
        }
        else
        {
            componentNames = ComponentDatabase.GetAllComponentNames(true);
            componentTypes = ComponentDatabase.GetAllComponentAssemblyNames(true);
            behaviour.IndexStringField(componentNames);
            var indexValue = behaviour.FindPropertyRelative("indexValue");

            MethodPropertyField(_methodProperty, System.Type.GetType(componentTypes[indexValue.intValue]));
        }

    }

    static void MethodPropertyWithoutGameObjectField(SerializedProperty _methodProperty, GameObject _go)
    {
        goToUse = _go;

        if (goToUse.IsPrefab())
        {
            Debug.LogError("Can't call methods to prefabs!");
            go.objectReferenceValue = null;
            return;
        }

        //get all components
        RefreshBehaviourTypes();

        //store all component names in a list for popup selection
        var behaviourNames = new string[behaviourTypes.Length];
        for (int i = 0; i < behaviourTypes.Length; i++)
        {
            behaviourNames[i] = behaviourTypes[i].ToString();
        }

        //reset indexes if gameobject is switched
        behaviourInd.intValue = Mathf.Clamp(behaviourInd.intValue, 0, behaviourTypes.Length - 1);
        goInstanceValue.intValue = goToUse.GetInstanceID();

        //component list popup
        behaviourInd.intValue = EditorGUILayout.Popup("Component", behaviourInd.intValue, behaviourNames);

        if (behaviourInd.intValue <= behaviourTypes.Length - 1)
        {
            var behavName = behaviourTypes[behaviourInd.intValue].ToString();
            //break the type name into parts so we only get the bottom class
            var parts = behavName.Split('.');

            //set the behaviour as a string so we can get component later
            behaviourToUse.stringValue = parts[parts.Length - 1];
        }


        //get the methods on the object script
        var type = behaviourTypes[behaviourInd.intValue];

        //display method fields
        MethodPropertyField(_methodProperty, type);
    }

    public static void MethodPropertyField(this SerializedProperty _methodProperty, System.Type _behaviour)
    {
        var propSource = _methodProperty.GetRootValue<MethodProperty>();
        GetProperties(_methodProperty);

        //get the methods on the object script
        var methods = _behaviour.GetMethods(BindingFlags.Instance | BindingFlags.Public);

        //create new method list that ignores return methods and return properties
        var setMethods = new List<MethodInfo>();
        foreach (var meth in methods)
        {
            //also ignore methods that have "Get" in the name
            if (meth.ReturnType == typeof(void) && !meth.Name.Contains("Get"))
                setMethods.Add(meth);
        }

        //store method names in a list for method popup
        var popUpNames = new string[setMethods.Count];
        var methodNames = new string[setMethods.Count];
        for (int i = 0; i < setMethods.Count; i++)
        {
            var param = setMethods[i].GetParameters();
            string paramNames = " ( ";
            if (param.Length > 0)
            {
                for (int ind = 0; ind < param.Length; ind++)
                {
                    var paramName = param[ind].ParameterType.Name;
                    var end = ", ";
                    if (ind == param.Length - 1)
                        end = " ";
                    paramNames += paramName + end;
                    if (ind == param.Length - 1)
                        paramNames += ")";
                }
            }
            else
                paramNames += " )";
            methodNames[i] = setMethods[i].Name;
            popUpNames[i] = methodNames[i] + paramNames;
        }

        //store index using a popup
        methodInd.intValue = EditorGUILayout.Popup("Method", methodInd.intValue, popUpNames);
        methodInd.intValue = Mathf.Clamp(methodInd.intValue, 0, popUpNames.Length - 1); //clamp to avoid index errors

        //store name of method for calling later
        methodToCall.stringValue = methodNames[methodInd.intValue];

        //set method to use
        var method = setMethods[methodInd.intValue];
        //get method parameters
        var parameters = method.GetParameters();
        //create list of custom parameter values for serializing
        parameterValues.arraySize = parameters.Length;

        if (parameterValues.arraySize > 0)
        {
            for (int i = 0; i < parameterValues.arraySize; i++)
            {

                var ele = parameterValues.GetArrayElementAtIndex(i);
                var par = parameters[i];
                var fieldName = par.Name + " (" + par.ParameterType.Name + ")";

                //serialize the object value in this parameter field
                par.ParameterField(ele, fieldName);
            }
        }

    }


    public static void MethodPropertyField(this SerializedProperty _methodProperty, Rect _position, int _curFieldAmount, out Rect _endPosition, out int _endFieldAmount)
    {
        GetProperties(_methodProperty);

        //prefabToSpawn
        int fieldAmount = _curFieldAmount;
        _position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        var goLabel = "Add Game Object To Begin";
        if (go.objectReferenceValue)
            goLabel = "Game Object";
        go.objectReferenceValue = EditorGUI.ObjectField(_position, goLabel, go.objectReferenceValue, typeof(GameObject), true);

        if (go.objectReferenceValue)
        {
            fieldAmount += 2;
            _position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            goToUse = go.objectReferenceValue as GameObject;

            if (goToUse.IsPrefab())
            {
                Debug.LogError("Can't call methods to prefabs!");
                go.objectReferenceValue = null;
                _endPosition = _position;
                _endFieldAmount = fieldAmount;
                return;
            }

            //get all components
            RefreshBehaviourTypes();

            //reset indexes if gameobject is switched
            if (goToUse.GetInstanceID() != goInstanceValue.intValue)
            {
                behaviourInd.intValue = 0;
                methodInd.intValue = 0;
                goInstanceValue.intValue = goToUse.GetInstanceID();
            }

            //store all component names in a list for popup selection
            var behaviourNames = new string[behaviourTypes.Length];
            for (int i = 0; i < behaviourTypes.Length; i++)
            {
                behaviourNames[i] = behaviourTypes[i].ToString();
            }

            //component list popup
            behaviourInd.intValue = EditorGUI.Popup(_position, "Component", behaviourInd.intValue, behaviourNames);

            if (behaviourInd.intValue <= behaviourTypes.Length - 1)
            {
                var behavName = behaviourTypes[behaviourInd.intValue].ToString();
                //break the type name into parts so we only get the bottom class
                var parts = behavName.Split('.');

                //set the behaviour as a string so we can get component later
                behaviourToUse.stringValue = parts[parts.Length - 1];
            }

            //methodlist
            _position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            //get the methods on the object script
            var type = behaviourTypes[behaviourInd.intValue];
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public);

            //create new method list that ignores return methods and return properties
            var setMethods = new List<MethodInfo>();
            foreach (var meth in methods)
            {
                //also ignore methods that have "Get" in the name
                if (meth.ReturnType == typeof(void) && !meth.Name.Contains("Get"))
                    setMethods.Add(meth);
            }

            //store method names in a list for method popup
            var methodNames = new string[setMethods.Count];
            for (int i = 0; i < setMethods.Count; i++)
            {
                methodNames[i] = setMethods[i].Name;
            }

            //store index using a popup
            methodInd.intValue = EditorGUI.Popup(_position, "Method", methodInd.intValue, methodNames);

            //store name of method for calling later
            methodToCall.stringValue = methodNames[methodInd.intValue];

            //set method to use
            var method = setMethods[methodInd.intValue];
            //get method parameters
            var parameters = method.GetParameters();
            //create list of custom parameter values for serializing
            parameterValues.arraySize = parameters.Length;

            if (parameterValues.arraySize > 0)
            {
                for (int i = 0; i < parameterValues.arraySize; i++)
                {
                    //gui positioning
                    fieldAmount++;
                    _position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    var ele = parameterValues.GetArrayElementAtIndex(i);
                    var par = parameters[i];
                    var fieldName = par.Name + " (" + par.ParameterType.Name + ")";

                    //serialize the object value in this parameter field
                    par.ParameterField(ele, _position, fieldName);
                }
            }

        }

        _endFieldAmount = fieldAmount;
        _endPosition = _position;
    }

    static void RefreshBehaviourTypes()
    {
        if (!goToUse)
            return;

        //get all components on game object we are using
        var comps = goToUse.GetComponents<Component>();
        //create type list on source for accessing all types including gameobject
        behaviourTypes = new System.Type[comps.Length + 1];
        if (behaviourTypes.Length > 0)
        {
            for (int i = 0; i < behaviourTypes.Length; i++)
            {
                //store gameobject as first type
                if (i == 0)
                    behaviourTypes[i] = goToUse.GetType();
                //get rest of component types attached to gameobject
                else
                    behaviourTypes[i] = comps[i - 1].GetType();
            }
        }
    }

}