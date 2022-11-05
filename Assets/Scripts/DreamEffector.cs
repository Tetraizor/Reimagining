using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamEffector : MonoBehaviour
{
    public enum DreamType
    {
        SwapBlock = 0,
        InvertBlock = 1,
        ToggleBlock = 2,
    }

    public DreamType dreamType;
    public BlockDatabase database;

    public GameObject effectorBlock;
    public GameObject effectedBlock;

    public GameObject effectorRenderer;
    public GameObject effectedRenderer;
    public GameObject toggle;
    public GameObject invertionToggle;

    private bool isUsed = false;
    public bool isEffectorNeeded;

    public float smoothAmount = 5f;

    public Vector3 target;
    
    [Header("Swap Block")]
    [Space(16)]
    public bool isSpecific;
    public bool isSpecificSet;

    void Start()
    {
        database = BlockDatabase.instance;

        switch (dreamType)
        {
            case DreamType.SwapBlock: // Swap Blocks
            if(isEffectorNeeded)
            effectorRenderer = gameObject.transform.Find("Effector").gameObject;
            effectedRenderer = gameObject.transform.Find("Effected").gameObject;
            toggle = gameObject.transform.Find("Toggle").gameObject;
            break;

            case DreamType.InvertBlock: // Invert
            effectedRenderer = gameObject.transform.Find("Effected").gameObject;
            toggle = gameObject.transform.Find("AmountToggle").gameObject;
            break;

            case DreamType.ToggleBlock:
            effectedRenderer = gameObject.transform.Find("Effected").gameObject;
            toggle = gameObject.transform.Find("Toggle").gameObject;
            break;
        }

        
    }

    public void Update()
    {
        if(dreamType == DreamType.SwapBlock)
        {
            if(effectedBlock != null && effectorBlock != null && !isUsed && isSpecificSet)
            {
                GetComponent<Animator>().SetTrigger("Dream");
                isUsed = true;
            }
        }
        if(dreamType == DreamType.InvertBlock)
        {
            if(effectedBlock != null && !isUsed && isSpecificSet)
            {
                GetComponent<Animator>().SetTrigger("Dream");
                isUsed = true;
            }
        }
        if(dreamType == DreamType.ToggleBlock)
        {
            if(effectedBlock != null && !isUsed && isSpecificSet)
            {
                GetComponent<Animator>().SetTrigger("Dream");
                isUsed = true;
            }   
        }
        
        transform.position = Vector3.Lerp(transform.position, target, smoothAmount * Time.deltaTime);
    }

    public void RequestTiles(int _requestIndex)
    {
        Player.instance.ActivateBlockPicker();
        Player.instance.requestIndex = _requestIndex;

        if(_requestIndex == 1 || _requestIndex == 2)
        {
            Player.instance.visionMask = Player.instance.platformLayer;   
        }
        else
        {
            Player.instance.visionMask = Player.instance.entityAndBlockFilter;   
        }
    }

    public void ShootParticle()
    {
        gameObject.transform.Find("Particle").GetComponent<ParticleSystem>().Play();
        gameObject.transform.Find("Particle").transform.parent = null;
    }

    public void StartDream()
    {
        switch (dreamType)
        {
            case DreamType.SwapBlock:
                if(isSpecific)
                {
                    GameObject newTile = Instantiate(BlockDatabase.instance.tileList[effectorBlock.GetComponent<ObjectProperties>().objectIndex], effectedBlock.transform.position, effectedBlock.transform.rotation);
                    newTile.GetComponent<ObjectProperties>().orientation = effectedBlock.GetComponent<ObjectProperties>().orientation;
                    newTile.GetComponent<ObjectProperties>().UpdateTile();
                    effectedBlock.GetComponent<ObjectProperties>().DestroyThis();
                }
                else
                {
                    for(int x = 0; x < TilesetBuilder.instance.mapSize.x; x++)
                    {
                        for(int y = 0; y < TilesetBuilder.instance.mapSize.y; y++)
                        {
                            if(BlockDatabase.instance.realWorldTiles[x, y] != null)
                            {
                                ObjectProperties tile = BlockDatabase.instance.realWorldTiles[x, y].GetComponent<ObjectProperties>();
                                if(tile.GetComponent<ObjectProperties>().objectIndex == effectedBlock.GetComponent<ObjectProperties>().objectIndex)
                                {
                                    GameObject newTile = BlockDatabase.instance.CreateBlock(x, y, effectorBlock.GetComponent<ObjectProperties>().objectIndex);
                                    newTile.GetComponent<ObjectProperties>().orientation = tile.GetComponent<ObjectProperties>().orientation;
                                    newTile.GetComponent<ObjectProperties>().UpdateTile();
                                    tile.GetComponent<ObjectProperties>().DestroyThis();
                                }
                            }
                        }
                    }
                }
                break;

                case DreamType.InvertBlock:
                if(effectedBlock.GetComponent<Player>() != null)
                {
                    effectedBlock.GetComponent<Player>().Invert();
                }
                else if(effectedBlock.GetComponent<Enemy>() != null)
                {
                    if(isSpecific)
                    {
                        effectedBlock.GetComponent<Enemy>().Invert();
                    }
                    else
                    {
                        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                        {
                            enemy.GetComponent<Enemy>().Invert();
                        }
                    }
                    
                }
                else
                {   
                    if(isSpecific)
                    {
                        effectedBlock.GetComponent<ObjectProperties>().Invert();
                    }
                    else
                    {
                        for(int x = 0; x < TilesetBuilder.instance.mapSize.x; x++)
                        {
                            for(int y = 0; y < TilesetBuilder.instance.mapSize.y; y++)
                            {
                                if(BlockDatabase.instance.realWorldTiles[x, y] != null)
                                {
                                    ObjectProperties tile = BlockDatabase.instance.realWorldTiles[x, y].GetComponent<ObjectProperties>();
                                    if(tile.GetComponent<ObjectProperties>().objectIndex == effectedBlock.GetComponent<ObjectProperties>().objectIndex)
                                    {
                                        tile.GetComponent<ObjectProperties>().Invert();
                                    }
                                }
                            }
                        }
                    }
                }
                break;

            case DreamType.ToggleBlock:
            if(effectedBlock.GetComponent<Player>())
            {
                effectedBlock.GetComponent<Player>().KillPlayer();
            }
            if(effectedBlock.GetComponent<Enemy>())
            {
                if(isSpecific)
                {
                    effectedBlock.GetComponent<Enemy>().KillEnemy();
                }
                else
                {
                    foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                    {
                        enemy.GetComponent<Enemy>().KillEnemy();
                    }
                }
            }
            if(effectedBlock.GetComponent<ObjectProperties>())
            {
                if(isSpecific)
                {
                    effectedBlock.GetComponent<ObjectProperties>().TurnObject();
                }
                else
                {
                    for(int x = 0; x < TilesetBuilder.instance.mapSize.x; x++)
                        {
                            for(int y = 0; y < TilesetBuilder.instance.mapSize.y; y++)
                            {
                                if(BlockDatabase.instance.realWorldTiles[x, y] != null)
                                {
                                    ObjectProperties tile = BlockDatabase.instance.realWorldTiles[x, y].GetComponent<ObjectProperties>();
                                    if(tile.GetComponent<ObjectProperties>().objectIndex == effectedBlock.GetComponent<ObjectProperties>().objectIndex)
                                    {
                                        tile.GetComponent<ObjectProperties>().TurnObject();
                                    }
                                }
                            }
                        }
                }
            }
            break;
        }

        Player.instance.DeactivateBlockPicker();
        Destroy(gameObject, 0.4f);
    }

    public void ButtonPress(int index)
    {
        switch(index)
        {
            case 0:
            SwapToggle();
            isSpecificSet = true;
            toggle.GetComponent<Animation>().Stop();
            break;

            case 1:
            RequestTiles(1);
            break;

            case 2:
            RequestTiles(2);
            break;

            case 4:
            RequestTiles(3);
            break;
        }
    }

    public void SwapToggle()
    {
        isSpecific = !isSpecific;
        if(isSpecific)
        {
            toggle.transform.GetChild(0).gameObject.SetActive(false);
            toggle.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            toggle.transform.GetChild(0).gameObject.SetActive(true);
            toggle.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void PlayBuildUp()
    {
        GameManager.instance.gameObject.GetComponent<AudioManager>().PlayAudio(4);
    }
}
