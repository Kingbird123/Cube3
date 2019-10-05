using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnObject", menuName = "Data/Interacts/SpawnObject", order = 1)]
public class InteractFXSpawnObject : InteractFX
{
    public enum SetTransformType { None, Sender, Receiver}
    public enum PosType { None, Sender, Receiver, WorldPos }
    [SerializeField] private GameObject objectToSpawn = null;
    [SerializeField] private SetTransformType setParent = SetTransformType.None;
    [SerializeField] private PosType setPosAndRot = PosType.None;
    [SerializeField] private Vector3 worldPos = Vector3.zero;

    private Transform spawn;

    protected override void DoFX(GameObject _sender, GameObject _receiver)
    {
        spawn = Instantiate(objectToSpawn).transform;
        if (setParent != SetTransformType.None)
        {
            if (setParent == SetTransformType.Receiver)
                spawn.SetParent(_receiver.transform);
            else
                spawn.SetParent(_sender.transform);

        }
        if (setPosAndRot != PosType.None)
        {
            if (setPosAndRot == PosType.Receiver)
                SetPositionAndRotation(_receiver.transform);
            else if (setPosAndRot == PosType.Sender)
                SetPositionAndRotation(_sender.transform);
            else if (setPosAndRot == PosType.WorldPos)
                SetPositionAndRotation(worldPos);
        }
    }

    void SetPositionAndRotation(Vector3 _worldPos)
    {
        spawn.position = _worldPos;
        spawn.rotation = Quaternion.identity;
    }

    void SetPositionAndRotation(Transform _trans)
    {
        spawn.position = _trans.position;
        spawn.rotation = _trans.rotation;
    }
}
