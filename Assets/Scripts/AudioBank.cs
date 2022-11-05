using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBank : MonoBehaviour
{
    public static AudioBank instance;

    public List<AudioClip> audioList = new List<AudioClip>();

    void Awake()
    {
        instance = this;
    }

    public AudioClip GetAudioClip(int index)
    {
        return audioList[index];
    }

    public AudioClip GetAudioClip(int index1, int index2)
    {
        return audioList[Random.Range(index1, index2)];
    }
}
