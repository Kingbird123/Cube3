using UnityEngine;
using UnityEditor;
using Object = System.Object;
using System.Reflection;
using System;
using System.Linq;

public class NestablePropertyDrawer : PropertyDrawer
{
    protected Object propertyObject = null;

    protected virtual void Initialize(SerializedProperty prop, Type _type)
    {
        //get the parent script this property resides in...mono or scriptable
        var tar = prop.serializedObject.targetObject;
        //find the property using its property path
        var tarFieldInfo = tar.GetType().GetField(prop.propertyPath);
        //return if property is found
        if (tarFieldInfo != null)
        {
            propertyObject = tarFieldInfo.GetValue(tar);
        }
        else//find out if property is nested in another class
        {
            var obj = FindNestedObject(prop, tar);
            if (obj.GetType() == _type)
                propertyObject = obj;
            else
                propertyObject = FindNestedObject(prop, obj);
        }
    }

    object FindNestedObject(SerializedProperty _prop, object _obj)
    {
        //split the property path
        string[] path = _prop.propertyPath.Split('.');
        //try to find a property within the split
        foreach (string pathNode in path)
        {
            //need to use reflection binding flags to obtain the nested info.
            var nestedInfo = _obj.GetType().GetField(pathNode, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (nestedInfo != null)
            {
                var obj = nestedInfo.GetValue(_obj);
                if (obj != null)
                {
                    //is the nested object array?
                    if (obj.GetType().IsArray)
                    {
                        var index = Convert.ToInt32(new string(_prop.propertyPath.Where(c => char.IsDigit(c)).ToArray()));
                        obj = ((object[])obj)[index];   
                    }
                    return obj;
                }

            }

        }
        return null;
    }

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        Initialize(prop, null);
    }
}
