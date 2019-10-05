using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHookShot : ItemAimable
{
    public new ItemHookShotData Data { get { return (ItemHookShotData)data; } }
    private float timer = 0;
    private float dragTime = 0;
    private float distance = 0;
    private Collider2D hookedCol = null;
    private Transform unitTrans = null;
    private Collider2D playerCol;
    private Vector2 direction = Vector2.zero;
    private Vector2 hookPoint = Vector2.zero;
    private LineRenderer hookLine = null;
    private GameObject hookSpawn = null;
    private bool collided = false;
    private bool retracting = false;
    private DistanceJoint2D joint = null;
    private PlayerController cont = null;

    private bool use;

    private List<Collider2D> damagedCols = new List<Collider2D>();

    private Coroutine shootCoroutine;
    private Coroutine hookedCoroutine;
    private bool cancelHook;

    private void Update()
    {
        GetInputs();
    }

    void GetInputs()
    {
        if (Data.shootButton.GetInputDown())
        {
            FireHookShot();
        }
        else if (Data.shootButton.GetInputUp() && Data.shootType == ItemHookShotData.ShootType.HoldButton)
        {
            RetractHookShot(muzzle);
        }

        if (Data.cancelButton.GetInputDown())
        {
            RetractHookShot(muzzle);
        }


    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DestroyHookShot();
    }

    void FireHookShot()
    {
        if (retracting || hookSpawn)
            return;

        if (shootCoroutine != null)
            StopCoroutine(shootCoroutine);
        shootCoroutine = StartCoroutine(StartFireHookShot());
    }

    IEnumerator StartFireHookShot()
    {
        //get player
        unitTrans = curUnitOwner.transform;
        //direction = controller.AimDirection;
        timer = 0;
        Vector2 startPos = muzzle.transform.position;
        var endPos = startPos + (direction * Data.fireSpeed);
        hookSpawn = Instantiate(Data.hookPrefab, startPos, Quaternion.LookRotation(direction));
        hookLine = Instantiate(Data.lineRenderer, startPos, Quaternion.identity).GetComponent<LineRenderer>();
        Vector2 lastPos = hookSpawn.transform.position;
        RaycastHit2D hit = new RaycastHit2D();
        RaycastHit2D cancelHit = new RaycastHit2D();
        damagedCols.Clear();
        cancelHook = false;
        while (timer < Data.lifeTime && !hit.collider && hookSpawn && !cancelHit && !cancelHook)
        {
            timer += Time.deltaTime;
            if (timer > Data.lifeTime)
                timer = Data.lifeTime;
            var perc = timer / Data.lifeTime;

            hookSpawn.transform.position = Vector2.Lerp(startPos, endPos, perc);

            //detect if hit hookable area
            hit = Physics2D.Linecast(lastPos, hookSpawn.transform.position, Data.hookableSurfaceMask);
            cancelHit = Physics2D.Linecast(lastPos, hookSpawn.transform.position, Data.cancelShotMask);

            //set line positions
            SetLinePositions(muzzle, hookSpawn.transform.position);

            //do damage
            DetectDamage();

            lastPos = hookSpawn.transform.position;
            yield return new WaitForFixedUpdate();
        }
        if (hit.collider)
        {
            hookedCol = hit.collider;
            if (Data.dragType == ItemHookShotData.DragType.StraightAuto)
                GameManager.instance.StartCoroutine(StartDragPlayer(muzzle, hit.point));
            else
            {
                GameManager.instance.StartCoroutine(StartControlledDrag(muzzle, hit.point, hookedCol.transform));
            }
                
        }
        else
            RetractHookShot(muzzle);
    }

    void SetLinePositions(Transform _source, Vector2 _target)
    {
        hookLine.SetPosition(0, _source.position);
        hookLine.SetPosition(1, _target);
    }

    void DetectDamage()
    {
        var cols = Physics2D.OverlapCircleAll(hookSpawn.transform.position, Data.damageRadius, Data.damageMask);
        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (!damagedCols.Contains(cols[i]))
                {
                    damagedCols.Add(cols[i]);
                    DoDamage(cols[i]);
                }
                
            }
            cancelHook = Data.retractOnDamage;
        }
    }

    void DoDamage(Collider2D _col)
    {
        var unit = _col.GetComponent<Unit>();
        //if (unit)
           // unit.DamageHp((int)Data.damage);
    }

    IEnumerator StartControlledDrag(Transform _senderPos, Vector2 _targetPos, Transform _hitTrans)
    {
        //get distance
        distance = Vector2.Distance(_senderPos.position, _targetPos);
        cont = unitTrans.GetComponent<PlayerController>();
        //cont.IsGrappling = true;
        //add joint component
        joint = unitTrans.gameObject.AddComponent<DistanceJoint2D>();
        joint.enableCollision = true;
        joint.autoConfigureDistance = false;
        joint.autoConfigureConnectedAnchor = false;

        //get anchor local target pos
        var localPos = _hitTrans.InverseTransformPoint(_targetPos);

        //set joint distance
        joint.distance = distance;
        joint.anchor = unitTrans.InverseTransformPoint(_senderPos.position);
        while (!retracting)
        {
            //set joint anchor position relative to collider
            var anchorPos = _hitTrans.TransformPoint(localPos);
            joint.connectedAnchor = anchorPos;
            hookSpawn.transform.position = anchorPos;

            //if (!cont.IsSideHitLeft && !cont.IsSideHitRight)
            //{
                float vInput = Input.GetAxis("Vertical");
                joint.distance += -vInput * Data.controlSpeed * Time.deltaTime;
                if (joint.distance < Data.minDistance)
                    joint.distance = Data.minDistance;
            //}
            
            //set line positions
            SetLinePositions(_senderPos, hookSpawn.transform.position);

            yield return new WaitForFixedUpdate();
        }
        //cont.IsGrappling = false;
        Destroy(joint);
    }

    IEnumerator StartDragPlayer(Transform _senderPos, Vector2 _targetPos)
    {

        //drag player
        collided = false;
        timer = 0;
        var startPos = unitTrans.position;
        distance = Vector2.Distance(_senderPos.position, _targetPos);
        dragTime = distance / Data.dragSpeed;
        hookSpawn.transform.position = _targetPos;
        while (timer<dragTime && !collided)
        {
            timer += Time.deltaTime;
            if (timer > dragTime)
                timer = dragTime;
            float perc = timer / dragTime;
            unitTrans.GetComponent<PlayerController>().DisableMovement(true);
            unitTrans.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(startPos, _targetPos, perc));
            Collider2D col = unitTrans.GetComponent<Collider2D>();
            Vector2 nearestPoint = col.Distance(hookedCol).pointA;
            collided = Physics2D.OverlapCircle(nearestPoint, Data.collisionRadius, Data.obstacleCollisionMask);

            //set line positions
            SetLinePositions(_senderPos, hookSpawn.transform.position);

            yield return new WaitForFixedUpdate();
        }
        unitTrans.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        unitTrans.GetComponent<PlayerController>().DisableMovement(false);
        DestroyHookShot();
    }

    void RetractHookShot(Transform _senderPos)
    {
        if (!retracting && hookSpawn)
            StartCoroutine(StartRetract(_senderPos));
    }

    IEnumerator StartRetract(Transform _senderPos)
    {
        retracting = true;
        distance = Vector2.Distance(_senderPos.position, hookSpawn.transform.position);
        float timer = 0;
        var time = distance / Data.fireSpeed;
        var startPos = hookSpawn.transform.position;
        damagedCols.Clear();
        while (timer < time && hookSpawn)
        {
            timer += Time.deltaTime;
            if (timer > time)
                timer = time;
            float perc = timer / time;
            hookSpawn.transform.position = Vector2.Lerp(startPos, _senderPos.position, perc);

            //set line positions
            SetLinePositions(_senderPos, hookSpawn.transform.position);

            //damage
            DetectDamage();

            yield return new WaitForFixedUpdate();
        }
        retracting = false;
        DestroyHookShot();
    }

    void DestroyHookShot()
    {
        if (hookSpawn)
        {
            Destroy(hookSpawn.gameObject);
            Destroy(hookLine.gameObject);
        }
        if (joint)
        {
            Destroy(joint);
            //cont.IsGrappling = false;
        }

    }
    
}
