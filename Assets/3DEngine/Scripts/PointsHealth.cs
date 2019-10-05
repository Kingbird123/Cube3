using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsHealth : MonoBehaviour {

    [SerializeField]
    private int healthAmount = 1;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Player pl = col.GetComponent<Player>();
            //pl.AddHp(healthAmount);

            Destroy(this.gameObject);
        }
    }
}
