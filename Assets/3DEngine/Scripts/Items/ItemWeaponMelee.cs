using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeaponMelee : ItemAimable
{
    public new ItemWeaponMeleeData Data { get { return (ItemWeaponMeleeData)data; } }
    [SerializeField] private InputProperty useButton = null;
    [SerializeField] private DetectZone detectZone = null;
    [SerializeField] private Color activeColor = Color.red;
    [SerializeField] private Color idleColor = Color.blue;

    private Collider[] cols;
    private List<Collider> damagedCols = new List<Collider>();

    private Coroutine attackCoroutine;
    private Coroutine waitCoroutine;

    private void Update()
    {
        GetInputs();
    }

    void GetInputs()
    {
        if (mm)
        {
            if (mm.IsPaused)
                return;
        }

        if (inputType == UnitEquip.InputType.None)
            return;

        if (useButton.GetInputDown())
            Use();
        else if (Data.repeatUntilStopped && useButton.GetInputUp())
            Stop();
    }

    protected override void OnOwnerFound()
    {
        base.OnOwnerFound();
        detectZone.debugColor = idleColor;
    }

    public override void Use()
    {
        if (Data.allowSpamming)
        {
            DoAttack();
        }
        else if (inUse)
        {
            if (waitCoroutine != null)
                StopCoroutine(waitCoroutine);
            waitCoroutine = StartCoroutine(StartWaitForNotInUse());
        }
        else
            DoAttack();
    }

    protected override void Stop()
    {
        base.Stop();
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
    }

    IEnumerator StartWaitForNotInUse()
    {
        while (inUse)
            yield return new WaitForEndOfFrame();
        DoAttack();
    }

    void DoAttack()
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        if (Data.repeatUntilStopped)
            attackCoroutine = StartCoroutine(StartRepeatAttack());
        else
            attackCoroutine = StartCoroutine(StartAttack());
    }

    IEnumerator StartRepeatAttack()
    {
        while (true)
        {
            if (!inUse)
                StartCoroutine(StartAttack());
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator StartAttack()
    {
        while (recoiling)
            yield return new WaitForEndOfFrame();

        inUse = true;
        if (ownerAnim)
            ownerAnim.PlayMeleeAttack();
        yield return new WaitForSeconds(Data.damageDelay);
        float timer = 0;
        detectZone.debugColor = activeColor;
        while (timer < Data.activeTime)
        {
            timer += Time.deltaTime;
            if (timer > Data.activeTime)
                timer = Data.activeTime;
            Attack();
            yield return new WaitForEndOfFrame();
        }
        detectZone.debugColor = idleColor;
        Recoil(true);
        inUse = false;
        damagedCols.Clear();
    }

    void Attack()
    {
        cols = detectZone.DetectColliders(transform);
        if (cols.Length > 0)
        {
            for (int i = 0; i < Data.unitAmount; i++)
            {
                if (!damagedCols.Contains(cols[i]))
                {
                    Vector2 dir = Data.direction;
                    if (Data.bounceType == ItemWeaponMeleeData.BounceType.ClosetPoints)
                        dir = (cols[i].bounds.center - detectZone.offset).normalized;
                    else if (Data.bounceType == ItemWeaponMeleeData.BounceType.XOnly)
                    {
                        if (curUnitOwner.transform.position.x < cols[i].transform.position.x)
                            dir = Vector2.right;
                        else
                            dir = Vector2.left;
                    }

                    var u = cols[i].GetComponent<Unit>();
                    if (u)
                    {
                        DamageUnit(u, dir);
                        damagedCols.Add(cols[i]);
                    }
                }      
            }
        }
        
    }

    void DamageUnit(Unit _unit, Vector2 _dir)
    {
        //_unit.DamageHp(Data.damage);
        var cont = _unit.GetComponent<UnitController>();
        if (cont)
        {
            cont.Bounce(_dir, Data.bounceForce);
        }
            
    }

}
