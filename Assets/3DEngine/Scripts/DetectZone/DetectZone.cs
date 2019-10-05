using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

//This is a custom serilizable detect zone property using Overlap functions.
//There is a custom editor property drawer for this class
[System.Serializable]
public class DetectZone
{
    public string zoneName;
    public bool overrideZoneName;
    public bool overrideLabel;
    public enum PositionType { Offset, Local, World }
    public enum DetectAreaType { Sphere, Box, Capsule, LineCast, TriggerEnter, CollisionEnter }
    public bool overrideDetectMask;
    public LayerMask detectMask;
    public bool ignoreTriggerColliders;
    public bool overrideDetectType;
    public DetectAreaType detectType;
    public Collider colliderToUse;
    public bool overridePositionType;
    public PositionType positionType;
    public Transform trans;
    public Vector3 worldPos;
    public Vector3 offset;

    public Vector3 size = Vector3.one;
    public bool useTransformAngle;
    public Vector3 angle;

    public float radius = 1;

    public float height = 1;
    private Vector3 point0;
    private Vector3 point1;

    public bool overrideColor;
    public Color debugColor = Color.cyan;

    //gui stuff
    public Vector3 handlePoint;

    private Collider col;
    private Collider[] cols;
    private Vector3 curDirection;
    private Vector3 curDetectPos;
    private Vector3 lastDetectPos;
    private RaycastHit hit;
    private float stepDistance;
    private ColliderPhysicsLink link;
    private Rigidbody linkRb;

    private Coroutine detectRoutine;
    private Coroutine triggerRoutine;
    private List<Collider> colsList = new List<Collider>();
    private List<Collider> enteredList = new List<Collider>();
    private List<System.Action<Collider>> enterCallBacks = new List<System.Action<Collider>>();
    private List<System.Action<Collider>> exitCallBacks = new List<System.Action<Collider>>();
    private List<System.Action<Collider>> stayCallBacks = new List<System.Action<Collider>>();

    #region DETECTION_OVERLAP

    void SetCurDetectPos(Transform _trans = null)
    {
        var selectedTrans = trans;
        if (positionType == PositionType.Offset && _trans)
            selectedTrans = _trans;
        else if (positionType == PositionType.Local && trans)
            selectedTrans = trans;

        var pointOffset = (height - (radius * 2)) / 2;
        if (selectedTrans)
        {
            point0 = selectedTrans.TransformPoint(offset + Vector3.up * pointOffset);
            point1 = selectedTrans.TransformPoint(offset + Vector3.down * pointOffset);
            curDetectPos = selectedTrans.TransformPoint(offset);
        }
        else
        {
            point0 = offset + Vector3.up * pointOffset;
            point1 = offset + Vector3.down * pointOffset;
            curDetectPos = worldPos + offset;
        }


        if (useTransformAngle)
        {
            if (positionType == PositionType.Local && trans)
                angle = trans.eulerAngles;
            else if (positionType == PositionType.Offset && _trans)
                angle = _trans.eulerAngles;
        }
        if (lastDetectPos == Vector3.zero)
            lastDetectPos = curDetectPos;
    }

    void GetDirection()
    {
        curDirection = (curDetectPos - lastDetectPos).normalized;
        stepDistance = Vector3.Distance(lastDetectPos, curDetectPos);
        lastDetectPos = curDetectPos;
    }

    public Collider[] DetectCollidersNonAlloc(Transform _trans = null, int _maxAmount = 1)
    {
        SetCurDetectPos(_trans);
        cols = new Collider[_maxAmount];
        if (detectType == DetectAreaType.Sphere)
            Physics.OverlapSphereNonAlloc(curDetectPos, radius, cols, detectMask);
        else if (detectType == DetectAreaType.Box)
            Physics.OverlapBoxNonAlloc(curDetectPos, size / 2, cols, Quaternion.Euler(angle), detectMask);
        else if (detectType == DetectAreaType.Capsule)
            Physics.OverlapCapsuleNonAlloc(point0, point1, radius, cols, detectMask);
        else if (detectType == DetectAreaType.LineCast)
        {
            var hitInfo = new RaycastHit[_maxAmount];
            Physics.RaycastNonAlloc(curDetectPos, curDirection, hitInfo, stepDistance, detectMask);
            if (hitInfo.Length > 0)
            {
                for (int i = 0; i < cols.Length; i++)
                    cols[i] = hitInfo[i].collider;
            }
            GetDirection();
        }
        else if (detectType == DetectAreaType.TriggerEnter || detectType == DetectAreaType.CollisionEnter)
        {
            GetColliderPhysicsCols(_maxAmount);
        }
        if (ignoreTriggerColliders)
            return GetNonTriggerCols(cols);
        else
            return cols;
    }

