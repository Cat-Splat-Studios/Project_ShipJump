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

    [Header("Button Sounds")]
    public AudioClip[] buttonSounds;

    private List<AudioSource> soundObjects;

    private void Start()
    {
        SwapIt();

        // Initialize sound object lists
        soundObjects = new List<AudioSource>();
        InitSounds();
    }


    public void PlayMusic(int idx)
    {
        if(music.clip != musicClips[idx])
        {
            music.Stop();
            music.clip = musicClips[idx];
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

    public void PlaySound(AudioClip clip, float vol = 1.0f)
    {
        // Check for available sound, create one if one is not available to play
        if (!FindSound(clip, vol))
        {
            CreateSound(clip, vol);
        }
    }

    public void PressButton(int buttonSound = 0)
    {
        PlaySound(buttonSounds[buttonSound]);
    }

    private bool FindSound(AudioClip clip, float vol)
    {
        foreach (AudioSource aud in soundObjects)
        {
            if (!aud.isPlaying)
            {
                aud.volume = vol;
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
        for(int i = 0; i < 10; ++i)
        {
            GameObject soundObj = Instantiate(soundPefab, this.transform) as GameObject;

            AudioSource aud = soundObj.GetComponent<AudioSource>();
            aud.volume = 0.5f;
            soundObjects.Add(aud);
        }
    }

    private void CreateSound(AudioClip clip, float vol)
    {
        // Created object
        GameObject soundObj = Instantiate(soundPefab, this.transform) as GameObject;

        // Use to play and add to list
        AudioSource aud = soundObj.GetComponent<AudioSource>();
        aud.volume = vol;
        aud.clip = clip;
        aud.Play();
        soundObjects.Add(aud);
    }

    public void SwapIt()
    {
        int idx = 0;
        if (PlayerPrefs.HasKey("musicIdx"))
        {
            idx = PlayerPrefs.GetInt("musicIdx");
        }
        else
        {
            idx = SwapManager.MusicIdx;
        }

        PlayMusic(idx);
    }
}
