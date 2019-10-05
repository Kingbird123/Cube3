using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public static class NGNMonoHandler
    {
        #region MONOHANDLERS

        //update
        public static void SubscribeToUpdate(System.Action _updateCallBack)
        {
            NGNMonoHandlerBase.Instance.SubscribeToUpdate(_updateCallBack);
        }

        public static void UnSubscribeToUpdate(System.Action _updateCallBack)
        {
            NGNMonoHandlerBase.Instance.UnSubscribeToUpdate(_updateCallBack);
        }

        public static void SubscribeToUpdate<T>(T _object, System.Action<T> _updateCallBack) where T : class
        {
            NGNMonoHandlerBase.Instance.SubscribeToUpdate(_object, _updateCallBack);
        }

        public static void UnSubscribeToUpdate<T>(T _object, System.Action<T> _updateCallBack) where T : class
        {
            NGNMonoHandlerBase.Instance.UnSubscribeToUpdate(_object, _updateCallBack);
        }

        //fixed update
        public static void SubscribeToFixedUpdate(System.Action _fixedUpdateCallback)
        {
            NGNMonoHandlerBase.Instance.SubscribeToFixedUpdate(_fixedUpdateCallback);
        }

        public static void UnSubscribeToFixedUpdate(System.Action _fixedUpdateCallback)
        {
            NGNMonoHandlerBase.Instance.UnSubscribeToFixedUpdate(_fixedUpdateCallback);
        }

        public static void SubscribeToFixedUpdate<T>(T _object, System.Action<T> _fixedUpdateCallback) where T : class
        {
            NGNMonoHandlerBase.Instance.SubscribeToFixedUpdate(_object, _fixedUpdateCallback);
        }

        public static void UnSubscribeToFixedUpdate<T>(T _object, System.Action<T> _fixedUpdateCallback) where T : class
        {
            NGNMonoHandlerBase.Instance.UnSubscribeToFixedUpdate(_object, _fixedUpdateCallback);
        }

        //lateupdate
        public static void SubscribeToLateUpdate(System.Action _lateUpdateCallback)
        {
            NGNMonoHandlerBase.Instance.SubscribeToLateUpdate(_lateUpdateCallback);
        }

        public static void UnSubscribeToLateUpdate(System.Action _lateUpdateCallback)
        {
            NGNMonoHandlerBase.Instance.UnSubscribeToLateUpdate(_lateUpdateCallback);
        }

        public static void SubscribeToLateUpdate<T>(T _object, System.Action<T> _lateUpdateCallback) where T : class
        {
            NGNMonoHandlerBase.Instance.SubscribeToLateUpdate(_object, _lateUpdateCallback);
        }

        public static void UnSubscribeToLateUpdate<T>(T _object, System.Action<T> _lateUpdateCallback) where T : class
        {
            NGNMonoHandlerBase.Instance.UnSubscribeToLateUpdate(_object, _lateUpdateCallback);
        }

        #endregion
    }
}


