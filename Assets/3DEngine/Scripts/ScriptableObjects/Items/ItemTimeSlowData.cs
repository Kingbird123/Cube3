using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeSlow", menuName = "Data/Items/Tools/TimeSlow", order = 1)]
public class ItemTimeSlowData : ItemAimableData
{
    public float slowTimeScale = 0.3f;
    public float physicsTimeScale = 0.02f;
    public float crossfadeTime = 0.3f;
}
