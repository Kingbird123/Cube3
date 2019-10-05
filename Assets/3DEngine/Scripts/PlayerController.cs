using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

[RequireComponent(typeof(Player))]
public class PlayerController : UnitController
{

    public enum JumpStyle { Consistent, Additive, Curve }
    public enum RotationType { None, AimControlled, MovementControlled }

    public enum MovementState { Idle, Walking, Backwards, Running, Crouching, InAir, Climbing, Grappling }
    private MovementState movementState;
    public MovementState CurMovementState { get { return movementState; } }


    //Movement
    [SerializeField] private InputProperty horizontalInput;
    [SerializeField] private InputProperty verticalInput;
    [SerializeField] private bool rawInputValues = true;
    [SerializeField] private bool constantSpeed;
    [SerializeField] private bool forwardZMovementOnly = false;
    [SerializeField] private bool enableJoystickUse = true;
    [SerializeField] private InputProperty keyboardAimInputHor;
    [SerializeField] private InputProperty keyboardAimInputVer;
    [SerializeField] private InputProperty joystickAimInputHor;
    [SerializeField] private InputProperty joystickAimInputVer;
    [SerializeField] private bool invertY = false;

    public bool JoystickInUse { get { return enableJoystickUse && Input.GetJoystickNames().Length > 0 && 
                mouseX == 0 && mouseY == 0; } }
    [SerializeField] private bool enableRun = true;
    public bool RunEnabled { get { return enableRun; } set { enableRun = value; } }
    [SerializeField] private InputProperty runButton = null;
    private BoolWrapper run = new BoolWrapper(false);
    [SerializeField] private float backwardSpeed = 3;
    [SerializeField] private bool enableMovementSmoothing;
    [SerializeField] private float moveSensitivity;

    //Camera Switching
    [SerializeField] private bool enableCameraSwitching = false;
    [SerializeField] private CameraSwitcherContainer cameraSwitch = null;

    //Rotation
    [SerializeField] private RotationType rotationType = RotationType.AimControlled;
    [SerializeField] private bool clampYRotation = false;
    [SerializeField] private float yRotationLimit = 90f;
    [SerializeField] private bool enableRotSmoothing = false;
    [SerializeField] private float turnSensitivity = 0;
    public float TurnSensitivity { get { return turnSensitivity; } set { turnSensitivity = value; } }
    private float rotY;
    public float RotY { get { return rotY; } set { rotY = value; } }

    //aiming properties
    [SerializeField] private LayerMask defaultAimMask = -1;
    [SerializeField] private Vector3 defaultAimOrigin = Vector3.up;
    [SerializeField] private float defaultAimDistance = 10;
    protected float curAimHitDistance;

    //interact
    [SerializeField] private bool enableInteraction = true;
    [SerializeField] private InputProperty interactButton = null;
    [SerializeField] private float interactDistance = 2;
    [SerializeField] private LayerMask interactMask = 0;
    private GameObject curInteractableGO;
    private Interactable curInteractable;
    private Interactable lastInteractable;

    //crouching
    [SerializeField] private bool enableCrouch = true;
    public bool CrouchEnabled { get { return enableCrouch; } set { enableCrouch = value; } }
    [SerializeField] private InputProperty crouchButton = null;
    [SerializeField] private bool toggleCrouch = false;
    [SerializeField] private float crouchTime = 0.5f;
    [SerializeField] private float crouchSpeed = 2;
    [SerializeField] private float crouchSpeedTime = 1;
    private BoolWrapper crouch = new BoolWrapper(false);
    private bool crouching;
    private CoroutineHandle crouchRoutine;

