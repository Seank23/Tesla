using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour
{
    public static Audio gameAudio;
    private AudioSource sfxSrc;

    public AudioClip sfx_btnClick1;
    public AudioClip sfx_btnClick2;
    public AudioClip sfx_unable;
    public AudioClip sfx_collectCrystal;
    public AudioClip sfx_collectControl;
    public AudioClip sfx_wrongCombo;
    public AudioClip sfx_itemsBought;

    void Start ()
    {
        if (gameAudio == null)
        {
            DontDestroyOnLoad(gameObject);
            gameAudio = this;
        }
        else if (gameAudio != this)
        {
            Destroy(gameObject);
        }
        sfxSrc = GetComponent<AudioSource>();
    }
	
	public void PlaySFX(string audioName)
    {
        if(audioName == "btnClick1")
        {
            sfxSrc.clip = sfx_btnClick1;
            sfxSrc.Play();
        }
        if (audioName == "btnClick2")
        {
            sfxSrc.clip = sfx_btnClick2;
            sfxSrc.Play();
        }
        if (audioName == "unable")
        {
            sfxSrc.clip = sfx_unable;
            sfxSrc.Play();
        }
        if (audioName == "crystal")
        {
            sfxSrc.clip = sfx_collectCrystal;
            sfxSrc.Play();
        }
        if (audioName == "control")
        {
            sfxSrc.clip = sfx_collectControl;
            sfxSrc.Play();
        }
        if (audioName == "wrongCombo")
        {
            sfxSrc.clip = sfx_wrongCombo;
            sfxSrc.Play();
        }
        if (audioName == "itemsBought")
        {
            sfxSrc.clip = sfx_itemsBought;
            sfxSrc.Play();
        }
    }
}