    public Collider[] DetectColliders(Transform _trans = null)
    {
        SetCurDetectPos(_trans);
        if (detectType == DetectAreaType.Sphere)
            cols = Physics.OverlapSphere(curDetectPos, radius, detectMask);
        else if (detectType == DetectAreaType.Box)
            cols = Physics.OverlapBox(curDetectPos, size / 2, Quaternion.Euler(angle), detectMask);
        else if (detectType == DetectAreaType.Capsule)
            cols = Physics.OverlapCapsule(point0, point1, radius, detectMask);
        else if (detectType == DetectAreaType.LineCast)
        {
            var hits = Physics.RaycastAll(lastDetectPos, curDirection, stepDistance, detectMask);
            if (hits.Length > 0)
            {
                var cols = new Collider[hits.Length];
                for (int i = 0; i < hits.Length; i++)
                {
                    cols[i] = hits[i].collider;
                }
            }
            GetDirection();
        }
        else if (detectType == DetectAreaType.TriggerEnter || detectType == DetectAreaType.CollisionEnter)
        {
            GetColliderPhysicsCols();
        }
        if (ignoreTriggerColliders)
            return GetNonTriggerCols(cols);
        else
            return cols;
    }

    public bool Detected(Transform _trans = null)
    {
        return DetectCollider(_trans);
    }

    public Collider DetectCollider(Transform _trans = null)
    {
        SetCurDetectPos(_trans);
        return DetectCollider(curDetectPos);
    }

    public Collider DetectCollider(Transform _trans, out RaycastHit _hitInfo)
    {
        SetCurDetectPos(_trans);
        _hitInfo = new RaycastHit();
        var thisHit = DetectCollider(curDetectPos);
        if (thisHit)
        {
            if (detectType != DetectAreaType.LineCast)
                _hitInfo = GetHitInfo(thisHit);
            else
            {
                _hitInfo = hit;
            }

        }

        return thisHit;
    }

    public Collider DetectCollider(Vector3 _pos)
    {
        if (detectType == DetectAreaType.Sphere)
        {
            cols = Physics.OverlapSphere(_pos, radius, detectMask);
        }
        else if (detectType == DetectAreaType.Box)
            cols = Physics.OverlapBox(_pos, size / 2, Quaternion.Euler(angle), detectMask);
        else if (detectType == DetectAreaType.Capsule)
        {
            var pointA = _pos + Vector3.up * (height / 4);
            var pointB = _pos + Vector3.down * (height / 4);
            cols = Physics.OverlapCapsule(pointA, pointB, radius, detectMask);
        }
        else if (detectType == DetectAreaType.LineCast)
        {
            cols = new Collider[1];
            if (Physics.Linecast(lastDetectPos, _pos, out hit, detectMask))
                cols[0] = hit.collider;
        }
        else if (detectType == DetectAreaType.TriggerEnter || detectType == DetectAreaType.CollisionEnter)
        {
            GetColliderPhysicsCols();
        }
        GetDirection();

        if (cols.Length > 0)
        {
            if (cols[0] != null)
            {
                if (ignoreTriggerColliders)
                {
                    if (!cols[0].isTrigger)
                        return cols[0];
                    else
                        return null;
                }
                else
                    return cols[0];
            }   
            else
                return null;
        }
        else
            return null;
    }

    void GetColliderPhysicsCols(int _maxInd = -1)
    {
        if (!link)
        {
            link = colliderToUse.gameObject.AddComponent<ColliderPhysicsLink>();
            
        }
            
        if (detectType == DetectAreaType.TriggerEnter)
        {
            int ind = link.EnteredTriggerCols.Count;
            if (_maxInd > -1)
                ind = _maxInd;
            cols = new Collider[ind];
            for (int i = 0; i < cols.Length; i++)
                cols[i] = link.EnteredTriggerCols[i];

        }
        else
        {
            linkRb = colliderToUse.GetComponent<Rigidbody>();
            if (linkRb)
            {
                if (linkRb.isKinematic)
                    Debug.LogError(linkRb + " is set to kinematic. collision enter will not trigger");
            }
            else
                Debug.LogError("No rigidbody found on " + colliderToUse.gameObject + ". This is needed to trigger collisions.");
                
            int ind = link.EnteredCollisionCols.Count;
            if (_maxInd > -1)
                ind = _maxInd;
            cols = new Collider[ind];
            for (int i = 0; i < cols.Length; i++)
                cols[i] = link.EnteredCollisionCols[i];

        }
    }