    //Jumping
    [SerializeField] protected InputProperty jumpButton = null;
    [SerializeField] private JumpStyle jumpStyle = JumpStyle.Consistent;
    private bool jump;
    private bool doubleJumping;
    [SerializeField] private bool enableDoubleJump = false;
    public bool DoubleJumpEnabled { get { return enableDoubleJump; } set { enableDoubleJump = value; } }
    private bool doubleJumpActive;
    [SerializeField] private bool enableJumpClimbing = true;
    private BoolWrapper jumpClimbing = new BoolWrapper(false);
    [SerializeField] private float gravityMultiplier = 0;
    [SerializeField] private float lowJumpMultiplier = 0;
    [SerializeField] private AnimationCurve jumpCurve = AnimationCurve.EaseInOut(0,0,1,0);
    [SerializeField] private float jumpTime = 0.3f;

    //Dashing
    [SerializeField] private bool enableDash = false;
    public bool DashEnabled { get { return enableDash; } set { enableDash = value; } }
    [SerializeField] private InputProperty dashButton = null;
    [SerializeField] private float dashPower = 10;
    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float dashCooldown = 2;
    private bool dashing;
    private bool dashCool = true;

    //Rolling
    [SerializeField] private bool enableRoll = false;
    public bool RollEnabled { get { return enableRoll; } set { enableRoll = value; } }
    [SerializeField] private InputProperty rollButton = null;
    [SerializeField] private float rollHeight = 1;
    [SerializeField] private float heightTransTime = 0.1f;
    [SerializeField] private float rollPower = 10;
    [SerializeField] private float rollTime = 0.5f;
    [SerializeField] private float rollCooldown = 2;
    private bool rolling;
    private bool rollCool = true;

    //Ceiling Detection
    [SerializeField] private LayerMask ceilingMask = -1;
    [SerializeField] public Vector3 ceilingBoxSize = Vector3.zero;
    [SerializeField] public Vector3 ceilingBoxCenter = Vector3.zero;
    [SerializeField] private Vector3 curCeilingBoxCenter = Vector3.zero;
    private Collider[] ceilingHits;
    private BoolWrapper ceilingHit = new BoolWrapper(false);
    private Collider curCeilingHit;
    private float lastCeilingYPos;

    //movement inputs
    protected float inputHor;
    protected float inputVer;
    protected Vector3 inputWorldDirection;

    //aiming
    private float mouseX;
    private float mouseY;
    private Vector2 lastJoyStickPos;
    private float conHor;
    private float conVer;

    //comps
    private new PlayerAnimations Anim { get { return (PlayerAnimations)anim; } }
    private Player pl;
    protected CapsuleCollider conCollider;
    private MenuManager mm;

    protected Camera playerCamera;

    //Aim Hover delegate
    public delegate void OnAimHitObjectHoverDelegate(GameObject _player, GameObject _aimHitObject);
    public static event OnAimHitObjectHoverDelegate aimHitObjectHoverDelegate;
    protected virtual void OnAimHitObjectHover(GameObject _aimHitObject)
    {
        aimHitObjectHoverDelegate?.Invoke(gameObject, _aimHitObject);
    }
    //Aim changed delegate
    public delegate void OnAimHitObjectChangedDelegate(GameObject _player, GameObject _aimHitObject);
    public static event OnAimHitObjectChangedDelegate aimHitObjectChangedDelegate;
    protected virtual void OnAimHitObjectChanged(GameObject _aimHitObject)
    {
        aimHitObjectChangedDelegate?.Invoke(gameObject, _aimHitObject);
    }


    protected override void Update()
    {
        base.Update();
        GetInputs();
        SetMovementStates();
        CheckMovementStates();
        CheckBouncing();
        CheckCameraSwitch();
    }

    protected override void FixedUpdate()
    {
        ControllerAim();
        MouseAim();
        GetAimPosition();
        DrawAimFX();
        DetectInteraction();
        base.FixedUpdate();
        CheckCeiling();
        CheckHeightVelocity();
        Move();
        Rotate();
    }

    public override void InitializeController()
    {
        base.InitializeController();

        curSpeed = baseSpeed;

        //get collider
        conCollider = GetComponent<CapsuleCollider>();
        if (conCollider)
        {
            startColHeight = conCollider.height;
            startColCenter = conCollider.center;
        }

        curCeilingBoxCenter = ceilingBoxCenter;

        //get comps
        anim = GetComponent<PlayerAnimations>();
        pl = GetComponent<Player>();
        equip = GetComponent<PlayerEquip>();
        var gm = GameManager.instance;
        if (gm)
        {
            mm = GameManager.instance.GetMenuManager();
        }

        //get camera
        playerCamera = Camera.main;

    }
    

