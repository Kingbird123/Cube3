using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeSkin", menuName = "Data/Interacts/ChangeSkin", order = 1)]
public class InteractFXChangeSkin : InteractFX
{
    [SerializeField] private bool needPrerequisiteSkin = false;
    [SerializeField] private UnitData prerequisiteSkin = null;
    [SerializeField] private bool copyLocalUnitValues = false;
    [SerializeField] private float changeSkinDelay = 0;
    [SerializeField] private UnitData skinData = null;
    [SerializeField] private bool freezeReciever = false;
    [SerializeField] private bool doAnimations = false;
    [SerializeField] private float crossFadeTime = 0;
    [SerializeField] private string senderAnimToPlay = null;
    [SerializeField] private string receiverAnimToPlay = null;
    [SerializeField] private bool spawnObjectAfterChange = false;
    [SerializeField] private GameObject[] spawnObjects = null;
    [SerializeField] private bool destroySenderAfterChange = false;
    [SerializeField] private bool destroyRoot = false;
    [SerializeField] private float destroyDelay = 0;

    protected override void DoFX(GameObject _sender = null, GameObject _receiver = null)
    {
        var unit = _receiver.GetComponent<Unit>();
        if (unit)
        {
            if (needPrerequisiteSkin)
            {
                if (unit.CurData != prerequisiteSkin)
                {
                    Debug.Log("Invalid skin prerequisite!");
                    return;
                }
            }
            unit.StartCoroutine(StartChangeSkin(_sender, _receiver));
        }
        else
        {
            Debug.Log("receiver is not a unit!");
            return;
        }
    }

    IEnumerator StartChangeSkin(GameObject _sender = null, GameObject _receiver = null)
    {
        var receiver = _receiver.GetComponent<Unit>();
        var sender = _sender.GetComponent<Unit>();
        var equip = _receiver.GetComponent<UnitEquip>();
        var cont = _receiver.GetComponent<UnitController>();
        var pos = _receiver.transform.position;
        var rot = _receiver.transform.rotation;
        var endPos = _sender.transform.position;
        var endRot = _sender.transform.rotation;
        if (cont)
        {
            cont.DisableAiming(true);
            cont.DisableMovement(true);
            cont.JumpEnabled = false;
        }

        if (doAnimations)
        {
            if (_sender)
            {
                var animSend = _sender.GetComponentInChildren<Animator>();
                if (animSend)
                    animSend.CrossFade(senderAnimToPlay, crossFadeTime);
            }
            if (_receiver)
            {
                var animRec = _receiver.GetComponentInChildren<Animator>();
                if (animRec)
                    animRec.CrossFade(receiverAnimToPlay, crossFadeTime);
            }

        }
        //make sure receiver lines up with sender
        float timer = 0;
        while (timer < crossFadeTime && _sender)
        {
            timer += Time.deltaTime;
            var perc = timer / crossFadeTime;
            receiver.transform.position = Vector2.Lerp(pos, endPos, perc);
            receiver.transform.rotation = Quaternion.Lerp(rot, endRot, perc);
            yield return new WaitForFixedUpdate();
        }
        receiver.transform.rotation = endRot;
        receiver.transform.position = endPos;
        //freeze player position
        timer = 0;
        while (timer < changeSkinDelay)
        {
            timer += Time.deltaTime;
            if (freezeReciever)
            {
                _receiver.transform.position = pos;
                _receiver.transform.rotation = endRot;
            }
            yield return new WaitForFixedUpdate();
        }
        if (cont)
        {
            cont.DisableAiming(false);
            cont.DisableMovement(false);
            cont.JumpEnabled = true;
        }
        //do swap
        if (receiver)
        {
            if (copyLocalUnitValues)
            {
                if (sender)
                    receiver.ChangeSkin(sender);
                else
                    Debug.LogError("You need a unit component on: " + _sender + " to copy local values.");
            }
            else
                receiver.ChangeSkin(skinData, true);
        }

        SpawnObjects(endPos, endRot);
        DestroySender(_sender);
    }

    void SpawnObjects(Vector2 _pos, Quaternion _rot)
    {
        if (!spawnObjectAfterChange)
            return;

        foreach (var spawn in spawnObjects)
        {
            Instantiate(spawn, _pos, _rot);
        }
    }

    void DestroySender(GameObject _obj)
    {
        if (!destroySenderAfterChange)
            return;

        if (_obj)
        {
            var objToDestroy = _obj;
            if (destroyRoot)
            {
                if (_obj.transform.root)
                    objToDestroy = _obj.transform.root.gameObject;
            }

            Destroy(objToDestroy, destroyDelay);
        }

    }
}
