using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public class NGNMonoHandlerBase : MonoBehaviour
    {
        protected static NGNMonoHandlerBase instance;
        public static NGNMonoHandlerBase Instance
        {
            get
            {
                if (instance)
                    return instance;
                else
                {
                    var go = new GameObject();
                    instance = go.AddComponent<NGNMonoHandlerBase>();
                    return instance;
                }
                    
            }
        }

        protected System.Action<object> T1ActionCache;
        protected UpdateHandler updateHandler;
        protected FixedUpdateHandler fixedUpdateHandler;
        protected LateUpdateHandler lateUpdateHandler;

        #region MONOHANDLERS

        //update
        public virtual void SubscribeToUpdate(System.Action _updateCallBack)
        {
            if (!updateHandler)
                updateHandler = gameObject.AddComponent<UpdateHandler>();
            updateHandler.Subscribe(_updateCallBack);
        }

        public virtual void UnSubscribeToUpdate(System.Action _updateCallBack)
        {
            if (!updateHandler)
                return;
            updateHandler.UnSubscribe(_updateCallBack);
        }

        public virtual void SubscribeToUpdate<T>(T _object, System.Action<T> _updateCallBack) where T: class
        {
            if (!updateHandler)
                updateHandler = gameObject.AddComponent<UpdateHandler>();
            updateHandler.Subscribe(_object, _updateCallBack);
        }

        public virtual void UnSubscribeToUpdate<T>(T _object, System.Action<T> _updateCallBack) where T:class
        {
            if (!updateHandler)
                return;
            updateHandler.UnSubscribe(_object, _updateCallBack);
        }

        //fixed update
        public virtual void SubscribeToFixedUpdate(System.Action _fixedUpdateCallback)
        {
            if (!fixedUpdateHandler)
                fixedUpdateHandler = gameObject.AddComponent<FixedUpdateHandler>();
            fixedUpdateHandler.Subscribe(_fixedUpdateCallback);
        }

        public virtual void UnSubscribeToFixedUpdate(System.Action _fixedUpdateCallback)
        {
            if (!fixedUpdateHandler)
                return;
            fixedUpdateHandler.UnSubscribe(_fixedUpdateCallback);
        }

        public virtual void SubscribeToFixedUpdate<T>(T _object, System.Action<T> _fixedUpdateCallback) where T : class
        {
            if (!fixedUpdateHandler)
                fixedUpdateHandler = gameObject.AddComponent<FixedUpdateHandler>();
            T1ActionCache = arg => _fixedUpdateCallback(arg as T);
            fixedUpdateHandler.Subscribe(_object, T1ActionCache);
        }

        public virtual void UnSubscribeToFixedUpdate<T>(T _object, System.Action<T> _fixedUpdateCallback) where T : class
        {
            if (!fixedUpdateHandler)
                return;
            fixedUpdateHandler.UnSubscribe(_object, _fixedUpdateCallback);
        }


        //lateupdate
        public virtual void SubscribeToLateUpdate(System.Action _lateUpdateCallback)
        {
            if (!lateUpdateHandler)
                lateUpdateHandler = gameObject.AddComponent<LateUpdateHandler>();
            lateUpdateHandler.Subscribe(_lateUpdateCallback);
        }

        public virtual void UnSubscribeToLateUpdate(System.Action _lateUpdateCallback)
        {
            if (!lateUpdateHandler)
                return;
            lateUpdateHandler.UnSubscribe(_lateUpdateCallback);
        }

        public virtual void SubscribeToLateUpdate<T>(T _object, System.Action<T> _lateUpdateCallback) where T : class
        {
            if (!lateUpdateHandler)
                lateUpdateHandler = gameObject.AddComponent<LateUpdateHandler>();
            lateUpdateHandler.Subscribe(_object, _lateUpdateCallback);
        }

        public virtual void UnSubscribeToLateUpdate<T>(T _object, System.Action<T> _lateUpdateCallback) where T : class
        {
            if (!lateUpdateHandler)
                return;
            lateUpdateHandler.UnSubscribe(_object, _lateUpdateCallback);
        }

        #endregion
    }
}


