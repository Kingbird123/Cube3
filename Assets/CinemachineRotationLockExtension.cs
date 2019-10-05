using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MEC;

public class CinemachineRotationLockExtension : CinemachineExtension
{
    public enum LockToType { None, Follow, LookAt, Custom }
    public enum LockAxisType { X, Y, Z }
    [SerializeField] private LockToType lockTo;
    [SerializeField] private int lockAxisMask;
    [SerializeField] private Vector3 rotation;

    private Transform tar;
    private float xRot;
    private float yRot;
    private float zRot;

    private bool adjustingRotation;
    private Quaternion rotationAdjust;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Aim)
        {
            var camRot = state.RawOrientation.eulerAngles;
            xRot = camRot.x;
            yRot = camRot.y;
            zRot = camRot.z;
            if (lockTo != LockToType.Custom)
            {

                if (lockTo == LockToType.Follow && vcam.Follow)
                    tar = vcam.Follow;
                if (lockTo == LockToType.LookAt && vcam.LookAt)
                    tar = vcam.LookAt;

                if (tar)
                {
                    var rot = tar.rotation.eulerAngles;

                    if (lockAxisMask == (lockAxisMask | (1 << (int)LockAxisType.X)))
                        xRot = rot.x;
                    if (lockAxisMask == (lockAxisMask | (1 << (int)LockAxisType.Y)))
                        yRot = rot.y;
                    if (lockAxisMask == (lockAxisMask | (1 << (int)LockAxisType.Z)))
                        zRot = rot.z;
                }
                
            }
            else
            {
                xRot = rotation.x;
                yRot = rotation.y;
                zRot = rotation.z;
            }

            //normal rotation overrides
            if (!adjustingRotation)
                state.RawOrientation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
            else
            {
                //keep last cameras rotation
                state.RawOrientation = rotationAdjust;
                var cam = vcam as CinemachineVirtualCamera;
                if (cam)
                {
                    var pov = cam.GetCinemachineComponent<CinemachinePOV>();
                    if (pov)
                    {
                        //adjust POV settings so it stays the same rotation...?
                    }

                }

            }

        }
    }

    public void SetRawOrientationOneFrame(Quaternion _orientation)
    {
        rotationAdjust = _orientation;
        //Timing.RunCoroutine(StartCameraAdjust());
    }

    IEnumerator<float> StartCameraAdjust()
    {
        adjustingRotation = true;
        yield return Timing.WaitForSeconds(1);//using seconds to debug
        adjustingRotation = false;
    }

}
