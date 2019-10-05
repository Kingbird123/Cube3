using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemForcePush : ItemAimable
{
    public new ItemForcePushData Data { get { return (ItemForcePushData)data; } }
    private string pushButton;
    private string pullButton;
    private Collider2D curCol;
    private Collider2D[] cols;
    private ContactFilter2D con;
    private ItemForcePushData.PhysicsProperty useProperty;
    private Vector2 curDirection;

    private bool pushing = true;
    private Coroutine physicsCoroutine;

    protected override void Start()
    {
        base.Start();
        SetupContactFilter();
        pushButton = Data.pushProperty.button.stringValue;
        pullButton = Data.pullProperty.button.stringValue;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopPhysics();
    }

    void SetupContactFilter()
    {
        con = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = Data.mask
        };
    }

    private void Update()
    {
        GetInputs();
    }

    void GetInputs()
    {
        if (Input.GetButtonDown(pushButton))
            physicsCoroutine = StartCoroutine(StartPhysics(true));
        else if (Input.GetButtonUp(pushButton))
            StopPhysics();

        if (Input.GetButtonDown(pullButton))
            physicsCoroutine = StartCoroutine(StartPhysics(false));
        else if (Input.GetButtonUp(pullButton))
            StopPhysics();

    }

    IEnumerator StartPhysics(bool _push)
    {
        ActivatePhysics(_push);
        while (true)
        {
            DoPhysics();
            if (curCol && muzzle)
            {
                curCol.transform.position = muzzle.position;
                //curCol.transform.LookAt2D(controller.AimPos, false, true);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    

    void ActivatePhysics(bool _push)
    {
        if (_push)
        {
            pushing = true;
            useProperty = Data.pushProperty;
            if (!curCol)
                curCol = Instantiate(Data.pushProperty.forceArea, muzzle.position, muzzle.rotation);
        }
        else
        {
            pushing = false;
            useProperty = Data.pullProperty;
            if (!curCol)
                curCol = Instantiate(Data.pullProperty.forceArea, muzzle.position, muzzle.rotation);
        }
       
    }

    void DoPhysics()
    {
        if (!curCol)
            return;

        cols = new Collider2D[Data.maxObjects];
        Physics2D.OverlapCollider(curCol, con, cols);
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i] != null)
            {
                var rb = cols[i].GetComponent<Rigidbody2D>();
                if (rb)
                {
                    if (useProperty.consistentForce)
                        rb.Sleep();
                    //muzzle.LookAt2D(controller.AimPos);
                    var dir = muzzle.TransformDirection(Vector2.right);
                    if (!pushing)
                        dir = -dir;
                    rb.AddForce(dir * useProperty.force, ForceMode2D.Impulse);
                }
                    
            }

        }     

    }

    void StopPhysics()
    {
        if (curCol)
            Destroy(curCol.gameObject);
        if (physicsCoroutine != null)
            StopCoroutine(physicsCoroutine);
    }

}