    protected override void CheckChaseTarget()
    {
    }

    protected override void GetAimPosition()
    {
        if (mm)
        {
            if (mm.IsPaused)
                return;
        }

        if (!playerCamera)
            return;

        //set default aiming properties
        aimOrigin = transform.TransformPoint(defaultAimOrigin);
        aimMask = defaultAimMask;
        aimDistance = defaultAimDistance;
        aimDirection = playerCamera.transform.forward;
        var aimOffset = Vector3.zero;

        //change aiming properties based on equipped items
        if (equip)
        {
            var aimable = equip.Aimable;
            if (aimable)
            {
                var muzzle = aimable.Muzzle;
                if (muzzle)
                {
                    aimOrigin = muzzle.position;
                    aimMask = aimable.Data.aimMask;
                    aimDistance = aimable.Data.aimDistance;
                    aimOffset = aimable.Data.aimOffset;
                }

            }
        }

        //fire a raycast from origion so see if we hit anything
        var ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        var distance = Vector3.Distance(playerCamera.transform.position, aimOrigin) + aimDistance;
        var hit = new RaycastHit();

        //default hit properties
        var point = ray.GetPoint(distance);
        var distanceToOrigin = Vector3.Distance(aimOrigin, point);
        aimPosition = point + (aimOffset * distanceToOrigin);
        aimDirection = (aimPosition - aimOrigin).normalized;
        aimHitNormal = Vector3.up;
        aimHitObject = null;
        curAimHitDistance = 0;
        //check raycast if hit
        if (Physics.Raycast(ray, out hit, distance, aimMask))
        {
            point = hit.point;
            distanceToOrigin = Vector3.Distance(aimOrigin, point);
            //only store data if hit point is infront of player
            var dir = (point - aimOrigin).normalized;
            var zDir = transform.InverseTransformDirection(dir).z;
            if (zDir > 0)
            {
                aimPosition = point + (aimOffset * distanceToOrigin);
                aimDirection = (aimPosition - aimOrigin).normalized;
                aimHitNormal = hit.normal;
                aimHitObject = hit.collider.gameObject;
                curAimHitDistance = distanceToOrigin;
                //delegate events
                if (aimHitObject != lastAimHitObject)
                {
                    lastAimHitObject = aimHitObject;
                    OnAimHitObjectChanged(aimHitObject);
                }
                OnAimHitObjectHover(aimHitObject);
            }

        }
        else if (lastAimHitObject)
        {
            lastAimHitObject = null;
            OnAimHitObjectChanged(aimHitObject);
        }
            

    }


    void DrawAimFX()
    {
        if (UIPlayer.instance)
            UIPlayer.instance.DrawAimFX(aimHitObject, curAimHitDistance, aimOrigin, aimPosition);
    }

    void DetectInteraction()
    {
        if (!enableInteraction)
            return;
        if (!aimHitObject)
        {
            ResetInteractable();
            return;
        }
            
        if (aimHitObject.IsInLayerMask(interactMask))
        {
            if (curAimHitDistance <= interactDistance)
            {
                if (curInteractable)
                {
                    if (curInteractableGO != aimHitObject)
                        ResetInteractable();
                }
                
                if (!curInteractable)
                {
                    curInteractable = aimHitObject.GetComponent<Interactable>();
                    if (curInteractable)
                    {
                        curInteractable.OnHoverEnter();
                        curInteractableGO = curInteractable.gameObject;
                    }
                }
                else
                {
                    curInteractable.OnHoverStay();

                    if (interactButton.GetInputDown())
                        curInteractable.Interact(gameObject);
                }

            }
            else
                ResetInteractable();

        }
        else
            ResetInteractable();
    }

