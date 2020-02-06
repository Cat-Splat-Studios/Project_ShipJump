using System.Collections.Generic;
using UnityEngine;

/** 
* Author: Matthew Douglas
* Purpose: To handle all the sound logic within the game
**/
public class AudioManager : MonoBehaviour
{
    public AudioSource music;

    public AudioClip musicClip;

    public GameObject soundPefab;

    private List<AudioSource> soundObjects;

    private float soundVol;

    private void Start()
    {
        PlayMusic();

        soundObjects = new List<AudioSource>();
        InitSounds();
    }


    public void PlayMusic()
    {
        music.clip = musicClip;
        music.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        if (!FindSound(clip))
        {
            CreateSound(clip);
        }
    }

    private bool FindSound(AudioClip clip)
    {
        foreach (AudioSource aud in soundObjects)
        {
            if (!aud.isPlaying)
            {
                aud.clip = clip;
                aud.Play();
                return true;
            }
        }

        return false;
    }

    private void InitSounds()
    {
        for(int i = 0; i < 5; ++i)
        {
            GameObject soundObj = Instantiate(soundPefab, this.transform) as GameObject;

            AudioSource aud = soundObj.GetComponent<AudioSource>();
            aud.volume = 0.5f;
            soundObjects.Add(aud);
        }
    }

    private void CreateSound(AudioClip clip)
    {
        GameObject soundObj = Instantiate(soundPefab, this.transform) as GameObject;

        AudioSource aud = soundObj.GetComponent<AudioSource>();
        aud.volume = soundVol;
        aud.clip = clip;
        aud.Play();
        soundObjects.Add(aud);
    }
}
