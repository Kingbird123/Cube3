using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable
{
    bool IsInUse { get; }
    void Use();
    void StopUse();
}
