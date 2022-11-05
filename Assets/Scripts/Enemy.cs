using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityBehaviour
{
    public float speed;

    public LayerMask sideRaycastLayers;

    public int way = 1;

    public void Start()
    {
        entityRigidbody = gameObject.GetComponent<Rigidbody2D>();
        startGravity = entityRigidbody.gravityScale;
        groundCheckDistance = 1;

        if(isInverted)
        {
            isInverted = false;
            Invert();
        }
    }

    public void Update()
    {
        transform.position += (Vector3)(way * speed * Time.deltaTime * Vector2.right);

        if(way == -1)
        {
            transform.localScale = new Vector3(-1, 1 * orientation, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1 * orientation, 1);
        }

        RaycastHit2D[] raycastSide = Physics2D.RaycastAll(transform.position + (Vector3)(new Vector2(0, 0.5f) * orientation), transform.right * way, 1f, sideRaycastLayers);
        Debug.DrawRay(transform.position + (Vector3)(new Vector2(0, 0.5f) * orientation), transform.right * way, Color.red);
        if(raycastSide.Length > 0)
        {
            for(int i = 0; i < raycastSide.Length; i++)
            {
                if(raycastSide[i].transform)
                {
                    if(raycastSide[i].transform.gameObject.GetComponent<ObjectProperties>())
                    {
                        if(raycastSide[i].transform.gameObject.GetComponent<ObjectProperties>().isSolid)
                        {
                            if(way == 1)
                            {
                                way = -1;
                            }else
                            {
                                way = 1;
                            }
                        }
                    }
                    else if(raycastSide[i].transform != transform)
                    {
                        if(way == 1)
                        {
                            way = -1;
                        }else
                        {
                            way = 1;
                        }
                    }
                }
            }
        }
    }

    public void KillEnemy()
    {
        Destroy(gameObject);
    }
}
