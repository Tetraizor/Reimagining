using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour
{
    public Vector2[] toggleList;
    public Vector2[] reverseList;

    public void Initialize()
    {
        foreach(Vector2 pos in reverseList)
        {
            if(BlockDatabase.instance.CheckBlock(new Vector2Int((int)pos.x, (int)pos.y)) != -1)
            {
                BlockDatabase.instance.realWorldTiles[(int)pos.x, (int)pos.y].GetComponent<ObjectProperties>().Invert();
            }
        }

        foreach(Vector2 pos in toggleList)
        {
            if(BlockDatabase.instance.CheckBlock(new Vector2Int((int)pos.x, (int)pos.y)) != -1)
            {
                BlockDatabase.instance.realWorldTiles[(int)pos.x, (int)pos.y].GetComponent<ObjectProperties>().TurnObject();
            }
        }
    }
}
