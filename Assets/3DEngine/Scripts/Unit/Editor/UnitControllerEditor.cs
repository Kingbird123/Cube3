using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(UnitController))]
public class UnitControllerEditor : Editor
{

    protected UnitController source;
    public UnitController Source { get { return source; } }
    protected SerializedObject sourceRef;

    //base props
    private SerializedProperty controllerType;
    private SerializedProperty moveForceType;
    private SerializedProperty agent;

    //movement
    protected SerializedProperty defaultMovement;
    protected SerializedProperty curMovement;
    protected SerializedProperty targetTrans;
    protected SerializedProperty doDefaultMovementIfNoTargets;

    //priorities
    protected SerializedProperty entityDataManager;
    protected SerializedProperty chasePriorities;
    protected SerializedProperty fleePriorities;
    protected SerializedProperty lookAtPriorities;

    //patrol
    protected SerializedProperty patrolType;
    protected SerializedProperty patrolDatas;
    protected SerializedProperty patrolPoints;

    //wander
    protected SerializedProperty wanderRadius;
    protected SerializedProperty minNextDistance;
    protected SerializedProperty resetWanderCenterOnReset;

    //chase flee
    protected SerializedProperty chaseIntervalCheckTime;
    protected SerializedProperty fleeIntervalCheckTime;
    protected SerializedProperty fleeSearchDistance;


    //common
    protected SerializedProperty arrivalTime;
    protected SerializedProperty arrivalDistance;
    protected SerializedProperty stuckCheckTime;

    //rotation
    protected SerializedProperty rotSensitivity;
    protected SerializedProperty ignoreLeanRotation;

    //physics
    protected SerializedProperty addedGravityType;
    protected SerializedProperty forceAmount;
    protected SerializedProperty forceDirection;

    //ground
    protected SerializedProperty useCharacterControllerGroundDetection;
    protected SerializedProperty groundMask;
    protected SerializedProperty platformMask;
    protected SerializedProperty groundBoxSize;
    protected SerializedProperty groundBoxCenter;

    //side detection
    protected SerializedProperty sideMask;
    protected SerializedProperty sideDetectCenter;
    protected SerializedProperty idleDetectRadius;
    protected SerializedProperty movingDetectRadius;
    protected SerializedProperty sideDetectHeight;
    protected SerializedProperty sideDetectDistanceMultiplier;
    protected SerializedProperty slowMask;
    protected SerializedProperty slowMultiplier;
    protected SerializedProperty ladderLayer;


    protected Bounds colBounds;

    public virtual void OnEnable()
    {
        source = (UnitController)target;
        sourceRef = serializedObject;
        GetProperties();
        SetGroundBox();
    }

    public override void OnInspectorGUI()
    {
        SetProperties();
        SetCurGroundCenter();
        sourceRef.ApplyModifiedProperties();  
    }

