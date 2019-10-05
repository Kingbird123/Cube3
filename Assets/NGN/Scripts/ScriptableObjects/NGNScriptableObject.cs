using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NGN
{
    //create unique ids for the scriptable objects for connecting instances
    public abstract class NGNScriptableObject : ScriptableObject
    {
        [HideInInspector] [SerializeField] protected string guid;
        public string GUID { get { return guid; } }

        protected virtual void OnEnable()
        {
            SetIDs();
        }

        protected virtual void SetIDs()
        {
            if (Application.isPlaying)
                return;
#if UNITY_EDITOR
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(this, out guid, out long id);
#endif
        }

    }
}

