using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour 
{
    [SerializeField]
    private float lifeTime = 1;

    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

}