    RaycastHit GetHitInfo(Collider _col)
    {
        var hitInfo = new RaycastHit();
        bool hit = false;
        //fire a physics cast if the detect area is moving in a direction
        if (stepDistance > 0)
        {
            var pos = lastDetectPos + (-curDirection * stepDistance);
            Ray ray = new Ray(pos, curDirection);
            if (detectType == DetectAreaType.Sphere)
                hit = Physics.SphereCast(ray, radius, out hitInfo, stepDistance * 2, detectMask);
            if (detectType == DetectAreaType.Box)
                hit = Physics.BoxCast(pos, size / 2, curDirection, out hitInfo, Quaternion.Euler(angle), stepDistance * 2, detectMask);
        }
        //find closest collider point if detect area is static or no hit from previous
        if (!hit)
        {
            //calculate distance and direction based on detectarea
            float dist = 1;
            if (detectType == DetectAreaType.Sphere)
                dist = radius;
            if (detectType == DetectAreaType.Box)
            {
                dist = size.x;
                if (size.y > dist)
                    dist = size.y;
            }
            //set the direction distance point
            var dirPos = lastDetectPos + (curDirection * dist);
            //find closest point on hit collider to the directional distance
            var hitPos = _col.bounds.ClosestPoint(dirPos);
            //calculate the direction and distance to that point
            var dir = (hitPos - lastDetectPos).normalized;
            dist = Vector2.Distance(hitPos, lastDetectPos);
            //fire a linecast to that point on the collider
            hit = Physics.Linecast(lastDetectPos, lastDetectPos + (dir * (dist + 0.1f)), out hitInfo, detectMask);

        }
        //if still no hit, create own raycast hit
        if (!hit)
        {
            hitInfo = new RaycastHit
            {
                distance = stepDistance,
                point = curDetectPos,
                normal = -curDirection
            };
        }
        return hitInfo;
    }

    Collider[] GetNonTriggerCols(Collider[] _cols)
    {
        List<Collider> nonTriggerCols = new List<Collider>();
        for (int i = 0; i < _cols.Length; i++)
        {
            if (!_cols[i].isTrigger)
                nonTriggerCols.Add(_cols[i]);
        }
        return nonTriggerCols.ToArray();
    }

    #endregion

    #region TRIGGER_CALLBACKS

    public void AddEnterTrigger(MonoBehaviour _sender, System.Action<Collider> _callback)
    {
        enterCallBacks.Add(_callback);
        DoTriggerDetection(_sender);  
    }

    public void RemoveEnterTrigger(MonoBehaviour _sender, System.Action<Collider> _callback)
    {
        enterCallBacks.Remove(_callback);
        CheckCallBacks(_sender);
    }

    public void AddExitTrigger(MonoBehaviour _sender, System.Action<Collider> _callback)
    {
        exitCallBacks.Add(_callback);
        DoTriggerDetection(_sender);
    }

    public void RemoveExitTrigger(MonoBehaviour _sender, System.Action<Collider> _callback)
    {
        exitCallBacks.Remove(_callback);
        CheckCallBacks(_sender);
    }

    public void AddStayTrigger(MonoBehaviour _sender, System.Action<Collider> _callback)
    {
        stayCallBacks.Add(_callback);
        DoTriggerDetection(_sender);
    }

    public void RemoveStayTrigger(MonoBehaviour _sender, System.Action<Collider> _callback)
    {
        stayCallBacks.Remove(_callback);
        CheckCallBacks(_sender);
    }

    public void ClearEnterTriggers(MonoBehaviour _sender)
    {
        enterCallBacks.Clear();
        CheckCallBacks(_sender);
    }

    public void ClearExitTriggers(MonoBehaviour _sender)
    {
        exitCallBacks.Clear();
        CheckCallBacks(_sender);
    }

    public void ClearStayTriggers(MonoBehaviour _sender)
    {
        stayCallBacks.Clear();
        CheckCallBacks(_sender);
    }

    public void ClearAllTriggers(MonoBehaviour _sender)
    {
        stayCallBacks.Clear();
        enterCallBacks.Clear();
        exitCallBacks.Clear();
        CheckCallBacks(_sender);
    }

    void DoTriggerDetection(MonoBehaviour _sender)
    {
        if (detectRoutine == null)
            detectRoutine = _sender.StartCoroutine(StartColliderDetection(_sender.transform));
        if (triggerRoutine == null)
            triggerRoutine = _sender.StartCoroutine(StartTriggerDetection());
    }

    void CheckCallBacks(MonoBehaviour _sender)
    {
        if (stayCallBacks.Count < 1 && enterCallBacks.Count < 1 && exitCallBacks.Count < 1)
        {
            if (detectRoutine != null)
                _sender.StopCoroutine(detectRoutine);
            if (triggerRoutine != null)
                _sender.StopCoroutine(triggerRoutine);

            detectRoutine = null;
            triggerRoutine = null;
        }
    }

