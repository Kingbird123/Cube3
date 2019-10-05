using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drawable", menuName = "Data/Items/Tools/Drawable", order = 1)]
public class ItemDrawableData : ItemAimableData
{
    public InputProperty drawButton;
    public GameObject topPrefab;
    public GameObject growLinePrefab;
    public GameObject trailRenderer;
    public float sensitivityDistance = 1;
    public float growSpeed = 1;
    public bool useMask;
    public LayerMask drawableMask;
    public LayerMask blockableMask;
}
