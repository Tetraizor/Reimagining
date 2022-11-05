using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinArea : MonoBehaviour
{
    public int targetScene;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.transform != null)
        {
            if(collider.tag == "Player")
            {
                StartCoroutine(LoadScene());
                GameObject.Find("Interface").GetComponent<Animator>().SetTrigger("CloudIn");
            }
        }
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(targetScene);
    }
}
