using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDatabase : MonoBehaviour
{
    public List<GameObject> tileList;
    public static BlockDatabase instance;
    public List<GameObject> dreamTypes;

    public GameObject[,] realWorldTiles;

    void Awake()
    {
        instance = this;
    }

    public GameObject CreateBlock(int x, int y, int index)
    {
        GameObject newTile = Instantiate(BlockDatabase.instance.tileList[index], new Vector2(x, y), transform.rotation);
        if(realWorldTiles[x,y] != null)
        {
            DestroyBlock(x, y);
        }
        realWorldTiles[x, y] = newTile;
        return newTile;
    }

    public void DestroyBlock(int x, int y)
    {
        Destroy(realWorldTiles[x, y]);
        realWorldTiles[x, y] = null;
    }

    public int CheckBlock(Vector2Int _posVector)
    {
        if(realWorldTiles[_posVector.x, _posVector.y] != null)
        {
            return realWorldTiles[_posVector.x, _posVector.y].GetComponent<ObjectProperties>().objectIndex;
        }
        else
        {
            return -1;
        }
    }
}
