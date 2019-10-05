using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

[System.Serializable]
public class MethodProperty
{
    public enum ObjectType { Assigned, Receiver }
    public ObjectType objectType;
    public IndexStringProperty behaviour;
    public GameObject go;
    public int goInstanceValue;
    public string behaviourToUse;
    public int behaviourInd;
    public MethodInfo method;
    public string methodToCall;
    public int methodInd;
    public ParameterValue[] parameterValues;

    private object objToUse;

    public void InvokeMethod()
    {
        if (method == null)
            GetMethod();
        if (objToUse != null)
            method.Invoke(objToUse, GetParameterObjectValues());
    }

    void GetMethod()
    {
        objToUse = go;
        if (behaviourToUse != "GameObject")
            objToUse = go.GetComponent(behaviourToUse);
        var type = objToUse.GetType();
        method = type.GetMethod(methodToCall, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    }

    object[] GetParameterObjectValues()
    {
        var objValues = new object[parameterValues.Length];
        for (int i = 0; i < parameterValues.Length; i++)
        {
            //objValues[i] = StringSerializer.Deserialize(typeof(object), parameterValues[i].serializedObjectValue);
        }
        return objValues;
    }
}