    public virtual void GetProperties()
    {

        //movement
        controllerType = sourceRef.FindProperty("controllerType");
        moveForceType = sourceRef.FindProperty("moveForceType");
        agent = sourceRef.FindProperty("agent");

        defaultMovement = sourceRef.FindProperty("defaultMovement");
        curMovement = sourceRef.FindProperty("curMovement");
        targetTrans = sourceRef.FindProperty("target");
        doDefaultMovementIfNoTargets = sourceRef.FindProperty("doDefaultMovementIfNoTargets");

        //priorities
        entityDataManager = sourceRef.FindProperty("entityDataManager");
        chasePriorities = sourceRef.FindProperty("chasePriorities");
        fleePriorities = sourceRef.FindProperty("fleePriorities");
        lookAtPriorities = sourceRef.FindProperty("lookAtPriorities");

        //patrolling
        patrolType = sourceRef.FindProperty("patrolType");
        patrolDatas = sourceRef.FindProperty("patrolDatas");
        patrolPoints = sourceRef.FindProperty("patrolPoints");

        //wandering
        wanderRadius = sourceRef.FindProperty("wanderRadius");
        minNextDistance = sourceRef.FindProperty("minNextDistance");
        resetWanderCenterOnReset = sourceRef.FindProperty("resetWanderCenterOnReset");

        //chase flee
        chaseIntervalCheckTime = sourceRef.FindProperty("chaseIntervalCheckTime");
        fleeIntervalCheckTime = sourceRef.FindProperty("fleeIntervalCheckTime");
        fleeSearchDistance = sourceRef.FindProperty("fleeSearchDistance");

        //Common
        arrivalDistance = sourceRef.FindProperty("arrivalDistance");
        arrivalTime = sourceRef.FindProperty("arrivalTime");
        stuckCheckTime = sourceRef.FindProperty("stuckCheckTime");

        //rotation
        rotSensitivity = sourceRef.FindProperty("rotSensitivity");
        ignoreLeanRotation = sourceRef.FindProperty("ignoreLeanRotation");

        //physics
        addedGravityType = sourceRef.FindProperty("addedGravityType");
        forceAmount = sourceRef.FindProperty("forceAmount");
        forceDirection = sourceRef.FindProperty("forceDirection");

        //ground
        useCharacterControllerGroundDetection = sourceRef.FindProperty("useCharacterControllerGroundDetection");
        groundMask = sourceRef.FindProperty("groundMask");
        platformMask = sourceRef.FindProperty("platformMask");
        groundBoxSize = sourceRef.FindProperty("groundBoxSize");
        groundBoxCenter = sourceRef.FindProperty("groundBoxCenter");

        //side detection
        sideMask = sourceRef.FindProperty("sideMask");
        sideDetectCenter = sourceRef.FindProperty("sideDetectCenter");
        sideDetectHeight = sourceRef.FindProperty("sideDetectHeight");
        idleDetectRadius = sourceRef.FindProperty("idleDetectRadius");
        movingDetectRadius = sourceRef.FindProperty("movingDetectRadius");
        sideDetectDistanceMultiplier = sourceRef.FindProperty("sideDetectDistanceMultiplier");
        slowMask = sourceRef.FindProperty("slowMask");
        slowMultiplier = sourceRef.FindProperty("slowMultiplier");
        ladderLayer = sourceRef.FindProperty("ladderLayer");

    }

    public virtual void SetProperties()
    {
        DisplayMovement();
        DisplayAI();
        DisplayPriorities();
        DisplayRotation();
        DisplayPhysics();
        DisplayGroundDetection();
        DisplaySideDetection();
    }

    protected virtual void SetGroundBox()
    {
        if (groundBoxSize.vector3Value == Vector3.zero)
        {
            colBounds = source.GetComponent<Collider>().bounds;
            var sizeX = colBounds.size.x;
            var posY = colBounds.center.y - colBounds.size.y / 2; 
            groundBoxSize.vector3Value = new Vector3(sizeX,0.15f,sizeX);
            groundBoxCenter.vector3Value = new Vector3(0, posY, 0);
            source.CurGroundCenter = groundBoxCenter.vector3Value;
        }
    }

    void SetCurGroundCenter()
    {
        if (!Application.isPlaying)
        {
            source.CurGroundCenter = groundBoxCenter.vector3Value;
        }
    }

    protected virtual void DisplayAI()
    {
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("AI Movement", FontStyle.Bold);
        if (Application.isPlaying)
            EditorGUILayout.LabelField("Movement State: " + curMovement.enumDisplayNames[curMovement.enumValueIndex]);
        EditorGUILayout.PropertyField(defaultMovement);
        EditorGUILayout.PropertyField(doDefaultMovementIfNoTargets);

        //patrolling
        if (defaultMovement.enumValueIndex == 1)
        {
            EditorGUILayout.PropertyField(patrolType);
            if (patrolType.enumValueIndex == 0 || patrolType.enumValueIndex == 1)
                EditorGUILayout.PropertyField(patrolPoints, true);
            else if (patrolType.enumValueIndex == 2)
                EditorGUILayout.PropertyField(patrolDatas, true);
        }
        else if (defaultMovement.enumValueIndex == 2) //wandering
        {
            EditorGUILayout.PropertyField(wanderRadius);
            minNextDistance.FloatFieldClamp(1, (wanderRadius.floatValue * 0.75f));
            EditorGUILayout.PropertyField(resetWanderCenterOnReset);
        }
        else if (defaultMovement.enumValueIndex == 3 || defaultMovement.enumValueIndex == 4)
            EditorGUILayout.PropertyField(targetTrans);

        chaseIntervalCheckTime.FloatFieldClamp(0.1f, Mathf.Infinity);
        fleeIntervalCheckTime.FloatFieldClamp(0.1f, Mathf.Infinity);
        EditorGUILayout.PropertyField(fleeSearchDistance);

        EditorGUILayout.PropertyField(arrivalDistance);
        EditorGUILayout.PropertyField(arrivalTime);
        EditorGUILayout.PropertyField(stuckCheckTime);
        if (source.IsStuck)
            EditorExtensions.LabelFieldCustom("This Unit is stuck!", FontStyle.Bold, Color.red);
    }

