using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform)
        {
            if(col.GetComponent<Player>())
            {
                col.GetComponent<Player>().KillPlayer();
            }
            if(col.GetComponent<Enemy>())
            {
                col.GetComponent<Enemy>().KillEnemy();
            }
        }
    }
}
