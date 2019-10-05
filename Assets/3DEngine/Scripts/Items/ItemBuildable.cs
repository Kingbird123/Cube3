using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuildable : ItemAimable
{
    public new ItemBuildableData Data { get { return (ItemBuildableData)data; } }

    [SerializeField] private InputProperty rotateButton = null;
    [SerializeField] private InputProperty placeButton = null;
    [SerializeField] private InputProperty toggleIndAdd = null;
    [SerializeField] private InputProperty toggleIndSubract = null;
    private ItemBuildableData.Placeable curPlaceable;
    private Transform previewPivot;
    private Transform curSnapTrans;
    private GameObject curPlaceablePreview;
    private int curPlaceableInd;

    private bool invalid;
    private bool lastCheck;
    private float lastYRot;
    private GameObject lastHitObject;

    private void Update()
    {
        GetInputs();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        CheckAimHit();
        CheckOverlap();
        PositionPreview();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (curPlaceablePreview)
            Destroy(curPlaceablePreview);
    }


    protected override void OnOwnerFound()
    {
        base.OnOwnerFound();
    }

    void GetInputs()
    {
        if (!ownerEquip)
            return;

        if (ownerEquip.InputOption == UnitEquip.InputType.None)
            return;

        if (placeButton.GetInputDown())
            PlaceSpawnedItem();

        if (toggleIndAdd.GetInputDown())
        {
            if (curPlaceableInd >= Data.placeables.Length - 1)
                curPlaceableInd = 0;
            else
                curPlaceableInd++;

            SpawnPreview();
        }
        
        if (toggleIndSubract.GetInputDown())
        {
            if (curPlaceableInd <= 0)
                curPlaceableInd = Data.placeables.Length - 1;
            else
                curPlaceableInd--;
            SpawnPreview();
        }

        if (rotateButton.GetInputDown())
            RotatePreview();
            
    }

    void CheckAimHit()
    {
        if (!unitController)
            return;

        if (unitController.AimHitObject)
        {
            if (!curPlaceablePreview)
                SpawnPreview();
        }
        else if (curPlaceablePreview)
            KillPreview();

        if (unitController.AimHitObject != lastHitObject)
        {
            if (curSnapTrans)
                curSnapTrans = null;
        }

        lastHitObject = unitController.AimHitObject;

    }

    void SpawnPreview()
    {
        if (!unitController)
            return;

        KillPreview();

        //spawn pivot..needed for rotating tower only on y axis easily
        previewPivot = new GameObject().transform;
        previewPivot.name = "[PlaceablePivot]";

        //spawn preview
        curPlaceable = Data.placeables[curPlaceableInd];
        curPlaceablePreview = Instantiate(curPlaceable.previewPrefab, unitController.AimPosition, Quaternion.identity, previewPivot);
        curPlaceablePreview.transform.localPosition = Vector3.zero;
        curPlaceablePreview.transform.localEulerAngles = new Vector3(0, lastYRot, 0);
        var col = curPlaceablePreview.GetComponent<Collider>();
        if (col)
            col.enabled = false;
        SetSpawnMaterials(invalid);
    }

    void PositionPreview()
    {
        if (!curPlaceablePreview)
            return;

        if (!unitController.AimHitObject)
            return;

        //snap to transform child
        if (curPlaceable.snapType == ItemBuildableData.Placeable.SnapType.Transform)
        {
            //find child if null
            if (!curSnapTrans)
            {
                var child = unitController.AimHitObject.transform.FindDeepChild(curPlaceable.transName);
                if (child)
                    curSnapTrans = child;
            }
            else
            {
                previewPivot.position = curSnapTrans.position;
                previewPivot.rotation = curSnapTrans.rotation;
            }
        }
        else
        {
            //free positioning
            var pos = unitController.AimPosition;
            var rot = Quaternion.FromToRotation(Vector3.up, unitController.AimHitNormal);

            previewPivot.position = pos;
            previewPivot.rotation = rot;

            //snap position to grid
            if (curPlaceable.snapType == ItemBuildableData.Placeable.SnapType.Grid)
            {
                //find local offset of pivot after initial positioning
                var offset = previewPivot.TransformDirection(curPlaceable.gridOffset);

                var spacing = curPlaceable.gridSpacing;
                var angle = Vector3.Angle(Vector3.up, previewPivot.up);

                //if (angle > 0 && angle < 90)
                    //spacing *= angle / 10;

                var snap = pos.SnapOffset(offset, spacing);
                var local = previewPivot.InverseTransformPoint(snap);
                var localXY = new Vector3(local.x, 0, local.z);
                pos = previewPivot.TransformPoint(localXY);
                
                previewPivot.position = pos;
            }
        }

        
        lastYRot = curPlaceablePreview.transform.localEulerAngles.y;
    }

    void RotatePreview()
    {
        var eul = curPlaceablePreview.transform.localEulerAngles;
        curPlaceablePreview.transform.localEulerAngles = new Vector3(eul.x, eul.y + Data.rotateAmount, eul.z);
    }

    void CheckOverlap()
    {
        if (!curPlaceablePreview)
            return;

        if (!unitController.AimHitObject)
            return;

        var halfY = new Vector3(0, curPlaceable.placeableSize.y / 2, 0);
        var center = curPlaceablePreview.transform.TransformPoint(halfY);
        var rot = curPlaceablePreview.transform.rotation;
        var cols = Physics.OverlapBox(center, (curPlaceable.placeableSize / 2) * 0.99f, rot, Data.overlapMask);

        invalid = cols.Length > 0 || !unitController.AimHitObject.IsInLayerMask(curPlaceable.validPlacementMask);

        if (invalid != lastCheck)
        {
            SetSpawnMaterials(invalid);
            lastCheck = invalid;
        }

    }

    void SetSpawnMaterials(bool _invalid)
    {
        var mat = Data.validMaterial;
        if (_invalid)
            mat = Data.invalidMaterial;

        Utils.SetAllChildMaterials(curPlaceablePreview.transform, mat);
    }

    void PlaceSpawnedItem()
    {
        if (!curPlaceablePreview)
            return;
        if (invalid)
            return;

        Instantiate(curPlaceable.placedPrefab, curPlaceablePreview.transform.position, curPlaceablePreview.transform.rotation);
        KillPreview();
    }


    void KillPreview()
    {
        if (curPlaceablePreview)
        {
            Destroy(curPlaceablePreview);
            Destroy(previewPivot.gameObject);
        }
            
    }

}
