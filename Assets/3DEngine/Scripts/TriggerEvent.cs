using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Internal;

[RequireComponent(typeof(Collider))]
public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private bool buttonDownToTrigger = false;
    [SerializeField] private InputProperty button = null;

    [SerializeField] private float delayTriggerTime = 0;

    [SerializeField] private bool useRepeatDelay = false;
    [SerializeField] private float repeatDelay = 0.1f;
    private float repeatTimer;

    public int mask = 0;
    public string[] maskOptions = new string[] { "TriggerEnter", "TriggerExit", "TriggerStay" };

    [SerializeField] private string triggerTag = "Untagged";

    //events
    [SerializeField] private bool useUnityEvents = false;
    [SerializeField] private UnityEvent triggerEnterEvents = null;
    [SerializeField] private UnityEvent triggerExitEvents = null;
    [SerializeField] private UnityEvent triggerStayEvents = null;

    //interacts
    [SerializeField] private InteractFX[] triggerEnterInteracts = null;
    [SerializeField] private InteractFX[] triggerExitInteracts = null;
    [SerializeField] private InteractFX[] triggerStayInteracts = null;

    private bool startTrigger;

    private bool triggered;
    public bool IsTriggered { get { return triggered; } }

    private Coroutine triggeredCoroutine;

    private void Start()
    {
        StartCoroutine(StartWait());
    }

    IEnumerator StartWait()
    {
        EnableColliders(false);
        yield return new WaitForSeconds(delayTriggerTime);
        EnableColliders(true);
        startTrigger = true;
    }

    void EnableColliders(bool _enable)
    {
        foreach (var col in GetComponents<Collider>())
        {
            col.enabled = _enable;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!startTrigger)
            return;

        if (mask == 1 | mask == 3 | mask == 5 | mask == -1)
        {
            if (col.tag == triggerTag)
            {
                DoTrigger(col, triggerEnterInteracts, triggerEnterEvents);
            }
        }   
    }

    private void OnTriggerExit(Collider col)
    {
        if (!startTrigger)
            return;

        if (mask == 2 | mask == 3 | mask == 6 | mask == -1)
        {

            if (col.tag == triggerTag)
            {
                DoTrigger(col, triggerExitInteracts, triggerExitEvents);
            }
        } 
    }

    private void OnTriggerStay(Collider col)
    {
        if (!startTrigger)
            return;

        if (mask == 4 | mask == 5 | mask == 6 | mask == -1)
        {
            if (col.tag == triggerTag)
            {
                if (buttonDownToTrigger)
                {
                    if (Input.GetButtonDown(button.stringValue))
                        DoTrigger(col, triggerStayInteracts, triggerStayEvents);
                    if (useRepeatDelay && Input.GetButton(button.stringValue))
                    {
                        repeatTimer += Time.deltaTime;
                        if (repeatTimer > repeatDelay)
                        {
                            DoTrigger(col, triggerStayInteracts, triggerStayEvents);
                            repeatTimer = 0;
                        }
                    }

                }
                else
                {
                    DoTrigger(col, triggerStayInteracts, triggerStayEvents);
                }
                
            }
        }

    }

    void DoTrigger(Collider _col, InteractFX[] _interacts, UnityEvent _events)
    {
        if (triggeredCoroutine != null)
            StopCoroutine(triggeredCoroutine);
        triggeredCoroutine = StartCoroutine(StartTriggerSwitch());
        if (useUnityEvents)
            _events.Invoke();
        DoInteractFX(_interacts, _col);
    }

    IEnumerator StartTriggerSwitch()
    {
        triggered = true;
        yield return new WaitForEndOfFrame();
        triggered = false;
    }

    void DoInteractFX(InteractFX[] _fx, Collider _col)
    {
        foreach (var fx in _fx)
        {
            fx.ActivateFX(this.gameObject, _col.gameObject);
        }
    }

}
