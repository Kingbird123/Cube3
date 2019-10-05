using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NGN
{
    public abstract class MonoHandler : MonoBehaviour
    {
        protected List<Action> callbacks = new List<Action>();
        private List<T1Action> T1Callbacks = new List<T1Action>();
        private List<T2Action> T2Callbacks = new List<T2Action>();
        private List<T3Action> T3Callbacks = new List<T3Action>();
        private List<T4Action> T4Callbacks = new List<T4Action>();

        private T1Action T1MatchCache;

        public virtual void Subscribe(Action _callback)
        {
            if (!callbacks.Contains(_callback))
                callbacks.Add(_callback);
        }
        public virtual void UnSubscribe(Action _callback)
        {
            if (callbacks.Contains(_callback))
                callbacks.Remove(_callback);
        }

        //T1
        public virtual void Subscribe<T>(T _object, Action<T> _callback) where T : class
        {
            T1MatchCache.Clear();
            T1MatchCache = GetMatch(_object, out int ind);
            if (T1MatchCache.IsEmpty)
            {
                T1MatchCache.objectValue = _object;
                T1MatchCache.action = arg => _callback(arg as T);
                T1Callbacks.Add(T1MatchCache.Copy());
            }
                
        }
        public virtual void UnSubscribe<T>(T _object, Action<T> _callback)
        {
            T1MatchCache.Clear();
            T1MatchCache = GetMatch(_object, out int ind);
            if (!T1MatchCache.IsEmpty)
            {
                T1Callbacks.RemoveAt(ind);
            }

        }
        //T2
        public virtual void Subscribe<T1, T2>(T1 _T1Value, T2 _T2Value, Action<T1, T2> _callback) 
            where T1 : class where T2 : class
        {
            if (GetMatch(_T1Value, _T2Value).IsEmpty)
            {
              
                var add = new T2Action
                { T1Value = _T1Value as T1,
                    T2Value = _T2Value as T2,
                    action = (arg1, arg2) => _callback(arg1 as T1, arg2 as T2)
                };
                T2Callbacks.Add(add);
            }

        }
        public virtual void UnSubscribe<T1, T2>(T1 _T1Value, T2 _T2Value)
            where T1 : class where T2 : class
        {
            var match = GetMatch(_T1Value, _T2Value);
            if (!match.IsEmpty)
            {
                T2Callbacks.Remove(match);
            }

        }
        //T3
        public virtual void Subscribe<T1, T2, T3>(T1 _T1Value, T2 _T2Value, T3 _T3Value, Action<T1, T2, T3> _callback)
            where T1 : class where T2 : class where T3 : class
        {
            if (GetMatch(_T1Value, _T2Value, _T3Value).IsEmpty)
            {

                var add = new T3Action
                {
                    T1Value = _T1Value as T1,
                    T2Value = _T2Value as T2,
                    T3Value = _T3Value as T3,
                    action = (arg1, arg2, arg3) => _callback(arg1 as T1, arg2 as T2, arg3 as T3)
                };
                T3Callbacks.Add(add);
            }

        }
        public virtual void UnSubscribe<T1, T2, T3>(T1 _T1Value, T2 _T2Value, T3 _T3Value)
            where T1 : class where T2 : class where T3 : class
        {
            var match = GetMatch(_T1Value, _T2Value, _T3Value);
            if (!match.IsEmpty)
            {
                T3Callbacks.Remove(match);
            }

        }
        //T4
        public virtual void Subscribe<T1, T2, T3, T4>(T1 _T1Value, T2 _T2Value, T3 _T3Value, T4 _T4Value, Action<T1, T2, T3, T4> _callback)
            where T1 : class where T2 : class where T3 : class where T4 : class
        {
            if (GetMatch(_T1Value, _T2Value, _T3Value).IsEmpty)
            {

                var add = new T4Action
                {
                    T1Value = _T1Value as T1,
                    T2Value = _T2Value as T2,
                    T3Value = _T3Value as T3,
                    T4Value = _T4Value as T4,
                    action = (arg1, arg2, arg3, arg4) => _callback(arg1 as T1, arg2 as T2, arg3 as T3, arg4 as T4)
                };
                T4Callbacks.Add(add);
            }

        }
        public virtual void UnSubscribe<T1, T2, T3, T4>(T1 _T1Value, T2 _T2Value, T3 _T3Value, T4 _T4Value)
            where T1 : class where T2 : class where T3 : class where T4 : class
        {
            var match = GetMatch(_T1Value, _T2Value, _T3Value, _T4Value);
            if (!match.IsEmpty)
            {
                T4Callbacks.Remove(match);
            }

        }

        //match getters
        T1Action GetMatch(object _T1Value, out int _ind)
        {
            _ind = -1;
            for (int i = 0; i < T1Callbacks.Count; i++)
            {
                if (T1Callbacks[i].objectValue == _T1Value)
                {
                    _ind = i;
                    return T1Callbacks[i];   
                }
                    
            }
            return default;
        }
        T2Action GetMatch(object _T1Value, object _T2Value)
        {
            for (int i = 0; i < T2Callbacks.Count; i++)
            {
                if (T2Callbacks[i].T1Value == _T1Value &&
                    T2Callbacks[i].T2Value == _T2Value)
                    return T2Callbacks[i];
            }
            return default;
        }
        T3Action GetMatch(object _T1Value, object _T2Value, object _T3Value)
        {
            for (int i = 0; i < T3Callbacks.Count; i++)
            {
                if (T3Callbacks[i].T1Value == _T1Value &&
                    T3Callbacks[i].T2Value == _T2Value &&
                    T3Callbacks[i].T3Value == _T3Value)
                    return T3Callbacks[i];
            }
            return default;
        }
        T4Action GetMatch(object _T1Value, object _T2Value, object _T3Value, object _T4Value)
        {
            for (int i = 0; i < T4Callbacks.Count; i++)
            {
                if (T4Callbacks[i].T1Value == _T1Value &&
                    T4Callbacks[i].T2Value == _T2Value &&
                    T4Callbacks[i].T3Value == _T3Value &&
                    T4Callbacks[i].T4Value == _T4Value)
                    return T4Callbacks[i];
            }
            return default;
        }

        //calls
        protected virtual void RunAllCallBacks()
        {
            RunCallbacks();
            RunT1Callbacks();
            RunT2Callbacks();
            RunT3Callbacks();
            RunT4Callbacks();
        }

        protected virtual void RunCallbacks()
        {
            if (callbacks.Count < 1)
                return;
            for (int i = 0; i < callbacks.Count; i++)
            {
                callbacks[i].Invoke();
            }
        }
        protected virtual void RunT1Callbacks()
        {
            if (T1Callbacks.Count < 1)
                return;
            for (int i = 0; i < T1Callbacks.Count; i++)
            {
                T1Callbacks[i].action.Invoke(T1Callbacks[i].objectValue);
            }
        }
        protected virtual void RunT2Callbacks()
        {
            if (T2Callbacks.Count < 1)
                return;
            for (int i = 0; i < T2Callbacks.Count; i++)
            {
                T2Callbacks[i].action.Invoke(
                    T2Callbacks[i].T1Value,
                    T2Callbacks[i].T2Value);
            }
        }
        protected virtual void RunT3Callbacks()
        {
            if (T3Callbacks.Count < 1)
                return;
            for (int i = 0; i < T3Callbacks.Count; i++)
            {
                T3Callbacks[i].action.Invoke(
                    T3Callbacks[i].T1Value,
                    T3Callbacks[i].T2Value,
                    T3Callbacks[i].T3Value);
            }
        }
        protected virtual void RunT4Callbacks()
        {
            if (T4Callbacks.Count < 1)
                return;
            for (int i = 0; i < T4Callbacks.Count; i++)
            {
                T4Callbacks[i].action.Invoke(
                    T4Callbacks[i].T1Value,
                    T4Callbacks[i].T2Value,
                    T4Callbacks[i].T3Value,
                    T4Callbacks[i].T4Value);
            }
        }
    }
}


