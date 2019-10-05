using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RigidbodyConstraintsProperty
{
    public bool freezePositionX;
    public bool freezePositionY;
    public bool freezePositionZ;
    public bool freezeRotationX;
    public bool freezeRotationY;
    public bool freezeRotationZ;

    public RigidbodyConstraints constraints;
}