    protected virtual void DisplayMovement()
    {
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Movement Settings", FontStyle.Bold);
        EditorGUILayout.PropertyField(controllerType);
        if (controllerType.enumValueIndex == (int)UnitController.BaseControllerType.NavAgent)
            EditorGUILayout.PropertyField(agent);
        else if (controllerType.enumValueIndex == (int)UnitController.BaseControllerType.Rigidbody)
            EditorGUILayout.PropertyField(moveForceType);
        
        
    }

    protected virtual void DisplayPriorities()
    {
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Priority Settings", FontStyle.Bold);
        EditorGUILayout.PropertyField(entityDataManager);
        if (!entityDataManager.objectReferenceValue)
            return;

        //get data
        var entData = new SerializedObject(entityDataManager.objectReferenceValue);
        var entities = entData.FindProperty("entities"); 
        chasePriorities.arraySize = entities.arraySize;
        fleePriorities.arraySize = entities.arraySize;
        lookAtPriorities.arraySize = entities.arraySize;

        //get id names
        var idNames = new string[chasePriorities.arraySize];
        for (int i = 0; i < entities.arraySize; i++)
            idNames[i] = i.ToString();

        //chase priorities
        DoPriorityList(chasePriorities, entities, idNames);
        DoPriorityList(lookAtPriorities, entities, idNames);
        DoPriorityList(fleePriorities, entities, idNames);
        entData.Dispose();
    }

    void DoPriorityList(SerializedProperty _priorProp, SerializedProperty _entityProp, string[] _idNames)
    {
        EditorExtensions.LabelFieldCustom(_priorProp.displayName, FontStyle.Bold);
        for (int i = 0; i < _priorProp.arraySize; i++)
        {
            var ele = _priorProp.GetArrayElementAtIndex(i);
            var entityId = ele.FindPropertyRelative("entityId");
            var priority = ele.FindPropertyRelative("priority");

            var entEle = _entityProp.GetArrayElementAtIndex(i);
            var entityName = entEle.FindPropertyRelative("entityName");
            var id = entEle.FindPropertyRelative("id");

            entityId.intValue = id.intValue;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(entityName.stringValue);
            priority.intValue = EditorGUILayout.Popup(priority.intValue, _idNames, GUILayout.Width(40));
            EditorGUILayout.EndHorizontal();
        }
    }

    public virtual void DisplayRotation()
    {
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Rotation Settings", FontStyle.Bold);
        rotSensitivity.FloatFieldClamp(1, Mathf.Infinity);
        EditorGUILayout.PropertyField(ignoreLeanRotation);

    }

    public virtual void DisplayPhysics()
    {
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Physics Settings", FontStyle.Bold);
        EditorGUILayout.PropertyField(addedGravityType);
        if (addedGravityType.enumValueIndex != 0)
        {
            EditorGUILayout.PropertyField(forceAmount);
            EditorGUILayout.PropertyField(forceDirection);
        }

    }

    public virtual void DisplayGroundDetection()
    {
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Ground Detect Settings", FontStyle.Bold);
        if (controllerType.enumValueIndex == (int)UnitController.BaseControllerType.CharacterController)
            EditorGUILayout.PropertyField(useCharacterControllerGroundDetection);
        else
            useCharacterControllerGroundDetection.boolValue = false;

        if (!useCharacterControllerGroundDetection.boolValue)
        {
            EditorGUILayout.PropertyField(groundMask);
            EditorGUILayout.PropertyField(platformMask);
            EditorGUILayout.PropertyField(groundBoxSize);
            EditorGUILayout.PropertyField(groundBoxCenter);
        }

    }

    public virtual void DisplaySideDetection()
    {
        EditorGUILayout.Space();
        EditorExtensions.LabelFieldCustom("Side Detect Settings", FontStyle.Bold);
        EditorGUILayout.PropertyField(sideMask);
        if (sideMask.intValue != 0)
        {
            EditorGUILayout.PropertyField(sideDetectCenter);
            if (!Application.isPlaying)
                source.CurSideDetectCenter = sideDetectCenter.vector3Value;
            EditorGUILayout.PropertyField(idleDetectRadius);
            EditorGUILayout.PropertyField(movingDetectRadius);
            EditorGUILayout.PropertyField(sideDetectHeight);
            sideDetectHeight.floatValue = Mathf.Clamp(sideDetectHeight.floatValue, idleDetectRadius.floatValue * 2, Mathf.Infinity);
            EditorGUILayout.PropertyField(sideDetectDistanceMultiplier);
            EditorGUILayout.PropertyField(slowMask);
            EditorGUILayout.PropertyField(slowMultiplier);
            EditorGUILayout.PropertyField(ladderLayer);
        }
    }

