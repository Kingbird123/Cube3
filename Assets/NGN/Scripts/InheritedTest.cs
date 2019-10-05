using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NGN.OdinSerializer;

[System.Serializable]
public class InheritedTest : BaseTest
{
    public float test;

    public override void DoIt()
    {
        Debug.Log("what");
    }
}