    void ResetInteractable()
    {
        if (curInteractable)
        {
            curInteractable.OnHoverExit();
            curInteractable = null;
            curInteractableGO = null;
        }
    }

    protected virtual void GetInputs()
    {
        if (mm)
        {
            if (mm.IsPaused || pl.IsDead)
            {
                inputHor = 0;
                inputVer = 0;
                return;
            }
                
        }

        if (pl.IsDead)
            return;

        //movement axis input
        inputHor = horizontalInput.GetAxis(rawInputValues);
        inputVer = verticalInput.GetAxis(rawInputValues);

        inputLocalDirection = new Vector3(inputHor, 0, inputVer).normalized;
        if (rotationType == RotationType.AimControlled)
            inputWorldDirection = transform.TransformDirection(inputLocalDirection);
        else if (playerCamera)
        {
            var dir = playerCamera.transform.TransformDirection(inputLocalDirection);
            dir.y = 0;
            inputWorldDirection = dir.normalized;
        }

        if (climbing)
            inputWorldDirection = new Vector3(inputWorldDirection.x, inputWorldDirection.z, 0);

        if (forwardZMovementOnly)
            inputWorldDirection.z = Mathf.Clamp(inputWorldDirection.z, 0, Mathf.Infinity);

        if (controllerType == BaseControllerType.CharacterController)
        {
            if (!climbing)
                movementInput = new Vector3(inputWorldDirection.x, curVerticalForce, inputWorldDirection.z);
            else
                movementInput = inputWorldDirection;
        }
        else if (controllerType == BaseControllerType.Rigidbody)
        {
            if (!controlFading)
                movementInput = inputWorldDirection;
        }

        //run input
        run.Value = Input.GetButton(runButton.stringValue);

        //jump input
        jump = jumpButton.GetInputDown() && enableJump;
        if (jump)
            Jump();

        //crouch input
        if (Input.GetButtonDown(crouchButton.stringValue))
        {
            if (toggleCrouch)
                crouch.Value = !crouch.Value;
            else
                crouch.Value = true;
        }
        if (Input.GetButtonUp(crouchButton.stringValue) && !toggleCrouch)
            crouch.Value = false;

        //dash input
        if (enableDash)
        {
            if (dashButton.GetInputDown())
                Dash();
        }

        //roll input
        if (enableRoll)
        {
            if (rollButton.GetInputDown())
                Roll();
        }
    }

    void MouseAim()
    {

        if (mm)
        {
            if (mm.IsPaused)
            {
                lookHor = 0;
                lookVer = 0;
                return;
            }
                
        }

        if (pl.IsDead)
            return;

        //get mouse position on screen
        mouseX = keyboardAimInputHor.GetAxis(rawInputValues);
        mouseY = -keyboardAimInputVer.GetAxis(rawInputValues);

        //return if controller is connected
        if (JoystickInUse)
            return;

        lookHor = mouseX * (rotSensitivity * 10) * Time.deltaTime;
        lookVer = mouseY * (rotSensitivity * 10) * Time.deltaTime;

        if (invertY)
            lookVer = -lookVer;
    }

    void ControllerAim()
    {
        if (mm)
        {
            if (mm.IsPaused)
                return;
        }

        //create position based on axis input and offset
        conHor = joystickAimInputHor.GetAxis(rawInputValues);
        conVer = -joystickAimInputVer.GetAxis(rawInputValues);

        //return if controller is not connected
        if (!JoystickInUse)
            return;

        lookHor = conHor * (rotSensitivity * 10) * Time.deltaTime;
        lookVer = conVer * (rotSensitivity * 10) * Time.deltaTime;

        if (invertY)
            lookVer = -lookVer;
    }

    void CheckCameraSwitch()
    {
        if (mm)
        {
            if (mm.IsPaused)
                return;
        }
        if (!enableCameraSwitching)
            return;

        cameraSwitch.DetectCameraSwitch();
    }

