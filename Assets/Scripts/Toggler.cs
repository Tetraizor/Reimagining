using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggler : MonoBehaviour
{
    public List<Vector2> positionList = new List<Vector2>();

    public bool isPressing = false;

    public bool toggleWhilePressing;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(isPressing == false)
        {
            foreach(Vector2 pos in positionList)
            {
                if(BlockDatabase.instance.CheckBlock(new Vector2Int((int)pos.x, (int)pos.y)) != -1)
                {
                    BlockDatabase.instance.realWorldTiles[(int)pos.x, (int)pos.y].GetComponent<ObjectProperties>().TurnObject();
                    gameObject.GetComponent<AudioManager>().PlayAudio(10);
                }
            }
            isPressing = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(toggleWhilePressing)
        {
            if(isPressing)
            {
                foreach(Vector2 pos in positionList)
                {
                    if(BlockDatabase.instance.CheckBlock(new Vector2Int((int)pos.x, (int)pos.y)) != -1)
                    {
                        BlockDatabase.instance.realWorldTiles[(int)pos.x, (int)pos.y].GetComponent<ObjectProperties>().TurnObject();
                        gameObject.GetComponent<AudioManager>().PlayAudio(10);
                    }
                }
                isPressing = false;
            }
        }
    }
}
