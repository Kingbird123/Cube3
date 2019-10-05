using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MEC;
[System.Serializable]
public class CameraSwitcher
{
    public enum SwitchType { Single, Toggle, Hold }
    public VirtualCameraManager enterCameraManager;
    public int enterCameraInd;
    public VirtualCameraManager exitCameraManager;
    public int exitCameraInd;
    public bool ignorePriorityOnExit;
    public InputProperty switchButton;
    public SwitchType switchType;
    public EngineEvent[] activateEvents;
    public EngineEvent[] deactivateEvents;

    private CinemachineBrain brain;
    private CinemachineVirtualCameraBase enterCamera;
    private CoroutineHandle switchCoroutine;
    private CinemachineSwitchMaster switchMaster;

    private bool active;
    public bool IsActive { get { return active; } }

    public void DetectCameraSwitch()
    {
        if (active)
            return;
        if (switchButton.GetInputDown())
            EnterCamera();
    }

    void EnterCamera()
    {
        active = true;
        if (!switchMaster)
        {
            switchMaster = Object.FindObjectOfType<CinemachineSwitchMaster>();
            enterCamera = switchMaster.GetCamera(enterCameraManager, enterCameraInd);
            brain = switchMaster.Brain;
        }
            
        if (switchMaster)
        {
            switchMaster.SwitchCamera(enterCameraManager, enterCameraInd, false, ActivateSwitchObjects);
        }

        //see if toggle or hold
        if (switchType == SwitchType.Toggle)
            Toggle(switchButton);
        else if (switchType == SwitchType.Hold)
            Hold(switchButton);
    }

    void ExitCamera()
    {
        active = false;
        if (switchMaster)
        {
            switchMaster.SwitchCamera(exitCameraManager, exitCameraInd, ignorePriorityOnExit);
        }
    }

    void Toggle(InputProperty _inputProperty)
    {
        StopSwitchRoutines();
        switchCoroutine = Timing.RunCoroutine(StartToggle(_inputProperty));
    }

    void Hold(InputProperty _inputProperty)
    {
        StopSwitchRoutines();
        switchCoroutine = Timing.RunCoroutine(StartHold(_inputProperty));
    }

    void StopSwitchRoutines()
    {
        if (switchCoroutine != null)
            Timing.KillCoroutines(switchCoroutine);
    }

    IEnumerator<float> StartToggle(InputProperty _inputProperty)
    {
        yield return Timing.WaitForOneFrame;
        while (brain.IsLive(enterCamera))
        {
            yield return Timing.WaitForOneFrame;
            if (_inputProperty.GetInputDown())
                break;
        }
        DeactivateSwitchObjects();
        ExitCamera();
    }

    IEnumerator<float> StartHold(InputProperty _inputProperty)
    {
        yield return Timing.WaitForOneFrame;
        while (_inputProperty.GetInput() && brain.IsLive(enterCamera))
            yield return Timing.WaitForOneFrame;
        DeactivateSwitchObjects();
        ExitCamera();
    }

    void ActivateSwitchObjects()
    {
        for (int i = 0; i < activateEvents.Length; i++)
        {
            activateEvents[i].DoEvent(enterCamera.gameObject, activateEvents, i);
        }
    }

    void DeactivateSwitchObjects()
    {
        for (int i = 0; i < deactivateEvents.Length; i++)
        {
            deactivateEvents[i].DoEvent(enterCamera.gameObject, activateEvents, i);
        }
    }

}
