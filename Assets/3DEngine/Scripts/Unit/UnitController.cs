using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MEC;

[RequireComponent(typeof(Unit))]
public class UnitController : MonoBehaviour
{

    public enum BaseControllerType { Rigidbody, CharacterController, NavAgent }
    public enum MoveForceType { TransformTranslate, RBAddForce, RBMove }
    public enum GravityForceType { None, Local, World }
    public enum MovementType { Idle, Patrol, Wander, LookAt, Chase, Flee }
    public enum PatrolType { LocalOffset, WorldPosition, FromData }

    //controller type
    [SerializeField] protected BaseControllerType controllerType = BaseControllerType.Rigidbody;
    [SerializeField] protected MoveForceType moveForceType = MoveForceType.TransformTranslate;
    [SerializeField] protected NavMeshAgent agent;
    public NavMeshAgent Agent { get { return agent; } }
    public bool IsNavAgent { get { return agent != null; } }

    //movement
    [SerializeField] protected MovementType defaultMovement = MovementType.Idle;
    [SerializeField] protected MovementType curMovement = MovementType.Idle;
    [SerializeField] protected Transform target = null;
    [SerializeField] protected bool doDefaultMovementIfNoTargets = true;
    protected Transform curChaseTarget;
    public Transform CurChaseTarget { get { return curChaseTarget; } set { curChaseTarget = value; } }
    protected Transform curFleeTarget;
    public Transform CurFleeTarget { get { return curFleeTarget; } set { curFleeTarget = value; } }
    protected Transform curLookAtTarget;
    public Transform CurLookAtTarget { get { return curLookAtTarget; } set { curLookAtTarget = value; } }
    private List<Transform> chaseTargets = new List<Transform>();
    private List<Transform> fleeTargets = new List<Transform>();
    private List<Transform> lookAtTargets = new List<Transform>();

    //priorities
    [SerializeField] protected EntityDataManager entityDataManager = null;
    [SerializeField] protected EntityPriority[] chasePriorities = null;
    [SerializeField] protected EntityPriority[] fleePriorities = null;
    [SerializeField] protected EntityPriority[] lookAtPriorities = null;

    //patrolling
    [SerializeField] protected PatrolType patrolType = PatrolType.FromData;
    [SerializeField] protected PatrolData[] patrolDatas = null;
    [SerializeField] protected PinPoint[] patrolPoints = null;
    protected Vector3[] curPoints;
    protected int patrolInd;

    //wandering
    [SerializeField] protected float wanderRadius = 3;
    [SerializeField] protected float minNextDistance = 1;
    [SerializeField] protected bool resetWanderCenterOnReset = false;
    protected Vector3 wanderCenter;
    public Vector3 WanderCenter { get { return wanderCenter; } }

    //chase and flee settings
    [SerializeField] protected float chaseIntervalCheckTime = 1;
    protected float chaseTimer;
    [SerializeField] protected float fleeIntervalCheckTime = 1;
    protected float fleeTimer;
    [SerializeField] protected float fleeSearchDistance = 10;

    //common settings
    [SerializeField] protected float arrivalDistance = 0.1f;
    [SerializeField] protected float arrivalTime = 0;
    [SerializeField] protected float stuckCheckTime = 5;
    protected float stuckTimer;
    protected bool stuck;
    public bool IsStuck { get { return stuck; } }
    protected bool arrived;
    protected bool findingNextWayPoint;
    protected Vector3 curDestination;
    public Vector3 CurDestination { get { return curDestination; } }

    //rotation
    [SerializeField] protected float rotSensitivity = 5;
    public float RotSensitivity { get { return rotSensitivity; } set { rotSensitivity = value; } }
    [SerializeField] protected bool ignoreLeanRotation = true;
    protected float lookHor;
    protected float lookVer;

    //climbing
    [SerializeField] protected bool enableClimbing = true;
    public bool ClimbingEnabled { get { return enableClimbing; } set { enableClimbing = value; } }
    public enum ClimbingDetectType { SideDetection, External }
    [SerializeField] private ClimbingDetectType climbingDetect = ClimbingDetectType.SideDetection;
    [SerializeField] protected float climbSpeed = 6;
    public bool climbing;
    protected bool onLadder;
    public bool IsOnLadder { get { return onLadder; } set { onLadder = value; } }
    [SerializeField] protected float inAirControlTime = 1;
    protected bool controlFading;

