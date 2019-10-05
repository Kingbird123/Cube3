using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoolWrapper
{
    public bool Value { get; set; }
    public BoolWrapper(bool value) { Value = value; }
}

[System.Serializable]
public class FloatWrapper
{
    public float Value { get; set; }
    public FloatWrapper(float value) { Value = value; }
}

[System.Serializable]
public class IntWrapper
{
    public int Value { get; set; }
    public IntWrapper(int value) { Value = value; }
}

[System.Serializable]
public class RectWrapper
{
    public Rect Value { get; set; }
    public RectWrapper(Rect value) { Value = value; }
}

[System.Serializable]
public class TagProperty
{
    public string stringValue;
}

[System.Serializable]
public class LayerProperty
{
    public int indexValue;
    public string stringValue;
    public LayerMask maskValue;
}

[System.Serializable]
public class ChildName
{
    public string overridePropertyName;
    public bool overrideParent;
    public GameObject parent;
    public string stringValue;
    public int indexValue;
    public Transform transformValue { get { return parent.transform.FindDeepChild(stringValue); } }
}

[System.Serializable]
public class AnimatorStateProperty
{
    public string stateToPlay;
    public float crossfadeTime;
    public string exitState;
}

[System.Serializable]
public class AnimatorParamStateInfo
{
    public int indexValue;
    public string stringValue;
    public int layer;
}

[System.Serializable]
public class IndexStringProperty
{
    public string stringValue;
    public int indexValue;
    public string[] stringValues;

    public string GetStringSelection()
    {
        if (stringValues[indexValue] != null)
            return stringValues[indexValue];
        Debug.LogError("String selection not found on " + stringValue);
        return null;
    }
}

[System.Serializable]
public class Spawner
{
    public Spawnable spawn;
    public float repeatTime;

    public IEnumerator StartSpawning(Transform _trans)
    {
        while (true)
        {
            yield return new WaitForSeconds(repeatTime);
            SpawnPool.Spawn(spawn.poolIndex, _trans.position, Quaternion.identity);
        }
    }
}

[System.Serializable]
public class PinPoint
{
    public Vector3 position;
    public Vector3 euler;
}

[System.Serializable]
public class TestProperty
{
    public MethodProperty method;
}

[System.Serializable]
public class SceneObjectProperty
{
    public enum SceneObjectType { Override, Sender, Receiver, ClosestByTag, FindByName }
    public SceneObjectType sceneObjectType;
    public GameObject overrideGameObject;
    public string closestTag;
    public string nameToFind;

    public GameObject GetSceneObject(GameObject _sender = null, GameObject _receiver = null)
    {
        if (sceneObjectType == SceneObjectType.Override)
            return overrideGameObject;
        else if (sceneObjectType == SceneObjectType.Sender && _sender)
            return _sender;
        else if (sceneObjectType == SceneObjectType.Receiver && _receiver)
            return _receiver;
        else if (sceneObjectType == SceneObjectType.ClosestByTag && _sender)
        {
            var obj = Utils.FindClosestByTag(_sender.transform, closestTag);
            if (obj)
                return obj.gameObject;
            else
                return null;
        }    
        else if (sceneObjectType == SceneObjectType.FindByName)
            return GameObject.Find(nameToFind);
        else
            return null;
    }
}

[System.Serializable]
public class EntityPriority
{
    public int entityId;
    public int priority;
}



