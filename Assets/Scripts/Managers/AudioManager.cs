/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle all the audio logic within the game.
*          Sound object are made to be reused when a sfx is requested
**/

using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, ISwapper
{
    [Header("Music")]
    [SerializeField]
    private AudioSource music;
    [SerializeField]
    private AudioClip[] musicClips;

    [Header("Sound")]
    public GameObject soundPefab;

    private List<AudioSource> soundObjects;
    private float soundVol;

    private void Start()
    {
        SwapIt();

        // Initialize sound object lists
        soundObjects = new List<AudioSource>();
        InitSounds();
    }


    public void PlayMusic()
    {
        if(music.clip != musicClips[SwapManager.MusicIdx])
        {
            music.Stop();
            music.clip = musicClips[SwapManager.MusicIdx];
            music.Play();
        }      
    }

    // previews music, but does not make current select
    public void PreviewMusic(int idx)
    {
        if (music.isPlaying)
            music.Stop();
        music.clip = musicClips[idx];
        music.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        // Check for available sound, create one if one is not available to play
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
        // Begin with 5 sound objects to work with
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
        // Created object
        GameObject soundObj = Instantiate(soundPefab, this.transform) as GameObject;

        // Use to play and add to list
        AudioSource aud = soundObj.GetComponent<AudioSource>();
        aud.volume = soundVol;
        aud.clip = clip;
        aud.Play();
        soundObjects.Add(aud);
    }

    public void SwapIt()
    {
        PlayMusic();
    }
}
