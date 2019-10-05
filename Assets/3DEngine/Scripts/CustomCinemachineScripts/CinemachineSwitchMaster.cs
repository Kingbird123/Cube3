using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CinemachineSwitchMaster : MonoBehaviour
{
    [SerializeField] private CinemachineBrain brain;
    public CinemachineBrain Brain { get { return brain; } }
    [SerializeField] private CinemachineSwitch[] cinemachineSwitches;
    [SerializeField] private int defaultSwitch;

    private int curPriority;

    private void Awake()
    {
        InitializeSwitches();
    }

    void Start()
    {
        SwitchToDefault();
    }

    void InitializeSwitches()
    {
        for (int i = 0; i < cinemachineSwitches.Length; i++)
        {
            cinemachineSwitches[i].Initialize(this);
        }
    }

    public void SwitchToDefault()
    {
        cinemachineSwitches[defaultSwitch].SwitchToDefaultCamera();
        DeactivateOthers(defaultSwitch);
    }

    public void SwitchCamera(VirtualCameraManager _manager, int _ind, bool _ignorePriority = false, System.Action _successCallback = null)
    {
        var sw = GetSwitch(_manager);
        if (sw != null)
        {
            if (!_ignorePriority)
            {
                if (!IsHigherPriority(sw))
                    return;
            }

            curPriority = sw.Priority;
            sw.SwitchCamera(_ind);
            DeactivateOthers(_manager);
            if (_successCallback != null)
                _successCallback.Invoke();

        }
    }

    CinemachineSwitch GetSwitch(VirtualCameraManager _manager)
    {
        for (int i = 0; i < cinemachineSwitches.Length; i++)
        {
            var sw = cinemachineSwitches[i];
            if (_manager == sw.Manager) return sw;
        }
        Debug.LogError("could not find switch with manager: " + _manager);
        return null;
    }

    bool IsHigherPriority(CinemachineSwitch _cinemachineSwitch)
    {
        return _cinemachineSwitch.Priority >= curPriority;
    }

    void DeactivateOthers(int _ind)
    {
        for (int i = 0; i < cinemachineSwitches.Length; i++)
        {
            var sw = cinemachineSwitches[i];
            if (i != _ind)
                sw.DeactivateAllCameras();
        }
    }

    void DeactivateOthers(VirtualCameraManager _manager)
    {
        for (int i = 0; i < cinemachineSwitches.Length; i++)
        {
            var sw = cinemachineSwitches[i];
            if (_manager != sw.Manager)
                sw.DeactivateAllCameras();
        }
    }

    public string[] GetSwitchNames()
    {
        var names = new string[cinemachineSwitches.Length];
        for (int i = 0; i < cinemachineSwitches.Length; i++)
        {
            names[i] = cinemachineSwitches[i].SwitchName;
        }
        return names;
    }

    public CinemachineVirtualCameraBase GetCamera(VirtualCameraManager _manager, int _ind)
    {
        var sw = GetSwitch(_manager);
        if (sw != null) return sw.GetCamera(_ind); return null;
    }

    public string[] GetCameraNames(int _ind)
    {
        return cinemachineSwitches[_ind].GetVirtualCameraNames();
    }
}
