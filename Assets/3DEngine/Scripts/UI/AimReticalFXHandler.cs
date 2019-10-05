using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AimReticalFXHandler
{
    protected AimReticalFX aimFXData;
    protected float  distance;
    protected Vector3 aimOrigion;
    protected Vector3 aimDestination;
    protected Transform aimRef;
    protected GameObject curHitObject;
    protected List<GameObject> swapAimerRefs = new List<GameObject>();

    public void Initialize(AimReticalFX _data)
    {
        aimFXData = _data;
        SpawnDefaultReticals();
        SetActiveAimReference(0);
    }

    public void DrawAimFX(GameObject _aimHitObject, float _distance, Vector3 _aimOrigin, Vector3 _aimDestination)
    {
        distance = _distance;
        aimOrigion = _aimOrigin;
        aimDestination = _aimDestination;
        curHitObject = _aimHitObject;

        DetectReticalSwaps();
    }

    void SpawnDefaultReticals()
    {
        if (aimFXData.defaultAimRetical)
            SpawnAimerReference(aimFXData.defaultAimRetical);
        if (aimFXData.reticalSwaps.Length > 0)
        {
            foreach (var ret in aimFXData.reticalSwaps)
            {
                SpawnAimerReference(ret.swapReticalPrefab);
            }
        }
    }

    void SpawnAimerReference(GameObject _spawn)
    {

        if (_spawn)
        {
            var aimer = GameObject.Instantiate(_spawn);
            aimer.transform.rotation = Quaternion.identity;
            var ui = UIPlayer.instance;
            if (ui)
            {
                aimer.transform.SetParent(ui.transform);
                aimer.transform.localScale = Vector3.one;
                aimer.transform.localPosition = Vector3.zero;
            }

            swapAimerRefs.Add(aimer);

        }

    }

    void SetActiveAimReference(int _index)
    {
        if (swapAimerRefs.Count < 1)
            return;

        for (int i = 0; i < swapAimerRefs.Count; i++)
        {
            var aimer = swapAimerRefs[i];
            if (aimer)
            {
                aimer.SetActive(i == _index);
                if (aimer.activeSelf)
                    aimRef = aimer.transform;
            }

        }
    }


    void DetectReticalSwaps()
    {

        if (aimFXData.reticalSwaps.Length < 1)
            return;

        if (swapAimerRefs.Count < 1)
            return;

        for (int i = 0; i < aimFXData.reticalSwaps.Length; i++)
        {
            var swap = aimFXData.reticalSwaps[i];
            var defaultRef = swapAimerRefs[0];
            var retTrans = swapAimerRefs[i + 1];
            if (curHitObject)
            {
                
                bool pass = curHitObject.IsInLayerMask(swap.swapMask) &&
                    distance <= swap.swapDistance;

                if (pass)
                {
                    if (retTrans)
                    {
                        if (aimRef != retTrans)
                            SetActiveAimReference(i + 1);
                    }

                }
                else if (defaultRef)
                {
                    if (aimRef != defaultRef.transform)
                        SetActiveAimReference(0);
                }

            }
            else if (defaultRef)
            {
                if (aimRef != defaultRef.transform)
                    SetActiveAimReference(0);
            }

        }
    }

    public void KillAimerReferences()
    {
        if (swapAimerRefs.Count > 0)
        {
            for (int i = 0; i < swapAimerRefs.Count; i++)
            {
                var swap = swapAimerRefs[i];
                if (swap)
                {
                    GameObject.Destroy(swap.gameObject);
                }

            }
            swapAimerRefs.Clear();
        }
    }

    public bool ContainsAimFXData(AimReticalFX _data)
    {
        return aimFXData == _data;
    }

}
