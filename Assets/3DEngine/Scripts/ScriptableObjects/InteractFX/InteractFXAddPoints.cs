using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddPoints", menuName = "Data/Interacts/AddPoints", order = 1)]
public class InteractFXAddPoints : InteractFX
{

    [SerializeField] private int pointsToAdd = 1;

    protected override void DoFX(GameObject _sender, GameObject _receiver)
    {
        _receiver.GetComponent<Player>().AddPoints(pointsToAdd);
    }
}
