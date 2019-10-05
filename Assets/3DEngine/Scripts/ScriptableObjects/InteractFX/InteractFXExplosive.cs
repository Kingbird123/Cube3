using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Explosive", menuName = "Data/Interacts/Explosive", order = 1)]
public class InteractFXExplosive : InteractFX
{
    [SerializeField] private float explosiveForce = 3;
    [SerializeField] private float explosionRadius = 1;
    [SerializeField] private GameObject explosionFXSpawn = null;
    [SerializeField] private LayerMask mask = -1;

    protected override void DoFX(GameObject _sender, GameObject _receiver)
    {
        DoExplosion(_sender.transform.position);
    }

    void DoExplosion(Vector2 _pos)
    {
        if (explosionFXSpawn)
            Instantiate(explosionFXSpawn, _pos, Quaternion.identity);

        Collider2D[] cols = Physics2D.OverlapCircleAll(_pos, explosionRadius, mask);
        foreach (var col in cols)
        {
            var dir = ((Vector2)col.bounds.center - _pos).normalized;
            var rb = col.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.velocity = dir * explosiveForce;
            }
        }
    }
}
