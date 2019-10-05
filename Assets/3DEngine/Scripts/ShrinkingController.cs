using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingController : PlayerController
{
    [Header("Shrinking Controller Stuff")]
    [SerializeField] private InputProperty shrinkButton = null;
    [SerializeField] private TagProperty shrinkTag = null;
    [SerializeField] private LayerProperty shrinkLayer = null;
    [SerializeField] private Vector3 shrunkSize = Vector3.one;
    [SerializeField] private bool lockPositionWhenShrinking = false;
    [SerializeField] private float shrinkTime = 1;
    [SerializeField] private float shrinkSpeedMultiplier = 1;
    [SerializeField] private float shrinkJumpPower = 10;

    private bool shrunk;
    public bool IsShrunk { get { return shrunk; } }
    private Vector3 startSize;
    private string startTag;
    private int startLayer;

    protected override void Start()
    {
        base.Start();
        startSize = transform.localScale;
        startTag = gameObject.tag;
        startLayer = gameObject.layer;
    }

    protected override void GetInputs()
    {
        base.GetInputs();

        if (shrinkButton.GetInputDown())
        {
            if (!shrunk)
                Shrink();
            else
                UnShrink();
        }

        if (shrunk)
            speedMultiplier = shrinkSpeedMultiplier;
        else
            speedMultiplier = 1;
            
    }

    void Shrink()
    {
        shrunk = true;
        StartCoroutine(StartSizeChange(shrunkSize, shrinkTag.stringValue, shrinkLayer.indexValue, shrinkSpeedMultiplier, shrinkJumpPower));
    }

    void UnShrink()
    {
        shrunk = false;
        StartCoroutine(StartSizeChange(startSize, startTag, startLayer, 1, curJumpPower));
    }

    IEnumerator StartSizeChange(Vector3 _endScale, string _tag, int _layer, float _multiplier, float _jumpPower)
    {
        float timer = 0;
        Vector3 startSize = transform.localScale;
        var pos = transform.position;
        while (timer < shrinkTime)
        {
            timer += Time.deltaTime;
            if (timer > shrinkTime)
                timer = shrinkTime;
            var perc = timer / shrinkTime;

            if (lockPositionWhenShrinking)
                transform.position = pos;
            transform.localScale = Vector3.Lerp(startSize, _endScale, perc);

            yield return new WaitForFixedUpdate();
        }

        curJumpPower = _jumpPower;
        speedMultiplier = _multiplier;
        gameObject.tag = _tag;
        gameObject.layer = _layer;
    }
}
