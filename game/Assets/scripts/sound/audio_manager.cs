using System;
using Unity.Collections;
using UnityEngine;

public class audio_manager : MonoBehaviour
{
    public static audio_manager Instance;
    public sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //PlayMusic("background music here The name")
        PlayMusic("ambient_test");
    } 
    public void PlayMusic(string name)
    {
        sound s = Array.Find(musicSounds, xxHash3 => xxHash3.name == name);

        if (s == null)
        {
            Debug.Log("MUSIC Sound Not Found line -> 12");
        }

        else
        {
            musicSource.clip=s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name, float pitch = 1.0f)
    {
        sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("SFX Sound Not Found line -> 27");
        }
        else
        {
            //sfxSource.PlayOneShot(s.clip);
            sfxSource.pitch = pitch; // Apply custom pitch
            sfxSource.PlayOneShot(s.clip);
        }
    }
}
