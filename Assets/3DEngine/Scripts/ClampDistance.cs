using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampDistance : MonoBehaviour
{
    public Transform[] objectsToClamp;
    public Transform centerObject;
    public float maxDistance = 10;
    public float clampSensitivity = 5;

    private bool clamping;

    private void FixedUpdate()
    { 
        CenterMiddle();
        ClampObjects();
    }

    void CenterMiddle()
    {
        if (objectsToClamp.Length < 2)
            return;
        if (clamping)
            return;
        var pos = new Vector3[objectsToClamp.Length];
        for (int i = 0; i < objectsToClamp.Length; i++)
            pos[i] = objectsToClamp[i].position;

            var addedPos = Vector3.zero;
            for (int i = 0; i < pos.Length; i++)
            {
                addedPos += pos[i];
            }
            centerObject.position = addedPos / pos.Length;
        
    }

    void ClampObjects()
    {
        if (!centerObject)
            return;

        var dirs = new Vector3[objectsToClamp.Length];
        var lastpos = new Vector3[objectsToClamp.Length];
        for (int i = 0; i < objectsToClamp.Length; i++)
        {
            var pos = objectsToClamp[i].position;
            dirs[i] = (pos - lastpos[i]).normalized;
            lastpos[i] = pos;
        }

        for (int i = 0; i < objectsToClamp.Length; i++)
        {
            var dist = Vector3.Distance(objectsToClamp[i].position, centerObject.position);
            var dir = (objectsToClamp[i].position - centerObject.position).normalized;
            clamping = dist > maxDistance;
            if (clamping)
            {
                Debug.Log(Vector3.Distance(objectsToClamp[i].position, centerObject.position));
                var newPos = centerObject.position + (dir * maxDistance);
                objectsToClamp[i].position = Vector3.Lerp(objectsToClamp[i].position, newPos, clampSensitivity * Time.deltaTime);
            }
            
                
        }
    }
}
