using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraSwitcherContainer
{
    [SerializeField] private CameraSwitcher[] cameraSwitchers;

    //should be called from another scripts update function
    public void DetectCameraSwitch()
    {
        for (int i = 0; i < cameraSwitchers.Length; i++)
        {
            cameraSwitchers[i].DetectCameraSwitch();
        }

    }

}
