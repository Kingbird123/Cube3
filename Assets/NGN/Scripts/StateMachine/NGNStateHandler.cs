using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    [System.Serializable]
    public class NGNStateHandler : IStateHandler
    {
        public NGNState state;
        public bool continuous;
        public NGNTransition finishedCondition;
        public bool IsActive { get ; set; }

        private readonly List<T1Action> finishedCallbacks = new List<T1Action>();
        protected T1Action T1Cache = new T1Action();

        public void Initialize<T>(T _object, System.Action<T> _onFinishedCallback) where T : class
        {
            T1Cache.objectValue = _object as T;
            T1Cache.action = arg => _onFinishedCallback(arg as T);
            finishedCallbacks.Add(T1Cache.Copy());
            Initialize();
        }

        public void Initialize()
        {
            state.Initialize();
            if (!continuous)
                finishedCondition.Initialize(OnFinished);
        }

        void OnFinished()
        {
            state.Finish();
            finishedCondition.UnSubscribe(OnFinished);
            IsActive = false;

            for (int i = 0; i < finishedCallbacks.Count; i++)
            {
                var obj = finishedCallbacks[i].objectValue;
                var action = finishedCallbacks[i].action;
                action.Invoke(obj);
            }
        }

        public void UnSubscribe<T>(T _object, System.Action<T> _onFinishedCallback) where T : class
        {
            T1Cache.Clear();
            T1Cache = GetMatch(_object, out int ind);
            if (!T1Cache.IsEmpty)
                finishedCallbacks.RemoveAt(ind);

            //unsub from condition as well
            finishedCondition.UnSubscribe(OnFinished);
        }

        T1Action GetMatch(object _T1Value, out int _ind)
        {
            _ind = -1;
            for (int i = 0; i < finishedCallbacks.Count; i++)
            {
                if (finishedCallbacks[i].objectValue == _T1Value)
                {
                    _ind = i;
                    return finishedCallbacks[i];
                }

            }
            return default;
        }

    }
}


