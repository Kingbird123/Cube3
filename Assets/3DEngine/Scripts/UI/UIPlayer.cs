using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayer : UIEngineValueEntity
{
    [System.Serializable]
    public class CursorSettings
    {
        public bool visible = true;
        public CursorLockMode lockMode = CursorLockMode.None;

        public void ActivateSettings()
        {
            Cursor.visible = visible;
            Cursor.lockState = lockMode;
        }
    }

    public static UIPlayer instance;

    [SerializeField] protected GameObject pauseMenu = null;
    [SerializeField] protected QuickMenuUI quickMenu = null;
    public QuickMenuUI QuickMenu { get { return quickMenu; } }
    [SerializeField] private CursorSettings startCursorSettings = null;
    [SerializeField] private CursorSettings pauseCursorSettings = null;
    [SerializeField] private AimReticalFX defaultAimFX = null;
    private List<AimReticalFXHandler> aimFXHandlers = new List<AimReticalFXHandler>();

    private MenuManager mm;

    protected override void Awake()
    {
        instance = this;
        startCursorSettings.ActivateSettings();
        SetMenuUI();
        InitializeAimFXHandlers();
    }

    void SetMenuUI()
    {
        mm = GameManager.instance.GetMenuManager();
        if (mm)
            mm.SetPlayerUI(this);
    }

    public void SetSystemCursor(bool _visible, bool _locked)
    {
        Cursor.visible = _visible;
        if (_locked)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }

    public void PauseMenuSetActive(bool _active)
    {
        if (_active)
            pauseCursorSettings.ActivateSettings();
        else
            startCursorSettings.ActivateSettings();
        pauseMenu.SetActive(_active);
    }

    protected virtual void InitializeAimFXHandlers()
    {
        if (!defaultAimFX)
            return;

        aimFXHandlers.Clear();
        AddAimFXHandler(defaultAimFX);
    }

    public void AddAimFXHandler(AimReticalFX _aimFXData)
    {
        aimFXHandlers.Add(new AimReticalFXHandler());
        aimFXHandlers[aimFXHandlers.Count - 1].Initialize(_aimFXData);
    }

    public void DrawAimFX(GameObject _aimHitObject, float _distance, Vector3 _aimOrigin, Vector3 _aimPosition)
    {
        for (int i = 0; i < aimFXHandlers.Count; i++)
        {
            aimFXHandlers[i].DrawAimFX(_aimHitObject, _distance, _aimOrigin, _aimPosition);
        }
    }
    public void RemoveAimFXHandler(AimReticalFX _aimFXData)
    {
        for (int i = 0; i < aimFXHandlers.Count; i++)
        {
            if (aimFXHandlers[i].ContainsAimFXData(_aimFXData))
            {
                aimFXHandlers[i].KillAimerReferences();
                aimFXHandlers.Remove(aimFXHandlers[i]);
            }
        }
    }
}
