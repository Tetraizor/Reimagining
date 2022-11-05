using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperties : MonoBehaviour
{
    public int objectIndex;
    public Vector3 position;

    public int orientation;

    public bool collisionKills = false;
    public bool canClimb = false;
    public bool canSwim = false;
    public bool isSolid = true;
    public bool isTransparent = false;
    public bool isOn = true;

    public bool isInverted = false;

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    public void UpdateTile()
    {
        
    }

    public void Invert()
    {
        transform.localScale = new Vector3(-transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        isInverted = !isInverted;
        if(isInverted)
        {
            transform.position += (Vector3)Vector2.one;
        }
        else
        {
            transform.position -= (Vector3)Vector2.one;
        }
    }

    public void TurnObject()
    {
        isOn = !isOn;
        switch(objectIndex)
        {
            case 0: // GRASS
            
            if(isOn)
            {
                GetComponent<SpriteRenderer>().sprite = BlockDatabase.instance.tileList[0].GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = BlockDatabase.instance.tileList[7].GetComponent<SpriteRenderer>().sprite;
            }
            break;

            case 3: // WINES
            if(isOn)
            {

            }
            else
            {
                GameObject newTile = BlockDatabase.instance.CreateBlock((int)this.position.x, (int)this.position.y, 8);
            }
            break;
            
            case 6:
            if(isOn)
            {
                gameObject.transform.GetChild(0).GetComponent<PolygonCollider2D>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().color = new Color(.8f, .8f, .8f, 0.4f);
                print("aa");
            }
            else
            {
                gameObject.transform.GetChild(0).GetComponent<PolygonCollider2D>().enabled = true;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                print("bb");
            }
            break;

            case 10:
            if(!isOn)
            {
                collisionKills = false;
                gameObject.GetComponent<SpriteRenderer>().size = new Vector2(1, 0.2f);
            }
            else
            {
                collisionKills = true;
                gameObject.GetComponent<SpriteRenderer>().size = new Vector2(1, 1);
            }
            break;

            case 11:
            if(!isOn)
            {
                gameObject.transform.GetChild(0).GetComponent<CapsuleCollider2D>().size = Vector2.zero;
                gameObject.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Stop();
            }
            else
            {
                gameObject.transform.GetChild(0).GetComponent<CapsuleCollider2D>().size = new Vector2(0.86f, 4.5f);
                gameObject.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Play();
            }
            break;

            case 8:
            if(isOn)
            {
                GameObject newTile = BlockDatabase.instance.CreateBlock((int)this.position.x, (int)this.position.y, 8);
            }
            break;

            default:
            break;
        }
        
    }

}
