using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : PlayerController
{
    [Header("Ball Controller Stuff")]

    [SerializeField] protected bool growWithDistance;
    public bool GrowWithDistance { get { return growWithDistance; } set { growWithDistance = value; } }
    [SerializeField] protected float growRate = 0.01f;
    public float GrowRate { get { return growRate; } set { growRate = value; } }
    [SerializeField] protected bool clampScale;
    public bool ClampScale { get { return clampScale; } set { clampScale = value; } }
    [SerializeField] protected float minScale = 1;
    public float MinScale { get { return minScale; } set { minScale = value; } }
    [SerializeField] protected float maxScale = 10;
    public float MaxScale { get { return maxScale; } set { maxScale = value; } }

    private Vector3 lastPos;

    protected override void GetComponents()
    {
        base.GetComponents();
        lastPos = transform.position;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        DetectGrowing();
    }

    protected override void Move()
    {
        rb.AddForce(inputWorldDirection * curSpeed, ForceMode.Force);
    }

    protected override void Rotate()
    {  
    }

    void DetectGrowing()
    {
        if (!growWithDistance)
            return;

        var distance = Vector3.Distance(transform.position, lastPos);

        var growAdd = distance * growRate;
        ScaleDelta(growAdd);

        lastPos = transform.position;
    }

    public void ScaleDelta(float _amount)
    {
        var growX = transform.localScale.x + _amount;
        var growY = transform.localScale.y + _amount;
        var growZ = transform.localScale.z + _amount;
        var grow = new Vector3(growX, growY, growZ);

        transform.localScale = grow;

        if (clampScale)
        {
            if (transform.localScale.x > maxScale)
                transform.localScale = Vector3.one * maxScale;
            if (transform.localScale.x < minScale)
                transform.localScale = Vector3.one * minScale;
        }
            
    }

}
