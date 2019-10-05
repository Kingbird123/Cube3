using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallThrough : MonoBehaviour 
{

    private bool ignoring;

    private bool down;

    [SerializeField] private float ignoreTime = 1;
    [SerializeField] private LayerMask mask = -1;

    private PlayerController pc;
    private Vector2 raycastPosition;

    void Start()
    {
        pc = GetComponent<PlayerController>();
        raycastPosition = new Vector2(0,GetComponent<CapsuleCollider2D>().size.y);
    }

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetAxisRaw("Vertical") < 0 && !down)
            down = true;
        else if(down)
            down = false;

        if (down && !ignoring)
        {
            StartCoroutine(StartIgnore());
        }

        //check if player is jumping into fall through...
        if (!pc.IsGrounded)
        {
            bool upperHit = Physics2D.Raycast(transform.TransformPoint(raycastPosition), Vector2.up, 0.5f, mask);
            Debug.DrawRay(transform.TransformPoint(raycastPosition), Vector2.up);
            if (upperHit && !ignoring)
            {
                StartCoroutine(StartIgnore());
            }
        }
	}

    void IgnoreLayers(bool _fall)
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("FallThrough"), _fall);
    }

    IEnumerator StartIgnore()
    {
        ignoring = true;
        IgnoreLayers(true);
        yield return new WaitForSeconds(ignoreTime);
        IgnoreLayers(false);
        ignoring = false;
    }

}
