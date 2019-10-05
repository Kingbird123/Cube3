using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetActive", menuName = "Data/Interacts/SetActive", order = 1)]
public class InteractFXSetActive : InteractFX
{
    public enum SetActiveType { Sender, Receiver }

    [SerializeField] private SetActiveType setActiveType = SetActiveType.Receiver;
    [SerializeField] private bool setActive = false;

    protected override void DoFX(GameObject _sender = null, GameObject _receiver = null)
    {
        if (setActiveType == SetActiveType.Sender)
           _sender.SetActive(setActive);
        else
            _receiver.SetActive(setActive);
    }

}