    void CheckMovementStates()
    {
        if (onLadder && enableClimbing && inputLocalDirection.z > 0.69f && !jumpClimbing.Value)
        {
            movementState = MovementState.Climbing;
        }
        else if (climbing)
            movementState = MovementState.Climbing;
        else if (crouching && ceilingHit.Value || crouch.Value && enableCrouch)
            movementState = MovementState.Crouching;
        else if (grounded)
        {
            if (inputLocalDirection != Vector3.zero)
            {
                if (movingBackwards)
                    movementState = MovementState.Backwards;
                else if (run.Value && enableRun)
                    movementState = MovementState.Running;
                else
                    movementState = MovementState.Walking;
            }
            else
                movementState = MovementState.Idle;
        }
        else
        {
            movementState = MovementState.InAir;
        }

    }

    void SetMovementStates()
    {
        switch (movementState)
        {
            case MovementState.Idle:
                Idle();
                break;
            case MovementState.Walking:
                Walking();
                break;
            case MovementState.Backwards:
                Backwards();
                break;
            case MovementState.Running:
                Running();
                break;
            case MovementState.Crouching:
                Crouching();
                break;
            case MovementState.InAir:
                InAir();
                break;
            case MovementState.Climbing:
                Climbing();
                break;
        }
    }

    void Backwards()
    {
        curSpeed = backwardSpeed;
    }

    void Idle()
    {
        curSpeed = baseSpeed;
        movingBackwards = false;
    }

    void Walking()
    {
        curSpeed = baseSpeed;
    }

    void Running()
    {
        curSpeed = unit.CurData.runSpeed;
    }

    void Crouching()
    {
        if (!crouch.Value && !ceilingHit.Value)
        {
            crouching = false;
            ResetColliderHeight(crouchTime, 0, grounded && velocityDirection.y < 0);
            return;
        }

        if (crouching)
            return;

        LerpColliderHeight
            (new Vector3(startColCenter.x, startColCenter.y / 2, startColCenter.z),
            startColHeight / 2, crouchTime, 0, grounded && velocityDirection.y < 0);

        if (curSpeed > crouchSpeed && crouchSpeedTime > 0)
            Timing.RunCoroutine(StartSpeedFade(curSpeed, crouchSpeed, crouchSpeedTime, crouch));
        else
            curSpeed = crouchSpeed;

        crouching = true;

    }

    void LerpColliderHeight(Vector3 _center, float _height, float _time, float _delay, bool _grounded, System.Action _onFinished = null)
    {
        if (crouchRoutine != null)
            Timing.KillCoroutines(crouchRoutine);
        crouchRoutine = Timing.RunCoroutine(StartLerpColliderHeight(_center, _height, _time, _delay, _grounded, _onFinished));
    }

    void ResetColliderHeight(float _time, float _delay, bool _grounded, System.Action _onFinished = null)
    {
        LerpColliderHeight(startColCenter, startColHeight, _time, _delay, _grounded, _onFinished);
    }

    IEnumerator<float> StartLerpColliderHeight(Vector3 _center, float _height, float _time, float _delay, bool _grounded, System.Action _onFinishedCallback = null)
    {
        yield return Timing.WaitForSeconds(_delay);

        var startCenter = conCollider.center;
        var startHeight = conCollider.height;
        var startPosY = transform.position.y;
        float timer = 0;
        float perc = 0;
        while (perc != 1)
        {
            timer += Time.deltaTime;
            if (timer > _time)
                timer = _time;
            perc = timer / _time;
            conCollider.center = Vector3.Lerp(startCenter, _center, perc);
            conCollider.height = Mathf.Lerp(startHeight, _height, perc);
            if (!_grounded)//in air
            {
                var heightAdjust = Mathf.Lerp(startPosY, startPosY + (startHeight / 2), perc);
                transform.position = new Vector3(transform.position.x, heightAdjust, transform.position.z);
            }

            yield return Timing.WaitForOneFrame;
        }

        if (_onFinishedCallback != null)
            _onFinishedCallback.Invoke();
    }

    void InAir()
    {
        if (inputLocalDirection != Vector3.zero)
        {
            if (controlFading)
                return;

            Timing.RunCoroutine(StartControlFade(inAirControlTime));
        }
    }

