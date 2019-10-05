using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveTowards", menuName = "Data/Interacts/MoveTowards", order = 1)]
public class InteractFXMoveTowards : InteractFX
{
    [SerializeField] private bool overrideSender = false;
    [SerializeField] private TagProperty senderTag = null;
    [SerializeField] private TagProperty targetTag = null;
    [SerializeField] private float speed = 5;
    [SerializeField] private bool doFXOnFinish = false;
    [SerializeField] private InteractFX[] interacts = null;

    protected override void DoFX(GameObject _sender, GameObject _receiver)
    {
        var sender = _sender.transform;
        if (overrideSender)
            sender = Utils.FindClosestByTag(sender, senderTag.stringValue);
        var closest = Utils.FindClosestByTag(sender, targetTag.stringValue);
        if (closest)
            sender.GetComponent<MonoBehaviour>().StartCoroutine(MoveTowards(sender,closest));
    }

    IEnumerator MoveTowards(Transform _trans, Transform _targetPos)
    {
        var startPos = _trans.position;
        float distance = Vector2.Distance(startPos, _targetPos.position);
        float timer = 0;
        float perc = 0;
        float time = distance / speed;
        while (perc < 1)
        {
            timer += Time.deltaTime;
            if (timer > time)
                timer = time;
            perc = timer / time;
            _trans.position = Vector2.Lerp(startPos, _targetPos.position, perc);
            yield return new WaitForFixedUpdate();
        }
        if (doFXOnFinish)
        {
            foreach (var fx in interacts)
            {
                fx.ActivateFX(_trans.gameObject, _targetPos.gameObject);
            }
        }
    }
        
}
