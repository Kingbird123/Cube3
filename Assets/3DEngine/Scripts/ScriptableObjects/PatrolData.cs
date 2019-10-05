using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PatrolData", menuName = "Data/Utilities/PatrolData", order = 1)]
public class PatrolData : ScriptableObject
{
    public PinPoint[] patrolPoints;
}
