using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[CreateAssetMenu(fileName = "VirtualCameraData", menuName = "Data/VirtualCamera/VirtualCameraData", order = 1)]
public class VirtualCameraData : ScriptableObject
{
        public GameObject virtualCameraPrefab;
        public bool findTargetOnEnable;
        public bool followAndLookatIdentical;
    public bool setFollowTarget;
        public SceneObjectProperty followTarget;
        public Vector3 followOffset;
    public bool setLookAtTarget;
    public SceneObjectProperty lookAtTarget;
        public Vector3 lookAtOffset;

    public GameObject Follow { get; set; }
    public GameObject LookAt { get; set; }

}
