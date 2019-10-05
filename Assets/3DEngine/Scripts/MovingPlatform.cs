using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MovingPlatform : MonoBehaviour
{
    public enum TravelType { Timed, ConsistentSpeed }
    public enum MoveType { Linear, EaseIn, EaseOut, SmoothStep }
    public enum LoopType { Loop, PingPong}
    [SerializeField] private TravelType travelType = TravelType.ConsistentSpeed;
    [SerializeField] private MoveType moveType = MoveType.Linear;
    [SerializeField] private float speed = 10;
    [SerializeField] private float travelTime = 10;
    [SerializeField] private float arrivalStopTime = 0;
    private bool stopped;
    [SerializeField] public List<Vector3> points = new List<Vector3>(2); //using list so we can reverse sort for ping pong
    [SerializeField] private LoopType loopType = LoopType.Loop;
    //gui vars
    [SerializeField] private Color lineColor;
    [SerializeField] private Color fontColor;

    public bool PauseMovement { get; set; }
    private Vector3 curPos;
    private Vector3 nextPos;
    private float nextDist;
    private float speedTime;
    private float curTime;

    private int curInd = -1;

    private float lerpTimer;
    private float perc;

	// Use this for initialization
	void Start ()
    {
        if (points.Count < 1)
            return;

        NextWaypoint();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (points.Count < 1)
            return;

        if (travelType == TravelType.Timed && travelTime == 0)
            return;

        if (travelType == TravelType.ConsistentSpeed && speed == 0)
            return;

        if (PauseMovement)
            return;

        MovePlatform();
	}

    void MovePlatform()
    {
        lerpTimer += Time.deltaTime;
        if (lerpTimer > curTime)
        {
            lerpTimer = curTime;
        }
        //linear movement
        perc = lerpTimer / curTime;
        //smoother movements
        if (moveType == MoveType.EaseIn)
            perc = 1f - Mathf.Cos(perc * Mathf.PI * 0.5f);
        else if (moveType == MoveType.EaseOut)
            perc = Mathf.Sin(perc * Mathf.PI * 0.5f);
        else if (moveType == MoveType.SmoothStep)
            perc = perc * perc * (3f - 2f * perc);

        transform.position = Vector3.Lerp(curPos, nextPos, perc);

        if (perc >= 1)
        {
            if (arrivalStopTime > 0)
            {
                if (!stopped)
                    StartCoroutine(StartArrivalStopTime());
            }
            else
                NextWaypoint();
        }
    }

    IEnumerator StartArrivalStopTime()
    {
        stopped = true;
        float timer = 0;
        while (timer < arrivalStopTime)
        {
            timer += Time.deltaTime;
            if (timer > arrivalStopTime)
                timer = arrivalStopTime;
            yield return new WaitForEndOfFrame();
        }
        stopped = false;
        NextWaypoint();
    }

    void NextWaypoint()
    {
        lerpTimer = 0;
        curPos = transform.position;
        curInd++;
        if (curInd < points.Count - 1)
        {
            nextPos = points[curInd + 1];
        }
        else
        {
            if (loopType == LoopType.PingPong)
            {
                points.Reverse();
                curInd = 0;
            }
            else
            {
                curInd = -1;
            }      
            nextPos = points[curInd + 1];
        }

        nextDist = Vector3.Distance(curPos, nextPos);
        //divide distance by speed to get the time
        speedTime = nextDist / speed;

        //swap lerp time values if using speed
        if (travelType == TravelType.ConsistentSpeed)
            curTime = speedTime;
        else
            curTime = travelTime;
    }
        
}
