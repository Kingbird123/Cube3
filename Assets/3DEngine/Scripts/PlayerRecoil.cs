using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecoil : MonoBehaviour
{

    [System.Serializable]
    private class RotationKey
    {
        public Vector3[] rotations = new Vector3[0];
    }

    [SerializeField] private Transform[] bones = null;
    [SerializeField] private RotationKey[] keys = null;
    [SerializeField] private float recoilTime = 0;
    private float timer;
    [SerializeField] private bool config = false;
    [SerializeField] private int configKey = 0;
    [SerializeField] private bool recoil = false;

    private enum RecoilType {First, Keys, Finish};

    private Vector3[] curRotations;
    private Vector3[] thisRot;

    private int keyInd;

    void Start()
    {
        curRotations = new Vector3[bones.Length];
    }

    void Update()
    {
        for (int i = 0; i < bones.Length; i++)
        {
            curRotations[i] = bones[i].transform.localEulerAngles; 

        }
    }

    void LateUpdate()
    {

        if (config && configKey < keys.Length)
            SetBoneRot();


        if (recoil)
        {
            if (keyInd < keys.Length)
            {
                bool finished = false;
                if (keyInd == 0)
                    LerpBones2(RecoilType.First,keys[keyInd], keys[keyInd], out finished);
                else
                    LerpBones2(RecoilType.Keys,keys[keyInd-1], keys[keyInd], out finished);

                if (finished)
                {
                    keyInd++;
                    return;
                }
            }
            else
            {
                bool finished = false;
                LerpBones2(RecoilType.Finish, keys[keyInd - 1], keys[keyInd - 1], out finished);
                if (finished)
                {
                    recoil = false;
                }
            }

        }else
        {
            keyInd = 0;
            timer = 0;
        }

    }


    void LerpBones2(RecoilType _type, RotationKey _lastKey, RotationKey _curKey, out bool _finished)
    {
        _finished = false;
        float recoilSplit = recoilTime / keys.Length;
        timer += Time.deltaTime;

        if (timer > recoilSplit)
            timer = recoilSplit;

        float perc = timer / recoilSplit;

        if (_type == RecoilType.First)
        {
            for (int i = 0; i < bones.Length; i++)
            {
                bones[i].localRotation = Quaternion.Lerp(bones[i].localRotation, Quaternion.Euler(_curKey.rotations[i]), perc);
            }
        }
        else if (_type == RecoilType.Keys)
        {
            for (int i = 0; i < bones.Length; i++)
            {
                bones[i].localRotation = Quaternion.Lerp(Quaternion.Euler(_lastKey.rotations[i]), Quaternion.Euler(_curKey.rotations[i]), perc);
            }
        }
        else
        {
            for (int i = 0; i < bones.Length; i++)
            {
                bones[i].localRotation = Quaternion.Lerp(Quaternion.Euler(_lastKey.rotations[i]), bones[i].localRotation, perc);
            }
        }

        if (perc >= 1)
        {
            _finished = true;
            timer = 0;
        }

    }

    void SetBoneRot()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            for (int boneInd = 0; boneInd < bones.Length; boneInd++)
            {
                bones[boneInd].localRotation = Quaternion.Euler(keys[configKey].rotations[boneInd]);
            }
            
        }
    }

    public void Recoil()
    {
        recoil = true;
        timer = 0;
        keyInd = 0;
    }
}
