using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using MEC;
using UnityEngine.SceneManagement;

public static class Utils
{

    public static List<T> Find<T>()
    {
        List<T> interfaces = new List<T>();
        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var rootGameObject in rootGameObjects)
        {
            T[] childrenInterfaces = rootGameObject.GetComponentsInChildren<T>();
            foreach (var childInterface in childrenInterfaces)
            {
                interfaces.Add(childInterface);
            }
        }
        return interfaces;
    }

    public static bool IsList(this Type _type)
    {
        return _type.GetTypeInfo().IsGenericType && _type.GetGenericTypeDefinition() == typeof(List<>);
    }

    public static bool IsList(this object _obj)
    {
        var type = _obj.GetType();
        return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
    }

    public static Vector3 Snap(this Vector3 _pos, float _gridSize = 1.0f)
    {
        return new Vector3(Mathf.Round(_pos.x / _gridSize) * _gridSize, Mathf.Round(_pos.y / _gridSize) * _gridSize, Mathf.Round(_pos.z / _gridSize) * _gridSize);
    }

    public static Vector3 SnapOffset(this Vector3 _pos, Vector3 _offset, float _gridSize = 1.0f)
    {
        Vector3 snapped = _pos + _offset;
        snapped = new Vector3( Mathf.Round(snapped.x / _gridSize) * _gridSize, Mathf.Round(snapped.y / _gridSize) * _gridSize, Mathf.Round(snapped.z / _gridSize) * _gridSize);
        return snapped - _offset;
    }

    public static Vector3 RandomXZDirection()
    {
        var randX = UnityEngine.Random.Range(-1f, 1f);
        var randZ = UnityEngine.Random.Range(-1f, 1f);
        return new Vector3(randX, 0, randZ).normalized;
    }

    public static bool IsPrefab(this GameObject _go)
    {
        return _go.scene.rootCount == 0;
    }

    public static bool IsInLayerMask(this GameObject _go, LayerMask _mask)
    {
        return _mask == (_mask | (1 << _go.layer));
    }

    public static bool IsInMask(this int _int, int _mask)
    {
        return _mask == (_mask | (1 << _int));
    }

    public static bool MaskContains(this int _mask, int _int)
    {
        return (_int & _mask) == _int;
    }

    public static Transform FindDeepChild(this Transform _parent, string _childName)
    {
        Transform[] children = _parent.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            if (child.name == _childName)
                return child;
        }
        Debug.Log("No match for name: " + _childName + " inside " + _parent.name);
        return null;  
    }

    public static string[] GetDeepChildNames(this Transform _parent)
    {
        var children = _parent.GetComponentsInChildren<Transform>();
        var names = new string[children.Length];
        for (int i = 0; i < children.Length; i++)
        {
            names[i] = children[i].name;
        }
        return names;
    }

    public static Material[] GetDeepChildMaterials(this Transform _parent)
    {
        var rends = _parent.GetComponentsInChildren<Renderer>();
        var startMats = new Material[rends.Length];
        for (int i = 0; i < rends.Length; i++)
        {
            startMats[i] = rends[i].material;
        }
        return startMats;
    }

    public static void SetAllChildMaterials(this Transform _parent, Material _mat)
    {
        var rends = _parent.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].material = _mat;
        }
    }

    public static bool IsClassOrSubClass(this Type _type, Type _baseClass)
    {
        return _type.IsSubclassOf(_baseClass) || _type.IsAssignableFrom(_baseClass);
    }

    public static IEnumerator StartBoolTimer(BoolWrapper _bool, float _time = default(float), int _frames = default(int) )
    {
        _bool.Value = true;
        if (_time > 0)
            yield return new WaitForSeconds(_time);
        else if (_frames > 0)
        {
            int count = 0;
            while (count < _frames)
            {
                count++;
                yield return new WaitForEndOfFrame();
            }
        }
        _bool.Value = false;
    }

    public static void LookAt2D(this Transform _transform, Vector2 _targetPos, bool _ignoreZRotation = false, bool _consistentUpDirection = false)
    {
        var transPos = (Vector2)_transform.position;
        var dir = _targetPos - transPos;
        if (_ignoreZRotation)
            dir = new Vector2(_targetPos.x, _transform.position.y) - transPos;
        _transform.right = dir;
        if (_consistentUpDirection && Vector2.Dot(_transform.up, Vector2.down) > 0)
            _transform.Rotate(180, 0, 0);
    }

    public static void LookAway2D(this Transform _transform, Vector2 _targetPos, bool _ignoreZRotation = false, bool _consistentUpDirection = false)
    {
        var transPos = (Vector2)_transform.position;
        var dir = transPos - _targetPos;
        if (_ignoreZRotation)
            dir = new Vector2(transPos.x, _targetPos.y) - _targetPos;
        _transform.right = dir;
        if (_consistentUpDirection && Vector2.Dot(_transform.up, Vector2.down) > 0)
            _transform.Rotate(180, 0, 0);
    }

    public static IEnumerator ChangeFloatValueBySpeed(FloatWrapper _curValue, float _targetValue, float _speed)
    {
        var startValue = _curValue.Value;
        var diff = Mathf.Abs(_targetValue - _curValue.Value);
        var time = diff / _speed;
        float timer = 0;
        float perc = 0;
        while (timer < time)
        {
            timer += Time.deltaTime;
            if (timer > time)
                timer = time;
            perc = timer / time;
            _curValue.Value = Mathf.Lerp(startValue, _targetValue, perc);
            yield return new WaitForEndOfFrame();

        }
    }

    public static Transform FindClosestByTag(this Transform _pos, string _tag)
    {
        if (_tag == "")
            return null;
        var objs = GameObject.FindGameObjectsWithTag(_tag);
        if (!(objs.Length > 0))
            return null;
        Transform closest = null;
        float distance = Mathf.Infinity;
        for (int i = 0; i < objs.Length; i++)
        {
            var dist = Vector2.Distance(_pos.position, objs[i].transform.position);
            if (dist < distance)
            {
                distance = dist;
                closest = objs[i].transform;
            }
                
        }
        return closest;
    }

    public static void LogComponentNullError(System.Type _type, GameObject _obj)
    {
        Debug.LogError("No " + _type.Name + " component found on " + _obj);
    }

    //web source: https://tech.spaceapegames.com/2016/07/05/trajectory-prediction-with-unity-physics/
    public static Vector3[] PhysicsPredictionPoints(Vector3 _startPos, Vector3 _velocity, float _drag, int _steps)
    {
        Vector3[] results = new Vector3[_steps];

        float timestep = Time.fixedDeltaTime / Physics.defaultSolverVelocityIterations;
        Vector3 gravityAccel = Physics.gravity * timestep * timestep;
        float drag = 1f - timestep * _drag;
        Vector3 moveStep = _velocity * timestep;

        for (int i = 0; i < _steps; ++i)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            _startPos += moveStep;
            results[i] = _startPos;
        }

        return results;
    }

    public static CoroutineHandle ReplayCoroutine(this CoroutineHandle _handle, IEnumerator<float> _coroutine)
    {
        if (_handle != null)
            Timing.KillCoroutines(_handle);
        _handle = Timing.RunCoroutine(_coroutine);
        return _handle;
    }
}