    void Climbing()
    {
        if (!onLadder)
        {
            StopClimbing();
            return;
        }

        if (jump && enableJumpClimbing && !jumpClimbing.Value)
        {
            Timing.RunCoroutine(StartClimbJump());
            StopClimbing();
            return;
        }

        if (jumpClimbing.Value)
            return;

        if (climbing)
            return;

        curSpeed = climbSpeed;
        //freeze y position so there is no slow fall
        ConstrainRigidbody(RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY);
        climbing = true;
    }

    void StopClimbing()
    {
        ConstrainRigidbody(RigidbodyConstraints.FreezeRotation);
        climbing = false;
    }

    public virtual void ConstrainRigidbody(RigidbodyConstraints _constraints)
    {
        if (controllerType == BaseControllerType.CharacterController)
            return;

        if (rb)
            rb.constraints = _constraints;
    }

    protected virtual void Move()
    {
        if (pl.IsDead || disableMovement.Value || dashing || rolling)
            return;

        var speed = curSpeed * speedMultiplier;
        var movement = movementInput * speed;

        if (controllerType == BaseControllerType.Rigidbody)
        {
            if (moveForceType == MoveForceType.TransformTranslate)
                transform.Translate(movement * Time.deltaTime, Space.World);
            else if (rb)
            {
                if (moveForceType == MoveForceType.RBAddForce)
                {
                    if (VelocitySpeed < speed)
                        rb.AddForce(movement * 10 * Time.deltaTime, ForceMode.Impulse);
                }    
                else if (moveForceType == MoveForceType.RBMove)
                {
                    //rb move does not work with moving platforms???
                    var rbMove = rb.position + (movement * Time.deltaTime);
                    rb.MovePosition(rbMove);
                }
            }
        }    
        else if (controllerType == BaseControllerType.CharacterController)
        {
            if (cc)
                cc.Move(movement * Time.deltaTime);
        }
    }

    protected virtual void Rotate()
    {
        if (pl.IsDead || mm.IsPaused)
        {
            lookHor = 0;
            lookVer = 0;
            return;
        }
            

        //rotate body
        if (rotationType == RotationType.AimControlled)
        {
            rotY += lookHor;
        }
        else if (rotationType == RotationType.MovementControlled)
        {
            if (!playerCamera || inputLocalDirection == Vector3.zero)
                return;

            var lookRot = Quaternion.LookRotation(inputWorldDirection);
            var yTurn = Quaternion.Euler(transform.eulerAngles.x, lookRot.eulerAngles.y, transform.eulerAngles.z);

            if (enableRotSmoothing)
                lookRot = Quaternion.Slerp(transform.rotation, yTurn, Time.deltaTime * turnSensitivity);
         
            rotY = lookRot.eulerAngles.y;
        }

        if (clampYRotation)
            rotY = Mathf.Clamp(rotY, -yRotationLimit, yRotationLimit);

        transform.rotation = Quaternion.Euler(transform.rotation.x, rotY, transform.rotation.z);
    }

    public virtual void Jump()
    {
        if (!grounded && !doubleJumping && enableDoubleJump)
        {
            //enable double jumping until grounded/climbing
            Timing.RunCoroutine(StartJumpSwitch());
            doubleJumpActive = true;
        }

        if (grounded || doubleJumpActive || climbing)
        {
            var force = Vector3.up * curJumpPower;

            //jump player
            if (controllerType == BaseControllerType.Rigidbody)
                rb.velocity = force;
            else if (controllerType == BaseControllerType.CharacterController)
                Timing.RunCoroutine(StartCharacterControllerJump());

            //set double jump
            if (enableDoubleJump)
            {
                if (doubleJumpActive)
                    doubleJumpActive = false;
            }

            //animations
            if (anim)
                anim.PlayJump();

        }

    }

