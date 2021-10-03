using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour 
{
    public static Music gameMusic;
    private AudioSource musicSrc;

    public AudioClip levelMusic1;
    public AudioClip levelMusic2;
    public AudioClip menuMusic1;
    public AudioClip menuMusic2;

    void Start () 
    {
        if (gameMusic == null)
        {
            DontDestroyOnLoad(gameObject);
            gameMusic = this;
        }
        else if (gameMusic != this)
        {
            Destroy(gameObject);
        }
        musicSrc = GetComponent<AudioSource>();
	}

    public void PlayMusic(string audioName)
    {
        if (audioName == "levelMusic1")
        {
            musicSrc.clip = levelMusic1;
            musicSrc.Play();
        }

        if (audioName == "levelMusic2")
        {
            musicSrc.clip = levelMusic2;
            musicSrc.Play();
        }

        if (audioName == "menuMusic1")
        {
            musicSrc.clip = menuMusic1;
            musicSrc.Play();
        }

        if (audioName == "menuMusic2")
        {
            musicSrc.clip = menuMusic2;
            musicSrc.Play();
        }

        if (audioName == "")
        {
            musicSrc.Stop();
        }
    }
}
