using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkController : PlayerController
{
    [Header("Drunk Controller Stuff")]
    [SerializeField] private ConfigurableJoint modelJoint = null;
    [SerializeField] private Rigidbody modelRb = null;
    [SerializeField] private float maxAngle = 45;
    private float curAngle;
    public float CurAngle { get { return curAngle; } }
    private bool overleaned;
    public bool IsOverleaned { get { return overleaned; } }
    [SerializeField] private float leanSensitivity = 10;
    [SerializeField] private float balanceSensitivity = 1;
    [SerializeField] private float minAngularDrag = 0;
    [SerializeField] private float maxAngularDrag = 25;
    private float balance;
    public float Balance { get { return balance; } }
    [SerializeField] private float wobbleStrength = 20;
    [SerializeField] private float stumbleStrength = 0.1f;
    [SerializeField] private float wobbleTimeMin = 1;
    [SerializeField] private float wobbleTimeMax = 1;
    private Vector3 wobbleRot;
    private Vector3 lastRot;
    private Vector3 wobbleMove;
    private float wobbleTime;
    private float wobbleTimer;
    [SerializeField] private float speedMultiplierMin = 1;
    [SerializeField] private float speedMultiplierMax = 1.5f;
    [SerializeField] private float speedLerpTimeMin = 1;
    [SerializeField] private float speedLerpTimeMax = 1;

    bool speedPing = true;    
    private Coroutine speedRoutine;
    private bool resetting;

    protected override void GetComponents()
    {
        base.GetComponents();
        balance = modelRb.angularDrag;
    }

    protected override void Move()
    {
        if (speedPing)
            speedRoutine = StartCoroutine(StartSpeedPong());

        rb.AddForce((inputWorldDirection + wobbleMove) * curSpeed * speedMultiplier, ForceMode.Force);
    }

    protected override void Rotate()
    {
        base.Rotate();

        //wobble
        wobbleTimer += Time.deltaTime;
       
        if (wobbleTimer > wobbleTime)
        {
            var dir = Utils.RandomXZDirection();
            wobbleRot = dir * wobbleStrength;
            wobbleMove = dir * stumbleStrength;
            wobbleTime = Random.Range(wobbleTimeMin, wobbleTimeMax);

            modelJoint.targetRotation = Quaternion.Euler(wobbleRot);

            wobbleTimer = 0;
        }

        //check angle
        curAngle = Vector3.Angle(Vector3.up, modelRb.transform.up);
        overleaned = curAngle > maxAngle;

        //balance the character
        if (inputLocalDirection == Vector3.zero)
            balance += balanceSensitivity * Time.deltaTime;
        else
            balance -= leanSensitivity * Time.deltaTime;

        balance = Mathf.Clamp(balance, minAngularDrag, maxAngularDrag);
        modelRb.angularDrag = balance;
            
    }

    //randomize speed with smoothing
    IEnumerator StartSpeedPong()
    {
        speedPing = false;
        var speed = speedMultiplier;
        var randSpeed = Random.Range(speedMultiplierMin, speedMultiplierMax);
        var randTime = Random.Range(speedLerpTimeMin, speedLerpTimeMax);
        float timer = 0;
        float perc = 0;
        while (perc != 1)
        {
            timer += Time.deltaTime;
            if (timer > randTime)
                timer = randTime;
            perc = timer / randTime;

            speedMultiplier = Mathf.Lerp(speed, randSpeed, perc);

            yield return new WaitForEndOfFrame();
        }
        speedPing = true;
    }

}
