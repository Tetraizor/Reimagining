using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesetBuilder : MonoBehaviour
{
    public Vector2Int mapSize;
    private Tilemap tilemap;

    public static TilesetBuilder instance;
    
    
    void Awake()
    {
        tilemap = GameObject.Find("Tilemap").gameObject.GetComponent<Tilemap>();
        instance = this;
    }

    void Start()
    {
        BlockDatabase.instance.realWorldTiles = new GameObject[mapSize.x, mapSize.y];

        for(int x = 0; x < mapSize.x; x++)
        {
            for(int y = 0; y < mapSize.y; y++)
            {
                if(tilemap.GetTile(new Vector3Int(x - (int)tilemap.transform.parent.position.x, y - (int)tilemap.transform.parent.position.y, 0)) != null)
                {
                    GameObject newTile = Instantiate(BlockDatabase.instance.tileList[int.Parse(tilemap.GetTile(new Vector3Int(x - (int)tilemap.transform.parent.position.x, y - (int)tilemap.transform.parent.position.y, 0)).name)], new Vector3(x, y, 0), transform.rotation);
                    BlockDatabase.instance.realWorldTiles[x, y] = newTile;
                    newTile.GetComponent<ObjectProperties>().position = new Vector2(x, y);
                }
            }
        }
        gameObject.GetComponent<Startup>().Initialize();
        Destroy(tilemap.gameObject);
    }
}
