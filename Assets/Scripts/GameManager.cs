using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isTextPanelIn = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void TextPanelIn(string _text)
    {
        if(!isTextPanelIn)
        {
            GameObject.Find("Interface").GetComponent<Animator>().SetTrigger("TextPanelIn");
            GameObject.Find("Interface/TextPanel/Text").GetComponent<Text>().text = _text;

            isTextPanelIn = true;
        }
        else
        {
            GameObject.Find("Interface/TextPanel/Text").GetComponent<Text>().text = _text;
            isTextPanelIn = true;
        }
        
    }

    public void TextPanelOut()
    {
        if(isTextPanelIn)
        {
            GameObject.Find("Interface").GetComponent<Animator>().SetTrigger("TextPanelOut");

            isTextPanelIn = false;
        }
    }
}
