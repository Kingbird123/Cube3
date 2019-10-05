using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Item))]
public class ItemEditor : EngineEntityEditor
{
    protected new Item Source { get { return (Item)source; } }

    protected override void DisplayDataProperties<T>()
    {
        base.DisplayDataProperties<ItemData>();
    }
}
