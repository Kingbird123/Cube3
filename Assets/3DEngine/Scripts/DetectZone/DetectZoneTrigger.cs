using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class DetectZoneTrigger : MonoBehaviour
{
    [SerializeField] protected DetectZone detectZone;
    public DetectZone DetectZone { get { return detectZone; } }

    public bool IsDetected { get { return enteredCols.Count > 0; } }

    private List<Collider> enteredCols = new List<Collider>();

    protected virtual void Start()
    {
        detectZone.AddEnterTrigger(this, OnEnter);
        detectZone.AddStayTrigger(this, OnStay);
        detectZone.AddExitTrigger(this, OnExit);
    }

    protected virtual void OnEnter(Collider _col)
    {
        if (!enteredCols.Contains(_col))
            enteredCols.Add(_col);
    }

    protected virtual void OnStay(Collider _col)
    {
    }

    protected virtual void OnExit(Collider _col)
    {
        if (enteredCols.Contains(_col))
            enteredCols.Remove(_col);
    }

}
