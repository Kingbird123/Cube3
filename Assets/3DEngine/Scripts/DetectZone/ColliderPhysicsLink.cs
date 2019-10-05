using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPhysicsLink : MonoBehaviour
{
    private List<Collider> enteredTriggerCols = new List<Collider>();
	public List<Collider> EnteredTriggerCols { get { return enteredTriggerCols; } }
    private List<Collider> enteredCollisionCols = new List<Collider>();
    public List<Collider> EnteredCollisionCols { get { return enteredCollisionCols; } }

    private void OnTriggerEnter(Collider _col)
    {
        enteredTriggerCols.Add(_col);
    }

    private void OnTriggerExit(Collider _col)
    {
        enteredTriggerCols.Remove(_col);
    }

    private void OnCollisionEnter(Collision _col)
    {
        enteredCollisionCols.Add(_col.collider);
    }

    private void OnCollisionExit(Collision _col)
    {
        enteredCollisionCols.Remove(_col.collider);
    }

}
