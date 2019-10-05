using System;

namespace NGN
{
    public struct T1Action
    {
        public bool IsEmpty { get { return objectValue == null; } }
        public object objectValue;
        public Action<object> action;
        public void Clear() { objectValue = null; action = null; }
        public T1Action Copy()
        {
            return new T1Action
            {
                objectValue = objectValue,
                action = action,
            };
        }
    }

    public struct T2Action
    {
        public bool IsEmpty { get { return T1Value == null; } }
        public object T1Value;
        public object T2Value;
        public Action<object, object> action;
    }

    public struct T3Action
    {
        public bool IsEmpty { get { return T1Value == null; } }
        public object T1Value;
        public object T2Value;
        public object T3Value;
        public Action<object, object, object> action;
    }

    public struct T4Action
    {
        public bool IsEmpty { get { return T1Value == null; } }
        public object T1Value;
        public object T2Value;
        public object T3Value;
        public object T4Value;
        public Action<object, object, object, object> action;
    }
}   
