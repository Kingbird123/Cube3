using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractFXSequenceFX", menuName = "Data/Interacts/InteractFXSequenceFX", order = 1)]
public class InteractFXSequenceFX : InteractFX
{
    [SerializeField] private InteractFXSequence sequence = null;

    protected override void DoFX(GameObject _sender, GameObject _receiver)
    {
        sequence.ActivateFXSequence(_sender, _receiver);
    }
}
