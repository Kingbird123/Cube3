// --------------------------------------------------------------------------------------------------------------------
// <author>
//   HiddenMonk
//   http://answers.unity3d.com/users/496850/hiddenmonk.html
//   
//   Johannes Deml
//   send@johannesdeml.com
//
//   Eris Koleszar
//   me@eris.codes
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;

/// <summary>
/// Extension class for SerializedProperties
/// See also: http://answers.unity3d.com/questions/627090/convert-serializedproperty-to-custom-class.html
/// </summary>

public static class SerializedPropertyExtensions
{
    public static T GetActualObjectForSerializedProperty<T>(this FieldInfo fieldInfo, SerializedProperty property) where T : class
    {
        var obj = fieldInfo.GetValue(property.serializedObject.targetObject);
        if (obj == null) { return null; }

        T actualObject = null;
        if (obj.GetType().IsArray)
        {
            var index = Convert.ToInt32(new string(property.propertyPath.Where(c => char.IsDigit(c)).ToArray()));
            actualObject = ((T[])obj)[index];
        }
        else
        {
            actualObject = obj as T;
        }
        return actualObject;
    }

    /// <summary>
    /// Get the object the serialized property of a Component holds by using reflection
    /// </summary>
    /// <typeparam name="T">The object type that the property contains</typeparam>
    /// <param name="property"></param>
    /// <returns>Returns the object type T if it is the type the property actually contains</returns>
    public static T GetRootValue<T>(this SerializedProperty property, bool _includeBaseClasses = false)
    {
        var obj = GetSerializedPropertyRootObject(property);
        return GetNestedObject<T>(property.propertyPath, obj, _includeBaseClasses);
    }

    public static bool SetValueOnRoot<T>(this SerializedProperty property, T value)
    {

        object obj = GetSerializedPropertyRootObject(property);
        //Iterate to parent object of the value, necessary if it is a nested object
        string[] fieldStructure = property.propertyPath.Split('.');
        for (int i = 0; i < fieldStructure.Length - 1; i++)
        {
            obj = GetFieldOrPropertyValue<object>(fieldStructure[i], obj);
        }
        string fieldName = fieldStructure.Last();

        return SetFieldOrPropertyValue(fieldName, obj, value);

    }

    /// <summary>
    /// Get the component of a serialized property
    /// </summary>
    /// <param name="property">The property that is part of the component</param>
    /// <returns>The root component of the property</returns>
    public static object GetSerializedPropertyRootObject(SerializedProperty property)
    {
        return property.serializedObject.targetObject;   
    }
    /// <summary>
    /// Iterates through objects to handle objects that are nested in the root object
    /// </summary>
    /// <typeparam name="T">The type of the nested object</typeparam>
    /// <param name="path">Path to the object through other properties e.g. PlayerInformation.Health</param>
    /// <param name="obj">The root object from which this path leads to the property</param>
    /// <param name="includeAllBases">Include base classes and interfaces as well</param>
    /// <returns>Returns the nested object casted to the type T</returns>
    public static T GetNestedObject<T>(string path, object obj, bool includeAllBases = false)
    {
        //store all array objs in a list for later
        var arrayObjs = new List<object>();
        //get rid of the array data names...keeping regular names
        var pathfixed = path.Replace(".Array.data[", "[");
        //split the path into separate fields
        var split = pathfixed.Split('.');
        //find the field with the path
        object splitObj = obj;
        //loop through all paths, arrays and lists to eventually get to the source object
        for (int i = 0; i < split.Length; i++)
        {
            if (splitObj != null)
            {
                //get array child if section of split is array
                if (split[i].Contains("["))
                {
                    //delete brackets so we find just the name array field
                    var arrayName = split[i].Substring(0, split[i].IndexOf("["));
                    //get index of the child
                    var arrayInd = Convert.ToInt32(new string(split[i].Where(c => char.IsDigit(c)).ToArray()));
                    //search array for the child object
                    splitObj = GetArrayChild(arrayName, splitObj, arrayInd);
                }
                else
                    //get the field
                    splitObj = GetFieldOrPropertyValue<object>(split[i], splitObj, includeAllBases);
            }
        }
        if (splitObj is T)//check if object can be casted..then return
        {
            return (T)splitObj;
        }   
        else
            return default(T); //return null
            

    }

