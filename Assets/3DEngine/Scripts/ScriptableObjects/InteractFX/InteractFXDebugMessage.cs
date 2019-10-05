using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugMessage", menuName = "Data/Interacts/DebugMessage", order = 1)]
public class InteractFXDebugMessage : InteractFX
{
    [TextArea]
    [SerializeField] private string message = null;

    protected override void DoFX(GameObject _sender = null, GameObject _receiver = null)
    {
        Debug.Log("| " + message + " | [Sender: " + _sender.name + "] [Receiver: " + _receiver.name + "]" );
    }

}
