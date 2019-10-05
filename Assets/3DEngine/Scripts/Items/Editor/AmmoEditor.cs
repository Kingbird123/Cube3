using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Ammo))]
public class AmmoEditor : EngineEntityEditor
{
    protected new Ammo Source { get { return (Ammo)source; } }

    protected override void DisplayDataProperties<T>()
    {
        base.DisplayDataProperties<AmmoData>();
    }
}
