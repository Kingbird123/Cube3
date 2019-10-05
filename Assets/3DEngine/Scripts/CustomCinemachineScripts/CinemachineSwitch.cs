using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MEC;

[System.Serializable]
public class CinemachineSwitch
{
    [SerializeField] protected string switchName;
    public string SwitchName { get { return switchName; } }
    [SerializeField] protected VirtualCameraManager virtualCameraManager;
    [SerializeField] protected int switchPriority = 0;
    public int Priority { get { return switchPriority; } }
    public VirtualCameraManager Manager { get { return virtualCameraManager; } }
    [SerializeField] protected Transform cameraSpawnLocation;
    [SerializeField] protected int activeCamera = 0;
    public int ActiveCamera { get { return activeCamera; } }
    protected int prevActiveCamera = 0;
    protected CinemachineSwitchMaster master;
    protected CinemachineVirtualCameraBase curVirtualCamera;
    protected VirtualCameraData curVirtualCameraData;
    protected List<CinemachineVirtualCameraBase> camSpawns = new List<CinemachineVirtualCameraBase>();
    public bool IsActive
    {
        get
        {
            for (int i = 0; i < camSpawns.Count; i++)
            {
                return camSpawns[i].gameObject.activeSelf;
            }
            return false;
        }
    }

    public void Initialize(CinemachineSwitchMaster _master)
    {
        master = _master;
        SpawnCameras();
    }

    public void SwitchToDefaultCamera()
    {
        SwitchCamera(activeCamera);
    }

    void SpawnCameras()
    {
        var cams = virtualCameraManager.virtualCameras;
        for (int i = 0; i < cams.Length; i++)
        {
            var spawn = Object.Instantiate(cams[i].virtualCameraPrefab);
            spawn.transform.SetParent(cameraSpawnLocation.transform);
            var camBase = spawn.GetComponent<CinemachineVirtualCameraBase>();
            camSpawns.Add(camBase);
            camBase.gameObject.SetActive(false);
            FindTargets(i);
        }
    }

    public void SwitchCamera(string _cameraName)
    {
        for (int i = 0; i < virtualCameraManager.virtualCameras.Length; i++)
        {
            if (_cameraName == virtualCameraManager.virtualCameras[i].name)
            {
                SwitchCamera(i);
                return;
            }
        }
        Debug.LogError("Could not find camera with the name " + _cameraName);
    }

    public void SwitchCamera(int _ind)
    {
        for (int i = 0; i < camSpawns.Count; i++)
        {
            var cam = camSpawns[i];
            var camData = virtualCameraManager.virtualCameras[i];
            if (i == _ind)
            {
                cam.gameObject.SetActive(true);
                prevActiveCamera = activeCamera;
                var prev = camSpawns[prevActiveCamera];
                activeCamera = i;
                curVirtualCameraData = camData;
                curVirtualCamera = cam;
                if (!curVirtualCamera.Follow && !curVirtualCamera.LookAt)
                {
                    if (curVirtualCameraData.findTargetOnEnable)
                        FindTargets(i);
                }
                    
            }
            else
                cam.gameObject.SetActive(false);
                
        }
    }

    public void DeactivateAllCameras()
    {
        for (int i = 0; i < camSpawns.Count; i++)
            camSpawns[i].gameObject.SetActive(false);
    }

    bool IsHigherPriority(int _ind)
    {
        bool higher = true;
        for (int i = 0; i < camSpawns.Count; i++)
        {
            var desired = camSpawns[_ind];
            var cs = camSpawns[i];
            higher = cs.Priority >= desired.Priority;
        }
        return higher;
    }


    public void FindTargets(int _ind)
    {
        Timing.RunCoroutine(StartFindTargets(_ind));
    }

    IEnumerator<float> StartFindTargets(int _ind)
    {

        var camData = virtualCameraManager.virtualCameras[_ind];
        if (camData.setFollowTarget)
        {
            while (!camData.Follow)
            {
                camData.Follow = camData.followTarget.GetSceneObject(master.gameObject);
                yield return Timing.WaitForOneFrame;
            }
        }
        if (camData.setLookAtTarget)
        {
            while (!camData.LookAt)
            {
                if (camData.followAndLookatIdentical)
                    camData.LookAt = camData.followTarget.GetSceneObject(master.gameObject);
                else
                    camData.LookAt = camData.lookAtTarget.GetSceneObject(master.gameObject);
                yield return Timing.WaitForOneFrame;
            }
        }

        CreateTargets(_ind);
    }

    void CreateTargets(int _ind)
    {
        var camData = virtualCameraManager.virtualCameras[_ind];
        var camSpawn = camSpawns[_ind];
        //create follow object pivot
        if (camData.setFollowTarget && camData.Follow)
        {
            if (!camSpawn.Follow)
            {
                var followTarget = new GameObject().transform;
                followTarget.name = "[CamFollowTarget_" + camSpawn.name + "]";
                followTarget.SetParent(camData.Follow.transform);
                followTarget.localPosition = camData.followOffset;
                followTarget.localEulerAngles = Vector3.zero;
                //follow player transform
                camSpawn.Follow = followTarget;
            }

        }

        //create look at object pivot
        if (camData.setLookAtTarget && camData.LookAt)
        {
            if (!camSpawn.LookAt)
            {
                var lookAtTarget = new GameObject().transform;
                lookAtTarget.name = "[CamLookAtTarget]";
                lookAtTarget.SetParent(camData.LookAt.transform);
                lookAtTarget.localPosition = camData.lookAtOffset;
                lookAtTarget.localEulerAngles = Vector3.zero;
                //look at pivot
                camSpawn.LookAt = lookAtTarget;
            }

        }
    }

    public CinemachineVirtualCameraBase GetCamera(int _ind)
    {
        return camSpawns[_ind];
    }

    public string[] GetVirtualCameraNames()
    {
        string[] names = new string[virtualCameraManager.virtualCameras.Length];
        for (int i = 0; i < virtualCameraManager.virtualCameras.Length; i++)
            names[i] = virtualCameraManager.virtualCameras[i].name;
        return names;
    }
}
