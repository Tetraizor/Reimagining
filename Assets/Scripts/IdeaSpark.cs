using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdeaSpark : MonoBehaviour
{
    public DreamEffector.DreamType dreamType;
    public GameObject dream;
    public bool isDreamOn;
    public bool canGain = true;

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if(canGain)
        {
            if(trigger.transform.tag == "Player")
            {
                GiveIdea();
                canGain = false;
            }
        }
    }

    public void GiveIdea()
    {
        int ideaIndex;
        
        ideaIndex = ((int)dreamType);

        dream = Instantiate(BlockDatabase.instance.dreamTypes[ideaIndex], transform.position, Quaternion.Euler(Vector3.zero));

        isDreamOn = true;

        dream.transform.parent = Camera.main.transform;

        dream.GetComponent<DreamEffector>().target = (Vector2)Camera.main.transform.position + new Vector2(Random.Range(-(Camera.main.orthographicSize * Camera.main.aspect)/2, (Camera.main.orthographicSize * Camera.main.aspect)/2), Random.Range(-(Camera.main.orthographicSize)/2, (Camera.main.orthographicSize)/2));

        Destroy(gameObject);
    }
}