    public static void SetNestedObject(string path, object obj, object value, bool includeAllBases = false)
    {
        //get rid of the array data names...keeping regular names
        var pathfixed = path.Replace(".Array.data[", "[");
        //split the path into separate fields
        var split = pathfixed.Split('.');
        //find the field with the path
        object splitObj = obj;
        object lastObj = obj;
        object final = obj;
        string lastField = "";
        int lastInd = 0;
        //loop through all paths, arrays and lists to eventually get to the source object
        for (int i = 0; i < split.Length; i++)
        {
            if (splitObj != null)
            {
                lastObj = splitObj;
                //get array child if section of split is array
                if (split[i].Contains("["))
                {
                    //delete brackets so we find just the name array field
                    var arrayName = split[i].Substring(0, split[i].IndexOf("["));
                    //get index of the child
                    var arrayInd = Convert.ToInt32(new string(split[i].Where(c => char.IsDigit(c)).ToArray()));
                    //search array for the child object
                    splitObj = GetArrayChild(arrayName, splitObj, arrayInd);
                    lastField = arrayName;
                    lastInd = arrayInd;
                }
                else
                {
                    //get the field
                    splitObj = GetFieldOrPropertyValue<object>(split[i], splitObj, includeAllBases);
                    lastField = split[i];
                }
                    
            }
            if (splitObj == null)//probably array
            {
                final = GetFieldOrPropertyValue<object>(lastField, lastObj, includeAllBases);
                if (final.GetType().IsArray)
                {
                    if (final.IsList())
                    {
                        var list = new List<object>((IEnumerable<object>)final);
                        if (lastInd <= list.Count - 1)
                            list[lastInd] = value;
                    }
                    else
                    {
                        var arr = final as object[];
                        if (lastInd <= arr.Length - 1)
                            arr[lastInd] = value;
                    }
                }
                else
                    final = value;
            }   
            else //single value
            {
                var field = splitObj.GetType().GetField(lastField);
                field.SetValue(splitObj, value);
            }
                   
        }


    }

    static object GetArrayChild(string fieldName, object obj, int index)
    {
        var array = GetFieldOrPropertyValue<object>(fieldName, obj);
        if (array != null)
        {
            if (array.IsList())
            {
                var list = new List<object>((IEnumerable<object>)array);
                if (index <= list.Count - 1)
                    return list[index];
            }
            else
            {
                var arr = array as object[];
                if (index <= arr.Length - 1)
                    return arr[index];
            }
        }
        return null;
    }


    public static T GetFieldOrPropertyValue<T>(string fieldName, object obj, bool includeAllBases = false, BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) where T:class
    {
        
        var fieldType = obj.GetType();
        if (fieldType != null)
        {
            FieldInfo field = fieldType.GetField(fieldName, bindings);
            if (field != null)
            {
                var fieldObj = field.GetValue(obj);
                if (fieldObj != null)
                    return (T)fieldObj;
            }
        }

        PropertyInfo property = obj.GetType().GetProperty(fieldName, bindings);
        if (property != null)
        {
            return (T)property.GetValue(obj, null);
        }

        var prop = obj as T;
        if (prop != null)
        {
            return prop;
        }

        if (includeAllBases)
        {

            foreach (Type type in GetBaseClassesAndInterfaces(obj.GetType()))
            {
                var field = type.GetField(fieldName, bindings);
                if (field != null) return (T)field.GetValue(obj);

                property = type.GetProperty(fieldName, bindings);
                if (property != null) return (T)property.GetValue(obj, null);
            }
        }

        Debug.LogError("Could not find field or property " + fieldName + " on " + obj);
        return default(T);
    }

    public static bool SetFieldOrPropertyValue(string fieldName, object obj, object value, bool includeAllBases = false, BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
    {
        FieldInfo field = obj.GetType().GetField(fieldName, bindings);
        if (field != null)
        {
            field.SetValue(obj, value);
            return true;
        }

        PropertyInfo property = obj.GetType().GetProperty(fieldName, bindings);
        if (property != null)
        {
            property.SetValue(obj, value, null);
            return true;
        }

        if (includeAllBases)
        {
            foreach (Type type in GetBaseClassesAndInterfaces(obj.GetType()))
            {
                field = type.GetField(fieldName, bindings);
                if (field != null)
                {
                    field.SetValue(obj, value);
                    return true;
                }

                property = type.GetProperty(fieldName, bindings);
                if (property != null)
                {
                    property.SetValue(obj, value, null);
                    return true;
                }
            }
        }
        return false;
    }

    public static IEnumerable<Type> GetBaseClassesAndInterfaces(this Type type, bool includeSelf = false)
    {
        List<Type> allTypes = new List<Type>();

        if (includeSelf) allTypes.Add(type);

        if (type.BaseType == typeof(object))
        {
            allTypes.AddRange(type.GetInterfaces());
        }
        else
        {
            allTypes.AddRange(
                    Enumerable
                    .Repeat(type.BaseType, 1)
                    .Concat(type.GetInterfaces())
                    .Concat(type.BaseType.GetBaseClassesAndInterfaces())
                    .Distinct());
        }

        return allTypes;
    }
}