    IEnumerator<float> StartCharacterControllerJump()
    {
        float timer = 0;
        float perc = 0;
        while (jumpButton.GetInput() && perc < 1)
        {
            timer += Time.deltaTime;
            if (timer > jumpTime)
                timer = jumpTime;
            perc = timer / jumpTime;
            curVerticalForce = jumpCurve.Evaluate(perc) * curJumpPower;
            yield return Timing.WaitForOneFrame;
        }

    }

    void Dash()
    {
        if (!dashCool || sideCols.Length > 0 || dashing)
            return;
        dashing = true;
        Timing.RunCoroutine(StartMovementShift(dashPower, dashTime, true, true, OnDashFinished), Segment.FixedUpdate);
    }

    IEnumerator<float> StartMovementShift(float _power, float _time, bool _freezeHeight = false, bool _detectSideCollisions = true, System.Action _onFinishedCallback = null)
    {
        float timer = 0;
        //freeze y position for mid air dash
        if (_freezeHeight)
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        //get start coordinates
        var force = inputWorldDirection;
        var inputDir = inputLocalDirection;
        var lastPos = conCollider.bounds.center;
        while (timer < _time && sideCols.Length < 1)
        {
            //lock input direction
            inputLocalDirection = inputDir;
            //move controller
            transform.Translate(force * _power * Time.deltaTime, Space.World);
            timer += Time.deltaTime;

            if (_detectSideCollisions)
            {
                //get current coordinate position and distance to last
                var center = conCollider.bounds.center;
                var dist = Vector3.Distance(lastPos, center);
                var dir = (center - lastPos).normalized;

                //fire cast to detect side collisions
                var pointOffset = ((conCollider.height - (conCollider.radius * 2)) / 2) * 0.8f;
                var point0 = new Vector3(lastPos.x, lastPos.y + pointOffset, lastPos.z);
                var point1 = new Vector3(lastPos.x, lastPos.y - pointOffset, lastPos.z);
                var hit = new RaycastHit();
                if (Physics.CapsuleCast(point0, point1, conCollider.radius * 0.8f, dir, out hit, dist, sideMask))
                {
                    //if we hit a wall, set the controller position to the wall minus radius
                    var hitOffset = hit.point - (dir * conCollider.radius);
                    transform.position = new Vector3(hitOffset.x, hitOffset.y - (conCollider.height / 2), hitOffset.z);
                }
                lastPos = center;
            }
            
            yield return Timing.WaitForOneFrame;
        }
        if (_freezeHeight)
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        if (_onFinishedCallback != null)
            _onFinishedCallback.Invoke();
    }

    void OnDashFinished()
    {
        dashing = false;
        Timing.RunCoroutine(StartDashCoolDown());
    }

