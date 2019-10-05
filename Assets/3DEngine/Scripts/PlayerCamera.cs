using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public enum CameraType { FirstPerson, ThirdPerson, Both }
    public enum FollowType { Locked, FreeAim }
    public enum CameraState { FirstPerson, ThirdPerson }
    public enum PivotBumpType { InvertX, PlayerPosition, Offset }

    [SerializeField] private bool autoFindPlayer = true;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private CameraType cameraType = CameraType.Both;
    [SerializeField] private Vector3 firstPersonOffset = Vector3.zero;
    [SerializeField] private Vector3 thirdPersonPivot = Vector3.up;
    private Vector3 curThirdPersonPivot;
    private bool switchingPivot;
    [SerializeField] private float thirdPersonDistance = 10;
    [SerializeField] private bool scaleDistanceWithPlayer = false;
    [SerializeField] private bool clampDistance = false;
    [SerializeField] private float maxDistance = 30;
    [SerializeField] private float xRotationLimit = 85;
    [SerializeField] private bool clampYRotation = false;
    [SerializeField] private float yRotationLimit = 360;
    [SerializeField] private FollowType followType = FollowType.FreeAim;
    [SerializeField] private bool enableFollowSmoothing;
    [SerializeField] private float followSensitivity;
    [SerializeField] private InputProperty toggleCameraState = null;
    [SerializeField] private LayerMask camBumpMask = -1;
    [SerializeField] private float bumpDetectPadding = 1;
    [SerializeField] private bool detectPivotCollision = false;
    [SerializeField] private float detectPivotRadius = 0.1f;
    [SerializeField] private PivotBumpType pivotBumpType = PivotBumpType.Offset;
    [SerializeField] private Vector3 pivotBumpOffset = Vector3.zero;
    [SerializeField] private float camBumpSensitivity = 5;

    public Vector3 rayRightPos;
    public Vector3 rightHitPoint;
    public Vector3 rayLeftPos;
    public Vector3 leftHitPoint;

    private Camera cam;
    private Transform player;
    private PlayerController controller;
    private Player pl;
    private Collider playerCol;
    private MenuManager mm;
    private float startHeight;

    private CameraState curState;
    private float curDistance;
    private Vector3 curPivot;
    private Vector3 curOffset;
    private Vector3 curPos;

    private float lookHor;
    private float lookVer;
    private float camRotX;
    private float camRotY;

    [HideInInspector] public Vector3[] screenPoints;
    [HideInInspector] public Transform pivot;
    public Transform CamPivot { get { return pivot; } }

    private void Start()
    {
        curDistance = thirdPersonDistance;
        curThirdPersonPivot = thirdPersonPivot;
        GetComponents();
        InitializeScreenPoints();
        if (autoFindPlayer)
            StartCoroutine(StartFindPlayerAndSetPosition());
    }

    private void Update()
    {
        GetInputs();
        DetectScale();
    }

    private void FixedUpdate()
    {
        CheckPivotHit();
        ViewPortCollision();
    }

    void LateUpdate()
    {
        if (!player)
            return;
        PositionAndRotateCamera();
    }

    void GetComponents()
    {
        cam = GetComponent<Camera>();
        mm = GameManager.instance.GetMenuManager();
    }

    void InitializeScreenPoints()
    {
        screenPoints = new Vector3[5];
    }

    IEnumerator StartFindPlayerAndSetPosition()
    {
        while (!player)
        {
            var playerGO = GameObject.FindGameObjectWithTag(playerTag);
            if (playerGO)
                SetCameraPlayerTarget(playerGO, false);

            yield return new WaitForEndOfFrame();
        }
        InitializeCamera();
    }

    void InitializeCamera()
    {
        if (cameraType == CameraType.FirstPerson || cameraType == CameraType.Both)
            SetCameraView(CameraState.FirstPerson);
        else
            SetCameraView(CameraState.ThirdPerson);

    }

    void GetInputs()
    {
        if (mm)
        {
            if (mm.IsPaused)
                return;
        }

        if (!controller)
            return;

        if (controller.JoystickInUse)
        {
            lookHor = Input.GetAxis("XboxRightStickHor") * (controller.RotSensitivity * 10) * Time.deltaTime;
            lookVer = Input.GetAxis("XboxRightStickVer") * (controller.RotSensitivity * 10) * Time.deltaTime;
        }
        else
        {
            lookHor = Input.GetAxisRaw("Mouse X") * (controller.RotSensitivity * 10) * Time.deltaTime;
            lookVer = -Input.GetAxisRaw("Mouse Y") * (controller.RotSensitivity * 10) * Time.deltaTime;
        }

        if (cameraType == CameraType.Both)
        {
            if (toggleCameraState.GetInputDown())
                ToggleCameraView();
        }
    }

    void PositionAndRotateCamera()
    {
        camRotX += lookVer;
        camRotX = Mathf.Clamp(camRotX, -xRotationLimit, xRotationLimit);
        camRotY += lookHor;

        if (curState == CameraState.FirstPerson)
        {
            var perc = playerCol.bounds.size.y / startHeight;
            var heightOffset = new Vector3(curOffset.x, curOffset.y * perc, curOffset.z);
            curPos = player.TransformPoint(heightOffset);
            transform.position = curPos;
            transform.rotation = player.rotation;

            transform.localEulerAngles = new Vector3(camRotX, transform.localEulerAngles.y, 0);
        }
        else if (curState == CameraState.ThirdPerson)
        {

            if (followType == FollowType.FreeAim)
            {
                if (clampYRotation)
                    camRotY = Mathf.Clamp(camRotY, -yRotationLimit, yRotationLimit);
                pivot.localEulerAngles = new Vector3(camRotX, camRotY, 0);
                pivot.position = player.position + curPivot;
            }
            else
            {
                pivot.eulerAngles = new Vector3(camRotX, player.eulerAngles.y, 0);
                pivot.position = player.TransformPoint(curThirdPersonPivot);
            }

        }
    }

    void DetectScale()
    {
        if (!scaleDistanceWithPlayer)
            return;

        if (!player)
            return;

        curDistance = thirdPersonDistance;
        curPivot = thirdPersonPivot;
        if (scaleDistanceWithPlayer)
        {
            curDistance *= player.transform.lossyScale.x;
            curPivot *= player.transform.lossyScale.y;
            if (clampDistance && curDistance > maxDistance)
                curDistance = maxDistance;
        }

    }

    void CheckPivotHit()
    {
        if (!detectPivotCollision)
            return;
        if (!pivot)
            return;

        var cols = Physics.OverlapSphere(player.TransformPoint(thirdPersonPivot), detectPivotRadius, camBumpMask);
        
        if (cols.Length > 0)
        {
            var endPos = pivotBumpOffset;
            if (pivotBumpType == PivotBumpType.InvertX)
                endPos = new Vector3(-thirdPersonPivot.x, thirdPersonPivot.y, thirdPersonPivot.z);
            else if (pivotBumpType == PivotBumpType.PlayerPosition)
                endPos = new Vector3(0, thirdPersonPivot.y, 0);

            if (!switchingPivot && curThirdPersonPivot != endPos)
                StartCoroutine(StartPivotSwitch(endPos));
        }
        else if (!switchingPivot && curThirdPersonPivot != thirdPersonPivot)
            StartCoroutine(StartPivotSwitch(thirdPersonPivot));
    }

    void ViewPortCollision()
    {
        if (!pivot)
            return;

        //top right
        screenPoints[0] = cam.ViewportToWorldPoint(new Vector3(1 + bumpDetectPadding, 1 + bumpDetectPadding, cam.nearClipPlane));
        //bottom right
        screenPoints[1] = cam.ViewportToWorldPoint(new Vector3(1 + bumpDetectPadding, -bumpDetectPadding, cam.nearClipPlane));
        //top Left
        screenPoints[2] = cam.ViewportToWorldPoint(new Vector3(-bumpDetectPadding, 1 + bumpDetectPadding, cam.nearClipPlane));
        //bottom Left
        screenPoints[3] = cam.ViewportToWorldPoint(new Vector3(-bumpDetectPadding, -bumpDetectPadding, cam.nearClipPlane));
        //center
        screenPoints[4] = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane));

        var distanceToCenter = Vector3.Distance(screenPoints[0], screenPoints[4]);

        var minDist = 0.5f;
        float closest = curDistance;
        var lerpPos = new Vector3(0, 0, -curDistance);
        for (int i = 0; i < screenPoints.Length; i++)
        {
            var pos = screenPoints[i];
            var hitInfo = new RaycastHit();
            var dir = (screenPoints[i] - pivot.position).normalized;
            if (Physics.Linecast(pivot.position, pivot.position + (dir * curDistance), out hitInfo, camBumpMask))
            {

                var hitPoint = hitInfo.point;
                var normal = hitInfo.normal;
                var dirToCenter = (screenPoints[4] - screenPoints[i]).normalized;
                var worldPos = hitPoint + (normal * bumpDetectPadding);
                var localPos = pivot.InverseTransformPoint(worldPos);
                var clampDist = localPos.z;
                clampDist = Mathf.Clamp(clampDist, -curDistance, -minDist);
                var distAbs = Mathf.Abs(clampDist);
                if (distAbs < closest)
                {
                    closest = distAbs;
                    lerpPos = new Vector3(0, 0, clampDist);
                }
            }
        }

        var step = camBumpSensitivity * Time.deltaTime;
        transform.localPosition = Vector3.Lerp(transform.localPosition, lerpPos, step);
        //transform.localPosition = lerpPos;

    }

    IEnumerator StartPivotSwitch(Vector3 _endPos)
    {
        switchingPivot = true;
        var startPos = player.InverseTransformPoint(pivot.position);
        var endPos = _endPos;
        var dist = Vector3.Distance(startPos, endPos);
        var time = dist / camBumpSensitivity;
        float timer = 0;
        float perc = 0;
        while (perc < 1)
        {
            timer += Time.deltaTime;
            if (timer > time)
                timer = time;
            perc = timer / time;
            curThirdPersonPivot = Vector3.Lerp(startPos, endPos, perc);

            yield return new WaitForEndOfFrame();
        }
        switchingPivot = false;

    }

    public void SetCameraView(CameraState _state)
    {
        if (_state == CameraState.FirstPerson)
        {
            if (pivot)
            {
                transform.SetParent(null);
                Destroy(pivot.gameObject);
            }

            transform.rotation = player.rotation;
            curOffset = firstPersonOffset;
            curState = CameraState.FirstPerson;
        }
        else
        {
            if (!pivot)
            {
                pivot = new GameObject().transform;
                pivot.name = "[CamPivot]";
                pivot.position = player.TransformPoint(thirdPersonPivot);
                pivot.rotation = player.rotation;
                transform.SetParent(pivot);
                transform.localPosition = new Vector3(0, 0, -thirdPersonDistance);
                transform.localEulerAngles = Vector3.zero;
            }

            curState = CameraState.ThirdPerson;
        }
    }

    public void ToggleCameraView()
    {
        if (curState == CameraState.FirstPerson)
            SetCameraView(CameraState.ThirdPerson);
        else
            SetCameraView(CameraState.FirstPerson);

    }

    public void SetCameraPlayerTarget(GameObject _target, bool _resetPosition)
    {
        pl = _target.GetComponent<Player>();
        if (!pl)
        {
            Debug.LogError("Could not find a Player Component on: |" + _target + "| this camera should only follow player targets!");
            return;
        }
        controller = _target.GetComponent<PlayerController>();
        playerCol = _target.GetComponentInChildren<Collider>();
        if (pl.CurData)
            startHeight = pl.CurData.skinSize.y;
        var cont = _target.GetComponent<PlayerController>();
        if (pl)
            player = pl.transform;
        //if (cont)
            //cont.PlayerCamera = this;


        if (_resetPosition)
            ResetCameraPosition();
    }

    public void ResetCameraPosition()
    {
        if (pivot)
        {
            transform.SetParent(null);
            Destroy(pivot.gameObject);
        }

        InitializeCamera();
    }
}
