using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[,] audioBank;
    public AudioSource audioSource;

    public int[] audioLengths;

    public void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
        audioSource.loop = false;
    }

    public void PlayAudio(int index)
    {
        audioSource.PlayOneShot(AudioBank.instance.GetAudioClip(index));
    }
}
