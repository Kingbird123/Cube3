using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using NGN.OdinSerializer;
[System.Serializable]
public class ParameterValue
{
    public byte[] serializedData;
    public Object objectValue;
    public bool boolValue;
    public Bounds boundsValue;
    public Color colorValue;
    public AnimationCurve animationCurveValue;
    public AnimationClip animationClipValue;
    public float floatValue;
    public int intValue;
    public Vector3 quaternionValue;
    public Rect rectValue;
    public string stringValue;
    public Vector2 vector2Value;
    public Vector3 vector3Value;
    public Vector3 vector4Value;
}