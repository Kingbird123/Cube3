using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : UnitControllerEditor
{
    private GUIStyle headerStyle = new GUIStyle();
    private PlayerController pSource;

    //Movement Properties
    private SerializedProperty constantSpeed;
    private SerializedProperty horizontalInput;
    private SerializedProperty verticalInput;
    private SerializedProperty rawInputValues;
    private SerializedProperty keyboardAimInputHor;
    private SerializedProperty keyboardAimInputVer;
    private SerializedProperty joystickAimInputHor;
    private SerializedProperty joystickAimInputVer;
    private SerializedProperty invertY;

    private SerializedProperty forwardZMovementOnly;
    private SerializedProperty enableJoystickUse;
    private SerializedProperty enableRun;
    private SerializedProperty runButton;
    private SerializedProperty backwardSpeed;
    //crouch
    private SerializedProperty enableCrouch;
    private SerializedProperty crouchButton;
    private SerializedProperty toggleCrouch;
    private SerializedProperty crouchTime;
    private SerializedProperty crouchSpeed;
    private SerializedProperty crouchSpeedTime;

    private SerializedProperty inAirControlTime;

    private SerializedProperty enableClimbing;
    private SerializedProperty climbingDetect;
    private SerializedProperty climbSpeed;
    private SerializedProperty enableMovementSmoothing;
    private SerializedProperty moveSensitivity;

    //cam switching
    private SerializedProperty enableCameraSwitching;
    private SerializedProperty cameraSwitch;


    //rotation properties
    private SerializedProperty rotationType;
    private SerializedProperty clampYRotation;
    private SerializedProperty yRotationLimit;
    private SerializedProperty enableRotSmoothing;
    private SerializedProperty turnSensitivity;

    //aiming
    private SerializedProperty defaultAimMask;
    private SerializedProperty defaultAimOrigin;
    private SerializedProperty defaultAimDistance;

    //interaction
    private SerializedProperty enableInteraction;
    private SerializedProperty interactButton;
    private SerializedProperty interactDistance;
    private SerializedProperty interactMask;

    //Jumping Properties
    protected SerializedProperty enableJump;
    protected SerializedProperty jumpButton;
    private SerializedProperty jumpStyle;
    private SerializedProperty enableDoubleJump;
    private SerializedProperty enableJumpClimbing;
    private SerializedProperty gravityMultiplier;
    private SerializedProperty lowJumpMultiplier;
    private SerializedProperty jumpCurve;
    private SerializedProperty jumpTime;

    //dashing
    private SerializedProperty enableDash;
    private SerializedProperty dashButton;
    private SerializedProperty dashPower;
    private SerializedProperty dashTime;
    private SerializedProperty dashCooldown;

    //rolling
    private SerializedProperty enableRoll;
    private SerializedProperty rollButton;
    private SerializedProperty rollHeight;
    private SerializedProperty heightTransTime;
    private SerializedProperty rollPower;
    private SerializedProperty rollTime;
    private SerializedProperty rollCooldown;

    //ceiling Detection
    private SerializedProperty ceilingMask;
    private SerializedProperty ceilingBoxSize;
    private SerializedProperty ceilingBoxCenter;
    private SerializedProperty curCeilingBoxCenter;

    private CapsuleCollider conCollider;
    private CharacterController charController;

    protected string[] axisNames;

    public override void OnEnable()
    {
        base.OnEnable();
        pSource = (PlayerController)source;
        SetHeaderStyle();
        axisNames = EditorExtensions.GetInputAxisNames();
    }

    public override void GetProperties()
    {
        base.GetProperties();
        GetMovementProperties();
        GetCameraSwitchingProperties();
        GetRotationProperties();
        GetAimProperties();
        GetInteractProperties();
        GetJumpingProperties();
        GetDashProperties();
        GetRollProperties();
        GetCeilingDetectionProperties();

        conCollider = source.GetComponent<CapsuleCollider>();
        charController = source.GetComponent<CharacterController>();
    }

    public override void SetProperties()
    {
        DisplayMovement();
        DisplayCameraSwitchingProperties();
        DisplayRotation();
        DisplayAim();
        DisplayInteraction();
        DisplayJumping();
        DisplayDash();
        DisplayRoll();
        DisplayPhysics();
        DisplayGroundDetection();
        DisplaySideDetection();
        DisplayCeilingDetection();
    }

    void GetMovementProperties()
    {
        //moevment
        horizontalInput = sourceRef.FindProperty("horizontalInput");
        verticalInput = sourceRef.FindProperty("verticalInput");
        rawInputValues = sourceRef.FindProperty("rawInputValues");
        keyboardAimInputHor = sourceRef.FindProperty("keyboardAimInputHor");
        keyboardAimInputVer = sourceRef.FindProperty("keyboardAimInputVer");
        joystickAimInputHor = sourceRef.FindProperty("joystickAimInputHor");
        joystickAimInputVer = sourceRef.FindProperty("joystickAimInputVer");
        invertY = sourceRef.FindProperty("invertY");

        constantSpeed = sourceRef.FindProperty("constantSpeed");
        forwardZMovementOnly = sourceRef.FindProperty("forwardZMovementOnly");
        enableJoystickUse = sourceRef.FindProperty("enableJoystickUse");
        enableRun = sourceRef.FindProperty("enableRun");
        runButton = sourceRef.FindProperty("runButton");
        backwardSpeed = sourceRef.FindProperty("backwardSpeed");
        //crouch
        enableCrouch = sourceRef.FindProperty("enableCrouch");
        crouchTime = sourceRef.FindProperty("crouchTime");
        crouchSpeed = sourceRef.FindProperty("crouchSpeed");
        toggleCrouch = sourceRef.FindProperty("toggleCrouch");
        crouchButton = sourceRef.FindProperty("crouchButton");
        crouchSpeedTime = sourceRef.FindProperty("crouchSpeedTime");

        inAirControlTime = sourceRef.FindProperty("inAirControlTime");
        enableMovementSmoothing = sourceRef.FindProperty("enableMovementSmoothing");
        moveSensitivity = sourceRef.FindProperty("moveSensitivity");

        //climbing
        enableClimbing = sourceRef.FindProperty("enableClimbing");
        climbingDetect = sourceRef.FindProperty("climbingDetect");
        climbSpeed = sourceRef.FindProperty("climbSpeed");
        
    }

    void GetCameraSwitchingProperties()
    {
        enableCameraSwitching = sourceRef.FindProperty("enableCameraSwitching");
        cameraSwitch = sourceRef.FindProperty("cameraSwitch");
    }

    void GetRotationProperties()
    {
        rotationType = sourceRef.FindProperty("rotationType");
        clampYRotation = sourceRef.FindProperty("clampYRotation");
        yRotationLimit = sourceRef.FindProperty("yRotationLimit");
        enableRotSmoothing = sourceRef.FindProperty("enableRotSmoothing");
        turnSensitivity = sourceRef.FindProperty("turnSensitivity");

    }

    void GetAimProperties()
    {
        defaultAimMask = sourceRef.FindProperty("defaultAimMask");
        defaultAimOrigin = sourceRef.FindProperty("defaultAimOrigin");
        defaultAimDistance = sourceRef.FindProperty("defaultAimDistance");
    }

    void GetInteractProperties()
    {
        enableInteraction = sourceRef.FindProperty("enableInteraction");
        interactButton = sourceRef.FindProperty("interactButton");
        interactDistance = sourceRef.FindProperty("interactDistance");
        interactMask = sourceRef.FindProperty("interactMask");
    }

    void GetJumpingProperties()
    {
        enableJump = sourceRef.FindProperty("enableJump");
        jumpButton = sourceRef.FindProperty("jumpButton");
        jumpStyle = sourceRef.FindProperty("jumpStyle");
        enableDoubleJump = sourceRef.FindProperty("enableDoubleJump");
        gravityMultiplier = sourceRef.FindProperty("gravityMultiplier");
        lowJumpMultiplier = sourceRef.FindProperty("lowJumpMultiplier");
        lowJumpMultiplier = sourceRef.FindProperty("lowJumpMultiplier");
        enableJumpClimbing = sourceRef.FindProperty("enableJumpClimbing");
        jumpCurve = sourceRef.FindProperty("jumpCurve");
        jumpTime = sourceRef.FindProperty("jumpTime");
    }

    void GetDashProperties()
    {
        enableDash = sourceRef.FindProperty("enableDash");
        dashButton = sourceRef.FindProperty("dashButton");
        dashPower = sourceRef.FindProperty("dashPower");
        dashTime = sourceRef.FindProperty("dashTime");
        dashCooldown = sourceRef.FindProperty("dashCooldown");

    }

    void GetRollProperties()
    {
        enableRoll = sourceRef.FindProperty("enableRoll");
        rollButton = sourceRef.FindProperty("rollButton");
        rollHeight = sourceRef.FindProperty("rollHeight");
        heightTransTime = sourceRef.FindProperty("heightTransTime");
        rollPower = sourceRef.FindProperty("rollPower");
        rollTime = sourceRef.FindProperty("rollTime");
        rollCooldown = sourceRef.FindProperty("rollCooldown");
    }

    void GetCeilingDetectionProperties()
    {
        ceilingMask = sourceRef.FindProperty("ceilingMask");
        ceilingBoxSize = sourceRef.FindProperty("ceilingBoxSize");
        ceilingBoxCenter = sourceRef.FindProperty("ceilingBoxCenter");
        curCeilingBoxCenter = sourceRef.FindProperty("curCeilingBoxCenter");
    }

    protected override void DisplayMovement()
    {
        base.DisplayMovement();
        EditorGUILayout.PropertyField(constantSpeed);
        EditorGUILayout.PropertyField(forwardZMovementOnly);
        EditorGUILayout.PropertyField(backwardSpeed);

        EditorExtensions.LabelFieldCustom("Movement Input", FontStyle.Bold);
        EditorGUILayout.PropertyField(enableJoystickUse);
        horizontalInput.InputPropertyField(axisNames);
        verticalInput.InputPropertyField(axisNames);
        keyboardAimInputHor.InputPropertyField(axisNames);
        keyboardAimInputVer.InputPropertyField(axisNames);
        if (enableJoystickUse.boolValue)
        {
            joystickAimInputHor.InputPropertyField(axisNames);
            joystickAimInputVer.InputPropertyField(axisNames);
        }
        EditorGUILayout.PropertyField(rawInputValues);
        EditorGUILayout.PropertyField(invertY);

        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Run Settings", FontStyle.Bold);
        EditorGUILayout.PropertyField(enableRun);
        if (enableRun.boolValue)
        {
            runButton.InputPropertyField(axisNames);
        }

        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Crouch Settings", FontStyle.Bold);
        EditorGUILayout.PropertyField(enableCrouch);
        if (enableCrouch.boolValue)
        {
            crouchButton.InputPropertyField(axisNames);
            EditorGUILayout.PropertyField(toggleCrouch);
            EditorGUILayout.PropertyField(crouchTime);
            EditorGUILayout.PropertyField(crouchSpeed);
            EditorGUILayout.PropertyField(crouchSpeedTime);
        }

        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Climbing Settings", FontStyle.Bold);
        EditorGUILayout.PropertyField(enableClimbing);
        if (enableClimbing.boolValue)
        {
            EditorGUILayout.PropertyField(climbingDetect);
            EditorGUILayout.PropertyField(climbSpeed);
        }
            

        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("In Air Settings", FontStyle.Bold);
        EditorGUILayout.PropertyField(inAirControlTime);
        EditorGUILayout.PropertyField(enableMovementSmoothing);
        if (enableMovementSmoothing.boolValue)
            EditorGUILayout.PropertyField(moveSensitivity);

    }

    void DisplayCameraSwitchingProperties()
    {
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Camera Switching Settings", FontStyle.Bold);
        EditorGUILayout.PropertyField(enableCameraSwitching);
        if (enableCameraSwitching.boolValue)
        {
            cameraSwitch.CameraSwitcherContainerField(axisNames);
        }
    }

    public override void DisplayRotation()
    {
        base.DisplayRotation();
        EditorGUILayout.PropertyField(rotationType);
        EditorGUILayout.PropertyField(clampYRotation);
        if (clampYRotation.boolValue)
            EditorGUILayout.PropertyField(yRotationLimit);

        if (rotationType.enumValueIndex == 2)
        {
            EditorGUILayout.PropertyField(enableRotSmoothing);
            if (enableRotSmoothing.boolValue)
                EditorGUILayout.PropertyField(turnSensitivity);
        }
        
    }

    void DisplayAim()
    {
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Aim Settings", FontStyle.Bold);
        EditorGUILayout.PropertyField(defaultAimMask);
        EditorGUILayout.PropertyField(defaultAimOrigin);
        EditorGUILayout.PropertyField(defaultAimDistance);

    }

    void DisplayInteraction()
    {
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Interact Settings", FontStyle.Bold);
        EditorGUILayout.PropertyField(enableInteraction);
        if (enableInteraction.boolValue)
        {
            EditorGUILayout.PropertyField(interactMask);
            interactButton.InputPropertyField(axisNames);
            EditorGUILayout.PropertyField(interactDistance);
        }
        

    }

    void DisplayJumping()
    {
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Jump Settings", FontStyle.Bold);
        EditorGUILayout.PropertyField(enableJump);

        if (enableJump.boolValue)
        {
            jumpButton.InputPropertyField(axisNames);
            EditorGUILayout.PropertyField(jumpStyle);
            EditorGUILayout.PropertyField(enableDoubleJump);
            if (enableClimbing.boolValue)
            {
                EditorGUILayout.PropertyField(enableJumpClimbing);
            }
            if (jumpStyle.enumValueIndex == 1)
            {
                EditorGUILayout.PropertyField(gravityMultiplier);
                EditorGUILayout.PropertyField(lowJumpMultiplier);
            }
            if (jumpStyle.enumValueIndex == 2)
            {
                EditorGUILayout.PropertyField(jumpCurve);
                EditorGUILayout.PropertyField(jumpTime);
            }
                

        }

    }

    void DisplayDash()
    {
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Dash Settings", FontStyle.Bold);
        EditorGUILayout.PropertyField(enableDash);
        if (enableDash.boolValue)
        {
            dashButton.InputPropertyField(axisNames);
            EditorGUILayout.PropertyField(dashPower);
            EditorGUILayout.PropertyField(dashTime);
            EditorGUILayout.PropertyField(dashCooldown);
        }
        
    }

    void DisplayRoll()
    {
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Roll Settings", FontStyle.Bold);
        EditorGUILayout.PropertyField(enableRoll);
        if (enableRoll.boolValue)
        {
            rollButton.InputPropertyField(axisNames);
            EditorGUILayout.PropertyField(rollHeight);
            heightTransTime.FloatFieldClamp(0, rollTime.floatValue / 2);
            EditorGUILayout.PropertyField(rollPower);
            EditorGUILayout.PropertyField(rollTime);
            EditorGUILayout.PropertyField(rollCooldown);
        }

    }

    void DisplayCeilingDetection()
    {
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Ceiling Detect Settings", FontStyle.Bold);
        EditorGUILayout.PropertyField(ceilingMask);
            EditorGUILayout.PropertyField(ceilingBoxSize);
            EditorGUILayout.PropertyField(ceilingBoxCenter);
        if (!Application.isPlaying)
        {
            Vector3 ceilingCenter = Vector3.zero;

            if (conCollider)
            {
                ceilingCenter = new Vector3(ceilingBoxCenter.vector3Value.x,
           ceilingBoxCenter.vector3Value.y + conCollider.center.y + (conCollider.height / 2), ceilingBoxCenter.vector3Value.z);
            }
            else if (charController)
            {
                ceilingCenter = new Vector3(ceilingBoxCenter.vector3Value.x,
           ceilingBoxCenter.vector3Value.y + charController.center.y + (charController.height / 2), ceilingBoxCenter.vector3Value.z);
            }
            
        }
        
    }

    public override void DisplaySideDetection()
    {
        base.DisplaySideDetection();
    }

    public override void OnSceneGUI()
    {
        base.OnSceneGUI();
        DrawCeilingDetection();
    }

    void DrawCeilingDetection()
    {
        EditorExtensions.DrawWireCube(source.transform.TransformPoint(curCeilingBoxCenter.vector3Value), source.transform.rotation, ceilingBoxSize.vector3Value, Color.yellow);
    }

    void SetHeaderStyle()
    {
        headerStyle.fontStyle = FontStyle.Bold;
        RectOffset paddingOffset = new RectOffset(0, 0, 3, 3);
        headerStyle.padding = paddingOffset;
    }
}
