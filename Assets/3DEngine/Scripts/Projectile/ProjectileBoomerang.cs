using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBoomerang : Projectile
{
    public new ProjectileBoomerangData Data { get { return (ProjectileBoomerangData)data; } }
    private Transform boomSpawn;

    private float timer;
    private float time;
    private float perc;

    public override void ShootProjectile(float _speed, int _damage, Vector3 _direction, LayerMask _mask, Transform _sender = null,Transform _target = null, Vector2 _targetPos = default(Vector2), bool _bounced = false)
    {
        base.ShootProjectile(_speed, _damage, _direction, _mask, _sender, _target, _targetPos, _bounced);
        Throw();
    }

    void Throw()
    {  
        StartCoroutine(StartThrow());
        StartCoroutine(StartRotation());
    }

    IEnumerator StartRotation()
    {
        while (gameObject)
        {
            transform.Rotate(0, 0, Data.rotateSpeed);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator StartThrow()
    {
        //disable sender renderers
        EnableRenderersOnSender(false);

        //throw to target
        var startPos = transform.position;
        var distance = Vector2.Distance(startPos, targetPos);
        time = distance / speed;
        timer = 0;
        while (perc < 1 && !bounced)
        {
            timer += Time.deltaTime;
            perc = timer / time;

            transform.position = Vector2.Lerp(startPos, targetPos, Data.throwCurve.Evaluate(perc));

            yield return new WaitForFixedUpdate();
        }
        //return to sender
        perc = 0;
        timer = 0;
        startPos = transform.position;
        distance = Vector2.Distance(startPos, sender.position);
        time = distance / speed;
        while (perc < 1)
        {
            timer += Time.deltaTime;
            perc = timer / time;
            transform.position = Vector2.Lerp(startPos, sender.position, Data.returnCurve.Evaluate(perc));

            yield return new WaitForFixedUpdate();
        }
        KillProjectile();
    }

    void EnableRenderersOnSender(bool _enable)
    {
        if (!Data.disableRenderersOnSender)
            return;

        foreach (var rend in sender.GetComponentsInChildren<Renderer>())
        {
            rend.enabled = _enable;
        }
    }

    public override void KillProjectile()
    {
        EnableRenderersOnSender(true);
        base.KillProjectile();
    }

}
