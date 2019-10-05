using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[CreateAssetMenu(fileName = "VirtualCameraManager", menuName = "Data/Managers/VirtualCameraManager", order = 1)]
public class VirtualCameraManager : ScriptableObject
{
    public VirtualCameraData[] virtualCameras;

    public string[] GetVirtualCameraNames()
    {
        var names = new string[virtualCameras.Length];
        for (int i = 0; i < virtualCameras.Length; i++)
        {
            if (virtualCameras[i])
                names[i] = virtualCameras[i].name;
        }
        return names;
    }
}
