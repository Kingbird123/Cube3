using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySplash : MonoBehaviour 
{

    [SerializeField] private DetectZone zone;

    [SerializeField] private float jumpDelay = 3;
    private float timer;
    [SerializeField] private float jumpTime = 1;
    private float jumpTimer;
    [SerializeField] private Transform startPoint = null;
    [SerializeField] private Transform endPoint = null;
    private enum SmoothOptions { Linear, SmoothStep, SmootherStep, EaseInOut};
    [SerializeField] private SmoothOptions smoothType = SmoothOptions.EaseInOut;
    [SerializeField] private bool rotate = false;
    private float rotTimer;

    private bool jumping;

    [SerializeField] private Quaternion upRot = Quaternion.identity;
    [SerializeField] private Quaternion downRot = Quaternion.identity;

    void Start()
    {
        //upRot = Quaternion.LookRotation(endPoint.position - startPoint.position);
       // downRot = Quaternion.LookRotation(startPoint.position - endPoint.position);
    }

	// Update is called once per frame
	void Update () 
	{
		if (!jumping)
        {
            timer += Time.deltaTime;
            if (timer > jumpDelay)
            {
                timer = 0;
                jumping = true;
                StartCoroutine(Jump());
            }
        }
	}

    IEnumerator Jump()
    {
        bool heightReached = false;
        bool startReached = false;
        float splitTime = jumpTime / 2;
        var rot1 = transform.rotation;
        var rot2 = transform.rotation;

        while (!heightReached)
        {
            LerpPos(true, splitTime, startPoint.position, endPoint.position, out heightReached);

            if (rotate)
                SmoothLookAt(transform.rotation ,upRot, splitTime);

            yield return new WaitForEndOfFrame();
        }

        Vector2 pos = transform.position;

        while (!startReached)
        {
            LerpPos(false, splitTime, pos, startPoint.position, out startReached);

            if (rotate)
                SmoothLookAt(transform.rotation, downRot, splitTime);

            yield return new WaitForEndOfFrame();
        }
        jumping = false;

    }

    void LerpPos(bool _up, float _time, Vector2 _startPos, Vector2 _endPos, out bool _finished)
    {
        _finished = false;
        jumpTimer += Time.deltaTime;

        if (jumpTimer > _time)
            jumpTimer = _time;

        float perc = jumpTimer / _time;

        if (smoothType == SmoothOptions.EaseInOut)
        {
            if (_up)
                perc = Mathf.Sin(perc * Mathf.PI * 0.5f); //ease out
            else
                perc = 1 - Mathf.Cos(perc * Mathf.PI * 0.5f); //ease in
        }
        else if (smoothType == SmoothOptions.SmoothStep)
            perc = perc * perc * (3 - 2 * perc);
        else if (smoothType == SmoothOptions.SmootherStep)
            perc = perc * perc * perc * (perc * (6 * perc - 15) + 10);
            

        transform.position = Vector2.Lerp(_startPos, _endPos, perc);

        if (perc >= 1)
        {
            _finished = true;
            jumpTimer = 0;
        }         
    }

    void SmoothLookAt(Quaternion _startRot, Quaternion _endRot, float _time)
    {
        rotTimer += Time.deltaTime;
        if (rotTimer > _time)
        {
            rotTimer = _time;
            rotTimer = 0;
        }
        float perc = rotTimer / _time;

        transform.rotation = Quaternion.Slerp(_startRot, _endRot, perc);

    }

}