    public virtual void OnSceneGUI()
    {
        DrawGroundedGizmos();
        DrawSideGizmos();
        DrawPointGizmos();
        DrawWanderDebug();
        DrawNavMeshDebug();
    }

    void DrawWanderDebug()
    {
        if (defaultMovement.enumValueIndex != 2)
            return;

        Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
        Handles.color = Color.blue;
        Handles.color = new Color(Handles.color.r, Handles.color.g, Handles.color.b, 0.1f);

        var center = source.transform.position;
        if (Application.isPlaying)
            center = source.WanderCenter;
        Handles.DrawSolidDisc(center, Vector3.up, wanderRadius.floatValue);
    }

    void DrawPointGizmos()
    {
        if (defaultMovement.enumValueIndex != 1)
            return;
        if (patrolType.enumValueIndex == 2)
            return;

        var view = SceneView.lastActiveSceneView;
        for (int i = 0; i < patrolPoints.arraySize; i++)
        {
            EditorGUI.BeginChangeCheck();

            var ele = patrolPoints.GetArrayElementAtIndex(i);
            var pos = ele.FindPropertyRelative("position");
            var rot = ele.FindPropertyRelative("euler");

            var dir = rot.vector3Value.normalized;
            var handlePos = pos.vector3Value;
            var labelPos = pos.vector3Value + Vector3.up * 1;
            var billboard = (view.camera.transform.position - labelPos).normalized;
            var size = HandleUtility.GetHandleSize(labelPos);

            if (patrolType.enumValueIndex == 0 && Application.isPlaying)
                return;

            //matrix config
            var matrix = Handles.matrix;
            if (patrolType.enumValueIndex == 0)
            {
                matrix = Matrix4x4.TRS(source.transform.position, source.transform.rotation, Handles.matrix.lossyScale);
            }
             
            using (new Handles.DrawingScope(matrix))
            {
                //labels and gui
                Handles.DrawSolidDisc(labelPos, billboard, 0.05f * size);
                Handles.Label(labelPos, "Point " + i);
                Handles.DrawLine(pos.vector3Value, labelPos);

                pos.vector3Value = Handles.PositionHandle(pos.vector3Value, Quaternion.Euler(rot.vector3Value));
                //rot.vector3Value = Handles.RotationHandle(Quaternion.Euler(rot.vector3Value), pos.vector3Value).eulerAngles;
            }

            if (EditorGUI.EndChangeCheck())
                sourceRef.ApplyModifiedProperties();
        }
    }

    void DrawNavMeshDebug()
    {
        if (!source.Agent)
            return;

        if (!source.Agent.hasPath || source.Agent.pathPending)
            return;

        Handles.color = Color.green;
        var pathPoints = source.Agent.path.corners;
        if (pathPoints.Length > 0)
        {
            for (int i = 0; i < pathPoints.Length; i++)
            {
                if (i > 0)
                {
                    Handles.DrawLine(pathPoints[i], pathPoints[i - 1]);
                }
            }

            EditorExtensions.DrawWireSphere(source.CurDestination, Quaternion.identity, 0.2f, Color.red);
        }
            
    }

    void DrawGroundedGizmos()
    {
        if (useCharacterControllerGroundDetection.boolValue)
            return;

        //draw grounded gizmo
        EditorExtensions.DrawWireCube(source.transform.TransformPoint(source.CurGroundCenter), source.transform.rotation, source.groundBoxSize, Color.cyan);
    }

    void DrawSideGizmos()
    {
        if (sideMask.intValue == 0)
            return;

        var radius = idleDetectRadius.floatValue;
        if (source.InputLocalDirection != Vector3.zero)
            radius = movingDetectRadius.floatValue;
        //draw collision gizmos
        EditorExtensions.DrawWireCapsule(source.CurSideDetectCenter, source.transform.rotation, radius, sideDetectHeight.floatValue, Color.magenta);
    }

}
