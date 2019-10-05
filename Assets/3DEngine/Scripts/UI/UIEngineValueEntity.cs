using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEngineValueEntity : MonoBehaviour
{
    [SerializeField] protected Image avatarImage;
    [SerializeField] protected UILayoutEngineValue[] UILayouts;
    protected bool initialized;
    protected virtual void Awake()
    {
        InitializeUIs();
    }

    void InitializeUIs()
    {
        initialized = true;
    }

    public virtual void SetAvatarIcon(Sprite _icon)
    {
        avatarImage.sprite = _icon;
    }

    public virtual void SyncEngineValue(EngineValue _engineValue, UILayoutMaster _master, int _ind)
    {
        var val = GetEngineValueUI(_master, _ind);
        if (val)
            val.SyncEngineValue(_engineValue);
    }

    public UIEngineValue GetEngineValueUI(UILayoutMaster _master, int _ind)
    {
        for (int i = 0; i < UILayouts.Length; i++)
        {
            if (UILayouts[i].layoutMaster == _master)
            {
                if (UILayouts[i].engineValues[_ind])
                return UILayouts[i].engineValues[_ind];
            }
                
        }
        Debug.LogError(this + " could not find ui with " + _master + " at index: " + _ind);
        return null;
    }
}