    //Physics settings
    [SerializeField] protected GravityForceType addedGravityType = GravityForceType.None;
    [SerializeField] protected float forceAmount = 0;
    [SerializeField] protected Vector3 forceDirection = Vector3.down;
    protected Vector3 curGravityForce;
    protected float curVerticalForce;

    //Ground Detection
    [SerializeField] protected bool useCharacterControllerGroundDetection;
    [SerializeField] protected LayerMask groundMask = -1;
    [SerializeField] protected LayerMask platformMask = -1;
    [SerializeField] public Vector3 groundBoxSize = Vector3.one;
    [SerializeField] protected Vector3 groundBoxCenter = Vector3.zero;
    protected Vector3 curGroundCenter;
    public Vector3 CurGroundCenter { get { return curGroundCenter; } set { curGroundCenter = value; } }
    protected Collider[] groundHits;
    protected bool onPlatform;
    protected GameObject currentGroundGO;
    protected bool grounded;
    public bool IsGrounded { get { return grounded; } }
    protected float lastYPos;
    protected float curYPos;

    //Side Detection
    [SerializeField] protected LayerMask sideMask = -1;
    [SerializeField] protected Vector3 sideDetectCenter = Vector3.zero;
    protected Vector3 curSideDetectCenter;
    public Vector3 CurSideDetectCenter { get { return curSideDetectCenter; } set { curSideDetectCenter = value; } }
    [SerializeField] protected float idleDetectRadius = 0.3f;
    [SerializeField] protected float movingDetectRadius = 0.3f;
    [SerializeField] protected float sideDetectHeight = 0.6f;
    [SerializeField] protected float sideDetectDistanceMultiplier = 0.6f;
    [SerializeField] protected LayerMask slowMask;
    [SerializeField] protected float slowMultiplier = 0.1f;
    [SerializeField] protected LayerProperty ladderLayer;
    protected Collider[] sideCols;

    [SerializeField] protected float baseSpeed = 1;
    public float BaseSpeed { get { return baseSpeed; } set { baseSpeed = value; } }
    [SerializeField] protected bool enableJump = true;
    public bool JumpEnabled { get { return enableJump; } set { enableJump = value; } }
    protected float curJumpPower;
    public float JumpPower { get { return curJumpPower; } set { curJumpPower = value; } }
    protected float curSpeed;
    public float CurSpeed { get { return curSpeed; } }
    protected float speedMultiplier = 1;
    public float SpeedMultiplier { get { return speedMultiplier; } set { speedMultiplier = value; } }
    protected bool speedEffected;
    public bool IsSpeedEffected { get { return speedEffected; } set { speedEffected = value; } }
    protected float velocitySpeed;
    public float VelocitySpeed { get { return velocitySpeed; } }
    protected Vector3 inputLocalDirection;
    public Vector3 InputLocalDirection { get { return inputLocalDirection; } }
    protected Vector3 velocityDirection;
    public Vector3 VelocityDirection { get { return velocityDirection; } }
    public Vector3 LocalVelocityDirection { get { return transform.InverseTransformDirection(velocityDirection) * velocitySpeed; } }
    protected Vector3 movementInput;

    protected float aimDistance = 10;
    protected LayerMask aimMask = -1;
    protected Vector3 aimOrigin;
    public Vector3 AimOrigin { get { return aimOrigin; } }
    protected Vector3 aimPosition;
    public Vector3 AimPosition { get { return aimPosition; } }
    protected Vector3 aimHitNormal;
    public Vector3 AimHitNormal { get { return aimHitNormal; } }
    protected GameObject aimHitObject;
    public GameObject AimHitObject { get { return aimHitObject; } }
    protected GameObject lastAimHitObject;
    protected Vector3 aimDirection;
    public Vector3 AimDirection { get { return aimDirection; } }
    protected bool movingBackwards;
    protected bool movingSideways;

    protected BoolWrapper disableMovement = new BoolWrapper(false);
    public bool IsMovementDisabled { get { return disableMovement.Value; } }

    protected BoolWrapper disableAiming = new BoolWrapper(false);
    public bool IsAimingDisabled { get { return disableAiming.Value; } }

