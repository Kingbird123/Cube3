using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buildable", menuName = "Data/Items/Tools/Buildable", order = 1)]
public class ItemBuildableData : ItemAimableData
{
    [System.Serializable]
    public class Placeable
    {
        public enum SnapType { None, Transform, Grid};
        public string placeableName;
        public LayerMask validPlacementMask;
        public SnapType snapType;
        public string transName;
        public float gridSpacing;
        public Vector3 gridOffset;
        public Vector3 placeableSize = Vector3.one;
        public GameObject previewPrefab;
        public GameObject placedPrefab;
    }

    public Placeable[] placeables;
    public Material validMaterial;
    public Material invalidMaterial;
    public LayerMask overlapMask;
    public float rotateAmount = 90;
}
