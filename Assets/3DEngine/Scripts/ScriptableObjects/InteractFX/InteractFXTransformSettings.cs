using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TransformSettings", menuName = "Data/Interacts/TransformSettings", order = 1)]
public class InteractFXTransformSettings : InteractFXDynamic
{
    //mask names
    public enum TransformSettings { Position, Rotation, Scale }
    [SerializeField] private int maskInd = 0;

    public enum SetToType { Vector3, SceneObject }

    [SerializeField] private SetToType setPositionTo = SetToType.Vector3;
    [SerializeField] private Vector3 positionVector = Vector3.zero;
    [SerializeField] private SceneObjectProperty positionObj = null;
    [SerializeField] private SetToType setRotationTo = SetToType.Vector3;
    [SerializeField] private Vector3 rotationVector = Vector3.zero;
    [SerializeField] private SceneObjectProperty rotationObj = null;
    [SerializeField] private SetToType setScaleTo = SetToType.Vector3;
    [SerializeField] private Vector3 scaleVector = Vector3.one;
    [SerializeField] private SceneObjectProperty scaleObj = null;


    protected override void AffectObject()
    {

        if (maskInd == (maskInd | (1 << (int)TransformSettings.Position)))
        {
            if (setPositionTo == SetToType.Vector3)
                affectedGameObject.transform.position = positionVector;
            else if (setPositionTo == SetToType.SceneObject)
            {
                var go = positionObj.GetSceneObject(sender, receiver);
                affectedGameObject.transform.position = go.transform.position;
            }
        }
        if (maskInd == (maskInd | (1 << (int)TransformSettings.Rotation)))
        {
            if (setRotationTo == SetToType.Vector3)
                affectedGameObject.transform.rotation = Quaternion.Euler(rotationVector);
            else if (setRotationTo == SetToType.SceneObject)
            {
                var go = rotationObj.GetSceneObject(sender, receiver);
                affectedGameObject.transform.rotation = go.transform.rotation;
            }
        }
        if (maskInd == (maskInd | (1 << (int)TransformSettings.Scale)))
        {
            if (setScaleTo == SetToType.Vector3)
                affectedGameObject.transform.position = scaleVector;
            else if (setScaleTo == SetToType.SceneObject)
            {
                var go = scaleObj.GetSceneObject(sender, receiver);
                affectedGameObject.transform.localScale = go.transform.localScale;
            }
        }
    }
}