    protected Unit unit;
    protected UnitAnimations anim;
    protected UnitAnimations Anim { get { return anim; } }
    protected UnitEquip equip;
    protected ItemAimable aimable;
    protected Rigidbody rb;
    protected CharacterController cc;
    protected bool bouncing;

    protected Vector3 startColCenter;
    public Vector3 StartColCenter { get { return startColCenter; } set { startColCenter = value; } }
    protected float startColHeight;
    public float StartColHeight { get { return startColHeight; } set { startColHeight = value; } }

    protected CoroutineHandle stopSpeedCoroutine;

    private Vector3 lastPos;

    protected virtual void Start()
    {
        InitializeBaseControllerType();
        GetComponents();
        InitializeController();
        BeginMovement();
    }

    protected virtual void Update()
    {
        CheckChaseTarget();
        CheckFleeTarget();
        CheckLookAtTarget();
        GetAimPosition();
        SyncAnimations();
    }

    protected virtual void FixedUpdate()
    {
        CalculateSpeed();
        CheckGrounded();
        CheckPlatform();
        CheckSideHits();
        CheckStuck();
        CheckMovement();
        ClampAiming();
        AddGravity();
    }

    protected virtual void InitializeBaseControllerType()
    {

    }

    protected virtual void GetComponents()
    {
        //get comps
        unit = GetComponent<Unit>();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
        equip = GetComponent<UnitEquip>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<UnitAnimations>();
    }