    IEnumerator StartColliderDetection(Transform _sender)
    {
        while (Application.isPlaying)
        {
            colsList = DetectColliders(_sender).ToList();
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator StartTriggerDetection()
    {
        while (Application.isPlaying)
        {
            CheckEnterTrigger();
            CheckExitTrigger();
            yield return new WaitForEndOfFrame();
        }
    }

    void CheckEnterTrigger()
    {
        if (colsList.Count < 1)
            return;

        for (int i = 0; i < colsList.Count; i++)
        {
            //match found in entered?
            if (!enteredList.Contains(cols[i]))
            {
                //new collider entered detection zone
                enteredList.Add(cols[i]);
                //do all enter callbacks
                for (int ind = 0; ind < enterCallBacks.Count; ind++)
                {
                    enterCallBacks[ind].Invoke(cols[i]);
                }
            }
        }


    }

    void CheckExitTrigger()
    {
        if (enteredList.Count < 1)
            return;

        for (int i = 0; i < enteredList.Count; i++)
        {
            //match found?
            if (!cols.Contains(enteredList[i]))//no
            {
                //a collider has exited detection zone..do exit callbacks
                if (enteredList[i] != null)
                {
                    foreach (var callback in exitCallBacks)
                        callback.Invoke(enteredList[i]);
                }
                //remove collider from entered list
                enteredList.RemoveAt(i);
            }
            else//yes
            {
                //the collider is still in detection zone..do stay callbacks
                foreach (var callback in stayCallBacks)
                    callback.Invoke(enteredList[i]);
            }
        }
    }

    #endregion

    #region EDITOR_FUNCTIONS

#if UNITY_EDITOR

    public void DrawDetectZone(Object _source, SerializedObject _sourceRef, Transform _sourceTrans = null)
    {
        Handles.color = debugColor;
        EditorGUI.BeginChangeCheck();

        var pos = offset;
        if (positionType == PositionType.World)
        {
            if (handlePoint != worldPos)
                handlePoint = worldPos;
            handlePoint = Handles.PositionHandle(handlePoint, Quaternion.identity);

            //position points after dragging
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_source, "Modified " + _source + " properties.");
                worldPos = handlePoint;
                _sourceRef.ApplyModifiedProperties();
                _sourceRef.Update();
            }

            //get final position
            pos = worldPos + offset;
        }
        else if (positionType == PositionType.Local && trans)
            pos = trans.TransformPoint(offset);
        else if (positionType == PositionType.Offset && _sourceTrans)
            pos = _sourceTrans.TransformPoint(offset);

        if (useTransformAngle)
        {
            if (positionType == PositionType.Local && trans)
                angle = trans.eulerAngles;
            else if (positionType == PositionType.Offset && _sourceTrans)
                angle = _sourceTrans.eulerAngles;
        }

        Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
        Matrix4x4 angleMatrix = Matrix4x4.TRS(pos, Quaternion.Euler(angle), Handles.matrix.lossyScale);
        using (new Handles.DrawingScope(angleMatrix))
        {
            //draw the objects
            if (detectType == DetectAreaType.Box)
                Handles.DrawWireCube(Vector2.zero, size);
            else if (detectType == DetectAreaType.Sphere)
                DrawWireSphere(radius);
            else if (detectType == DetectAreaType.Capsule)
                DrawWireCapsule(radius, height);
        }

    }

    void DrawWireSphere(float _radius)
    {

        Handles.DrawWireDisc(Vector3.zero, Vector3.up, _radius);
        Handles.DrawWireDisc(Vector3.zero, Vector3.left, _radius);
        Handles.DrawWireDisc(Vector3.zero, Vector3.back, _radius);
    }

    void DrawWireCapsule(float _radius, float _height)
    {

        var pointOffset = (_height - (_radius * 2)) / 2;

        //draw sideways
        Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
        Handles.DrawLine(new Vector3(0, pointOffset, -_radius), new Vector3(0, -pointOffset, -_radius));
        Handles.DrawLine(new Vector3(0, pointOffset, _radius), new Vector3(0, -pointOffset, _radius));
        Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);
        //draw frontways
        Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
        Handles.DrawLine(new Vector3(-_radius, pointOffset, 0), new Vector3(-_radius, -pointOffset, 0));
        Handles.DrawLine(new Vector3(_radius, pointOffset, 0), new Vector3(_radius, -pointOffset, 0));
        Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);
        //draw center
        Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, _radius);
        Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, _radius);
    }

#endif

    #endregion
}


