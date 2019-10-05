using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitAI : MonoBehaviour
{
    public DetectZone[] detectZones = null;
    [SerializeField] private UnitAITriggerContainer unitAIEvents = null;

    protected UnitController controller;
    public UnitController Controller { get { return controller; } }
    protected Bounds bounds;

    private void Start()
    {
        GetComponents();
        ActivateAIEvents(true);
    }

    void GetComponents()
    {
        controller = GetComponent<UnitController>();
        bounds = GetComponent<Collider>().bounds;
    }
    
    public void ActivateAIEvents(bool _activate)
    {
        unitAIEvents.ActivateAllEvents(this, _activate);
    }

    public string[] GetDetectZoneNames()
    {
        var detectNames = new string[detectZones.Length];
        for (int i = 0; i < detectZones.Length; i++)
        {
            detectNames[i] = detectZones[i].zoneName;
        }
        return detectNames;
    }
}