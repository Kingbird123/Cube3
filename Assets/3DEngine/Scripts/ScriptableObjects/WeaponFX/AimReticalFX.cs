using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AimReticalFX", menuName = "Data/FX/AimReticalFX", order = 1)]
public class AimReticalFX : ScriptableObject
{
    [System.Serializable]
    public class ReticalSwap
    {
        public GameObject swapReticalPrefab;
        public float swapDistance;
        public LayerMask swapMask;
    }

    public GameObject defaultAimRetical;
    public ReticalSwap[] reticalSwaps;
}
