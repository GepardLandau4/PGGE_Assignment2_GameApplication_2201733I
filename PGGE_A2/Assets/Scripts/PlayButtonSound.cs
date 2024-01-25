using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonSound : MonoBehaviour
{
    public AudioSource audio;

    void Start()
    {
        audio = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioSource>();
        //DontDestroyOnLoad(gameObject);
    }

    public void onClickPlayButtonSound()
    {
        audio.Play();
    }
}