    public virtual void InitializeController()
    {
        curSpeed = baseSpeed;
        curGroundCenter = groundBoxCenter;

        if (target)
            curChaseTarget = target;

        //set Direction
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    void BeginMovement()
    {
        //movement type
        curMovement = defaultMovement;

        if (curMovement == MovementType.Patrol)
        {
            var points = patrolPoints;
            if (patrolType == PatrolType.FromData)
            {
                if (patrolDatas.Length < 1)
                {
                    Debug.Log("need patrol data to patrol on " + gameObject.name + "!");
                    return;
                }

                var randInd = Random.Range(0, patrolDatas.Length);
                points = patrolDatas[randInd].patrolPoints;

            }

            curPoints = new Vector3[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                curPoints[i] = points[i].position;
                if (patrolType == PatrolType.LocalOffset)
                    curPoints[i] = transform.TransformPoint(points[i].position);
            }

            patrolInd = -1;
            NextPatrolPoint();
        }
        else if (curMovement == MovementType.Wander)
        {
            //wander stuff
            wanderCenter = transform.position;
            NextWanderPoint();
        }
    }

    void CalculateSpeed()
    {
        if (agent)
        {
            velocityDirection = agent.velocity;
            velocitySpeed = agent.velocity.magnitude;
        }
        else
        {
            velocityDirection = transform.position - lastPos;
            velocitySpeed = (velocityDirection / Time.deltaTime).magnitude;
        }
        velocityDirection.Normalize();
        lastPos = transform.position;
    }

    public void AddChaseTarget(Transform _target)
    {
        if (!chaseTargets.Contains(_target))
            chaseTargets.Add(_target);

        RefreshChaseTarget();
    }

    public void RemoveChaseTarget(Transform _target)
    {
        if (chaseTargets.Contains(_target))
            chaseTargets.Remove(_target);

        RefreshChaseTarget();
    }

    public void AddFleeTarget(Transform _target)
    {
        if (!fleeTargets.Contains(_target))
            fleeTargets.Add(_target);

        RefreshFleeTarget();
    }

    public void RemoveFleeTarget(Transform _target)
    {
        if (fleeTargets.Contains(_target))
            fleeTargets.Remove(_target);

        RefreshFleeTarget();
    }

    public void AddLookAtTarget(Transform _target)
    {
        if (!lookAtTargets.Contains(_target))
            lookAtTargets.Add(_target);

        RefreshLookAtTarget();
    }

    public void RemoveLookAtTarget(Transform _target)
    {
        if (lookAtTargets.Contains(_target))
            lookAtTargets.Remove(_target);

        RefreshLookAtTarget();
    }

    protected virtual void CheckFleeTarget()
    {
        if (controllerType != BaseControllerType.NavAgent)
            return;

        if (curMovement != MovementType.Flee)
            return;

        if (!curFleeTarget)
            RefreshFleeTarget();
    }

    protected virtual void CheckChaseTarget()
    {
        if (controllerType != BaseControllerType.NavAgent)
            return;

        if (curMovement != MovementType.Chase)
            return;

        if (!curChaseTarget)
            RefreshChaseTarget();
    }

    protected virtual void CheckLookAtTarget()
    {
        if (controllerType != BaseControllerType.NavAgent)
            return;

        if (curMovement != MovementType.LookAt)
            return;

        if (!curLookAtTarget)
            RefreshLookAtTarget();
    }

    void RefreshChaseTarget()
    {
        curChaseTarget = RefreshTargetList(chaseTargets, chasePriorities, MovementType.Chase);
    }

    void RefreshLookAtTarget()
    {
        curLookAtTarget = RefreshTargetList(lookAtTargets, lookAtPriorities, MovementType.LookAt);
    }

    void RefreshFleeTarget()
    {
        curFleeTarget = RefreshTargetList(fleeTargets, fleePriorities, MovementType.Flee);
    }

    Transform RefreshTargetList(List<Transform> _targets, EntityPriority[] _priorities, MovementType _moveType)
    {
        if (_targets.Count < 1)
        {
            if (curMovement == _moveType)
                DoDefaultMovement();

            return null;
        }

        //remove all null targets from list
        _targets.RemoveAll(i => i == null);

        //return first in list if no priorities
        if (!entityDataManager)
            return _targets[0];

        //set lowest to highest index
        int lowest = _priorities.Length;
        Transform tar = null;
        for (int i = 0; i < _targets.Count; i++)
        {
            var ent = _targets[i].GetComponent<EngineEntity>();
            if (ent)
            {
                int id = ent.EntityID;
                foreach (var prior in _priorities)
                {
                    if (prior.entityId == id)
                    {
                        if (prior.priority < lowest)
                        {
                            lowest = prior.priority;
                            if (ent.AttackTarget)
                                tar = ent.AttackTarget;
                            else
                                tar = ent.transform;
                        }
                    }
                }
            }
        }

        if (tar == null && curMovement == _moveType && doDefaultMovementIfNoTargets)
            DoDefaultMovement();

        return tar;
    }

    protected virtual void GetAimPosition()
    {
        if (curLookAtTarget)
        {
            aimPosition = curLookAtTarget.position;
            aimDirection = (aimPosition - transform.position).normalized;
        }
        else if (curChaseTarget)
        {
            aimPosition = curChaseTarget.position;
            aimDirection = (aimPosition - transform.position).normalized;
        }
        else
        {
            aimOrigin = transform.position;
            aimDirection = transform.forward;
            aimPosition = aimOrigin + aimDirection * aimDistance;
        }

        if (equip)
        {
            if (equip.Aimable)
            {
                var muzzle = equip.Aimable.Muzzle;
                if (muzzle)
                    aimDirection = (aimPosition - muzzle.position).normalized;
            }
        }
    }

    protected virtual void CheckDirection()
    {
        if (agent)
            inputLocalDirection = agent.desiredVelocity;
    }

    void CheckStuck()
    {
        if (!agent)
            return;
        if (!agent.isActiveAndEnabled)
            return;
        if (agent.isStopped)
            return;

        if (velocitySpeed < 0.1f && agent.hasPath)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer > stuckCheckTime && !stuck)
            {
                stuckTimer = stuckCheckTime;
                agent.isStopped = true;
                stuck = true;

                Debug.Log(gameObject.name + " is stuck!");
            }
        }
        else if (stuckTimer != 0)
        {
            stuckTimer = 0;
            stuck = false;
            if (agent.isStopped)
                agent.isStopped = false;
        }
    }

    protected virtual void CheckMovement()
    {
        if (controllerType != BaseControllerType.NavAgent)
            return;

        if (curMovement == MovementType.Patrol)
            Patrol();
        else if (curMovement == MovementType.LookAt)
            LookAt();
        else if (curMovement == MovementType.Wander)
            Wander();
        else if (curMovement == MovementType.Chase)
            Chase();
        else if (curMovement == MovementType.Flee)
            Flee();
        else if (curMovement == MovementType.Idle)
            Idle();
    }

    void Idle()
    {

    }

    void LookAt()
    {
        if (!curLookAtTarget)
            return;

        var targetPos = curLookAtTarget.position;
        if (ignoreLeanRotation)
            targetPos = new Vector3(curLookAtTarget.position.x, transform.position.y, curLookAtTarget.position.z);

        var dir = (targetPos - transform.position).normalized;

        var smoothLook = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotSensitivity);

        transform.rotation = smoothLook;
    }

    void Patrol()
    {
        if (!agent.isActiveAndEnabled)
            return;
        if (agent.isStopped)
            return;

        //set speed
        curSpeed = unit.CurData.speed;

        if (!agent.pathPending)
        {
            if (agent.remainingDistance < arrivalDistance && !arrived)
            {
                NextPatrolPoint();
            }
        }
    }

    void NextPatrolPoint()
    {
        if (patrolInd < curPoints.Length - 1)
            patrolInd++;
        else
            patrolInd = 0;

        curDestination = curPoints[patrolInd];
        Timing.RunCoroutine(StartArrival());
    }

    void Wander()
    {
        if (!agent.isActiveAndEnabled)
            return;
        if (agent.isStopped)
            return;

        //set speed
        curSpeed = unit.CurData.speed;

        if (agent.hasPath && !agent.pathPending)
        {
            if (agent.remainingDistance < arrivalDistance && !findingNextWayPoint && !arrived)
            {
                NextWanderPoint();
            }
        }
    }

    void NextWanderPoint()
    {
        Timing.RunCoroutine(StartFindNextWanderPoint());
        Timing.RunCoroutine(StartArrival());
    }

    IEnumerator<float> StartFindNextWanderPoint()
    {
        findingNextWayPoint = true;
        float distance = 0;
        var nextPos = Vector3.zero;
        while (distance < minNextDistance)
        {
            var randDir = Utils.RandomXZDirection();
            var randDist = Random.Range(0f, wanderRadius - agent.radius);

            nextPos = wanderCenter + randDir * randDist;
            distance = Vector3.Distance(nextPos, transform.position);
            yield return Timing.WaitForOneFrame;
        }
        curDestination = nextPos;
        findingNextWayPoint = false;
    }

    IEnumerator<float> StartArrival()
    {
        arrived = true;
        float timer = 0;
        float perc = 0;
        while (perc != 1)
        {
            timer += Time.deltaTime;
            if (timer > arrivalTime)
                timer = arrivalTime;
            perc = timer / arrivalTime;
            yield return Timing.WaitForOneFrame;
        }
        arrived = false;
        if (agent)
        {
            if (agent.isActiveAndEnabled)
                agent.SetDestination(curDestination);
        }

    }

    void Chase()
    {
        if (!curChaseTarget)
            return;
        if (!agent.isActiveAndEnabled)
            return;
        if (agent.isStopped)
            return;

        //set speed
        curSpeed = unit.CurData.runSpeed;

        chaseTimer += Time.deltaTime;
        if (chaseTimer > chaseIntervalCheckTime || !agent.hasPath)
        {
            curDestination = curChaseTarget.position;
            agent.SetDestination(curDestination);
            chaseTimer = 0;
        }
    }

    void Flee()
    {
        if (!curFleeTarget)
            return;
        if (!agent.isActiveAndEnabled)
            return;
        if (agent.isStopped)
            return;

        //set speed
        curSpeed = unit.CurData.runSpeed;

        fleeTimer += Time.deltaTime;
        if (fleeTimer > fleeIntervalCheckTime || !agent.hasPath)
        {
            //calculate path away from target
            float randX = Random.Range(-1, 1);
            var randForward = new Vector3(randX, 0, 0);
            var dir = (transform.TransformPoint(randForward) - curFleeTarget.position).normalized;
            var pos = transform.position + (dir * fleeSearchDistance);
            var angleToTarget = Vector3.Angle(-dir, transform.forward);

            //is path reachable?
            var path = new NavMeshPath();
            agent.CalculatePath(pos, path);
            if (path.status == NavMeshPathStatus.PathComplete && angleToTarget > 90)
            {
                curDestination = pos;
            }
            else//find random path if not reachable
            {
                int tries = 0;
                while (path.status != NavMeshPathStatus.PathComplete && tries < 10)
                {
                    dir = Utils.RandomXZDirection();
                    pos = transform.position + (dir * fleeSearchDistance);
                    agent.CalculatePath(pos, path);
                    tries++;
                }
                curDestination = pos;

            }
            //set destination
            agent.SetDestination(curDestination);
            fleeTimer = 0;
        }
    }

    protected virtual void ClampAiming()
    {
    }

    protected virtual void AddGravity()
    {
        if (addedGravityType == GravityForceType.None)
            return;

        else if (addedGravityType == GravityForceType.Local)
        {
            curGravityForce = transform.InverseTransformDirection(forceDirection) * forceAmount;
        }
        else if (addedGravityType == GravityForceType.World)
        {
            curGravityForce = forceDirection * forceAmount;
        }

        if (controllerType == BaseControllerType.Rigidbody)
            rb.AddForce(curGravityForce, ForceMode.Impulse);
        else if (controllerType == BaseControllerType.CharacterController)
        {
            if (!grounded)
                curVerticalForce -= forceAmount * Time.deltaTime;
            else
                curVerticalForce = 0;
        }

    }

    public virtual void Bounce(Vector2 _direction, float _force, bool _consistent = false)
    {
        bouncing = true;
        if (_consistent)
        {
            rb.Sleep();
            rb.velocity = Vector2.zero;
        }

        rb.velocity = _direction * _force;
    }

    public virtual void SetCurSpeed(float _speed)
    {
        curSpeed = _speed;
    }

    protected virtual void CheckGrounded()
    {
        if (useCharacterControllerGroundDetection)
        {
            if (cc)
                grounded = cc.isGrounded;
            return;
        }

        //ground detect
        groundHits = Physics.OverlapBox(transform.TransformPoint(curGroundCenter), groundBoxSize / 2, transform.rotation, groundMask);
        if (groundHits.Length > 0)
        {
            currentGroundGO = groundHits[0].gameObject;
            grounded = true;
        }
        else
            grounded = false;

    }

    protected virtual void CheckPlatform()
    {
        if (groundHits == null)
            return;

        if (groundHits.Length > 0)
        {
            foreach (var col in groundHits)
            {
                if (col)
                {
                    onPlatform = col.gameObject.IsInLayerMask(platformMask);

                    if (onPlatform && transform.parent == null)
                        transform.SetParent(col.transform, true);
                }

            }
        }
        else if (onPlatform)
        {
            onPlatform = false;
            transform.SetParent(null, true);
        }

    }

    protected virtual void CheckSideHits()
    {
        if (sideMask == 0)
            return;

        var radius = idleDetectRadius;
        if (inputLocalDirection != Vector3.zero)
            radius = movingDetectRadius;

        //side detection
        curSideDetectCenter = new Vector3(sideDetectCenter.x + (inputLocalDirection.x * sideDetectDistanceMultiplier), sideDetectCenter.y,
            sideDetectCenter.z + (inputLocalDirection.z * sideDetectDistanceMultiplier));
        curSideDetectCenter = transform.TransformPoint(curSideDetectCenter);
        var pointOffset = (sideDetectHeight - (radius * 2)) / 2;
        var point0 = new Vector3(curSideDetectCenter.x, curSideDetectCenter.y + pointOffset, curSideDetectCenter.z);
        var point1 = new Vector3(curSideDetectCenter.x, curSideDetectCenter.y - pointOffset, curSideDetectCenter.z);
        sideCols = Physics.OverlapCapsule(point0, point1, radius, sideMask);

        if (sideCols.Length > 0)
        {
            bool slowCols = false;
            bool anyLadder = false;
            foreach (var col in sideCols)
            {
                var layer = col.gameObject.layer;
                if (slowMask == (slowMask | (1 << layer)))
                    slowCols = true;
                if (layer == ladderLayer.indexValue)
                    anyLadder = true;
            }

            if (slowCols)
                speedMultiplier = slowMultiplier;

            if (climbingDetect == ClimbingDetectType.SideDetection)
                onLadder = anyLadder;
        }
        else
        {
            speedMultiplier = 1;
            if (climbingDetect == ClimbingDetectType.SideDetection)
                onLadder = false;
        }

    }

    public virtual void DoDefaultMovement(Transform _target = null)
    {
        if (curMovement == MovementType.Flee)
        {
            if (_target)
            {
                if (_target != curFleeTarget)
                {
                    RemoveFleeTarget(_target);
                    return;
                }

            }
        }

        if (curMovement == MovementType.Chase)
        {
            if (_target)
            {
                if (_target != curChaseTarget)
                {
                    RemoveChaseTarget(_target);
                    return;
                }
            }
        }

        ClearAllTargetLists();
        curMovement = defaultMovement;

        if (defaultMovement == MovementType.Wander)
        {
            if (resetWanderCenterOnReset)
                wanderCenter = transform.position;

            NextWanderPoint();
        }
        else if (defaultMovement == MovementType.Patrol)
        {
            NextPatrolPoint();
        }

    }

    void ClearAllTargetLists()
    {
        curChaseTarget = null;
        curFleeTarget = null;
        curLookAtTarget = null;
        lookAtTargets.Clear();
        chaseTargets.Clear();
        fleeTargets.Clear();
    }

    public virtual void PauseMovement(Transform _target, bool _pause)
    {
        if (_target == transform)
            return;

        PauseMovement(_pause);
    }

    public virtual void PauseMovement(bool _pause)
    {
        if (!agent)
            return;
        if (!agent.isActiveAndEnabled)
            return;

        agent.isStopped = _pause;
    }

    public virtual void ChaseTarget(Transform _target)
    {
        if (_target == transform)
            return;

        if (_target == curChaseTarget && curMovement == MovementType.Chase)
            return;

        AddChaseTarget(_target);
        chaseTimer = chaseIntervalCheckTime;
        curMovement = MovementType.Chase;
    }

    public virtual void FleeTarget(Transform _target)
    {
        if (_target == transform)
            return;

        if (_target == curFleeTarget && curMovement == MovementType.Flee)
            return;
        AddFleeTarget(_target);
        fleeTimer = fleeIntervalCheckTime;
        curMovement = MovementType.Flee;
    }

    public virtual void LookAtTarget(Transform _target)
    {
        if (_target == transform)
            return;

        AddLookAtTarget(_target);
        curMovement = MovementType.LookAt;
    }

    public virtual void SetCurMovement(MovementType _movement)
    {
        curMovement = _movement;
    }

    public virtual void DisableAiming(bool _disable)
    {
        disableAiming.Value = _disable;
    }

    public virtual void DisableMovement(bool _disable)
    {
        disableMovement.Value = _disable;
    }

    public virtual void DisableMovement(float _time)
    {
        StartCoroutine(Utils.StartBoolTimer(disableMovement, _time, 0));
    }

    public virtual void DisableMovement(int _frameCount)
    {
        StartCoroutine(Utils.StartBoolTimer(disableMovement, 0, _frameCount));
    }

    public virtual void DisableSpeedSmooth(float _time, bool _fadeIn = false)
    {
        if (stopSpeedCoroutine != null)
            Timing.KillCoroutines(stopSpeedCoroutine);
        stopSpeedCoroutine = Timing.RunCoroutine(StartDisableSpeedSmooth(_time, _fadeIn));
    }

    IEnumerator<float> StartDisableSpeedSmooth(float _time, bool _fadeIn)
    {
        var spd = speedMultiplier;
        float timer = 0;
        if (_fadeIn)
        {
            while (timer < _time)
            {
                timer += Time.deltaTime;
                if (timer > _time)
                    timer = _time;
                float perc = timer / _time;
                speedMultiplier = Mathf.Lerp(spd, 0, perc);
                yield return Timing.WaitForOneFrame;
            }
            timer = 0;
        }
        while (timer < _time)
        {
            timer += Time.deltaTime;
            if (timer > _time)
                timer = _time;
            float perc = timer / _time;
            speedMultiplier = Mathf.Lerp(0, 1, perc);
            yield return Timing.WaitForOneFrame;
        }

    }

    protected virtual void SyncAnimations()
    {
        if (!anim)
            return;

        anim.Grounded = grounded;
        anim.OnPlatform = onPlatform;
        anim.VelocitySpeed = velocitySpeed;
        anim.DirectionX = LocalVelocityDirection.x;
        anim.DirectionY = LocalVelocityDirection.y;
        anim.DirectionZ = LocalVelocityDirection.z;
    }
}
