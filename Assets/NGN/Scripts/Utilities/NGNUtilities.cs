using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NGN
{
    public static class NGNUtilities
    {
        public static T[] GetInstanceArrayCopy<T>(this T[] _array) where T : class
        {
            var copy = new T[_array.Length];
            for (int i = 0; i < _array.Length; i++)
            {
                var type = typeof(T);
                var obj = _array[i] as T;
                if (type == typeof(GameObject))
                    copy[i] = Object.Instantiate(obj as GameObject) as T;
                else if (type.IsSubclassOf(typeof(ScriptableObject)))
                    copy[i] = Object.Instantiate(obj as ScriptableObject) as T;
                else
                    Debug.LogError("Could not find type: " + type);
            }
            return copy;
        }

        public static bool IsIn<T>(this T _var, params T[] _possibles)
        {
            return _possibles.Contains(_var);
        }
    }
}


