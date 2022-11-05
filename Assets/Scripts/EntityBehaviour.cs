using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour
{
    public bool deneme;

    public bool isDead = false;

    public LayerMask platformLayer;

    [HideInInspector] public Rigidbody2D entityRigidbody;

    [HideInInspector] public float startGravity;

    public int orientation = 1;

    [HideInInspector] public float gravityMultiplier = 1;

    [HideInInspector] public bool isClimbing;
    [HideInInspector] public bool isFloating;

    public bool isInverted;

    [HideInInspector] public float groundCheckDistance = 0.2f;

    public bool GroundCheck()
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position - (Vector3)(new Vector2(0, 0.2f) * orientation), Vector2.down * orientation, groundCheckDistance, platformLayer);

        if(raycast.collider)
        {
            if(raycast.collider.GetComponent<ObjectProperties>().isSolid)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        else
        {
            return false;
        }
    }

    public void Invert()
    {
        isInverted = !isInverted;
        
        if(orientation == 1 && isInverted)
        {
            transform.position += Vector3.up * orientation;

            orientation *= -1;
            
            gameObject.GetComponent<Rigidbody2D>().gravityScale = startGravity * orientation;
        }
        else if(orientation == -1 && !isInverted)
        {
            transform.position += Vector3.up * orientation;

            orientation *= -1;
            
            gameObject.GetComponent<Rigidbody2D>().gravityScale = startGravity * orientation;
        }

        
        
    }
}
