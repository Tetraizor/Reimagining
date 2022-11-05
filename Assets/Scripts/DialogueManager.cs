using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public int levelIndex = 0;
    public float dialogueAmount = 1;

    void Start()
    {
        if(levelIndex == 1)
        {
            StartCoroutine(Level1());
        }
        if(levelIndex == 2)
        {
            StartCoroutine(Level2());
        }
        else
        {
            Player.instance.canInput = true;
        }
    }

    public IEnumerator Level1()
    {
        GameManager.instance.TextPanelIn("Merhaba, hayal dunyasına hoş geldin. Burada kurallar senin hayal gucun ile sınırlı. Gecmen gereken bazı engeller olacak, ve bu engeller ancak senin dunyayı farklı şekillerde hayal etmenle aşılabilir.");
        yield return new WaitForSeconds(dialogueAmount);
        GameManager.instance.TextPanelIn("Yapman gereken, görduğun sarı ilham parcalarını toplayıp bir şeyler hayal etmeye başlamak. Hayali yakala ve kendine yaklaştır, daha sonra boşlukları doldur ve engelleri aş.");
        yield return new WaitForSeconds(dialogueAmount);
        GameManager.instance.TextPanelIn("Dikdörtgen şeklindeki boşluklar tum objelerin mi, yoksa sadece sectiğin objenin mi etkilendiğini gösterir.");
        yield return new WaitForSeconds(dialogueAmount);
        GameManager.instance.TextPanelIn("Elmas şeklindeki boşluklara ise bir blok koymalısın. Tum boşluklar dolduğunda hayal etmeye başlayacak ve dunyayı değiştireceksin, bol şans.");
        yield return new WaitForSeconds(dialogueAmount);
        GameManager.instance.TextPanelOut();
        Player.instance.canInput = true;
    }

    public IEnumerator Level2()
    {
        GameManager.instance.TextPanelIn("Şimdi farklı hayaller ile karşılaşacaksın. İşlevleri aynen ustunde yazıldığı gibi, dene ve test et.");
        yield return new WaitForSeconds(dialogueAmount);
        GameManager.instance.TextPanelIn("Bu arada bir ipucu, yuvarlak şekilli boşluklara bir blok koyabildiğin gibi canlıları da yerleştirebilirsin, kendini bile!");
        yield return new WaitForSeconds(dialogueAmount);
        GameManager.instance.TextPanelOut();
        Player.instance.canInput = true;
    }
}