    IEnumerator<float> StartDashCoolDown()
    {
        dashCool = false;
        float timer = 0;
        while (timer < dashCooldown)
        {
            timer += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        dashCool = true;
    }

    void Roll()
    {
        if (!rollCool || sideCols.Length > 0 || !grounded || rolling)
            return;
        rolling = true;
        LerpColliderHeight
            (new Vector3(startColCenter.x, rollHeight / 2, startColCenter.z), 
            rollHeight, heightTransTime,0, grounded, OnRollHalfFinished);
        Timing.RunCoroutine
            (StartMovementShift(rollPower, rollTime, false, true, OnRollFinished), Segment.FixedUpdate);
        
    }
    
    void OnRollFinished()
    {
        rolling = false;
        Timing.RunCoroutine(StartRollCoolDown());
    }

    void OnRollHalfFinished()
    {
        ResetColliderHeight(heightTransTime, rollTime - (heightTransTime * 2), grounded);
    }

    IEnumerator<float> StartRollCoolDown()
    {
        rollCool = false;
        float timer = 0;
        while (timer < rollCooldown)
        {
            timer += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        rollCool = true;
    }

    void CheckCeiling()
    {
        if (ceilingMask == 0)
            return;

        curCeilingBoxCenter = new Vector3(ceilingBoxCenter.x, ceilingBoxCenter.y + conCollider.center.y + (conCollider.height / 2), ceilingBoxCenter.z);
        ceilingHits = Physics.OverlapBox(transform.TransformPoint(curCeilingBoxCenter), ceilingBoxSize, transform.rotation, ceilingMask);
        if (ceilingHits.Length > 0)
        {
            ceilingHit.Value = true;

            if (ceilingHits[0] != curCeilingHit)
            {
                curCeilingHit = ceilingHits[0];
                lastCeilingYPos = curCeilingHit.transform.position.y;
                return;
            }

            if (curCeilingHit.transform.position.y < lastCeilingYPos)//is ceiling moving down?
                pl.Die("being crushed by " + ceilingHits[0].ToString());

            lastCeilingYPos = curCeilingHit.transform.position.y;

        }
        else if (ceilingHit.Value)
            ceilingHit.Value = false;
    }

    void CheckBouncing()
    {
        if (grounded && bouncing || jump)
            bouncing = false;
    }

    void CheckHeightVelocity()
    {
        if (climbing || jumpStyle != JumpStyle.Additive || bouncing || disableMovement.Value)
            return;

        if (rb.velocity.y < 0)
            AddGravity(gravityMultiplier);
        if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            AddGravity(lowJumpMultiplier);
    }

    void AddGravity(float _multiplier)
    {
        if (controllerType == BaseControllerType.Rigidbody)
            rb.velocity += Vector3.up * Physics.gravity.y * _multiplier * Time.deltaTime;
        else if (controllerType == BaseControllerType.CharacterController)
            cc.Move(Vector3.up * Physics.gravity.y * _multiplier * Time.deltaTime);
    }

    void RunTimer(float _time, FloatWrapper _timer, FloatWrapper _perc)
    {
        _timer.Value += Time.deltaTime;
        if (_timer.Value > _time)
            _timer.Value = _time;
        _perc.Value = _timer.Value / _time;
    }

    IEnumerator<float> StartControlFade(float _time)
    {
        FloatWrapper timer = new FloatWrapper(0);
        FloatWrapper perc = new FloatWrapper(0);
        var startDir = movementInput;
        controlFading = true;
        while (perc.Value < 1 && !grounded && !climbing)
        {
            RunTimer(_time, timer, perc);
            var tempMove = Vector3.Lerp(startDir, movementInput, perc.Value);
            movementInput = tempMove;
            yield return Timing.WaitForOneFrame;
        }
        controlFading = false;
    }

    IEnumerator<float> StartSpeedFade(float _startSpeed, float _endSpeed, float _time, BoolWrapper _condition)
    {
        float timer = 0;
        float perc = 0;
        while (perc < 1 && _condition.Value)
        {
            timer += Time.deltaTime;
            if (timer > _time)
                timer = _time;
            perc = timer / _time;
            curSpeed = Mathf.Lerp(_startSpeed, _endSpeed, perc);
            yield return Timing.WaitForOneFrame;
        }
    }

    IEnumerator<float> StartJumpSwitch()
    {
        //anim switch
        if (Anim)
            Anim.PlayDoubleJump();

        //possible to do a jump out of the air if an input jump was not the cause for being not grounded.
        doubleJumping = true;
        while (!grounded && !climbing)
            yield return Timing.WaitForOneFrame;
        doubleJumping = false;
    }

    IEnumerator<float> StartClimbJump()
    {
        jumpClimbing.Value = true;
        while (rb.velocity.y > 0)
        {
            yield return Timing.WaitForOneFrame;
        }
        jumpClimbing.Value = false;
    }

    public void SetSpeed(float _newSpeed, float _time)
    {
        Timing.RunCoroutine(StartSpeedFade(curSpeed, _newSpeed, _time, new BoolWrapper(true)));
    }

    protected override void SyncAnimations()
    {
        base.SyncAnimations();

        if (!anim)
            return;

        Anim.InputHorizontal = inputHor;
        Anim.InputVertical = inputVer;
        Anim.Running = run.Value;
        Anim.Crouching = crouching;
        Anim.Climbing = climbing;
        Anim.Dashing = dashing;
        Anim.Rolling = rolling;
    }

}
